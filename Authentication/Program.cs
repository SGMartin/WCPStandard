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
            Console.WriteLine(@" _______        _______ _______  ______ _______ _______ _     _");
            Console.WriteLine(@" |_____| |         |    |______ |_____/ |______ |  |  | |     |");
            Console.WriteLine(@" |     | |_____    |    |______ |    \_ |______ |  |  | |_____|");
            Console.ForegroundColor = ConsoleColor.Gray;
            Console.WriteLine(new string('_', Console.WindowWidth));
            Console.WriteLine();

        }
    }
}
