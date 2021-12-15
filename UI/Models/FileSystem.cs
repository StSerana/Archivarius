using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class FileSystem
    {
        public static string CurrentDirectory = "D:/Sharaga";

        public static List<ArchivariusEntity> GetCurrentDirectoryContent()
        {
            var dirContent = Directory.GetFileSystemEntries(CurrentDirectory).Select(entry =>
            {
                var file = new FileInfo(entry);
                var extension = file.Exists ? file.Extension : null;

                return new ArchivariusEntity(file.Name, file.FullName, extension);
            }).ToList();

            return dirContent;
        }
    }
}
