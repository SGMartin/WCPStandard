/*

     Simple but essential packet used EVERY time a new connection is established. Both Auth and Game server use this packet implementation.

 */



namespace Core.Packets {

    using Networking;
    
    public class Connection : Core.Networking.OutPacket {
        public Connection(byte xOrKey)
            : base((ushort)PacketList.Connection, xOrKey) {
            Append(new System.Random().Next(111111111, 999999999));
            Append(77);
        }
    }
}