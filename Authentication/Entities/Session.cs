using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Authentication.Entities {
    class Session : Core.Entities.Entity {

        public uint DatabaseID { get; private set; }
        public uint SessionID { get; private set; }
        public bool IsActivated { get; private set; }
        public bool IsEnded { get; private set; }
        public byte ServerID { get; private set; }

        public DateTime Created { get; private set; }
        public DateTime Activated { get; private set; }


        public Session(uint sessionId, uint userId, string username, string displayname, byte _accessLevel) : base(userId, username, _accessLevel)
        {
            this.SessionID = sessionId;
            IsActivated = false;
            IsEnded = false;
        }


        public void Activate(byte serverId) {
            if (IsActivated) return;
            IsActivated = true;
            this.ServerID = serverId;
            Activated = DateTime.Now;

            lock (Program.sessionLock)
            {
                Program.onlinePlayers++;
                if (Program.onlinePlayers > Program.playerPeak)
                    Program.playerPeak = Program.onlinePlayers;
                Program.totalPlayers++;

                Managers.StatisticsManager.Instance.UpdatePlayerCount(Program.onlinePlayers, Program.playerPeak, Program.totalPlayers);
            }
        }

        public void End() {
           
            lock (Program.sessionLock)
            {
                Program.onlinePlayers--;
            }

            IsEnded = true;
            Managers.SessionManager.Instance.Remove(this.SessionID);
            Managers.StatisticsManager.Instance.UpdatePlayerCount(Program.onlinePlayers, Program.playerPeak, Program.totalPlayers);
        }

    }

}