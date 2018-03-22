using DTCore.Database.Attributes;
using DTCore.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DTCore.Database
{
    public abstract class Entity
    {
        [Serial]
        [PrimaryKey]
        [NotNull]
        public virtual int? id { get; set; }

        [NotNull]
        [Default(DefaultFunctions.Now)]
        public virtual DateTime? insert_time { get; set; }

        public List<string> GetColumns()
        {
            return this.GetType().GetProperties().Select(x => x.Name).ToList();
        }

        public void SetValuesFromModel(object model)
        {
            throw new NotImplementedException();
        }

        public void SetValuesFromDictionary(Dictionary<string, object> dictionary)
        {
            throw new NotImplementedException();
        }

        private void SetValues(object model, object value)
        {
            throw new NotImplementedException();
        }
    }
}
