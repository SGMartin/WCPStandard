namespace Game.Networking.Handlers
{
    public class Ping : Networking.PacketHandler //TODO: THIS COULD BE BETTER
    {
        protected override void Process(Entities.User u)
        {
            if (u.Authorized)
                u.PingReceived();

            else
                u.Disconnect(); // Player not authorized - cheating?     
        }
    }
}
