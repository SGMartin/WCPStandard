/*
 *              Send to the authentication server to ask wether the incoming player is really authenticated or not
 * 
 * 
 */

namespace Game.Networking.Packets.Internal
{
    
   class PlayerAuthorization : Core.Networking.OutPacket
    {
        public PlayerAuthorization(uint sessionId, uint id, string name)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalRecieve)
        {
            Append((ushort)Core.Networking.ErrorCodes.Success);
            Append(sessionId);
            Append(id);
            Append(name);
        }
        public PlayerAuthorization(uint sessionId, uint id, string name, byte _accessLevel)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalRecieve)
        {
            Append((ushort)Core.Networking.ErrorCodes.Success);
            Append(sessionId);
            Append(id);
            Append(name);
            Append(_accessLevel);
        }

        public PlayerAuthorization(uint id)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalRecieve)
        {
            Append((ushort)Core.Networking.ErrorCodes.EndConnection);
            Append(id);
        }

        public PlayerAuthorization(Entities.User u)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalRecieve)
        {
            Append((ushort)Core.Networking.ErrorCodes.Update);
            Append(u.ID);


        }
    }
}