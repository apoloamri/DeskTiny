using Tenderfoot.DTSystem;
using Tenderfoot.DTSystem.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Tenderfoot.Tools
{
    public static class DictionaryClassConverter
    {
        public static Object DictionaryToClass<Object>(Dictionary<string, object> dictionary)
        {
            Type type = typeof(Object);

            object obj = Activator.CreateInstance(type);

            foreach (var keyValue in dictionary)
            {
                var property = type.GetProperty(keyValue.Key);
                if (property != null)
                {
                    if (keyValue.Value != DBNull.Value)
                    {
                        try
                        {
                            var value = DTConvert.ChangeType(keyValue.Value, property.PropertyType);

                            type.GetProperty(keyValue.Key).SetValue(obj, value);
                        }
                        catch
                        {
                            DTDebug.WriteLog(
                                Settings.Logs.System,
                                $"Ignored Malformed Line - {DateTime.Now}",
                                $"Name: {keyValue.Key}{Environment.NewLine}" +
                                $"Value: {keyValue.Value}{Environment.NewLine}" +
                                $"Type: {property.PropertyType}");
                        }
                    }
                }
            }

            return (Object)obj;
        }
        
        public static Dictionary<string, object> ClassToDictionary(object obj, string optionalName = null)
        {
            return obj.GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .ToDictionary(prop => $"{optionalName}{prop.Name}", prop => prop.GetValue(obj, null));
        }
    }
}
