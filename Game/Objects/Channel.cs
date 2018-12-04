/*
 * 
 *                      Channel.cs keeps track of user and room count for a given channel and sends a RoomUpdate packet when necessary
 * 
 * 
 */ 

using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;

using Game.Enums;

namespace Game.Objects
{
    public class Channel
    {
        public ChannelType Type { get; private set; }
        public ConcurrentDictionary<uint, Entities.User> Users { get; private set; }
        public ConcurrentDictionary<uint, Entities.Room> Rooms { get; private set; }

        private HashSet<int> takenRoomIds;

        private object scanLock;

        public Channel(ChannelType type)
        {
            this.Type = type;
            this.Users = new ConcurrentDictionary<uint, Entities.User>();
            this.Rooms = new ConcurrentDictionary<uint, Entities.Room>();
            this.takenRoomIds = new HashSet<int>();
            this.takenRoomIds.Clear();
            this.scanLock = new object();
        }

        public bool Add(Entities.User u)
        {
            if (!Users.ContainsKey(u.ID))
                return Users.TryAdd(u.ID, u);
            return false;
        }

        public void Remove(Entities.User u)
        {
            Entities.User usr;
            if (u.LobbyState.Room != null)
                u.LobbyState.Room.Remove(u);

            if (Users.ContainsKey(u.ID))
                Users.TryRemove(u.ID, out usr);
        }

        public bool Add(Entities.Room r)
        {

            if (!Rooms.ContainsKey(r.ID))
                return Rooms.TryAdd(r.ID, r);

            return false;
        }

        public void Remove(Entities.Room r)
        {
            Entities.Room rm;
            if (Rooms.ContainsKey(r.ID))
            {
                Rooms.TryRemove(r.ID, out rm);

                byte roomPage = (byte)Math.Floor((decimal)(r.ID / 8));
                var targetList = this.Users.Select(n => n.Value).Where(n => n.LobbyState.RoomListPage == roomPage && n.LobbyState.Room == null);
                if (targetList.Count() > 0)
                {
             //       byte[] outBuffer = new Packets.RoomUpdate(r.ID).BuildEncrypted();

                    foreach (Entities.User u in Managers.UserManager.Instance.Sessions.Values)
                    {
                        if (u.LobbyState.LastRoomId == r.ID)
                            u.LobbyState.LastRoomId = -1;

                      //  else
                        //    u.Send(outBuffer);

                    }
                }
            }

            lock (scanLock)
            {
                if (takenRoomIds.Contains((int)r.ID))
                    takenRoomIds.Remove((int)r.ID);
            }

        }

        public void ForceFreeSlot(int roomSlot)
        {
            if (takenRoomIds.Contains(roomSlot))
                takenRoomIds.Remove(roomSlot);
        }

        public int GetOpenRoomID()
        {
            int openId = 0;
            lock (scanLock)
            {
                while (takenRoomIds.Contains(openId))
                {
                    openId++;
                }
                takenRoomIds.Add(openId);
            }
            return openId;
        }
    }
}
