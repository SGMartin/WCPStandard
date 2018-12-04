using System.Collections;

namespace Game.Networking.Packets
{
    class UserList : Core.Networking.OutPacket
    {

        public UserList(int intPage, ArrayList userList) //TODO: Clan STUFF
            : base(28928)
        {

            Append(userList.Count);
            Append(intPage);

            for (int i = 0; i < userList.Count; i++)
            {
                Entities.User u = (Entities.User)userList[i];
                Append(i + intPage * 10); // List Index
                Append(u.ID); // UID
                Append(u.SessionID); // Session ID
                Append(u.DisplayName); // Nickname

              //  Objects.Clan Clan = Managers.ClanManager.Instance.GetClan(u.ClanId);

                //if (Clan == null)
                    Fill(4, -1);
               // else
               // {
                 //   Append(u.ClanId);
                   // Append(Clan.Name);
                    //Append(u.ClanRank);
                   // Append(((u.ClanId > 0) ? 0 : -1)); // Unknown?
               // }

                Append(0); // Unknown
                Append(16); // Unknown
                Append(u.XP);
                Append((byte)u.Premium);
                Append(0);

            }

        }
    }
}