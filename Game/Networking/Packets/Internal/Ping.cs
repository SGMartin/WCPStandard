using Core.Networking;

namespace Game.Networking.Packets.Internal
    
{
    class Ping : OutPacket
    {
        public Ping()
            : base((ushort)PacketList.Ping, Constants.xOrKeyInternalRecieve)
        {
            Append(Constants.ERROR_OK);
            Append(System.DateTime.Now.Ticks);
            Append(0);
            Append(0);
            //TODO: add this stuff
         //   Append(Managers.UserManager.Instance.Sessions.Values.Count); // Player count
           // Append(Managers.ChannelManager.Instance.RoomCount); // Room Count
        }
    }
}