using System;
using System.Collections.Concurrent;

using Game.Entities;

namespace Game.Managers
{
    class UserManager
    {

        public readonly ConcurrentDictionary<uint, Entities.User> Sessions;
        private int playerPeak = 0;

        public UserManager()
        {
            this.Sessions = new ConcurrentDictionary<uint, Entities.User>();
        }

        public bool Add(uint sessionId, Entities.User u)
        {
            if (!Sessions.ContainsKey(sessionId))
            {
                u.SetSession(sessionId);
                if (Sessions.TryAdd(sessionId, u))
                {
                    if (Sessions.Count > playerPeak)
                        playerPeak = Sessions.Count;
                    return true;
                }
            }
            return false;
        }

        public User Get(uint sessionId)
        {
            User u = null;
            if (Sessions.ContainsKey(sessionId))
            {
                try { Sessions.TryGetValue(sessionId, out u); }
                catch { u = null; }

            }
            return u;
        }

        public void Remove(uint sessionId)
        {
            if (Sessions.ContainsKey(sessionId))
            {
                User u = null;
                Sessions.TryRemove(sessionId, out u);

                if (u != null)
                {
                    if (u.Authorized)
                    {
                        // SAVE THE PLAYER DATA //
                      //  string query = string.Concat("UPDATE user_details SET kills = '", u.Kills, "', deaths = '", u.Deaths, "', headshots = '", u.Headshots, "', xp = '", u.XP, "', play_time = '0', rounds_played = '", u.RoundsPlayed, "', bombs_planted = '", u.BombsPlanted, "', bombs_defused = '", u.BombsDefused, "', flags_taken = '", u.FlagsTaken, "', wins = '", u.Wins, "', losses = '", u.Losses, "', vehicles_destroyed = '", u.VehiclesDestroyed, "' WHERE id = ", u.ID, ";");
                        //Databases.Game.Query(query);

                        //Save session end
                  //      string query2 = string.Concat("UPDATE sessions SET session_end ='", System.DateTime.Now.ToString("yyyyMMddHHmmss"), "', expired = '1' WHERE expired = '0' AND sessionid = ", u.SessionID, ";");
                    //    Databases.Game.Query(query2);
                    }
                }
                // TELL THE AUTH SERVER THAT THE SESSION IS EXPIRED //
                Program.AuthServer.Send(new Networking.Packets.Internal.PlayerAuthorization(sessionId));
            }
        }

        public int Peak
        {
            get
            {
                return this.playerPeak;
            }
        }

        public Entities.User GetUser(string _displayName)
        {
            Entities.User User = null;

            foreach (Entities.User pUser in Sessions.Values)
            {
                if (pUser.DisplayName == _displayName)
                    User = pUser;
            }

            return User;
        }
        private static UserManager instance = null;
        public static UserManager Instance { get { if (instance == null) instance = new UserManager(); return instance; } set { } }
    }
}