/*

                This class is just a socket to listen for incoming connections. Foreach new connection, an user or server instance is created depending
                on boolean _isGameServerListener. Every user/server has his own socket to handle communication
                between the client and the server or the server and the authentication

 */
using Serilog;
using System;
using System.Net;
using System.Net.Sockets;

namespace Authentication.Networking
{
    class ServerListener
    {

        public bool IsListening { get; }

        private readonly int  _bindPort;
        private readonly bool _isGameServerListener;
        private Socket socket;

        public ServerListener(bool isGameServerListener, int port)
        {
            _bindPort = port;
            _isGameServerListener = isGameServerListener;
            IsListening = StartListener();
        }

        private bool StartListener()
        {
            try
            {
                Log.Information(string.Concat("Binding a socket listener to port: ", _bindPort, "."));
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Bind(new IPEndPoint(IPAddress.Any, _bindPort));
                socket.Listen(1);
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
                Log.Information("The socket is successfully binded to the port!");
                return true;
            }
            catch
            {
                Log.Fatal("Failed to bind a network socket to the port " + _bindPort.ToString() + "Is a server already running on this port?");
                return false;
            }
        }

        private void OnAcceptConnection(IAsyncResult iAr)
        {
                Socket s = socket.EndAccept(iAr);

            if (!_isGameServerListener){
                Entities.User usr = new Entities.User(s);
            }
            else {
                Entities.Server server = new Entities.Server(s);
            }     
            if (socket != null)
                socket.BeginAccept(new AsyncCallback(this.OnAcceptConnection), null);
        }

    }
}