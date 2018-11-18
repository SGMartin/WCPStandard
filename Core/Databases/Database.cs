/*
                Disposable Database Object
                This Emulator uses MysqlConnector 0.47.1, an ADO.net based API
 */


using System;
using MySql.Data.MySqlClient;

namespace Core.Databases
{
    public class Database : IDisposable
    {
        public MySqlConnection Connection;

        public Database(string connectionData)
        {
            Connection = new MySqlConnection(connectionData);
        }
        public void Dispose()
        {
            Connection.Close();
        }
    }
}