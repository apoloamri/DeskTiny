using System;
using DTCore.Database;
using DTCore.Database.Enums;

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
            TestJoinSelect();
        }
        
        private static void TestSelect()
        {
            var clients = Schemas.Clients;
            
            clients.Conditions.Where(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Conditions.LimitBy(5);
            
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

            clients.Conditions.Where(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Update();
            clients.ClearEntity();
        }

        private static void TestDelete()
        {
            var clients = Schemas.Clients;

            clients.Conditions.Where(
                clients.Column(x => x.username), 
                Condition.EqualTo, 
                "username1");

            clients.Delete();
            clients.ClearEntity();
        }

        private static void TestJoinSelect()
        {
            var clients = Schemas.Clients;
            var accesses = Schemas.Accesses;

            clients.Relate(Join.INNER, accesses,
                clients.Relation(accesses.Column(x => x.token), clients.Column(x => x.username)));

            var dictionaries = clients.Select.Dictionaries;
        }
    }
}
