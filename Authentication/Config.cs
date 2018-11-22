/*
 * 
 *                                   Using ini-parser 2.5.2 by rickyah    
 *                                    MITT license -see license file.
 *                                    https://www.nuget.org/packages/ini-parser/
 */

using System;
using IniParser;
using IniParser.Model;
using Serilog;

namespace Authentication
{
    public class Config
    {
   
        //default CFG

        public static string AUTH_CONNECTION = string.Concat("Server=",
               "localhost",
               ";Port=",
               "3306",
               ";Uid=",
               "root",
               ";Pwd=",
               "",
               ";Database=",
               "wcps-authentication",
               ";");  

        public static int    SERILOGLEVEL = 1; 
        public static string GAMESERVERKEY = "wcps-auth";

        public static int  MAXIMUM_SERVER_COUNT = 10;
        public static bool ENABLEOLDLAUNCHER = true;
        public static bool ENABLENICKCHANGE = true;

        //TODO: Add this to CFG
        public static int FORMAT = 0;
        public static int LAUNCHER = 0;
        public static int CLIENT = 0;
        public static int UPDATER = 0;
        public static int SUB = 0;
        public static int OPTION = 0;
        public static string URL = "http://www.google.es";


        public static bool Read(string iniFile)
        {
            try
            {
                var parser = new FileIniDataParser();
                IniData AuthData = parser.ReadFile(iniFile);

                AUTH_CONNECTION = string.Concat("Server=",
                    AuthData["Database"]["Host"],
                   ";Port=",
                   AuthData["Database"]["Port"],
                   ";Uid=",
                   AuthData["Database"]["User"],
                   ";Pwd=",
                   AuthData["Database"]["Password"],
                   ";Database=",
                   AuthData["Database"]["DbName"],
                   ";");

                SERILOGLEVEL             = Convert.ToInt16(AuthData["Logging"]["SetLoggingLevel"]);
                GAMESERVERKEY            = AuthData["Authentication"]["SetServerKey"];
                MAXIMUM_SERVER_COUNT     = Convert.ToInt16(AuthData["Authentication"]["SetMaximumGameServers"]);
                ENABLEOLDLAUNCHER        = Convert.ToBoolean(AuthData["Authentication"]["EnableOldLauncherPacket"]);
                ENABLENICKCHANGE         = Convert.ToBoolean(AuthData["Authentication"]["EnableNickNamePacket"]);

                FORMAT                  = Convert.ToInt16(AuthData["Authentication"]["Format"]);
                LAUNCHER                = Convert.ToInt16(AuthData["Authentication"]["Launcher"]);
                CLIENT                  = Convert.ToInt16(AuthData["Authentication"]["Client"]);
                UPDATER                 = Convert.ToInt16(AuthData["Authentication"]["Updater"]);
                SUB                     = Convert.ToInt16(AuthData["Authentication"]["Sub"]);
                OPTION                  = Convert.ToInt16(AuthData["Authentication"]["Option"]);
                URL                     = AuthData["Authentication"]["Url"];

                return true;
            }
            catch(Exception e)
            {
                Log.Error(String.Concat("Could not load ", iniFile, ":"));
                Log.Error(e.ToString());
                Log.Error("Reverting to default...");

                return false;
            }      
        }
    }
}