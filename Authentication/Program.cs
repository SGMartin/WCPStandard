using System;
using System.Threading;

namespace Authentication 
{
    class Program {
        private static bool isRunning = false;
        public static object sessionLock = new Object();
        public static ulong totalPlayers = 0;
        public static ulong onlinePlayers = 0;
        public static ulong playerPeak = 0;

        static void Main(string[] args) 
        {

            Console.Title = "「Starting」Authentication server";
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" _______        _______ _______  ______ _______ _______ _     _");
            Console.WriteLine(@" |_____| |         |    |______ |_____/ |______ |  |  | |     |");
            Console.WriteLine(@" |     | |_____    |    |______ |    \_ |______ |  |  | |_____|");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(new string('_', Console.WindowWidth));
            Console.WriteLine();

            if (!Config.Read())
            {
               // Log.Instance.WriteError("Failed to load the configuration file.");
                Console.ReadKey();
                return;
            }

/*
            if (!Databases.Init()) {
             //   Log.Instance.WriteError("Failed to initilize all database connections.");
                Console.ReadKey();
                return;
            }

*/
            if (!new Networking.GameServerListener((int)Core.Networking.Constants.Ports.Internal).Start()) {
                return;
            }

            isRunning = (new Networking.ServerListener((int)Core.Networking.Constants.Ports.Login)).Start();


            Console.ForegroundColor = ConsoleColor.Gray;

           
            while (isRunning) {
                Console.Title = string.Format("「Authentication」Players: {0} | Peak: {1} | Total: {2}", onlinePlayers, playerPeak, totalPlayers);
                // TODO: Update the console title + basic queries.
                Thread.Sleep(1000);
            }
        }
    }
}