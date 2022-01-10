using Archivarius.Utils.Managers;
using CommandLine;

namespace Archivarius.UserOptions
{
    public abstract class Option
    {
        protected static readonly ConsoleArchiveManager ConsoleArchiveManager = new();
        
        [Option('f', "file", Required = true, HelpText = "Input file")]
        public string InputFile { get; init; }

        public abstract void Execute();
    }
}