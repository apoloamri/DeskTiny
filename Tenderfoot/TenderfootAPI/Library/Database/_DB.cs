﻿using Tenderfoot.Database;

namespace TenderfootAPI.Library.Database
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
    }
}