/*
 *                                          One of the earliest packets sent by the client when attempting to connect to the game server. It asks for the server time
 *                                          and provides some useful information such as versionID and macAddress
 */ 

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
