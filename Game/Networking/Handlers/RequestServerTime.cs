using System;
using System.Collections.Generic;
using System.Text;

namespace Game.Networking.Handlers
{
    class RequestServerTime : PacketHandler
    {
        protected override void Process(Entities.User u)
        {
            int    versionID  = GetInt(1);
        //    string macAddress = GetString(2);

            if (versionID == 3) //TODO: is this usable?
            {
                u.Send(new Packets.ServerTime());
            }
            else
                u.Send(new Packets.ServerTime(Packets.ServerTime.ErrorCodes.DifferentClientVersion));
        }
    }
}
