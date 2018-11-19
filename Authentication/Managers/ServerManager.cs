/*

        This manager keeps tracks of every game server instance. It has a list of authorized game servers.
        When the list is full of servers, it rejects new servers.

 */


using System;
using System.Collections;

using Core;

namespace Authentication.Managers {
    class ServerManager {
        private byte MAX_SERVERS = Config.MAXIMUM_SERVER_COUNT; //TODO: Is the second page usable?

        private Hashtable servers = new Hashtable();

        public byte Add(Authentication.Entities.Server s, string name, string ip, int port, Core.GameConstants.ServerTypes type) {

            byte serverId = 0;

            for (byte i = 1; i <= MAX_SERVERS; i++) {
                if (!servers.ContainsKey(i)) {
                    serverId = i;
                    break;
                }
            }

            if (serverId > 0) {
                // Add server to the hashtable :p
                 s.OnAuthorize(serverId, name, ip, port, type);
                 servers.Add(serverId, s);
            }

            return serverId;
        }

        public void Remove(byte serverId) {
            if (servers.ContainsKey(serverId)) {
                servers.Remove(serverId);
            }
        }

        public ArrayList GetAllAuthorized() {
            ArrayList authorizedServers = new ArrayList();

            foreach(Entities.Server server in servers.Values) {
                if (server != null) {
                    if (server.Authorized) {
                        authorizedServers.Add(server);
                    }
                }
            }

            return authorizedServers;
        }

        public ArrayList GetAll() {
            return new ArrayList(servers.Values);
        }

        private static ServerManager instance;
        public static ServerManager Instance { get { if (instance == null) { instance = new ServerManager(); } return instance; } }
    }
}