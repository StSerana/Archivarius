using Archivarius.Algorithms;
using CommandLine;

namespace Archivarius.UserOptions
{
    [Verb("create", HelpText = "Create new archive" )]
    public class CreateArchiveOptions : Option
    {
        [Option('k', "key", Required = true, HelpText = "Using algorithm: 1.Huffman; 2.Lzw; 3.Gzip")]
        public AlgorithmType Algorithm { get; init; }
        
        public override void Execute()
        {
            ConsoleArchiveManager.Create(this);
        }
    }
}