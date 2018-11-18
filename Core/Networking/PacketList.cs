/* A list of packets shared by both Auth and Game server. There are not actual packets, but an enumeration of packets IDs in HEX 0xblablabla */


namespace Core.Networking
{
   enum PacketList : ushort
   {
       ServerAuthorization = 0x1000,  // <- Internal packet a.k.a. used between the Auth and Game server
       Ping  = 0x1100,               //  <- Internal packet
       Connection = 0x1200,
       PlayerAuthorization = 0x1300
   }
}