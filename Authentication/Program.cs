/*
 * 
 * 
 *                               This is the actual server application. The core of it is just a loop and two listener sockets.
 *                                                        The command line is parsed using Eric Newton´s CLR parser
 *                                                        https://github.com/commandlineparser/commandline                                                                                  
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

using Serilog;
using Serilog.Core;
using CommandLine;

using MySql.Data.MySqlClient;

namespace Authentication 
{
    class Program
    {

        //Defines the global logging level for the server
        public static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

        //CMD related var.
        private static bool useDifferentFileLocation = false;
        private static string iniFileLocation        = string.Empty;

        private static bool isRunning                = true;
        private static DateTime startTime;


        static void Main(string[] args) 
        {
            startTime = DateTime.Now;


            //setting up the logger config. Defaulting to debug unless overwritten by a config file.
            levelSwitch.MinimumLevel = (Serilog.Events.LogEventLevel)Config.SERILOGLEVEL;

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


            //initialize logger
            Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(levelSwitch).WriteTo.Console().
                WriteTo.File("Authentication.log", rollingInterval: RollingInterval.Day).CreateLogger();


            //setting up CMD reader
            var CMD = Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));


            //Path.Combine is platform friendly :) Windows uses \ whereas Linux uses /
            //iniFileLocation can be redefined by CMD... see RunOptionsAndReturnExitCode
            if (!useDifferentFileLocation)
                iniFileLocation = Path.Combine(String.Concat(Environment.CurrentDirectory, Path.DirectorySeparatorChar, "CFG", Path.DirectorySeparatorChar, "Authentication.ini"));
            

            Config.Read(iniFileLocation);

            // test database connection
            using (MySqlConnection TestConnection = new MySqlConnection(Config.AUTH_CONNECTION))
            {
                try
                {
                    TestConnection.Open();
                    Log.Information("Database connection test... successfull");
                }
                catch
                {
                    Log.Fatal("Database connection test failed!!");
                    Console.ReadKey();
                    return;
                    
                }
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

        //Console methods
        static int RunOptionsAndReturnExitCode(Options options)
        {
            if(options.InputCFG != string.Empty) //the program won´t crash even if something wrong is written here.
            {
                useDifferentFileLocation = true;
                         iniFileLocation = options.InputCFG;
            }
           
            return 0;
        }

        static void HandleParseError(IEnumerable<Error> errs)
        {
            Log.Error("NOT WORKING");
        }
    }
}