﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DTCore.DTSystem;
using DTCore.Tools;

namespace DTCore.Database.System
{
    public class NonConditions
    {
        private string OptionalName = "nq_";
        private string Param = ConnectProvider.Param();
        internal string ColumnNames { get; set; }
        internal string ColumnParameters { get; set; }
        internal string ColumnValues { get; set; }
        internal Dictionary<string, object> Parameters { get; set; } = new Dictionary<string, object>();

        internal void CreateColumnParameters(object entity)
        {
            var properties = 
                entity
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetValue(entity) != null)?
                .ToArray();
            
            foreach (var property in properties)
            {
                if (property.GetCustomAttribute<EncryptAttribute>(false) != null && 
                    property.PropertyType == typeof(string))
                {
                    property.SetValue(entity, Encryption.Encrypt(Convert.ToString(property.GetValue(entity)), true));
                }
            }

            this.ColumnNames = string.Join(", ", properties?.Select(x => x.Name));
            this.ColumnParameters = string.Join(", ", properties?.Select(x => $"{Param}{this.OptionalName}{x.Name}"));
            this.ColumnValues = string.Join(", ", properties?.Select(x => { return $"{x.Name} = {Param}{this.OptionalName}{x.Name}"; }));
            this.Parameters =
                DictionaryClassConverter.ClassToDictionary(entity, this.OptionalName)
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
