using Tenderfoot.Database;
using Tenderfoot.TfSystem;
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
            session.Case.Where(session._(x => x.session_id), Is.EqualTo, sessionId);
            session.Case.Where(session._(x => x.session_time), Is.GreaterThan, DateTime.Now.AddMinutes(-TfSettings.Web.SessionTimeOut));

            var result = session.Select.Entity;

            if (result != null)
            {
                return result.session_key;
            }

            session.ClearCase();

            var sessionKey = KeyGenerator.GetUniqueKey(64);
            session.Case.Where(session._(x => x.session_key), Is.EqualTo, sessionKey);

            while (session.Count > 0)
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
            session.Case.Where(session._(x => x.session_id), Is.EqualTo, Encryption.Decrypt(sessionId));
            session.Case.Where(session._(x => x.session_key), Is.EqualTo, sessionKey);
            session.Case.Where(session._(x => x.session_time), Is.GreaterThan, DateTime.Now.AddMinutes(-TfSettings.Web.SessionTimeOut));
            
            var count = session.Count;
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
