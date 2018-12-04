using Game.Enums;
using Game.Managers;
using Game.Entities;
using System;
using Serilog;

namespace Game.Objects
{
    public class UserLobbyState
    {
        public ChannelType Channel { get; private set; }
        public User Owner { get; private set; }
        public Room Room { get; private set; }

        public byte VisibleUserListPage { get; private set; } //which userlist page the user is looking at
        public byte RoomListPage        { get; private set; }
        public byte RoomSlot            { get; private set; }
        public int  LastRoomId          { get;  set; }

        public UserLobbyState(Entities.User owner)
        {
            Owner      = owner;
            Room       = null;
            LastRoomId = -1;
            RoomListPage = 0;
        }


        public void SetRoom(Room room, byte slot)
        {
            Room = room;
            RoomSlot = slot;

            if (room != null)
                LastRoomId = (int)room.ID;
        }

        public void SetChannel(ChannelType newChannel)
        {
            if (Channel != newChannel)
            {
                ChannelManager.Instance.Remove(Channel, Owner); // Remove from old

                Channel = newChannel; // change
                RoomListPage = 0;

                //TODO: Boolean and implement in User.cs
                if (!ChannelManager.Instance.Add(Channel, Owner))
                   Owner.Disconnect(); // Failed to join :'(
            }
        }
    }
}
