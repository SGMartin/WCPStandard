using System;
using MySql.Data.MySqlClient;

using Core;
using Serilog;

namespace Authentication.Networking.Handlers
{
    class NewNickName : PacketHandler
    {
        protected override void Process(Entities.User u)
        {
            if (u.Authorized)
            {
                string newName = GetString(0);

                if (newName.Length > 3 && Utils.isAlphaNumeric(newName)) //legal nickname. TODO: add reserved/allowed shit
                {
                    if (!DBIsNameTaken(newName))
                    {

                        if (DBUpdateDisplayName(u.ID, newName))
                        {
                            u.UpdateDisplayname(newName);
                            u.Send(new Packets.ServerList(u));
                            u.Disconnect();
                        }
                    }
                    else
                        u.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.NicknameTaken));
                }
                else
                    u.Send(new Packets.ServerList(Packets.ServerList.ErrorCodes.IlligalNickname));
            }
            else
                u.Disconnect();
        }

        #region MySQL methods
        // TODO: update with await and async methods
        private bool DBUpdateDisplayName(uint userId, string newName)
        {
            using (MySqlConnection connection = new MySqlConnection(Config.AUTH_CONNECTION))
            {
                try
                {
                    var commandQuery = connection.CreateCommand() as MySqlCommand;
                    commandQuery.CommandText = string.Concat("UPDATE users SET `displayname` ='", newName, "' WHERE ID=", userId, ";");

                    connection.OpenAsync();
                    commandQuery.ExecuteNonQueryAsync();

                    return true;
                }
                catch(Exception ex)
                {
                    Log.Error(ex.ToString());
                    return false;
                }
               
            }
        }

        private bool DBIsNameTaken(string newName)
        {
            bool result = true;

            using (MySqlConnection connection = new MySqlConnection(Config.AUTH_CONNECTION))
            {
                try
                {
                    var commandQuery = connection.CreateCommand() as MySqlCommand;

                    commandQuery.CommandText = string.Concat("SELECT username FROM `users` WHERE displayname=","'", newName,"'", ";");
                    connection.Open();

                    MySqlDataReader Reader = commandQuery.ExecuteReader();

                    if (Reader.HasRows && Reader.Read()){
                      result = true; }

                    else {
                        result = false; }
                        

                    Reader.Close();
                }
                catch(Exception e)
                {
                    Log.Error(e.ToString());
                }

            }
            return result;
        }
        #endregion
    }
}