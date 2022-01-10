using CommandLine;

namespace Archivarius.UserOptions
{
    
    [Verb("decompress", HelpText = "Decompress archive" )]
    public class DecompressArchiveOptions : Option
    {
        public override void Execute()
        {
            ConsoleArchiveManager.Decompress(this);
        }
    }
}