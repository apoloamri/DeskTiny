using DTCore.Database.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public void OverwriteWithModel(object model)
        {
            //To do...
        }
    }
}
