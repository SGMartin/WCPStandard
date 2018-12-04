using System;
using System.Collections;
using System.Linq;
using Game.Enums;

namespace Game.Networking.Handlers
{
    class ChangeChannel : PacketHandler
    {
        protected override void Process(Entities.User u)
        {
            if (u.Authorized)
            {
                sbyte target = GetSByte(0);
                if (target >= 0 && target <= Core.GameConstants.maxChannelsCount)
                {
                    if (Enum.IsDefined(typeof(ChannelType), target))
                    {
                        u.LobbyState.SetChannel((ChannelType)target);
                        u.Send(new Packets.ChangeChannel(u.LobbyState.Channel));

                        var result = Managers.ChannelManager.Instance.Get(u.LobbyState.Channel).Rooms.Select(n => n.Value)
                            .Where(n => n.ID >= (uint)(8 * u.LobbyState.RoomListPage) && n.ID < (uint)(8 * (u.LobbyState.RoomListPage + 1))).OrderBy(n => n.ID);

                  //      u.Send(new Packets.RoomList(u.LobbyState.RoomListPage, new ArrayList(result.ToArray())));

                    }
                    else
                    {
                        u.Disconnect(); // Channel is not defined?
                    }
                }
                else
                {
                    u.Disconnect(); // Channel is out of range.
                }
            }
            else
            {
                u.Disconnect(); // Unauthorized user.
            }
        }
    }
}
