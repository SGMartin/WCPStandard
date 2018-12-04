/*
 * 
 *                                                  Ping packets are constantly sent by the gameserver once the player successfully logs in.
 *                                                  Instead of responding right away, some operations are made before by the User.class                            
 */ 

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
