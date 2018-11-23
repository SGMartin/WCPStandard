/*
                This Emulator uses MysqlConnector 0.47.1, an ADO.net based API instead of the Oracle one. It is faster, fully asynchronous
                and compatible with .NET Core 2.0.

                Best read ever and the true reason for replacing the old class and database workflow: 
                https://stackoverflow.com/questions/9705637/executereader-requires-an-open-and-available-connection-the-connections-curren/9707060#9707060

                Also read: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx, 
                           https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling
 */

using System;
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
      
    }
}
