using System;
using System.ComponentModel.DataAnnotations;

namespace DTCore.Database.Attributes
{
    public class Default : Attribute
    {
        public string DefaultObject { get; set; }

        public Default(string defaultValue)
        {
            this.DefaultObject = defaultValue;
        }

        public Functions? DefaultFunction { get; set; }

        public Default(Functions defaultValue)
        {
            this.DefaultFunction = defaultValue;
        }
    }

    public class Length : Attribute
    {
        public int LengthCount { get; set; }

        public Length(int lenth)
        {
            this.LengthCount = lenth;
        }
    }

    public class PrimaryKey : Attribute { }

    public class NotNull : Attribute { }

    public class Serial : Attribute { }

    public class NonTableColumn : Attribute { }

    public class Unique : Attribute { }

    public class Text : Attribute { }

    public enum Functions { Now }
}
