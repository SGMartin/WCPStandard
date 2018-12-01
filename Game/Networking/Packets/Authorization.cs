using Game.Enums;

namespace Game.Networking.Packets
{
    class Authorization : Core.Networking.OutPacket
    {
        public enum ErrorCodes : uint
        {
            NormalProcedure = 73030,    // Please log in using the normal procedure!
            InvalidPacket = 90100,      // Invalid Packet.
            UnregisteredUser = 90101,   // Unregistered User.
            AtLeast6Chars = 90102,      // You must type at least 6 characters .
            NicknameToShort = 90103,    // Nickname should be at least 6 charaters.
            IdInUseOtherServer = 90104, // Same ID is being used on the server.
            NotAccessible = 90105,      // Server is not accessible.
            TrainingServer = 90106,     // Trainee server is accesible until the rank of a private..
            ClanWarError = 90112,       // You cannot participate in Clan War
            LackOfResponse = 91010,     // Connection terminated because of lack of response for a while.
            ServerIsFull = 91020,       // You cannot connect. Server is full.
            InfoReqInTrafic = 91030,    // Info request are in traffic.
            AccountUpdateFailed = 91040,// Account update has failed.
            BadSynchronization = 91050, // User Info synchronization has failed.
            IdInUse = 92040,            // That ID is currently being used.
            PremiumOnly = 98010         // Available to Premium users only.
        }

        public Authorization(ErrorCodes errorCode) : base((ushort)Networking.PacketList.Authorization)
        {
            Append((uint)errorCode);
        }

        public Authorization(Entities.User u)
            : base((ushort)Networking.PacketList.Authorization)
        {
            Append(Core.Networking.Constants.ERROR_OK);
            Append(string.Concat("Gameserver", Config.SERVER_ID));
            //     Append(u.SessionID);
            Append(1);
            Append(u.ID); // User id.
                                        //     Append(u.SessionID);        // User session id.
            Append(1);
            Append(u.DisplayName);      // User Displayname (Nickname).

            // CLAN BLOCKS //
         //   if (u.ClanId == -1)
                Fill(4, -1);
           /*
            else
            {
                Objects.Clan Clan = Managers.ClanManager.Instance.GetClan(u.ClanId);

                if (Clan != null)
                {
                    Append(u.ClanId);
                    Append(Clan.Name);
                    Append(u.ClanRank);
                    Append(u.ClanRank);
                }
                else
                {
                    Log.Instance.WriteError("User clan is " + u.ClanId.ToString() + " but the server failed to load the clan");
                    Fill(4, -1);
                }

            }
            */
            // CLAN BLOCKS
            Append((byte)u.Premium);    // Premium Type.
            Append(0);                  // Unknown.
            Append(0);                  // Unknown.
            Append(Core.Utils.GetLevelforExp(u.XP)); // User Level (based on XP).
            Append(u.XP);               // User XP.
            Append(0);                  // Unknown.
            Append(0);                  // Unknown.
            Append(u.Money);            // User Money
            Append(u.Stats.Kills);            // User Kills
            Append(u.Stats.Deaths);           // User Deaths
            Fill(5, 0);                 // 5 Unknown blocks

            // SLOT STATE //
            // Append(u.Inventory.SlotState); // T = Slot Enabled, F = Slot disabled.
            Append("T,T,T,T");
            Append("DA02,DB01,DF01,DR01,^,^,^,^");
            Append("DA02,DB01,DF01,DQ01,^,^,^,^");
            Append("DA02,DB01,DG05,DN01,^,^,^,^");
            Append("DA02,DB01,DC02,DN01,^,^,^,^");
            Append("DA02,DB01,DJ01,DL01,^,^,^,^");
            // EQUIPMENT //
            //    Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Engineer]);    // Equipment - Engeneer
            //  Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Medic]);       // Equipment - Medic
            //  Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Sniper]);      // Equipment - Sniper
            //  Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Assault]);     // Equipment - Assault
            //  Append(u.Inventory.Equipment.ListsInternal[(byte)Classes.Heavy]);       // Equipment - Heavy
            // INVENTORY //
            // Append(u.Inventory.Itemlist);
            Append("^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^,^"); // ItemListStr
            // END INVENTORY //
            Fill(2, 0); // Two unknown blocks.
        }
    }
}
