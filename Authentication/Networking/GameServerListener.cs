/*
            This socket works exactly like ServerListener, but it listens to new GAME servers attempting to connect to the Authentication server

 */

using Serilog;
using System;
using System.Net;
using System.Net.Sockets;

namespace Authentication.Networking {
    class GameServerListener {
        
        private readonly int bindPort;

        private Socket socket;

        public GameServerListener(int port) {
            this.bindPort = port;
        }

        public bool Start() {
            try {
                Log.Information(string.Concat("Binding a socket listener to port: ", this.bindPort, "."));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, this.bindPort));
                socket.Listen(1);
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
                Log.Information("The Game Listener Socket is successfully binded to the port!");
                return true;
            } catch {
                Log.Fatal("Failed to bind a network socket to the port " + this.bindPort.ToString() + "Is a server already running on this port?");
                return false;
            }
        }

        private void OnAcceptConnection(IAsyncResult iAr) {
            try {
                Socket s = socket.EndAccept(iAr);
                Entities.Server gs = new Entities.Server(s);
            } catch { }

            if (socket != null)
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
        }
    }
}