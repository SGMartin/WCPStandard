/*

        This class is instanced whenever an user attempts to connect to the Authentication server. 
        It manages session, basic user data andthe socket to communicate with the client.

 */

using Serilog;
using System;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;

namespace Authentication.Entities {
    public class User : Core.Entities.Entity {

        public bool Authorized { get { return this.isAuthorized; } set { } }
        public uint SessionID { get { return this.sessionId; } set { } }
        public string DisplayName{get {return this.displayName;} set{} }
        
        private Socket socket;
        private byte[] buffer = new byte[1024];
        private byte[] cacheBuffer = new byte[0];
        private uint packetCount = 0;

        private bool isDisconnect = false;
        private bool isAuthorized = false;

        private uint sessionId = 0;
        private string displayName = "Player";

        public User(Socket socket)
            : base(0, "Unknown", Core.GameConstants.Rights.Regular)
        {
            this.socket = socket;
            isDisconnect = false;
            this.socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
            Send(new Core.Networking.Packets.Connection(Core.Networking.Constants.xOrKeyClientSend));
        }

        public void OnAuthorize(uint id, string name, string displayname) {
            this.ID = id;
            this.Name = name;
            this.displayName = displayname;
            Managers.SessionManager.Instance.Add(this);
            this.isAuthorized = true;
            this.AccessLevel = Core.GameConstants.Rights.Regular;
        }

        public void OnAuthorize(uint id, string name, string displayname, Core.GameConstants.Rights rights)
        {
            this.ID = id;
            this.Name = name;
            this.displayName = displayname;
            this.AccessLevel = rights;
            Managers.SessionManager.Instance.Add(this);
            this.isAuthorized = true;          
        }

        public void UpdateDisplayname(string displayname)
        {
            this.displayName = displayname;
            Session s = Managers.SessionManager.Instance.Get(this.sessionId);
            if (s != null) {
                s.UpdateSessionDisplayName(displayname);
            }
        }

        public void SetSession(uint sessionId) {
            this.sessionId = sessionId;
        }

        private void OnDataReceived(IAsyncResult iAr) 
        {
            try 
            {
                int bytesReceived = socket.EndReceive(iAr);

                if (bytesReceived > 0) {
                    byte[] packetBuffer = new byte[bytesReceived];

                    // Decrypt the bytes with the xOrKey.
                    for (int i = 0; i < bytesReceived; i++) {
                        packetBuffer[i] = (byte)(this.buffer[i] ^ Core.Networking.Constants.xOrKeyClientRecieve);
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
                                        catch (Exception e) { Log.Error(e.ToString());}
                                    }
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

        public void Disconnect() {
            if (isDisconnect) return;
            isDisconnect = true;

            try { socket.Close(); } catch { }
        }

    
    }
}