/*
 * 
 *                                 
 *                                
 */

using System;

namespace Authentication
{
    public class Config
    {
        public static string[] AUTH_DATABASE = new string[] { "localhost", "3306", "root", "", "wcps-authentication"};

        public static int SERILOGLEVEL = 1; 
        public static string GAMESERVERKEY = "wcps-auth";

        public static int  MAXIMUM_SERVER_COUNT = 10;
        public static bool ENABLEOLDLAUNCHER = true;
        public static bool ENABLENICKCHANGE = true;

        public static int FORMAT = 0;
        public static int LAUNCHER = 0;
        public static int CLIENT = 0;
        public static int UPDATER = 0;
        public static int SUB = 0;
        public static int OPTION = 0;
        public static string URL = "http://www.google.es";


        public static bool Read(string fileToRead)
        {
           

                return true;
       
        }
    }
}