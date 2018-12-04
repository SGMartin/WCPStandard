using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using System.Net;
using System.Net.Sockets;

using Serilog;

using Core;
using Core.Databases;

using Game.Networking;
using Game.Objects;

namespace Game.Entities
{
    public class User : Core.Entities.Entity
    {

      
        public IPEndPoint RemoteEndPoint;
        public IPEndPoint LocalEndPoint;

        public ushort RemotePort;
        public ushort LocalPort;

        public uint Ping { get; private set; }
        public bool Authorized { get; private set; }
        public uint SessionID { get; private set; }

        //public ChannelType Channel { get; private set; }


        //user related objects
        public string DisplayName { get; private set; }
        public UserStats Stats           { get; private set; }
        public UserLobbyState LobbyState { get; private set; }
        public ulong XP { get; private set; }
        public uint Money { get; private set; }


        //TODO: Inventory stuff here

        //Premium related  
        public Enums.Premium Premium { get; private set; }
        public ulong PremiumExpireDate { get; private set; }
        public long PremiumTimeInSeconds { get; private set; }

        private Socket _socket;
        private byte[] _buffer         = new byte[1024];
        private byte[] _cacheBuffer    = new byte[0];
        private bool   _isDisconnected = false;
        private uint   _packetCount    = 0;


        private DateTime lastPingTime = DateTime.Now;
        private object pingLock = new object();
        private bool   pingOk = true;

        public User(Socket socket):base(0,"Unknown", GameConstants.Rights.Blocked)
        {
            Ping = 0;
            _isDisconnected = false;

             LobbyState = new UserLobbyState(this);
             Stats = new UserStats();

            _socket = socket;
            _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);

            Send(new Core.Networking.Packets.Connection(Core.Networking.Constants.xOrKeyClientSend));
        }

        public async void OnAuthorize(uint id, string displayName, byte accessLevel)
        {
            ID          = id;
            DisplayName = displayName;
            AccessLevel = (GameConstants.Rights)accessLevel;


            //Loading basic user data: xp, money, premium
            bool userDataRetrieved = await GetUserData(ID);

             if(!userDataRetrieved)
            {
                Send(new Networking.Packets.Authorization(Networking.Packets.Authorization.ErrorCodes.BadSynchronization));
                return;
            }

            //Wait for stats to load
             await Stats.GetDataBaseUserStats(ID);

            //TODO: Load inventory

            Authorized = true;

            //TODO: send authorization packet
            Send(new Networking.Packets.Authorization(this));
            SendPing();

            //TODO: update userlist
        }

        public void SetSession(uint sessionId)
        {
            if (sessionId > 0)
            {
                SessionID = sessionId;
            }
            else
            {
                this.Disconnect();
            }
        }

        public void SendPing()
        {
            lock (pingLock)
            {
                if (!pingOk)
                {
                    Disconnect();
                    Log.Warning("Could not send ping to player: " + DisplayName);
                    return;
                }

               // UpdatePremiumState();

                pingOk = false;
                Send(new Packets.Ping(this));
            }
        }

        public void PingReceived()
        {
            lock (pingLock)
            {
                this.pingOk = true;
                TimeSpan pingDiff = DateTime.Now - this.lastPingTime;
                this.Ping = (uint)pingDiff.TotalMilliseconds;
            }
        }

    
        public void Disconnect()
        {
            _isDisconnected = true;
            _socket.Close();
        }

        public void Send(byte[] sendBuffer)
        {
            try
            {
                _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Send(Core.Networking.OutPacket outPacket)
        {
            try
            {
                byte[] sendBuffer = outPacket.BuildEncrypted();
                _socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
            catch
            {
              Disconnect();
            }
        }

        private async Task<bool> GetUserData(uint userID)
        {
            List<object> BasicUserData = new List<object>();

            try
            {
                using (Database DB = new Database(Config.GAME_CONNECTION))
                {
                    BasicUserData = await DB.AsyncGetRowFromTable(
                        new string[] { "xp", "money", "premium", "premium_expiredate" }, "users",
                        new Dictionary<string, object>() { { "ID", ID } });

                    if (BasicUserData.Count > 0)
                        return true;
                }
            }
            catch (Exception e)
            {
                Log.Error(e.ToString());
            }
            return false;
        }


        private void SendCallback(IAsyncResult iAr)
        {
            try
            {
                _socket.EndSend(iAr);
            }
            catch
            {
                Disconnect();
            }
        }
        
        private void OnDataReceived(IAsyncResult iAr)
        {
            try
            {

                int bytesReceived = _socket.EndReceive(iAr);

                if (bytesReceived > 0)
                {
                    byte[] packetBuffer = new byte[bytesReceived];

                    // Decrypt the bytes with the xOrKey.
                    for (int i = 0; i < bytesReceived; i++)
                    {
                        packetBuffer[i] = (byte)(_buffer[i] ^ Core.Networking.Constants.xOrKeyClientRecieve);
                    }

                    int oldLength = _cacheBuffer.Length;
                    Array.Resize(ref _cacheBuffer, oldLength + bytesReceived);
                    Array.Copy(packetBuffer, 0, _cacheBuffer, oldLength, packetBuffer.Length);

                    int startIndex = 0; // Determs where the bytes should split
                    for (int i = 0; i < _cacheBuffer.Length; i++)
                    { // loop trough our cached buffer.
                        if (_cacheBuffer[i] == 0x0A)
                        { // Found a complete packet
                            byte[] newPacket = new byte[i - startIndex]; // determ the new packet size.
                            for (int j = 0; j < (i - startIndex); j++)
                            {
                                newPacket[j] = _cacheBuffer[startIndex + j]; // copy the buffer to the buffer of the new packet.
                            }
                            _packetCount++;
                            // Instant handeling
                            Core.Networking.InPacket inPacket = new Core.Networking.InPacket(newPacket, this);
                            if (inPacket != null)
                            {
                                if (inPacket.Id > 0)
                                {
                                    Networking.PacketHandler pHandler = Managers.PacketManager.Instance.FindExternal(inPacket);
                                    if (pHandler != null)
                                    {
                                        try
                                        {
                                            pHandler.Handle(inPacket);
                                        }
                                        catch (Exception e) { Log.Error(e.ToString()); }
                                    }
                                }
                            }
                            // Increase start index.
                            startIndex = i + 1;
                        }
                    }

                    if (startIndex > 0)
                    {
                        byte[] fullCopy = _cacheBuffer;
                        Array.Resize(ref _cacheBuffer, (_cacheBuffer.Length - startIndex));
                        for (int i = 0; i < (_cacheBuffer.Length - startIndex); i++)
                        {
                            _cacheBuffer[i] = fullCopy[startIndex + i];
                        }
                        fullCopy = null;
                    }
                    _socket.BeginReceive(_buffer, 0, _buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                }
                else
                {
                    Disconnect();
                }
            }
            catch
            {
                Disconnect();
            }
        }

    

    }
}
