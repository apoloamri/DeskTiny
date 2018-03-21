using DTCore.Database;
using DTCore.Database.Enums;

namespace TestDeskTiny
{
    public static class DatabaseTest
    {
        public static void TestAll()
        {
            //TestInsert();
            TestSelect();
            //TestUpdate();
            //TestDelete();
        }

        private static void TestSelect()
        {
            var clients = Schemas.Clients;
            
            clients.Wherein.Which(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Wherein.LimitBy(5);
            
            var result = clients.Select.Entities;

            clients.ClearConditions();
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

            clients.Wherein.Which(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Update();
            clients.ClearEntity();
        }

        private static void TestDelete()
        {
            var clients = Schemas.Clients;

            clients.Wherein.Which(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Delete();
            clients.ClearEntity();
        }
    }
}
