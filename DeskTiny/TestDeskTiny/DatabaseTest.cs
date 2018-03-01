using DeskTiny.Database;

namespace TestDeskTiny
{
    public static class DatabaseTest
    {
        public static void TestAll()
        {
            TestInsert();
            TestSelect();
            TestUpdate();
            TestDelete();
        }

        private static void TestSelect()
        {
            var clients = Schemas.Clients;

            clients.QueryConditions.AddWhere(nameof(clients.Entity.username), Condition.Equal, "username1");
            clients.QueryConditions.AddLimit(5);
            
            var result = clients.Select.Dictionaries;

            clients.ClearQueryConditions();
        }

        private static void TestInsert()
        {
            var clients = Schemas.Clients;

            clients.Entity.username = "username1";
            clients.Entity.password = "password1";
            clients.Insert();
            clients.ClearEntity();
        }

        private static void TestUpdate()
        {
            var clients = Schemas.Clients;

            clients.Entity.password = "updated";
            clients.QueryConditions.AddWhere(nameof(clients.Entity.username), Condition.Equal, "username1");
            clients.Update();
            clients.ClearEntity();
        }

        private static void TestDelete()
        {
            var clients = Schemas.Clients;

            clients.QueryConditions.AddWhere(nameof(clients.Entity.username), Condition.Equal, "username1");
            clients.Delete();
            clients.ClearEntity();
        }
    }
}
