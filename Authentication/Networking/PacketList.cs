/*

            Packet List for the Authentication server: 
            
            - ServerList sends the whole gameserver list to the client after a successful login.

            - Nickname. It wasn't used in the 2008 ver. because the player's nickname was already defined when the account was created.
                        It is implemented for those who want to use it, although a web-based nickname would be preferable. Disabled by default.
                        There is not OUT packet for Nickname. The response to the incoming packet is a serverlist packet with different bytes.

            - Launcher is used when the client launcher attempts to connect to the Auth server. 
               The server responds with a packet containing PATCH,SUB, FORMAT etc... 
               The original packet is implemented in the code, for those who want to use it but disabled by default.
               A new launcher is planned, rendering the original useless.




 */


namespace Authentication.Networking 
{
    public enum PacketsList : ushort {
        ServerList = 0x1100,
        Nickname = 0x1101,
        Launcher = 0x1010
    }
}