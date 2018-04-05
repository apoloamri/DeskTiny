﻿using DTCore.Database.Attributes;
using System;

namespace DTCore.Database.Tables
{
    public class Sessions : Entity
    {
        [NotNull]
        public string session_id { get; set; }

        [NotNull]
        public string session_key { get; set; }

        [NotNull]
        public DateTime? session_time { get; set; }
    }
}
