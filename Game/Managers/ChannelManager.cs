using System;
using System.Linq;
using System.Collections.Concurrent;

using Game.Enums;
using Game.Objects;

namespace Game.Managers
{
    public class ChannelManager
    {
        public readonly ConcurrentDictionary<ChannelType, Channel> channels;

        public ChannelManager()
        {
            channels = new ConcurrentDictionary<ChannelType, Channel>();
            InitializeChannel(ChannelType.CQC);
            InitializeChannel(ChannelType.Urban_Ops);
            InitializeChannel(ChannelType.Battle_Group);
        }

        private void InitializeChannel(ChannelType channel)
        {
            if (!channels.ContainsKey(channel))
                channels.TryAdd(channel, new Channel(channel));
        }

        public bool Add(ChannelType channel, Entities.User u)
        {
            if (channel > ChannelType.None)
                return channels[channel].Add(u);
            return false;
        }

        public void Remove(ChannelType channel, Entities.User u)
        {
            if (channel > ChannelType.None)
                channels[channel].Remove(u);
        }

        public Channel Get(ChannelType type)
        {
            Channel channel = null;
            if (channels.ContainsKey(type))
                channels.TryGetValue(type, out channel);
            return channel;
        }

        public void Send(ChannelType type, byte[] data)
        {
            Channel channel = null;
            if (channels.ContainsKey(type))
            {
                if (channels.TryGetValue(type, out channel))
                {
                    foreach (Entities.User u in channels[type].Users.Values)
                    {
                        u.Send(data);
                    }
                }
            }
        }

        public void SendAll(byte[] data)
        {
            foreach (Channel channel in channels.Values)
            {
                foreach (Entities.User u in channel.Users.Values)
                {
                    u.Send(data);
                }
            }
        }

        public void SendLobby(ChannelType type, byte[] data)
        {
            Channel channel = null;
            if (channels.ContainsKey(type))
            {
                if (channels.TryGetValue(type, out channel))
                {
                    var players = channels[type].Users.Select(n => n.Value).Where(n => n.LobbyState.Room == null);
                    foreach (Entities.User u in players)
                        u.Send(data);
                }
            }
        }

        public void SendAllLobbies(byte[] data)
        {
            foreach (Channel channel in channels.Values)
            {
                var players = channel.Users.Select(n => n.Value).Where(n => n.LobbyState.Room == null);
                foreach (Entities.User u in players)
                    u.Send(data);
            }
        }

        public int RoomCount
        {
            get
            {
                int count = 0;
                foreach (Channel channel in channels.Values)
                    count += channel.Rooms.Count;
                return count;
            }
        }

        private static ChannelManager instance = null;
        public static ChannelManager Instance { get { if (instance == null) instance = new ChannelManager(); return instance; } set { } }
    }
}
