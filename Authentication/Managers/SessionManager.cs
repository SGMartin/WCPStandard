/*
        This manager keeps tracks of all active player sessions. A player session is created whenever an user makes a successful login.
        Sessions are removed on player logout or GameServer crash.
        
 */

using System;
using Serilog;
using System.Collections.Concurrent;

namespace Authentication.Managers
{
    class SessionManager
    {
        public readonly ConcurrentDictionary<uint, Entities.Session> Sessions;

        public SessionManager()
        {
            Sessions = new ConcurrentDictionary<uint, Entities.Session>();

        }

        public void Add(Entities.User u) {
            uint sessionId = 0;

            do {
                sessionId++;
            } while (Sessions.ContainsKey(sessionId));

            u.SetSession(sessionId);
            Sessions.TryAdd(sessionId, new Entities.Session(sessionId, u.ID, u.Name, u.DisplayName, u.AccessLevel));
        }

        public Entities.Session Get(uint sessionId) {
            Entities.Session session = null;
            if (Sessions.ContainsKey(sessionId)) {
                Sessions.TryGetValue(sessionId, out session);
            }
            return session;
        }

        public void Remove(uint sessionId) {
            Entities.Session session = null;
            if (Sessions.ContainsKey(sessionId)) {
                Sessions.TryRemove(sessionId, out session);
            }
        }

        private static SessionManager instance;
        public static SessionManager Instance { get { if (instance == null) { instance = new SessionManager(); } return instance; } }
    }
}