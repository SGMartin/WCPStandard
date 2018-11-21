/*
                This Emulator uses MysqlConnector 0.47.1, an ADO.net based API instead of the Oracle one. It is faster, fully asynchronous
                and compatible with .NET Core 2.0.

                Best read ever and the true reason for replacing the old class and database workflow: 
                https://stackoverflow.com/questions/9705637/executereader-requires-an-open-and-available-connection-the-connections-curren/9707060#9707060

                Also read: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx, 
                           https://docs.microsoft.com/en-us/dotnet/framework/data/adonet/sql-server-connection-pooling
 */
 /*
using System.Data;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Core.Databases
{

    public class QueryManager
    {
        
        public QueryManager()
        {
           
        }


       public void AsyncQuery(string connectionData, string query)
       {

            using (MySqlConnection connection = new MySqlConnection(connectionData)) //cierra la conexión de manera implícita al terminar
            {
                try
                {
                    connection.OpenAsync();

                    var commandQuery = connection.CreateCommand() as MySqlCommand;
                    commandQuery.CommandText = query;
                    commandQuery.ExecuteNonQueryAsync();

                }
                catch
                {
                    //TODO: log this
                }
            }        
       }


        public DataTable Select(string connectionData, string[] keys, string table, Dictionary<string, object> values)
        {
            string query = string.Concat("SELECT ", string.Join(",", keys), " FROM ", table);
            string valuesString = string.Empty;

            if (values.Count > 0)
            {
                byte index = 0;
                foreach (KeyValuePair<string, object> entry in values)
                {
                    if (index == 0)
                    {
                        valuesString = string.Concat(valuesString, entry.Key, "=@", entry.Key);
                        index++;
                    }
                    else
                    {
                        valuesString = string.Concat(valuesString, " AND ", entry.Key, "=@", entry.Key);
                    }
                }
                query = string.Concat(query, " WHERE ", valuesString);
            }

            using (MySqlConnection connection = new MySqlConnection(connectionData)) //cierra la conexión de manera implícita al terminar
            {
                try
                {
                    var commandQuery = connection.CreateCommand() as MySqlCommand;
                    commandQuery.CommandText = query;

                    foreach(KeyValuePair<string, object> entry in values)
                    {
                        commandQuery.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                    }

                    connection.OpenAsync();
                    DataTable Result = new DataTable();

                    using(MySqlDataReader reader = commandQuery.ExecuteReader()) //Async this?
                    {
                        Result.Load(reader);
                        return Result;
                    }
                }
                catch
                {
                    //TODO: log this
                    return null;
                }
            }
  
        }
        */
        /*
        //Synchronous version of AsyncQuery. Most queries should be asynchcronous nonetheless
       public void Query(string query)
       {
           try
           {
               var commandQuery = Database.Connection.CreateCommand() as MySqlCommand;
               commandQuery.CommandText = query;
               commandQuery.ExecuteNonQuery();
           }
           catch
           {
               //TODO: log this
           }
       }

       public void AsyncInsert(string table, Dictionary<string, object> values) {

            if (values.Count <= 0)
                throw new Exception("Please provide some data to insert.");

            string query = string.Concat("INSERT INTO ", table, "(", string.Join(",", values.Keys), ") VALUES ");
            string valuesString = string.Empty;

            if (values.Count > 0) 
            {
                foreach (KeyValuePair<string, object> entry in values) {
                    valuesString = string.Concat(valuesString, "@", entry.Key, ",");
                }
                query = string.Concat(query, "(", valuesString.Remove(valuesString.Length - 1), ")");
            }

            try
            {
                var commandQuery         = Database.Connection.CreateCommand() as MySqlCommand;
                commandQuery.CommandText = query;
                commandQuery.ExecuteNonQueryAsync();
            }
            catch
            {

                //TODO: LOG
            }
       }

       public void Insert(string table, Dictionary<string, object> values) 
       {
            if (values.Count <= 0)
                throw new Exception("Please provide some data to insert.");

            string query = string.Concat("INSERT INTO ", table, "(", string.Join(",", values.Keys), ") VALUES ");
            string valuesString = string.Empty;

            if (values.Count > 0)
            {
                foreach (KeyValuePair<string, object> entry in values) {
                    valuesString = string.Concat(valuesString, "@", entry.Key, ",");
                }
                query = string.Concat(query, "(", valuesString.Remove(valuesString.Length - 1), ")");
            }
            try
            {
                var commandQuery         = Database.Connection.CreateCommand() as MySqlCommand;
                commandQuery.CommandText = query;
                commandQuery.ExecuteNonQueryAsync();
            }
            catch
            {

                //TODO: LOG
            }
        }

            //THIS METHOD USED TO BE SYNCH. DOES IT MATTER WHEN IT COMES TO PLAYER INVENTORY AND STUFF?
         public void UpdateAsync(string table, Dictionary<string, object> val, Dictionary<string, object> where) 
         {
            string query = string.Concat("UPDATE ", table, " SET ");
            string valuesString = string.Empty;

            byte index = 0;
            foreach (KeyValuePair<string, object> entry in val) {
                if (index == 0) {
                    valuesString = string.Concat(valuesString, entry.Key, "=@", entry.Key);
                    index++;
                } else {
                    valuesString = string.Concat(valuesString, ", ", entry.Key, "=@", entry.Key);
                }
            }
            query = string.Concat(query, valuesString);

            if (where.Count > 0) {
                index = 0;
                foreach (KeyValuePair<string, object> entry in where) {
                    if (index == 0) {
                        valuesString = string.Concat(valuesString, entry.Key, "=@", entry.Key);
                        index++;
                    } else {
                        valuesString = string.Concat(valuesString, " AND ", entry.Key, "=@", entry.Key);
                    }
                }
                query = string.Concat(query, " WHERE ", valuesString);
            }

            try {
                var commandQuery         = Database.Connection.CreateCommand() as MySqlCommand;
                commandQuery.CommandText = query;

                foreach (KeyValuePair<string, object> entry in where) {
                    commandQuery.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                foreach (KeyValuePair<string, object> entry in val) {
                    commandQuery.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                commandQuery.ExecuteNonQueryAsync();

            } 
            catch 
            { 
                //TODO: LOG
            }
        }

       
         
   /}
}
*/