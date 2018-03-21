﻿using DTCore.Database;
using DTCore.Database.Enums;
using DTCore.Tools;
using System;

namespace DTCore.WebApi
{
    public static class Session
    {
        public static string AddSession(string sessionId)
        {
            var session = Schemas.Sessions;
            
            session.Wherein.Which(
                session.Column(x => x.session_id), 
                Condition.EqualTo, 
                sessionId);

            session.Wherein.Which(
                session.Column(x => x.session_time), 
                Condition.GreaterThan, 
                DateTime.Now.AddMinutes(-ConfigurationBuilder.API.SessionTimeOut));

            var result = session.Select.Entity;

            if (result != null)
            {
                return result.session_key;
            }

            session.ClearConditions();

            var sessionKey = KeyGenerator.GetUniqueKey(64);
            
            session.Wherein.Which(
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
            var session = Schemas.Sessions;

            session.Wherein.Which(
                session.Column(x => x.session_id),
                Condition.EqualTo,
                sessionId);

            session.Wherein.Which(
                session.Column(x => x.session_key),
                Condition.EqualTo,
                sessionKey);

            session.Wherein.Which(
                session.Column(x => x.session_time),
                Condition.GreaterThan,
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
