using System;
using System.Threading;

namespace Authentication
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "「Starting」Authentication server";
            Console.ForegroundColor = ConsoleColor.Yellow;
            
            
            string ServerName = "Authentication Server";   
          //  Console.SetCursorPosition((Console.WindowWidth - ServerName.Length) / 2, Console.CursorTop);
            Console.WriteLine(ServerName);
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(new string('_', Console.WindowWidth));
            Console.WriteLine();

        }
    }
}
