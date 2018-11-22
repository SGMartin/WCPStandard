/*
*  This packet is sent to ask the authentication server for authorization
*/ 
using Core.Networking;

namespace Game.Networking.Packets.Internal
{
    class Authorization : OutPacket
    {
        public Authorization()
            : base((ushort)PacketList.ServerAuthentication, Constants.xOrKeyServerRecieve)
        {
            Append(Constants.ERROR_OK);
            Append(Config.SERVER_KEY);
            string serverName = Config.SERVER_NAME + " "; //Fixes a WarRock client bug: when ingame, the server name displays like this: NAMEserver.
            Append(serverName);
            Append(Config.SERVER_IP);
            Append((ushort)Constants.Ports.Game);
            Append((byte)Core.GameConstants.ServerTypes.Normal); //TODO: configure this
        }
    }
}