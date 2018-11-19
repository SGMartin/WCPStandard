/*

    Internal packet used between Auth and Game servers. Sent by the Auth in response to a game server Authentication attempt. 
    On a succesful Authentication, the game server is added to the serverlist.

 */


namespace Authentication.Networking.Packets.Internal 
{
    class Authorize : Core.Networking.OutPacket {
        public Authorize(Core.Networking.ErrorCodes ErrorCode)
            : base((ushort)Core.Networking.PacketList.ServerAuthentication, Core.Networking.Constants.xOrKeyServerSend) {
            Append((ushort)ErrorCode);
        }

        public Authorize(byte serverId)
            : base((ushort)Core.Networking.PacketList.ServerAuthentication, Core.Networking.Constants.xOrKeyServerSend) {
                Append(Core.Networking.Constants.ERROR_OK);
                Append(serverId);
        }
    }
}