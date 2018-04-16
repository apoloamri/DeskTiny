using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Tenderfoot.DTSystem
{
    public class DTConvert
    {
        public static object ChangeType(object value, Type conversion)
        {
            if (typeof(IEnumerable).IsAssignableFrom(conversion) &&
                conversion != typeof(string))
            {
                var values = value.ToString().Split(",");

                if (conversion == typeof(List<string>) || conversion == typeof(string[]))
                {
                    if (conversion.IsArray)
                    {
                        return values;
                    }
                    else
                    {
                        return values.ToList();
                    }
                }
                else if (conversion == typeof(List<int>) || conversion == typeof(int[]))
                {
                    var returnList = values.Select(x => { return (int)ChangeType(x, typeof(int)); });
                    
                    if (conversion.IsArray)
                    {
                        return returnList.ToArray();
                    }
                    else
                    {
                        return returnList.ToList();
                    }
                }
            }
            
            Type type = Nullable.GetUnderlyingType(conversion) ?? conversion;

            return (value == null) ? null : Convert.ChangeType(value, type);
        }
    }
}
