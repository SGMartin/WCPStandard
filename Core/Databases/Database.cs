/*
                This Emulator uses MysqlConnector 0.47.1, an ADO.net based API instead of the Oracle one. It is faster, fully asynchronous
                and compatible with .NET Core 2.0.

                Best read ever and the true reason for replacing the old class and database workflow: 
                https://stackoverflow.com/questions/9705637/executereader-requires-an-open-and-available-connection-the-connections-curren/9707060#9707060

                Also read: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx, 
                           https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling
 */

using System.Data.Common;
using System;
using System.Collections.Generic;
using Serilog;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Core.Databases
{
    public class Database : IDisposable
    {
        private readonly MySqlConnection Connection;

        public Database(string connectionData) //TODO: add pooling max
        {
            Connection = new MySqlConnection(connectionData);

        }
        public void Dispose()
        {
            Connection.Close();
        }

        public async Task<bool> AsyncQuery(string query)
        {
            try
            {
                await Connection.OpenAsync();

                using (var commandQuery = Connection.CreateCommand() as MySqlCommand)
                {
                    commandQuery.CommandText = query;
                    await commandQuery.ExecuteNonQueryAsync();

                    return true;
                }
            }
            catch
            {
                Log.Fatal("Could not parse query " + query);
                return false;
            }

        }


        public async Task<List<object>> AsyncGetRowFromTable(string[] keys, string table, Dictionary<string,object> values)
        {
            List<object> dataRow = new List<object>();

            string query     = string.Concat("SELECT ", string.Join(",", keys), " FROM ", table);
            string allValues = string.Empty;

            if (values.Count > 0)
            {
                byte index = 0;
                foreach (KeyValuePair<string, object> entry in values)
                {
                    if (index == 0)
                    {
                        allValues = string.Concat(allValues, entry.Key, "=@", entry.Key);
                        index++;
                    }
                    else
                    {
                        allValues = string.Concat(allValues, " AND ", entry.Key, "=@", entry.Key);
                    }
                }
                query = string.Concat(query, " WHERE ", allValues);
            }

            try
            {
                await Connection.OpenAsync();

                using (var commandQuery = Connection.CreateCommand() as MySqlCommand)
                {
                    commandQuery.CommandText = query;

                    foreach(KeyValuePair<string, object> entry in values)
                    {
                        commandQuery.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    using (DbDataReader Reader = await commandQuery.ExecuteReaderAsync())
                    {
                        if (await Reader.ReadAsync())
                        {
                            if (Reader.HasRows)
                            {
                                for (int i = 0; i < Reader.FieldCount; i++)
                                    dataRow.Add(Reader.GetValue(i));
                            }
                        }
                    }
                }
                return dataRow;
            }
            catch(Exception e)
            {
                Log.Error(e.ToString());
                return null;
            }
        }
    }
}
