using DTCore.Tools;
using System.Collections.Generic;
using System.Linq;

namespace DTCore.System
{
    public static class Messages
    {
        public static string Get(string message, params string[] values)
        {
            string defaultMessage = ConfigurationBuilder.Configuration().GetSection("DefaultMessages").GetSection(message).Value;

            if (values != null && values.Count() > 0)
            {
                var fields = new List<string>();

                foreach (var value in values)
                {
                    string fieldName = ConfigurationBuilder.Configuration().GetSection("FieldNames").GetSection(value).Value;

                    if (string.IsNullOrEmpty(fieldName))
                    {
                        fields.Add(value);
                    }

                    fields.Add(fieldName);
                }

                return string.Format(defaultMessage, fields.ToArray());
            }

            return 
                string.IsNullOrEmpty(defaultMessage) ?
                message :
                defaultMessage;
        }
    }
}
