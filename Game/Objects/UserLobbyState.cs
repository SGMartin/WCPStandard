using Game.Enums;
using Game.Entities;
using System;
using Serilog;

namespace Game.Objects
{
    public class UserLobbyState
    {
        public ChannelType Channel { get; private set; }

        public Room Room { get; private set; }

        public byte VisibleUserListPage { get; private set; } //which userlist page the user is looking at
        public byte RoomListPage        { get; private set; }
        public byte RoomSlot            { get; private set; }
        public int  LastRoomId          { get; private set; }

        public UserLobbyState()
        {
            Room       = null;
            LastRoomId = -1;
            RoomListPage = 0;
        }

        /*
        public void SetRoom(Room room, byte slot)
        {
            Room = room;
            RoomSlot = slot;

            if (room != null)
                LastRoomId = (int)room.ID;
        }

        public void SetChannel(ChannelType type)
        {
            if (Channel != type)
            {
                //ChannelManager.Instance.Remove(Channel, this); // Remove from old
                Channel = type; // change
                RoomListPage = 0;

                //TODO: Boolean and implement in User.cs
             //   if (!ChannelManager.Instance.Add(Channel, this))
               //     this.Disconnect(); // Failed to join :'(
            }
        }*/
    }
}
