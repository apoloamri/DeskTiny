using DTCore.Database;
using DTCore.Database.Enums;
using System;

namespace TestDeskTiny
{
    public static class DatabaseTest
    {
        public static void TestAll()
        {
            TestInsert();
            TestSelect();
            TestUpdateDelete();
        }

        private static void TestInsert()
        {
            var sessions = Schemas.Sessions;

            sessions.Entity.session_id = "user";
            sessions.Entity.session_key = "user";
            sessions.Entity.session_time = DateTime.Now;
            sessions.Insert();
        }

        private static void TestSelect()
        {
            var sessions = Schemas.Sessions;

            sessions.Conditions.Where(sessions.Column(x => x.session_id), Condition.EqualTo, "user");
            
            var result = sessions.Select.Entities;
        }
        
        private static void TestUpdateDelete()
        {
            var sessions = Schemas.Sessions;

            sessions.Commit(session =>
            {
                session.Entity.session_key = "updated";
                session.Conditions.Where(session.Column(x => x.session_id), Condition.EqualTo, "user");
                session.Update();

                session.Conditions.Where(session.Column(x => x.session_id), Condition.EqualTo, "user");
                session.Delete();
            });
        }
    }
}
