/*
 *                                                      This class contains one socket used between Authentication and Game server connections
 *                                                      Here packets to the Auth server are sent.
*/
using System;
using System.Linq;
using System.Net.Sockets;
using Game.Networking.Packets.Internal;
using Serilog;

namespace Game.Networking
{
    
    public class AuthenticationClient
    {
        public bool Authorized { get { return this._isAuthorized; } set { } }

        private Socket socket;
        private byte[] buffer = new byte[1024];
        private byte[] cacheBuffer = new byte[0];
        private uint packetCount = 0;
        private bool isDisconnect = false;
        public bool IsFirstConnect { get; private set; }
        private bool _isAuthorized = false;

        private byte serverId;

        private string ip;
        private int port;

        public AuthenticationClient(string ip, int port)
        {
            this.ip = ip;
            this.port = port;
        }

        public bool Connect()
        {
            try
            {
                IsFirstConnect = true;
                isDisconnect = false;
                Log.Information("Fetching the local ip address to: " + Config.SERVER_IP + "." + " Using port: " + port.ToString());
                Log.Information("Attempting to connect to the auth server.");
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(ip, port);
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
                Log.Information("Connection established with authentication server.");
                return true;
            }
            catch { Log.Fatal("failed to connect to the auth server."); Disconnect(IsFirstConnect); } //TODO: really fatal...? implement reconnection routine?
            return false;
        }

        public void OnAuthorize(byte serverId)
        {
            this.serverId = serverId;
            Config.SERVER_ID = serverId;
            _isAuthorized = true;
            Log.Information(string.Concat("Authorized as server: ", serverId, "."));
        }

        private void OnDataReceived(IAsyncResult iAr)
        {
            try
            {
                int bytesReceived = socket.EndReceive(iAr);

                if (bytesReceived > 0)
                {
                    byte[] packetBuffer = new byte[bytesReceived];

                    // Decrypt the bytes with the xOrKey.
                    for (int i = 0; i < bytesReceived; i++)
                    {
                        packetBuffer[i] = (byte)(this.buffer[i] ^ Core.Networking.Constants.xOrKeyInternalSend);
                    }

                    int oldLength = cacheBuffer.Length;
                    Array.Resize(ref cacheBuffer, oldLength + bytesReceived);
                    Array.Copy(packetBuffer, 0, cacheBuffer, oldLength, packetBuffer.Length);

                    int startIndex = 0; // Determs whre the bytes should split
                    for (int i = 0; i < cacheBuffer.Length; i++)
                    { // loop trough our cached buffer.
                        if (cacheBuffer[i] == 0x0A)
                        { // Found a complete packet
                            byte[] newPacket = new byte[i - startIndex]; // determ the new packet size.
                            for (int j = 0; j < (i - startIndex); j++)
                            {
                                newPacket[j] = cacheBuffer[startIndex + j]; // copy the buffer to the buffer of the new packet.
                            }
                            packetCount++;

                            // Handle the packet instantly.
                            Core.Networking.InPacket inPacket = new Core.Networking.InPacket(newPacket, this);
                            if (inPacket.Id > 0)
                            {
                                Networking.PacketHandler pHandler = Managers.PacketManager.Instance.FindInternal(inPacket);
                                if (pHandler != null)
                                {
                                    //try {
                                    pHandler.Handle(inPacket);
                                    //} catch { /*Disconnect(); }
                                }
                            }

                            startIndex = i + 1;
                        }
                    }

                    if (startIndex > 0)
                    {
                        byte[] fullCopy = cacheBuffer;
                        Array.Resize(ref cacheBuffer, (cacheBuffer.Length - startIndex));
                        for (int i = 0; i < (cacheBuffer.Length - startIndex); i++)
                        {
                            cacheBuffer[i] = fullCopy[startIndex + i];
                        }
                        fullCopy = null;
                    }

                    socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(OnDataReceived), null);
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

        public void Send(Core.Networking.OutPacket outPacket)
        {
            try
            {
                byte[] sendBuffer = outPacket.BuildEncrypted();
                socket.BeginSend(sendBuffer, 0, sendBuffer.Length, SocketFlags.None, new AsyncCallback(SendCallback), null);
            }
            catch
            {
                Disconnect();
            }
        }

        private void SendCallback(IAsyncResult iAr)
        {
            try
            {
                socket.EndSend(iAr);
            }
            catch
            {
                Disconnect();
            }
        }

        public void Disconnect()
        {
            this.Disconnect(false);
        }

        public void Disconnect(bool force)
        {
            if (IsFirstConnect)
                IsFirstConnect = false;

            if (isDisconnect) return;
            isDisconnect = true;

            try { socket.Close(); } catch { }

            if (!force)
                Connect();
        }
    }

}
