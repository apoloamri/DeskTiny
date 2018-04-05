using System;

namespace DTCore.Database.Attributes
{
    public class DefaultAttribute : Attribute
    {
        public string DefaultObject { get; set; }

        public DefaultAttribute(string defaultValue)
        {
            this.DefaultObject = defaultValue;
        }

        public Functions? DefaultFunction { get; set; }

        public DefaultAttribute(Functions defaultValue)
        {
            this.DefaultFunction = defaultValue;
        }
    }

    public class EncryptAttribute : Attribute { }

    public class LengthAttribute : Attribute
    {
        public int LengthCount { get; set; }

        public LengthAttribute(int lenth)
        {
            this.LengthCount = lenth;
        }
    }

    public class PrimaryKeyAttribute : Attribute { }

    public class NotNullAttribute : Attribute { }

    public class SerialAttribute : Attribute { }

    public class NonTableColumnAttribute : Attribute { }

    public class UniqueAttribute : Attribute { }

    public class TextAttribute : Attribute { }

    public enum Functions { Now }
}
