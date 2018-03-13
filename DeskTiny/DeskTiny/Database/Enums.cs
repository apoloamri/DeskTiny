namespace DTCore.Database.Enums
{
    public enum Condition { Equal, NotEqual, Greater, Lesser, GreaterEqual, LesserEqual, LIKE, NOT_LIKE }
    public enum Join { INNER, OUTER, LEFT, RIGHT }
    public enum Operator { AND, OR }
    public enum Order { ASC, DESC }
}
