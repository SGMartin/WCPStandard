/*
 * 
 *                                                      When a player selects a GameServer to play, it sends a "login" packet
 *                                                      asking for User data such as inventory or equipment. Instead of responding right away,
 *                                                      this handler sends an internal packet to the Authentication server asking wether this player  
 *                                                      has an active session there or not.
 */                                                                         


using Game.Entities;

namespace Game.Networking.Handlers
{
    class Authorization : PacketHandler
    {
        protected override void Process(User u)
        {
            
            uint userId         = GetuInt(0);
            string username     = GetString(2);
            string displayname  = GetString(3);
            uint sessionId      = GetuInt(4); // Login session Id
            byte _accessLevel   = GetByte(7);

            if (userId > 0 && username.Length > 2 && displayname.Length > 2 && sessionId > 0)
            {
               if (Managers.UserManager.Instance.Add(sessionId, u)){
                     Program.AuthServer.Send(new Packets.Internal.PlayerAuthorization(sessionId, userId, username, _accessLevel));                
               }
                 else {
                  u.Send(new Packets.Authorization(Packets.Authorization.ErrorCodes.NormalProcedure));
                  u.Disconnect();
                }

            }
            else
            {
                u.Send(new Packets.Authorization(Packets.Authorization.ErrorCodes.NormalProcedure));
                u.Disconnect();
            }
            
        }
    }
}
