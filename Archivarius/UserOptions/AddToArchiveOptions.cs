using CommandLine;

namespace Archivarius
{
    [Verb("add", HelpText = "Append file to archive" )]
    public class AddToArchiveOptions : Option
    {
        [Option('a', "add", Required = true, HelpText = "Archive to be updated")]
        public string ArchiveFile { get; set; }

        public override void Execute()
        {
            ConsoleArchiveManager.Append(this);
        }
    }
}