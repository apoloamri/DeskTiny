using Tenderfoot.Database;

namespace TenderfootStorybook.Library.Database
{
    public class _DB : Schemas
    {
        public static Schema<Members> Members => CreateTable<Members>("members");
    }
}
