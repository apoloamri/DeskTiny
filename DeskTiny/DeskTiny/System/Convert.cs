using System;
using SystemConvert = System.Convert;

namespace DTCore.System
{
    public class Convert
    {
        public static object ChangeType(object value, Type conversion)
        {
            var t = conversion;

            if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                {
                    return null;
                }

                t = Nullable.GetUnderlyingType(t);
            }

            return SystemConvert.ChangeType(value, t);
        }
    }
}
