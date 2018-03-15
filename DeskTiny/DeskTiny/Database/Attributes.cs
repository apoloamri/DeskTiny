using System;
using System.ComponentModel.DataAnnotations;

namespace DTCore.Database.Attributes
{
    public class Default : Attribute
    {
        public object DefaultObject { get; set; }

        public Default(object defaultValue)
        {
            this.DefaultObject = defaultValue;
        }

        public DefaultFunctions? DefaultFunction { get; set; }

        public Default(DefaultFunctions defaultValue)
        {
            this.DefaultFunction = defaultValue;
        }
    }

    public class Length : Attribute
    {
        public int LengthCount { get; set; }

        public Length([Range(1, 255)]int lenth)
        {
            this.LengthCount = lenth;
        }
    }

    public class PrimaryKey : Attribute { }

    public class NotNull : Attribute { }

    public class Serial : Attribute { }

    public class NonTableColumn : Attribute { }

    public class Unique : Attribute { }

    public enum DefaultFunctions { Now }
}
