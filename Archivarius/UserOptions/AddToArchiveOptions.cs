using CommandLine;

namespace Archivarius.UserOptions
{
    [Verb("add", HelpText = "Append file to archive" )]
    public class AddToArchiveOptions : Option
    {
        [Option('a', "add", Required = true, HelpText = "Archive to be updated")]
        public string ArchiveFile { get; init; }

        public override void Execute()
        {
            ConsoleArchiveManager.Append(this);
        }
    }
}