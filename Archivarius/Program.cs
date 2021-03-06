using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Archivarius.UserOptions;
using Archivarius.Utils.Managers;
using CommandLine;
using NLog;
using static System.String;

namespace Archivarius
{
    class Program
    {
        private const string HelloText = "Hello! I am Archivarius, and I'm ready to help you with archives.\n" + 
                                          "1) If you want to know more, please, write \"help\".\n" + 
                                          "2) If you want to turn off program, please, write \"exit\".";

        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private static void Main(string[] args)
        {
            Logger.Info("App started");

            Console.WriteLine(HelloText);
            var input = Console.ReadLine();

            var commandVerbs = LoadVerbs();
            while (input != "exit")
            {
                try
                {
                    if (IsNullOrEmpty(input)) continue;

                    Logger.Info($"Recieved input: {input}");

                    Parser.Default.ParseArguments(input.Split(), commandVerbs)
                        .WithParsed(obj =>
                        {
                            ((Option) obj).Execute();
                            Console.WriteLine("Process finished.");
                        });
                }
                catch (IOException ex)
                {
                    Logger.Error(ex.Message);
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