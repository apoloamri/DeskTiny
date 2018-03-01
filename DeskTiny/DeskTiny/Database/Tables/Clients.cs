namespace DeskTiny.Database.Tables
{
    public class Clients : Entity
    {
        public long? id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
    }
}
