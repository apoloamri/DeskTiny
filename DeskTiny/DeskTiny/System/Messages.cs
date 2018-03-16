using DTCore.Tools;
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
                return string.Format(defaultMessage, values);
            }

            return 
                string.IsNullOrEmpty(defaultMessage) ?
                message :
                string.Format(defaultMessage, message);
        }
    }
}
