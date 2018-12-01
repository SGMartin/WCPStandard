/*

            This packet is used by the Authentication server to respond to the game server when it asks wether the incoming connection
            is authorized or not. Usually, incoming GameServer connections are already-authorized players redirected from the auth
            to their desired game server. Think of this packet as a security measure.

            See also: wiki

 */



//using Core.Networking;

namespace Authentication.Networking.Packets.Internal {
    class PlayerAuthentication : Core.Networking.OutPacket {

        public PlayerAuthentication()
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalSend) {
            
        }

        public PlayerAuthentication(Entities.Session session)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalSend) {
            
                Append((ushort)Core.Networking.ErrorCodes.Success);
                Append(session.SessionID);
                Append(session.ID);
                Append(session.Name);
                Append(session.UserDisplayName);
                Append((byte)session.AccessLevel);       
        }

        public PlayerAuthentication(Core.Networking.ErrorCodes errorCode, uint targetId)
            : base((ushort)Core.Networking.PacketList.PlayerAuthentication, Core.Networking.Constants.xOrKeyInternalSend) {
                Append((ushort)errorCode);
                Append(targetId);
        }
    }
}