using DTCore.System.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DTCore.Tools
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
                Debug.WriteLine(ex.Message, ex.StackTrace);
                return false;
            }
        }
    }
}
