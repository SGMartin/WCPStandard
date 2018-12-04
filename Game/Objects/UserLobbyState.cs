/*
 * 
 *                                          Lobby related user data and methods are stored here.
 * 
 */ 

using System.Collections;

using Game.Entities;
using Game.Enums;
using Game.Managers;



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
            VisibleUserListPage = 0;
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

                if (!ChannelManager.Instance.Add(Channel, Owner))
                   Owner.Disconnect(); // Failed to join :'(
            }
        }

        public void UpdateUserList() //uses default page
        {
            ArrayList UserList = new ArrayList();

            foreach (User User in UserManager.Instance.Sessions.Values) //CP1  userlist reports all users despite their channel
                UserList.Add(User);

            Owner.Send(new Networking.Packets.UserList(VisibleUserListPage, UserList));
        }

        public void UpdateUserList(byte userListPage)
        {
            VisibleUserListPage = userListPage;

            ArrayList UserList = new ArrayList();

            foreach (User User in UserManager.Instance.Sessions.Values) //CP1  userlist reports all users despite their channel
                UserList.Add(User);

            Owner.Send(new Networking.Packets.UserList(VisibleUserListPage, UserList));
        }
    }
}
