using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Archivarius.Utils.Managers;
using CommandLine;
using static System.String;

namespace Archivarius
{
    class Program
    {
        private const string HELLO_TEXT = "Hello! I am Archivarius, and I'm ready to help you with archives.\n" + 
                                          "1) If you want to know more, please, write \"help\".\n" + 
                                          "2) If you want to turn off program, please, write \"exit\".";
        
        private static readonly ConsoleArchiveManager ConsoleArchiveManager = new();

        private static void Main(string[] args)
        {
            Console.WriteLine(HELLO_TEXT);
            var input = Console.ReadLine();
            var commandVerbs = LoadVerbs();
            while (input != "exit")
            {
                try
                {
                    if (IsNullOrEmpty(input)) continue;
                    Parser.Default.ParseArguments(input.Split(), commandVerbs)
                        .WithParsed(obj => ((Option) obj).Execute());
                }
                catch (IOException ex)
                {
                    Console.WriteLine("Something went wrong: " + ex.Message);
                }
                input = Console.ReadLine();
            }
        }
        
        private	static Type[] LoadVerbs()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttribute<VerbAttribute>() != null).ToArray();		 
        }
    }
}