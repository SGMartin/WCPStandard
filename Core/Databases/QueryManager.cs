/*
                This Emulator uses MysqlConnector 0.47.1, an ADO.net based API instead of the Oracle one. It is faster, fully asynchronous
                and compatible with .NET Core 2.0.

                The new QueryManager replaces the old MySQL class.
                Also read: https://msdn.microsoft.com/en-us/magazine/jj991977.aspx
 */

using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

using Core.Databases;

namespace Core.Databases
{

    public class QueryManager
    {
        public Database Database{ get; private set;}

        public QueryManager(string host, int port, string user, string password, string database)
        {
           string connectionData = string.Concat(
               "Server=",
                host,
                ";Port=",
                port,
                ";Uid=",
                user,
                ";Pwd=",
                password,
                ";Database=",
                database,
                ";");

                Database = new Database(connectionData);
        }
       public void AsyncQuery(string query)
       {
           try
           {  
               var commandQuery = Database.Connection.CreateCommand() as MySqlCommand;
               commandQuery.CommandText = query;
               commandQuery.ExecuteNonQueryAsync();
           }
           catch
           {
               //TODO: Log this
           }
          
       }

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

            //This method used to be synch... same as above
        public MySqlDataReader Select(string[] keys, string table, Dictionary<string, object> values) 
        {
            string query = string.Concat("SELECT ", string.Join(",", keys), " FROM ", table);
            string valuesString = string.Empty;

            if (values.Count > 0) {
                byte index = 0;
                foreach (KeyValuePair<string, object> entry in values) {
                    if (index == 0) {
                        valuesString = string.Concat(valuesString, entry.Key, "=@", entry.Key);
                        index++;
                    } else {
                        valuesString = string.Concat(valuesString, " AND ", entry.Key, "=@", entry.Key);
                    }
                }
                query = string.Concat(query, " WHERE ", valuesString);
            }

            try 
            {

                var commandQuery         = Database.Connection.CreateCommand() as MySqlCommand;
                commandQuery.CommandText = query;

                foreach (KeyValuePair<string, object> entry in values) {
                     commandQuery.Parameters.AddWithValue("@" + entry.Key, entry.Value);
                }

                return commandQuery.ExecuteReader();

            } catch { }

        return null;

         }

    }
}