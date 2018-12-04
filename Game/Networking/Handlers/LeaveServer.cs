/*
 *                                     Self explanatory
 * 
 */

using System;
using Serilog;
namespace Game.Networking.Handlers
{
    class LeaveServer : Networking.PacketHandler
    {
        protected override void Process(Entities.User u)
        {
            if (!u.Authorized)
                return;

            u.Send(new Packets.LeaveServer());
            Log.Information("Player " + u.DisplayName + " with ID: " + u.ID.ToString() + " has left the server");
        }
    }
}
