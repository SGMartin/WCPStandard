/*
 *              Send to the authentication server to ask wether the incoming player is really authenticated or not
 * 
 * 
 */

using Core.Networking;

namespace Game.Networking.Packets.Internal
{
    /*
   class PlayerAuthorization : Core.Networking.OutPacket
    {
        public PlayerAuthorization(uint sessionId, uint id, string name)
            : base((ushort)PacketList.PlayerAuthentication, Constants.xOrKeyServerRecieve) //TODO: check these keys
        {
            Append((ushort)PlayerAuthorizationErrorCodes.Login);
            Append(sessionId);
            Append(id);
            Append(name);
        }
        public PlayerAuthorization(uint sessionId, uint id, string name, byte _accessLevel)
            : base((ushort)PacketList.PlayerAuthentication, Constants.xOrKeyServerRecieve)
        {
            Append((ushort)PlayerAuthorizationErrorCodes.Login);
            Append(sessionId);
            Append(id);
            Append(name);
            Append(_accessLevel);
        }

        public PlayerAuthorization(uint id)
            : base((ushort)PacketList.PlayerAuthentication, Constants.xOrKeyServerRecieve)
        {
            Append((ushort)PlayerAuthorizationErrorCodes.Logout);
            Append(id);
        }

        public PlayerAuthorization(Entities.User u)
            : base((ushort)PacketList.PlayerAuthentication, Constants.xOrKeyServerRecieve)
        {
            Append((ushort)PlayerAuthorizationErrorCodes.Update);
            Append(u.ID);


        }
    }*/
}