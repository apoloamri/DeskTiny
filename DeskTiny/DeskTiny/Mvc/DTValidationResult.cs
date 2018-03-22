using DTCore.System;
using DTCore.Tools.Extensions;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DTCore.Mvc
{
    public static class DTValidationResult
    {
        public static ValidationResult Compose(string message, params string[] memberNames)
        {
            var newMessage = Messages.Get(message);

            if (memberNames != null && memberNames.Count() > 0)
            {
                return new ValidationResult(newMessage, memberNames.Select(x => x.ToUnderscore()));
            }

            return new ValidationResult(newMessage, new[] { "common" });
        }

        public static ValidationResult FieldRequired(string fieldName, object value, string message = null)
        {
            if (string.IsNullOrEmpty(message))
            {
                message = Messages.Get("RequiredField", fieldName);
            }

            if (value == null)
            {
                return Compose(message, fieldName);
            }
            else
            {
                if (string.IsNullOrEmpty(value.ToString()))
                {
                    return Compose(message, fieldName);
                }
            }

            return null;
        }
    }
}
