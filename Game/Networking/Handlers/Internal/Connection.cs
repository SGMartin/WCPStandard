/*
*       When a gameserver manages to connect to the authentication server, the auth sends a connection packet.
*       In response, this handler sends an authorization packet
*/
namespace Game.Networking.Handlers.Internal
{
    class Connection : PacketHandler
    {
        protected override void Process(AuthenticationClient s)
        {
            s.Send(new Packets.Internal.Authorization());
        }
    }
}