using Tenderfoot.TfSystem.Diagnostics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
    }
}
