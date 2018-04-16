using Tenderfoot.Tools;
using System;
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
        public virtual long? id { get; set; }

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

                if (thisProperty != null &&
                    thisProperty.GetCustomAttribute<NonTableColumnAttribute>(false) == null)
                {
                    thisProperty.SetValue(this, property.GetValue(model));
                }
            }
        }

        public void SetValuesFromDictionary(Dictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, object> ToDictionary()
        {
            return DictionaryClassConverter.ClassToDictionary(this);
        }

        private void SetValues(object model, object value)
        {
            throw new NotImplementedException();
        }
    }
}
