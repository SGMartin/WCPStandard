/*
 *                                                            Option.cs manages command line arguments
 * 
 *                                                      The command line is parsed using Eric Newton´s CLR parser
 *                                                        https://github.com/commandlineparser/commandline                                                                                  
 * 
 * 
 * 
*/
using System.Collections.Generic;
using CommandLine;

namespace Game
{
    class Options
    {
        [Option('c', "config", Default = "", Required = false, HelpText = "Specify custom config file.")]
        public string InputCFG { get; set; }

    }
}
