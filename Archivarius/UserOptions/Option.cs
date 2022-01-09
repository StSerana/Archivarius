using Archivarius.Algorithms;
using Archivarius.Utils.Managers;
using CommandLine;

namespace Archivarius
{
    public abstract class Option
    {
        protected static readonly ConsoleArchiveManager ConsoleArchiveManager = new ConsoleArchiveManager();
        
        [Option('f', "file", Required = true, HelpText = "Input file")]
        public string InputFile { get; set; }

        public abstract void Execute();
    }
}