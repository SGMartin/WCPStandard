/*

            /*Version
/subver
Open Browser
/url http://google.com
Close Browser
/close url
Close Room
/rdis <room #>
Kick Player
/kick <player name>
Player Info
/find <player name>
Will display like this
ID, IP, Prem, Exp, Dinar, Kill, Death, Scale(CQC,UBAN,BG)
             */

using System;
using System.Collections;


namespace Authentication.Networking.Packets {
    class ServerList : Core.Networking.OutPacket {
        public enum ErrorCodes : uint {
            Exception = 70101,
            NewNickname = 72000,
            WrongUser = 72010,
            WrongPW = 72020,
            AlreadyLoggedIn = 72030,
            ClientVerNotMatch = 70301,
            Banned = 73050,
            BannedTime = 73020,
            NotActive = 73040,
            EnterIDError = 74010,
            EnterPasswordError = 74020,
            ErrorNickname = 74030,
            NicknameTaken = 74070,
            NicknameToLong = 74100, // Longer then 10 characters.
            IlligalNickname = 74110
        }

        public ServerList(ErrorCodes errorCode)
            : base((ushort)PacketList.ServerList) {
                Append((uint)errorCode);
        }

        public ServerList(Entities.User u)
            : base((ushort)PacketList.ServerList) {
                Append(Core.Networking.Constants.ERROR_OK);
                Append(u.ID);         // UserId
                Append(0);              // Unknown
                Append(u.Name);       // User login name
                Append("NULL");       // User password (NULL).
                Append(u.DisplayName);    // Displayname or nickname.
                Append(u.SessionID);      // Session ID
                Append(1);              // Unknown?
                Append(0);              // Unknown?
                Append(u.AccessLevel);  //1 = user Admin is 3, DEV is 5
                Append(1.11025);        // PF_20
             
                ArrayList serverList = Managers.ServerManager.Instance.GetAllAuthorized();
                Append(serverList.Count);               // Server count
                foreach (Entities.Server server in serverList) {
                    Append(server.ID);                  // Server ID
                    Append(server.ServerName);         // Server Name
                    Append(server.IP);                  // Server IP
                    Append((int)Core.Networking.Constants.Ports.Game); // Server Port
                    Append(server.TotalPlayerCount);        // Server Player count. Maximum 3600
                    Append((byte)server.Type);          // Server Type
                }
                
                Fill(4, -1);
              //  Append(-1); // ID
               // Append(-1); // Name
               // Append(-1); // Master? 1 = YES 0 = NO
               // Append(-1); // ?
                Append(0);
                Append(0);
        }
    }
}