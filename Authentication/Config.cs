using Core.IO;
using System;
namespace Authentication
{
    public class Config
    {
        public static byte MAXIMUM_SERVER_COUNT = 10;
        public static string[] AUTH_DATABASE;

       
        public static bool Read()
        {          
            bool result = false;

            try
            {
                ConfigParser XMLConfig = new ConfigParser("ServerConfig");

                Config.AUTH_DATABASE = new string[]
                {
                    XMLConfig.Read("AuthenticationServer", "Database", "Host"),
                    XMLConfig.Read("AuthenticationServer", "Database", "Port"),
                    XMLConfig.Read("AuthenticationServer", "Database", "UserName"),
                    XMLConfig.Read("AuthenticationServer", "Database", "UserPassword"),
                    XMLConfig.Read("AuthenticationServer", "Database", "DatabaseName")
                };

                MAXIMUM_SERVER_COUNT     = Convert.ToByte(XMLConfig.Read("AuthenticationServer", "Server", "MaximumGameServers"));

                if (MAXIMUM_SERVER_COUNT > 10)
                    MAXIMUM_SERVER_COUNT = 10;

                result = true;
            }
            catch {}
  
            return result;
        }
    }
}