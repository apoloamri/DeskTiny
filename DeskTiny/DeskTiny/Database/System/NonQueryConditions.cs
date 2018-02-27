using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DeskTiny.Tools;

namespace DeskTiny.Database.System
{
    public class NonQueryConditions
    {
        private string OptionalName = "nq_";
        internal string ColumnNames { get; set; }
        internal string ColumnParameters { get; set; }
        internal string ColumnValues { get; set; }
        internal Dictionary<string, object> EntityDictionary { get; set; } = new Dictionary<string, object>();

        internal void CreateColumnParameters(object entity)
        {
            var properties = 
                entity
                .GetType()
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetValue(entity) != null)?
                .ToArray();
            
            this.ColumnNames = string.Join(", ", properties?.Select(x => x.Name));
            this.ColumnParameters = string.Join(", ", properties?.Select(x => $":{this.OptionalName}{x.Name}"));
            this.ColumnValues = string.Join(", ", properties?.Select(x => { return $"{x.Name} = :{this.OptionalName}{x.Name}"; }));
            this.EntityDictionary =
                DictionaryClassConverter.ClassToDictionary(entity, this.OptionalName)
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, x => x.Value);
        }
    }
}
