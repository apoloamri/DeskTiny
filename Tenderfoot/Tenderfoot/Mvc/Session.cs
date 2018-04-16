using Tenderfoot.Database;
using Tenderfoot.DTSystem;
using Tenderfoot.Tools;
using Tenderfoot.Tools.Extensions;
using System;

namespace Tenderfoot.Mvc
{
    public static class Session
    {
        public static string AddSession(string sessionId)
        {
            if (sessionId.IsEmpty())
            {
                return null;
            }

            var session = Schemas.Sessions;
            
            session.Conditions.Where(
                session.Column(x => x.session_id), 
                Condition.EqualTo, 
                sessionId);

            session.Conditions.Where(
                session.Column(x => x.session_time), 
                Condition.GreaterThan, 
                DateTime.Now.AddMinutes(-Settings.Web.SessionTimeOut));

            var result = session.Select.Entity;

            if (result != null)
            {
                return result.session_key;
            }

            session.ClearConditions();

            var sessionKey = KeyGenerator.GetUniqueKey(64);
            
            session.Conditions.Where(
                session.Column(x => x.session_key), 
                Condition.EqualTo, 
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
            if (sessionId.IsEmpty() || sessionKey.IsEmpty())
            {
                return false;
            }

            var session = Schemas.Sessions;

            session.Conditions.Where(
                session.Column(x => x.session_id),
                Condition.EqualTo,
                sessionId);

            session.Conditions.Where(
                session.Column(x => x.session_key),
                Condition.EqualTo,
                sessionKey);

            session.Conditions.Where(
                session.Column(x => x.session_time),
                Condition.GreaterThan,
                DateTime.Now.AddMinutes(-Settings.Web.SessionTimeOut));
            
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
