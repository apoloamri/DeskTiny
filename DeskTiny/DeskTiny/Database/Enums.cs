﻿namespace DTCore.Database
{
    public enum Condition { EqualTo, NotEqualTo, GreaterThan, LessThan, GreaterThanEqualTo, LessThanEqualTo, Like, NotLike }
    public enum Join { INNER, OUTER, LEFT, RIGHT }
    public enum Operator { AND, OR }
    public enum Order { ASC, DESC }
    public enum Provider { MySql, Postgres, SqlServer }
}
