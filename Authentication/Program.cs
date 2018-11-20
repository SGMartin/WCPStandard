/*
 * 
 * 
 *           This is the actual server application. The core of it is just a loop and two listener sockets.
 *                                                                                                                                          
 * 
 */ 

using System;
using System.IO;
using System.Threading;

using Serilog;
using Serilog.Core;


namespace Authentication 
{
    class Program
    {

        public static object sessionLock = new Object();

        //Defines the global logging level for the server
        public static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

        private static bool isRunning = true;
        private static DateTime startTime;


        static void Main(string[] args) 
        {
            startTime = DateTime.Now;

            //setting up the logger. Defaulting to debug unless overwritten by a config file.
            levelSwitch.MinimumLevel = (Serilog.Events.LogEventLevel)Config.SERILOGLEVEL;

            Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(levelSwitch).WriteTo.Console().
                WriteTo.File("Authentication.log", rollingInterval: RollingInterval.Day).CreateLogger();

           

            Console.Title = "Authentication server";

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(@" __          _______ _____   _____ _                  _               _ ");
            Console.WriteLine(@" \ \        / / ____|  __ \ / ____| |                | |             | |");
            Console.WriteLine(@"  \ \  /\  / / |    | |__) | (___ | |_ __ _ _ __   __| | __ _ _ __ __| |");
            Console.WriteLine(@"   \ \/  \/ /| |    | ___ / \___ \| __ / _` | '_ \ / _` |/ _` | '__ / _|");
            Console.WriteLine(@"    \  /\  / | |____| |     ____) | || (_| | | | | (_| | (_| | | | (_| |");
            Console.WriteLine(@"     \/  \/   \_____|_|    |_____/ \__\__,_|_| |_|\__,_|\__,_|_|  \__,_|");
            Console.WriteLine(new string('_', Console.WindowWidth));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();


            //Path.Combine is platform friendly :) Windows uses \ whereas Linux uses /
            //TODO: be able to add cfg file using command line
            if (!Config.Read(Path.Combine(
                String.Concat(
                    Environment.CurrentDirectory, Path.DirectorySeparatorChar, "CFG", Path.DirectorySeparatorChar, "Authentication.cfg"))))
            {
                Log.Fatal("Failed to load the configuration file.");
                Console.ReadKey();
                return;
            }


            if (!Databases.Init())
            {
                Log.Fatal("Failed to connect to the database");
                Console.ReadKey();
                return;
            }


            if (!new Networking.GameServerListener((int)Core.Networking.Constants.Ports.Internal).Start())
            {
                Log.Fatal("Could not start GameServer listener. Is the port already in use?");
                isRunning = false;
                return;
            }

            if(!new Networking.ServerListener((int)Core.Networking.Constants.Ports.Login).Start())
            {
                Log.Fatal("Could not start Server Listener. Is the login port already in use?");
                isRunning = false;
            }

            Log.Debug("Server started");
            Console.ForegroundColor = ConsoleColor.Gray;

            if(isRunning)
            {
                TimeSpan loadTime = DateTime.Now - startTime;
                Log.Information(string.Format("Emulator loaded in {0} milliseconds!", loadTime.TotalMilliseconds));
            }
           
            while (isRunning)
            {
               
                Thread.Sleep(1000);
            }
        }
    }
}