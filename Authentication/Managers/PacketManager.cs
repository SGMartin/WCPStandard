/*

            The packet manager esentially binds a packet to a handler. It keeps two separate lists of internal (server) packets
            and game packets. Whenever an incoming packet arrives, the manager is asked for a convenient handler to return.

 */



using System;
using System.Collections;
using Authentication.Networking;

namespace Authentication.Managers {
    class PacketManager {

        private Hashtable _externalPacketList = new Hashtable();
        private Hashtable _internalPacketList = new Hashtable();

        public PacketManager() {
            _internalPacketList.Clear();
            _externalPacketList.Clear();

            // Server Packets//
           AddInternal(Core.Networking.PacketList.ServerAuthentication, new Networking.Handlers.Internal.ServerAuthorization());
           AddInternal(Core.Networking.PacketList.Ping,                 new Networking.Handlers.Internal.Ping());
           AddInternal(Core.Networking.PacketList.PlayerAuthentication, new Networking.Handlers.Internal.PlayerAuthorization());

            // Game Packets //
           AddExternal(Authentication.Networking.PacketList.ServerList, new Networking.Handlers.PlayerLogin());
          // AddExternal(Authentication.Networking.PacketList.Nickname,   new Networking.Handlers.Nickname());
        }

        private void AddInternal(Core.Networking.PacketList packetType, PacketHandler handler) {
            if (!_internalPacketList.ContainsKey(packetType)) {
                _internalPacketList.Add((ushort)packetType, handler);
            }
        }

        public PacketHandler FindInternal(Core.Networking.InPacket inPacket) {
            if (_internalPacketList.ContainsKey(inPacket.Id)) {
                return (Authentication.Networking.PacketHandler)_internalPacketList[inPacket.Id];
            } else {
                Console.WriteLine("UNKNOWN PACKET :: " + inPacket.fullPacket.Remove(inPacket.fullPacket.Length-1));
            }
            return null;
        }

        private void AddExternal(Authentication.Networking.PacketList packetType, PacketHandler handler) {
            if (!_externalPacketList.ContainsKey(packetType)) {
                _externalPacketList.Add((ushort)packetType, handler);
            }
        }

        public PacketHandler FindExternal(Core.Networking.InPacket inPacket) {
            if (_externalPacketList.ContainsKey(inPacket.Id)) {
                return (Authentication.Networking.PacketHandler)_externalPacketList[inPacket.Id];
            } else {
                Console.WriteLine("UNKNOWN PACKET :: " + inPacket.fullPacket.Remove(inPacket.fullPacket.Length - 1));
            }
            return null;
        }

        private static PacketManager instance;
        public static PacketManager Instance { get { if (instance == null) { instance = new PacketManager(); } return instance; } }
    }
}