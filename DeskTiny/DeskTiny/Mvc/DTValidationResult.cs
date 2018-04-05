using DTCore.DTSystem;
using DTCore.Tools.Extensions;
using DTCore.WebApi;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTCore.Mvc
{
    public static class DTValidationResult
    {
        public static ValidationResult CheckSessionActivity(string sessionId, string sessionKey)
        {
            if (Session.IsSessionActive(sessionId, sessionKey))
            {
                return null;
            }

            return DTValidationResult.Compose("SessionExpired", "SessionKey");
        }

        public static ValidationResult Compose(string message, params string[] memberNames)
        {
            return Compose(message, null, memberNames);
        }

        public static ValidationResult Compose(string message, string[] values, params string[] memberNames)
        {
            var newMessage = Resources.GetMessage(message, values);

            if (memberNames != null && memberNames.Count() > 0)
            {
                return new ValidationResult(newMessage, memberNames.Select(x => x.ToUnderscore()));
            }

            return new ValidationResult(newMessage, new[] { "common" });
        }

        public static ValidationResult FieldRequired(string fieldName, object value, string message = null)
        {
            if (message.IsEmpty())
            {
                message = Resources.GetMessage("RequiredField", fieldName);
            }

            if (value == null)
            {
                return Compose(message, fieldName);
            }
            else
            {
                if (value.ToString().IsEmpty())
                {
                    return Compose(message, fieldName);
                }
            }

            return null;
        }
    }
}
