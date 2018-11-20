/*
 *              Sessions are used to keep track of logged on players, their current rights and userId.
 *              No matter what server the players are in, there is always a session associated with them
 *              When a GameServer asks for a player data before allowing him in, internal packets are sent
 *              with session info.
 */ 


using System;

namespace Authentication.Entities {
    class Session : Core.Entities.Entity
    {

        public uint SessionID { get; private set; }
        public byte ServerID { get; private set; }
        public uint UserId { get; private set; }
        public string UserName { get; private set; }
        public string UserDisplayName { get; private set; }

        public Core.GameConstants.Rights SessionRights { get; private set; }
        public bool IsActivated { get; private set; }
        public bool IsEnded { get; private set; }


        public DateTime Created { get; private set; }
        public DateTime Activated { get; private set; }

     

        public Session(uint sessionId, uint userId, string username, string displayName, Core.GameConstants.Rights rights) 
            : base(userId, username, rights)
        {
            this.SessionID       = sessionId;
            this.UserId          = userId;
            this.UserName        = username;
            this.SessionRights   = rights;
            this.UserDisplayName = displayName;

            IsActivated = false;
            IsEnded = false;
        }


        public void Activate(byte serverId)
        {
            if (IsActivated) return;
            IsActivated = true;
            this.ServerID = serverId;
            Activated = DateTime.Now;
            
        }

        public void UpdateSessionDisplayName(string newname)
        {
            UserDisplayName = newname;
        }


        public void End()
        {
            IsEnded = true;  
            Managers.SessionManager.Instance.Remove(this.SessionID);
        }

    }

}