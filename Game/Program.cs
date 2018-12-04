using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

using Serilog;
using Serilog.Core;

using CommandLine;

using MySql.Data.MySqlClient;


using Game.Networking;

namespace Game
{
    class Program
    {
        
        private static DateTime startTime;
        
        //Defines the global logging level for the server
        public static LoggingLevelSwitch levelSwitch = new LoggingLevelSwitch();

        public static AuthenticationClient AuthServer;

        //CMD related var.
        private static bool   useDifferentFileLocation = false;
        private static string iniFileLocation = string.Empty;

        private static bool isRunning = false;
        private static uint serverLoops = 0;

        static void Main(string[] args)
        {
            startTime = DateTime.Now;

            //setting up the logger. Defaulting to debug unless overwritten by a config file.
            levelSwitch.MinimumLevel = (Serilog.Events.LogEventLevel)Config.SERILOGLEVEL;

            Console.Title = "WCPS Game server";

            Console.ForegroundColor = ConsoleColor.DarkGreen;
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
            Log.Logger = new LoggerConfiguration().MinimumLevel.ControlledBy(levelSwitch)
                        .WriteTo.Console()
                        .WriteTo.File("Game.log", rollingInterval: RollingInterval.Day).CreateLogger();

            //UNIT TESTS
            Unit_tests.WeaponTest WeaponTesting = new Unit_tests.WeaponTest();     

            //setting up CMD reader
            var CMD = Parser.Default.ParseArguments<Options>(args)
                .WithParsed<Options>(opts => RunOptionsAndReturnExitCode(opts))
                .WithNotParsed<Options>((errs) => HandleParseError(errs));

            //Path.Combine is platform friendly :) Windows uses \ whereas Linux uses /
            //iniFileLocation can be redefined by CMD... see RunOptionsAndReturnExitCode
            if (!useDifferentFileLocation)
                iniFileLocation = Path.Combine(String.Concat(Environment.CurrentDirectory, Path.DirectorySeparatorChar, "CFG", Path.DirectorySeparatorChar, "Game.ini"));

            Config.Read(iniFileLocation);
  
            // test database connection
            using (MySqlConnection TestConnection = new MySqlConnection(Config.GAME_CONNECTION))
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

            /*
            if (!GameConfig.Read())
            {
                Log.Instance.WriteError("Failed to load the game configuration");
                Console.ReadKey();
                return;
            }

            if (!Managers.ItemManager.Instance.Load())
            {
                Log.Instance.WriteError("Failed to initilize the item manager.");
                Console.ReadKey();
                return;
            }

            if (!Managers.VehicleManager.Instance.Load())
            {
                Log.Instance.WriteError("Failed to initialize the vehicle Manager.");
                Console.ReadKey();
                return;
            }

            if (!Managers.MapManager.Instance.Load())
            {
                Log.Instance.WriteError("Failed to initilize the map manager.");
                Console.ReadKey();
                return;
            }

            if (!Managers.CouponManager.Instance.Load())
            {
                Log.Instance.WriteError("Failed to initialize the coupon manager.");
                Console.ReadKey();
                return;
            }

            if (!Managers.ClanManager.Instance.Load())
            {
                Log.Instance.WriteError("Failed to initialize the clan manager.");
                Console.ReadKey();
                return;
            }
            */
            // CONNECT TO THE AUTHORIZATION SERVER //

                AuthServer = new AuthenticationClient(Config.AUTH_SERVER_IP, (int)Core.Networking.Constants.Ports.Internal);
                if (!AuthServer.Connect())
                {
                      Console.ReadKey();
                          return;
                }
            
            
            
            if (!new UDPListener((int)Core.Networking.Constants.Ports.UDP1).Start())
            {
                return;
            }

            if (!new UDPListener((int)Core.Networking.Constants.Ports.UDP2).Start())
            {
                return;
            }
            
            //SHOW THE CONFIGURATION RATES
            /*
            if (Config.SERVER_DEBUG == 1)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("SERVER IS RUNNING IN DEBUG MODE!!!");
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Dinar rate is: " + GameConfig.DinarRate.ToString());
            Console.WriteLine("EXP rate is: " + GameConfig.ExpRate.ToString());
            Console.WriteLine("Bomb time is " + GameConfig.BombTime.ToString());
            Console.WriteLine("Maximum team difference is " + GameConfig.MaxTeamDifference.ToString());
            Console.WriteLine("Maximum room count is " + GameConfig.MaxRoomCount.ToString());
            Console.ForegroundColor = ConsoleColor.Gray;

            ServerLogger.Instance.Append("Server initialiced correctly");
            */
            // Start up the listener :)
            isRunning = (new ServerListener((int)Core.Networking.Constants.Ports.Game)).Start();

            if (isRunning)
            {
                TimeSpan loadTime = DateTime.Now - startTime;
                Log.Information(string.Format("Emulator loaded in {0} milliseconds!", loadTime.TotalMilliseconds));
            }

            startTime = DateTime.Now;
            while (isRunning)
            {

                TimeSpan runTime = DateTime.Now - startTime;
          //      Console.Title = string.Format("「Game Server」Uptime {0} | Players: {1} | Peak: {2} | Rooms: {3}", runTime.ToString(@"dd\:hh\:mm\:ss"), Managers.UserManager.Instance.Sessions.Values.Count, Managers.UserManager.Instance.Peak, Managers.ChannelManager.Instance.RoomCount);


                if (serverLoops % 5 == 0)
                {
                    Parallel.ForEach(Managers.UserManager.Instance.Sessions.Values, user => {
                        if (user.Authorized)
                            user.SendPing();
                    });

                }

                //ping to auth  server
            //    AuthServer.Send(new Networking.Packets.Internal.Ping());

                serverLoops++;

                Thread.Sleep(1000);
            }
        }

        //Console methods
        static int RunOptionsAndReturnExitCode(Options options)
        {
            if (options.InputCFG != string.Empty) //the program won´t crash even if something wrong is written here.
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