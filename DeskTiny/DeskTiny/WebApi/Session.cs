using DeskTiny.Database;
using DeskTiny.Database.Enums;
using DeskTiny.Tools;
using System;

namespace DeskTiny.WebApi
{
    public static class Session
    {
        public static string AddSession(string sessionId)
        {
            var session = Schemas.Sessions;
            
            session.Conditions.AddWhere(
                nameof(session.Entity.session_id), 
                Condition.Equal, 
                sessionId);

            session.Conditions.AddWhere(
                nameof(session.Entity.session_time), 
                Condition.Greater, 
                DateTime.Now.AddMinutes(-ConfigurationBuilder.API.SessionTimeOut));

            var result = session.Select.Entity;

            if (result != null)
            {
                return result.session_key;
            }

            session.ClearConditions();

            var sessionKey = KeyGenerator.GetUniqueKey(64);
            
            session.Conditions.AddWhere(
                nameof(session.Entity.session_key), 
                Condition.Equal, 
                sessionKey);

            while (session.Count() > 0)
            {
                sessionKey = KeyGenerator.GetUniqueKey(64);
            }

            session.Entity.session_id = sessionId;
            session.Entity.session_key = sessionKey;
            session.Entity.session_time = DateTime.Now;

            session.Insert();

            return sessionKey;
        }

        public static bool IsSessionActive(string sessionId, string sessionKey)
        {
            var session = Schemas.Sessions;

            session.Conditions.AddWhere(
                nameof(session.Entity.session_id),
                Condition.Equal,
                sessionId);

            session.Conditions.AddWhere(
                nameof(session.Entity.session_key),
                Condition.Equal,
                sessionKey);

            session.Conditions.AddWhere(
                nameof(session.Entity.session_time),
                Condition.Greater,
                DateTime.Now.AddMinutes(-ConfigurationBuilder.API.SessionTimeOut));
            
            var count = session.Count();

            if (count > 0)
            {
                session.Entity.session_time = DateTime.Now;
                session.Update();

                return true;
            }

            return false;
        }
    }
}
