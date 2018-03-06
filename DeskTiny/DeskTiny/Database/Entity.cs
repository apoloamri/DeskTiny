using System;

namespace DeskTiny.Database
{
    public abstract class Entity
    {
        public abstract int? id { get; set; }
        public abstract DateTime? insert_time { get; set; }

        public void OverwriteWithModel(object model)
        {

        }
    }
}
