using System.Collections.Generic;
using MySql.Data.MySqlClient;

using Core;

namespace Authentication.Networking.Handlers
{
    class NewNickName : PacketHandler
    {
        protected override void Process(Entities.User u)
        {
            if (u.Authorized)
            {
                string nickname = GetString(0);
                if (nickname.Length >= 3 && Utils.isAlphaNumeric(nickname))
                {
                    if (nickname.Length <= 16)
                    {
                        try
                        {
                            MySqlDataReader reader = Databases.Auth.Select(
                                new string[] { "ID" },
                                "users",
                                new Dictionary<string, object>() {
                                    { "displayname", nickname }
                                });

                            if (!reader.HasRows)
                            { // TODO: is the nickname allowed?
                                reader.Close();

                                Databases.Auth.AsyncQuery(string.Concat("UPDATE users SET `displayname` ='", nickname, "' WHERE ID=", u.ID, ";"));
                                u.UpdateDisplayname(nickname);
                                u.Send(new Packets.ServerList(u));
                                u.Disconnect();
                            }
                            else
                            {
                                reader.Close();
                                u.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.NicknameTaken));
                            }

                        }
                        catch { u.Disconnect(); }
                    }
                    else
                    {
                        u.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.NicknameToLong));
                    }
                }
                else
                {
                    u.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.ErrorNickname));
                }
            }
            else
            {
                u.Disconnect(); // Not authorized, cheating!
            }
        }
    }
}