using System;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

using Core;
using Serilog;
using System.Data.Common;

namespace Authentication.Networking.Handlers
{
    class NewNickname : PacketHandler
    {
        protected override async void Process(Entities.User u)
        {
            if (u.Authorized)
            {
                string newName = GetString(0);

                if (newName.Length > 3 && Utils.isAlphaNumeric(newName)) //legal nickname. TODO: add reserved/allowed shit
                {
                    bool dbNameIsTaken = await DBIsNameTaken(newName);
                  
                    if(!dbNameIsTaken)
                    {                    
                        u.UpdateDisplayname(newName);
                        u.Send(new Packets.ServerList(u));
                        u.Disconnect();

                        // Update the DB
                        DBUpdateDisplayName(u.ID, newName);

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

        private async Task<bool> DBUpdateDisplayName(uint userId, string newName)
        {
            using (Core.Databases.Database Database = new Core.Databases.Database(Config.AUTH_CONNECTION))
            {
               await Database.AsyncQuery(string.Concat("UPDATE users SET `displayname` ='", newName, "' WHERE ID=", userId, ";"));
            }
            return false;
        }

        //TODO: refactor this and add to Database.cs
        private async Task<bool>DBIsNameTaken(string newName)
        {
              bool result = true;

            using (MySqlConnection connection = new MySqlConnection(Config.AUTH_CONNECTION))
            {
                try
                {
                    using (var commandQuery = connection.CreateCommand() as MySqlCommand)
                    {
                        commandQuery.CommandText = string.Concat("SELECT username FROM `users` WHERE BINARY displayname=", "'", newName, "'", ";");

                        await connection.OpenAsync();

                        using (DbDataReader Reader = await commandQuery.ExecuteReaderAsync())
                        {
                            if (await Reader.ReadAsync()) //rows found
                                result = true;
                            else
                                result = false;
                        }
                    }
                       
                }
                catch (Exception e)
                {
                    Log.Error(e.ToString());
                }

            }
            return result;
        }
        #endregion
    }
}