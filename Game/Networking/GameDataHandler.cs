/*
 *                              This is a special handler: packet 30000 is used for different purposes by the game: just one byte determines wether
 *                              the packet is a healing packet, a weapon change packet etc... Consider them "subpackets" each with its own handler.
 *                              Each "sub" handler stems from this class inheriting properties and data manipulation. Compare it to PacketHandler for
 *                              more information.
 */                                    
 /*
using System;
using System.Linq;
using System.Text;

namespace Game.Networking
{
    public class GameDataHandler
    {
        
        public Enums.GameSubs type;
        protected ushort errorCode = 1;
        protected byte roomSlot = 0;
        protected object handleLock = null;

        public bool updateLobby = false;
        public bool selfTarget = false;
        public bool respond = true;
        protected bool mapData = false;

        protected Core.Networking.InPacket packet;

        public Entities.Player Player { get; private set; }
        public Entities.Room Room { get; private set; }

        private string[] blocks;

        protected virtual void Handle() { }

        public GameDataHandler()
        {
            this.handleLock = new object();
        }

        public void Process(Entities.User u, Core.Networking.InPacket packet)
        {
            string[] blocks = packet.Blocks;
            ushort subId = 0;
            try
            {
                ushort.TryParse(blocks[3], out subId);
            }
            catch { subId = 0; }

            if (subId > 0 && Enum.IsDefined(typeof(Enums.GameSubs), subId))
            {

                this.type = (Enums.GameSubs)subId;

                lock (handleLock)
                {
                    this.packet = packet;
                    mapData = false;
                    updateLobby = false;
                    selfTarget = false;
                    respond = false;

                    this.blocks = new string[blocks.Length - 4];
                    Array.Copy(blocks, 4, this.blocks, 0, this.blocks.Length);

                    this.Room = u.LobbyState.Room;
                    Entities.Player p = null;
                    selfTarget = false;

                    try
                    {
                        this.Room.Players.TryGetValue(u.RoomSlot, out p);
                    }
                    catch { p = null; }

                    if (p != null)
                    {
                        this.Player = p;
                        roomSlot = p.Id;

                        try
                        {
                            Handle();
                        }
                        catch { respond = false; }

                        if (respond)
                        {

                            string[] packetData;
                            if (errorCode == 1)
                            {
                                packetData = new string[this.blocks.Length + 5];
                                packetData[0] = errorCode.ToString();
                                packetData[1] = roomSlot.ToString();
                                packetData[2] = Room.ID.ToString();
                                packetData[3] = blocks[2]; // 2 - 0
                                packetData[4] = ((ushort)type).ToString(); // Type

                                Array.Copy(this.blocks, 0, packetData, 5, this.blocks.Length);
                            }
                            else
                            {
                                packetData = new string[] { errorCode.ToString() };
                            }

                            // Generate packet buffer //
                            byte[] buffer = new Packets.GameData(packetData).BuildEncrypted();
                            if (errorCode > 1 || selfTarget)
                                u.Send(buffer);
                            else
                                Room.Send(buffer);

                            if (mapData)
                                u.Send(new Packets.MapData(Room));

                            if (updateLobby)
                            { // Send a update to the lobby :)
                                byte roomPage = (byte)Math.Floor((decimal)(Room.ID / 8));
                                var targetList = Managers.ChannelManager.Instance.Get(Room.Channel).Users.Select(n => n.Value).Where(n => n.RoomListPage == roomPage && n.Room == null);
                                if (targetList.Count() > 0)
                                {
                                    byte[] outBuffer = new Packets.RoomUpdate(Room, true).BuildEncrypted();
                                    foreach (Entities.User usr in targetList)
                                    {
                                        usr.Send(outBuffer);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                u.Disconnect(); // Wrong id?
            }
        }

        public void Set(byte index, string value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value;
        }

        public void Set(byte index, int value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, uint value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, short value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, ushort value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, byte value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, sbyte value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = value.ToString();
        }

        public void Set(byte index, bool value)
        {
            if (index < this.blocks.Length)
                this.blocks[index] = (value) ? "1" : "0";
        }

        public string GetString(byte index)
        {
            if (index < this.blocks.Length)
            {
                return this.blocks[index];
            }
            return string.Empty;
        }

        public bool GetBool(byte index)
        {
            if (index < this.blocks.Length)
            {
                return (byte.Parse(this.blocks[index]) > 0);
            }
            return false;
        }

        public int GetInt(byte index)
        {
            if (index < this.blocks.Length)
            {
                return int.Parse(this.blocks[index]);
            }
            return 0;
        }

        public uint GetuInt(byte index)
        {
            if (index < this.blocks.Length)
            {
                return uint.Parse(this.blocks[index]);
            }
            return 0;
        }

        public byte GetByte(byte index)
        {
            if (index < this.blocks.Length)
            {
                return byte.Parse(this.blocks[index]);
            }
            return 0;
        }

        public sbyte GetSByte(byte index)
        {
            if (index < this.blocks.Length)
            {
                return sbyte.Parse(this.blocks[index]);
            }
            return 0;
        }

        public short GetShort(byte index)
        {
            if (index < this.blocks.Length)
            {
                return short.Parse(this.blocks[index]);
            }
            return 0;
        }

        public ushort GetUShort(byte index)
        {
            if (index < this.blocks.Length)
            {
                return ushort.Parse(this.blocks[index]);
            }
            return 0;
        }

        public long GetLong(byte index)
        {
            if (index < this.blocks.Length)
            {
                return long.Parse(this.blocks[index]);
            }
            return 0;
        }

        public Core.Networking.InPacket GetIncPacket()
        {
            return packet;
        }
        

    
    }
}
*/