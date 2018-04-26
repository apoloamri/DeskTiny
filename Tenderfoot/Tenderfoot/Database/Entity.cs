﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tenderfoot.Tools.Extensions;

namespace Tenderfoot.Database
{
    public abstract class Entity
    {
        [Serial]
        [PrimaryKey]
        [NotNull]
        public int? id { get; set; }

        [NotNull]
        [Default("Now()")]
        public virtual DateTime? insert_time { get; set; }

        public List<string> GetColumns()
        {
            return this
                .GetType()
                .GetProperties()
                .Where(x =>
                {
                    if (x.GetCustomAttribute<NonTableColumnAttribute>(false) == null)
                    {
                        return true;
                    }

                    return false;
                })
                .Select(x => x.Name)
                .ToList();
        }

        public void SetValuesFromModel(object model)
        {
            var type = model.GetType();

            foreach (var property in type.GetProperties())
            {
                var thisProperty = this.GetType().GetProperty(property.Name.ToUnderscore());
                if (this.HasAttribute(thisProperty))
                {
                    thisProperty.SetValue(this, property.GetValue(model));
                }
            }
        }

        public void SetValuesFromDictionary(Dictionary<string, object> dictionary)
        {
            foreach (var item in dictionary)
            {
                var thisProperty = this.GetType().GetProperty(item.Key.ToUnderscore());
                if (this.HasAttribute(thisProperty))
                {
                    thisProperty.SetValue(this, item.Value);
                }
            }
        }

        public Dictionary<string, object> ToDictionary()
        {
            return this.ToDictionary();
        }

        private bool HasAttribute(PropertyInfo property)
        {
            return
                property != null &&
                property?.GetCustomAttribute<NonTableColumnAttribute>(false) == null;
        }
    }
}
