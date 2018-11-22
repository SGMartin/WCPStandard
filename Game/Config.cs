using System;

using Serilog;
using IniParser;
using IniParser.Model;
namespace Game
{
    class Config
    {
        //Default CFG here.

        public static string AUTH_SERVER_IP = "127.0.0.1";

        public static byte SERVER_ID = 0;

        public static string SERVER_KEY  = "SERVER-KEY";
        public static string SERVER_NAME = "DEFAULT";
        public static string SERVER_IP = "127.0.0.1";

        public static string GAME_CONNECTION = 
            string.Concat("Server=",
               "localhost",
               ";Port=",
               "3306",
               ";Uid=",
               "root",
               ";Pwd=",
               "",
               ";Database=",
               "wcps-server",
               ";");
    
        public static int SERILOGLEVEL = 1;


        public static bool Read(string iniFile)
        {
            try
            {
                var parser = new FileIniDataParser();
                IniData GameData = parser.ReadFile(iniFile);

                GAME_CONNECTION = string.Concat("Server=",
                    GameData["Database"]["Host"],
                   ";Port=",
                    GameData["Database"]["Port"],
                   ";Uid=",
                    GameData["Database"]["User"],
                   ";Pwd=",
                    GameData["Database"]["Password"],
                   ";Database=",
                    GameData["Database"]["DbName"],
                   ";");

                SERILOGLEVEL    = Convert.ToInt16(GameData["Logging"]["SetLoggingLevel"]);

                SERVER_KEY      = GameData["Authentication"]["SetAuthenticationKey"];
                AUTH_SERVER_IP  = GameData["Authentication"]["SetAuthenticationIP"];

                SERVER_IP       = GameData["Core"]["SetServerIP"];
                SERVER_NAME     = GameData["Core"]["SetServerName"];

                return true;
            }
            catch (Exception e)
            {
                Log.Error(string.Concat("Could not load ", iniFile, ":"));
                Log.Error(e.ToString());
                Log.Error("Reverting to default...");

                return false;
            }
        }
    }
}
