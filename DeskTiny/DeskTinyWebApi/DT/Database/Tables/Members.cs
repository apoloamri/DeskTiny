namespace DeskTinyWebApi.DT.Database.Tables
{
    public class Members : DeskTiny.Database.Entity
    {
        public long? id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
