using System;
using System.Linq;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using Core;
using Serilog;

namespace Authentication.Networking.Handlers
{
    class PlayerLogin : Networking.PacketHandler
    {
        protected override void Process(Entities.User user)
        {
            string inputUserName = GetString(2);
            string inputPassword = GetString(3);

            bool isSettingNewNickName = false;

            //valid UserName?
            if (inputUserName.Length >= 3 && Core.Utils.isAlphaNumeric(inputUserName))
            {
                //is password long enough?
                if (inputPassword.Length >= 3)
                {
                    MySqlDataReader reader = Databases.Auth.Select(
                      new string[] { "ID", "username", "displayname", "password", "salt", "rights"},
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
                        string displayname = reader.GetString(2);
                        string dbPassword = reader.GetString(3);
                        string dbPasswordSalt = reader.GetString(4);
                        GameConstants.Rights dbRights;

                        try { dbRights = (GameConstants.Rights)reader.GetByte(5); }
                        catch
                        { Log.Error("User " + dbUserName + " rights could not be parsed. Blocking user.");
                            dbRights = GameConstants.Rights.Blocked;
                        }
                       


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
                                //TODO: Add ban time? Delegate it to game servers?
                                //TODO: Add gameserver blacklisting
                                if (dbRights == GameConstants.Rights.Blocked)
                                    user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.Banned));
                                else
                                {
                                    //Authenticate player
                                    user.OnAuthorize(id, dbUserName, displayname);

                                    //check if the player has a NickName
                                    if (user.DisplayName.Length > 0)
                                        user.Send(new Packets.ServerList(user));

                                    else
                                    {
                                        if (Config.ENABLENICKCHANGE) //can they set their nickname ingame ???
                                        {
                                            isSettingNewNickName = true;
                                            user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.NewNickname));
                                        }
                                        else { user.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.IlligalNickname)); }

                                    }
                                }
                               
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


            //people who successfully logged on can be safely disconnected... 
            //the client will show the server list and they will be redirected by the client.
            //keep the socket for those who are setting up nickname
            if (!isSettingNewNickName)
                user.Disconnect();
        }
    }
}