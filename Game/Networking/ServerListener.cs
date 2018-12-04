/*
*                                       Players clicking on a server in the server list will be redirected to a game server and send a connection packet
*                                       This socket handles these incoming connections and create a new user to verify player authentication with auth server
*/
using System;
using System.Net;
using System.Net.Sockets;

using Serilog;

namespace Game.Networking
{
    class ServerListener
    {

        private readonly int bindPort;

        private Socket socket;

        public ServerListener(int port)
        {
            this.bindPort = port;
        }

        public bool Start()
        {
            try
            {
                Log.Information(string.Concat("Binding a socket listener to port: ", this.bindPort, "."));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, this.bindPort));
                socket.Listen(1);
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
                return true;
            }
            catch
            {
               Log.Fatal("Failed to bind a network socket to the port.");
               Log.Fatal("Is a server already running on this port?");
                return false;
            }
        }

        private void OnAcceptConnection(IAsyncResult iAr)
        {
            try
            {
                Socket s = socket.EndAccept(iAr);
                Entities.User usr = new Entities.User(s);
            }
            //     catch { }
            catch(Exception e)
            {
                throw new Exception(e.ToString());
            }
            if (socket != null)
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
        }
    }
}