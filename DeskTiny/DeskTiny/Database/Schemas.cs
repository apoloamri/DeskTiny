using DeskTiny.Database.System;
using DeskTiny.Database.Tables;

namespace DeskTiny.Database
{
    public static class Schemas
    {
        public static Repository<Affiliate> Affiliate() => new Repository<Affiliate>("affiliate_t");
        public static Repository<Accesses> Accesses() => new Repository<Accesses>("accesses");
        public static Repository<Clients> Clients() => new Repository<Clients>("clients");
    }
}
