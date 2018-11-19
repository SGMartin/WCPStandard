using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Authentication.Networking.Handlers
{
    class PlayerLogin : Networking.PacketHandler
    {
        protected override void Process(Entities.User user)
        {
            string inputUserName = GetString(2);
            string inputPassword = GetString(3);

            bool forceDisconnect = true;

            //valid UserName?
            if (inputUserName.Length >= 3 && Core.Utils.isAlphaNumeric(inputUserName))
            {
                //is password long enough?
                if (inputPassword.Length >= 3)
                {
                    MySqlDataReader reader = Databases.Auth.Select(
                      new string[] { "id", "username", "status", "displayname", "password", "passwordsalt" },
                       "users",
                      new Dictionary<string, object>() {
                        { "username", inputUserName }
                   });

                    //Does the username exists?
                    if (reader.HasRows && reader.Read())
                    {
                        //The  user does exist:  retrieve data
                        uint id = reader.GetUInt32(0);
                        string dbUserName = inputUserName;
                        byte status = reader.GetByte(2); //0 = global network account ban
                        string displayname = reader.GetString(3);
                        string dbPassword = reader.GetString(4);
                        string dbPasswordSalt = reader.GetString(5);


                        //We hash password typed  by the player and check it against  the one stored in the DB
                        string hashedPassword = Core.Utils.CreateSHAHash(String.Concat(inputPassword, dbPasswordSalt));

                        //CHECK!! Proceed
                        if (hashedPassword == dbPassword.ToLower())
                        {
                            var IsOnline = Managers.SessionManager.Instance.Sessions.Select(n => n.Value).Where(n => n.ID == id && n.IsActivated && !n.IsEnded).Count();

                            //Check to see if the same account is already logged in
                            //TODO: Improve this. What if a GameServer does not update this?
                            if (IsOnline == 0)
                            {
                                if (status > 0)
                                {
                                    user.OnAuthorize(id, dbUserName, displayname, status);
                                    user.Send(new Packets.ServerList(user));
                                }
                                else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.Banned)); }

                            }
                            else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.AlreadyLoggedIn)); }

                        }
                        else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.WrongPW)); }

                    }
                    else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.WrongUser)); }


                    if (!reader.IsClosed) { reader.Close(); }
                }
                else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.EnterPasswordError)); }

            }
            else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.EnterIDError)); }


            if (forceDisconnect)
                user.Disconnect();
        }
    }
}