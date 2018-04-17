using Tenderfoot.Database;

namespace TenderfootWebAPI.Library._Database
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
    }
}
