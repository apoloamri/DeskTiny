using Tenderfoot.TfSystem.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq;
using System.Web;

namespace Tenderfoot.Tools
{
    public static class JsonTools
    {
        public static bool IsValidJson(string jsonString)
        {
            try
            {
                JToken.Parse(jsonString);
                return true;
            }
            catch (JsonReaderException ex)
            {
                TfDebug.WriteLine(ex.Message, ex.StackTrace);
                return false;
            }
        }

        public static string QueryStringToJson(string queryString)
        {
            var queryStrings = HttpUtility.ParseQueryString(queryString);
            var jsonString = "{";
            var jsonList = new List<string>();
            var objectStrings = new List<string>();
            var parentDictionary = new Dictionary<string, object>();
            var parentList = new Dictionary<string, List<Dictionary<string, object>>>();
            var parentName = string.Empty;

            foreach (var item in queryStrings.AllKeys)
            {
                var value = queryStrings[item];
                var regex = Regex.Matches(item, @"\[.*?\]");
                
                if (regex.Count >= 2)
                {
                    var newParentName = item.Split("[")[0];
                    var keyName = regex[1].Groups[0].Value?
                        .Replace("[", "")
                        .Replace("]", "");
                    
                    if (parentName == newParentName)
                    {
                        if (parentDictionary.ContainsKey(keyName))
                        {
                            if (parentList.ContainsKey(parentName))
                            {
                                parentList[parentName].Add(parentDictionary);
                            }
                            else
                            {
                                parentList.Add(
                                    parentName,
                                    new List<Dictionary<string, object>> { { parentDictionary } });
                            }
                            
                            parentDictionary = new Dictionary<string, object>();
                            parentDictionary.Add(keyName, value);
                        }
                        else
                        {
                            parentDictionary.Add(keyName, value);
                        }
                    }
                    else
                    {
                        parentDictionary.Add(keyName, value);
                        parentName = newParentName;
                    }
                }
                else if (item.Contains("[]"))
                {
                    var keyName = item
                        .Replace("[", "")
                        .Replace("]", "");
                    var array = value.Split(",")?.Select(x => $"\"{x}\"");
                    jsonList.Add($"\"{keyName}\" : [{string.Join(",", array)}]");
                }
                else
                {
                    jsonList.Add($"\"{item}\" : \"{value}\"");
                }
            }

            if (parentList.ContainsKey(parentName))
            {
                parentList[parentName].Add(parentDictionary);
            }
            else
            {
                parentList.Add(
                    parentName,
                    new List<Dictionary<string, object>> { { parentDictionary } });
            }
            
            foreach (var item in parentList)
            {
                var arrayString = $"\"{item.Key}\" : [";
                var objectList = new List<string>();
                foreach (var subItem in item.Value)
                {
                    objectList.Add(DictionaryToJson(subItem));
                }
                arrayString += string.Join(",", objectList);
                arrayString += "]";
                objectStrings.Add(arrayString);
            }

            jsonList.Add(string.Join(",", objectStrings));
            jsonString += string.Join(",", jsonList);
            jsonString += "}";
            return jsonString;
        }

        public static string DictionaryToJson(this Dictionary<string, object> dictionary)
        {
            var jsonString = "{";
            var jsonList = new List<string>();

            foreach (var item in dictionary)
            {
                jsonList.Add($"\"{item.Key}\" : \"{item.Value}\"");
            }

            jsonString += string.Join(",", jsonList);
            jsonString += "}";
            return jsonString;
        }
    }
}
