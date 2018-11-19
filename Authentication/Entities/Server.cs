/*
        Game server class. Whenever a server attempts to connect to the Authentication, a new instance of this class is created.
        Authoriced servers are shown to the players when they log successfully. This class stores not only the name, but also
        the IP and port of said game servers to be sent to the players.
 */


using System;
using System.Net;
using System.Net.Sockets;

using Core;


namespace Authentication.Entities {
    public class Server : Core.Entities.Entity {

        //Public properties of every server. They are self-explanatory
        public bool Authorized { get { return this.isAuthorized; } set { } }
        public ushort TotalPlayerCount { get { return playerCount; } private set { } }
        public ushort TotalRoomCount { get { return roomCount; } private set { } }
        public string IP { get { return this.ip; } set { } }

        //Normal, clan, testing etc...
        public GameConstants.ServerTypes Type { get { return this.serverType; } set { } }

        public string ServerName {get {return this.serverName;}set{} }


        // Private server variables.

        private GameConstants.ServerTypes serverType = GameConstants.ServerTypes.Normal; //by default, set server to normal
        private string ip;
        private int port;
        private string serverName = "WCPS Server";
        private Socket socket;
        private byte[] buffer = new byte[1024];
        private byte[] cacheBuffer = new byte[0];
        private uint packetCount = 0;

        private ushort playerCount = 0;
        private ushort roomCount   = 0;

        private bool isDisconnect = false;
        private bool isAuthorized = false;

        public Server(Socket socket)
            : base(0, "Unknown", 1) {
            this.socket = socket;
            isDisconnect = false;
            this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
            Send(new Core.Networking.Packets.Connection(Core.Networking.Constants.xOrKeyServerSend));
        }

        public void OnAuthorize(byte id, string name, string ip, int port, Core.GameConstants.ServerTypes type) {
            this.ID = (uint)id;
            this.Name = string.Concat("GameServer", id); // TODO: Is this really used ANYWHERE????
            this.serverName = name;
            this.ip = ip;
            this.port = port;
            this.serverType = type;
            this.isAuthorized = true;
        }

        private void OnDataReceived(IAsyncResult iAr) {
            try {
                int bytesReceived = socket.EndReceive(iAr);

                if (bytesReceived > 0) {
                    byte[] packetBuffer = new byte[bytesReceived];

                    // Decrypt the bytes with the xOrKey.
                    for (int i = 0; i < bytesReceived; i++) {
                        packetBuffer[i] = (byte)(this.buffer[i] ^ Core.Networking.Constants.xOrKeyServerRecieve);
                    }

                    int oldLength = cacheBuffer.Length;
                    Array.Resize(ref cacheBuffer, oldLength + bytesReceived);
                    Array.Copy(packetBuffer, 0, cacheBuffer, oldLength, packetBuffer.Length);

                    int startIndex = 0; // Determs whre the bytes should split
                    for (int i = 0; i < cacheBuffer.Length; i++) { // loop trough our cached buffer.
                        if (cacheBuffer[i] == 0x0A) { // Found a complete packet
                            byte[] newPacket = new byte[i - startIndex]; // determ the new packet size.
                            for (int j = 0; j < (i - startIndex); j++) {
                                newPacket[j] = cacheBuffer[startIndex + j]; // copy the buffer to the buffer of the new packet.
                            }
                            packetCount++;

                            // Handle the packet instantly.
                            Core.Networking.InPacket inPacket = new Core.Networking.InPacket(newPacket, this);
                           //TODO: REMOVE ThIS ServerLogger.Instance.AppendPacket(newPacket);
                            if (inPacket.Id > 0) {
                                Networking.PacketHandler pHandler = Managers.PacketManager.Instance.FindInternal(inPacket);
                                if (pHandler != null) {
                                    try 
                                    {
                                        pHandler.Handle(inPacket);
                                    } catch { /* TODO: LOG THIS Disconnect();*/ }
                                }
                            }

                            startIndex = i + 1;
                        }
                    }

                    if (startIndex > 0) {
                        byte[] fullCopy = cacheBuffer;
                        Array.Resize(ref cacheBuffer, (cacheBuffer.Length - startIndex));
                        for (int i = 0; i < (cacheBuffer.Length - startIndex); i++) {
                            cacheBuffer[i] = fullCopy[startIndex + i];
                        }
                        fullCopy = null;
                    }


                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                } else {
                    Disconnect();
                }
            } catch {
                Disconnect();
            }
        }

        public void Send(Core.Networking.OutPacket outPacket) {
            try {
                byte[] sendBuffer = outPacket.BuildEncrypted();
                socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            } catch {
                Disconnect();
            }
        }

        private void SendCallback(IAsyncResult iAr) {
            try {
                socket.EndSend(iAr);
            } catch {
                Disconnect();
            }
        }

        public void AddPlayers(ushort currentPlayerCount)
        {
            this.playerCount = currentPlayerCount;
        }

        public void AddRooms(ushort currentCount)
        {
            this.roomCount = currentCount;
        }
        public void Disconnect() {
            if (isDisconnect) return;
            isDisconnect = true;

            if (ID > 0)
            {     
                //Flush player sessions associated with this server
                foreach(Entities.Session Session in Managers.SessionManager.Instance.Sessions.Values)
                {
                    if (Session.IsActivated && Session.ServerID == ID)
                        Session.End();
                }
                
                //Remove server
                Managers.ServerManager.Instance.Remove((byte)ID);
            }
            try { socket.Close(); } catch { }
        }

    }
}