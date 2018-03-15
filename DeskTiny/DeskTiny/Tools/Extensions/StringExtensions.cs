using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTCore.Tools.Extensions
{
    public static class StringExtensions
    {
        public static string ToUnderscore(this string value)
        {
            return string.Concat(value.Select((x, i) =>
                i > 0 && char.IsUpper(x) ?
                "_" + x.ToString()?.ToLower() :
                x.ToString().ToLower()));
        }

        public static string ToCamelCase(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return value;
            }

            string[] array = value.Split('_');

            for (int i = 0; i < array.Length; i++)
            {
                string s = array[i];
                string first = string.Empty;
                string rest = string.Empty;

                if (s.Length > 0)
                {
                    first = Char.ToUpperInvariant(s[0]).ToString();
                }

                if (s.Length > 1)
                {
                    rest = s.Substring(1).ToLowerInvariant();
                }

                array[i] = first + rest;
            }

            string newname = string.Join("", array);
            
            return newname;
        }
    }
}
