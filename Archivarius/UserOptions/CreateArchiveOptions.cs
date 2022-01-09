using System.Collections.Generic;
using System.Reflection.Emit;
using Archivarius.Algorithms;
using CommandLine;
using CommandLine.Text;

namespace Archivarius
{
    [Verb("create", HelpText = "Create new archive" )]
    public class CreateArchiveOptions : Option
    {
        [Option('k', "key", Required = true, HelpText = "Using algorithm: 0.Huffman; 1.Lzw")]
        public AlgorithmType Algorithm { get; set; }
        
        public override void Execute()
        {
            ConsoleArchiveManager.Create(this);
        }
    }
}