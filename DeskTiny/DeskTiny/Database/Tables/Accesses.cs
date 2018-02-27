namespace DeskTiny.Database.Tables
{
    public class Accesses
    {
        public long? id { get; set; }
        public string token { get; set; }
        public string token_secret { get; set; }
    }
}
