using Archivarius;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class FileSystem
    {
        public readonly static List<string> SupportedFileExtenstions = new List<string>() 
        {
            ".txt", ".jpg", ".archivarius" //add jpeg/png?
        };

        public static List<ArchivariusEntity> GetDirectoryContent(string path)
        {
            var dirContent = Directory.GetFileSystemEntries(path)
            .Select(entry =>
            {
                var file = new FileInfo(entry);
                var extension = file.Exists ? file.Extension : null;

                return new ArchivariusEntity(file.Name, file.FullName, extension);
            })
            .Where(entry => entry.Extension == null || SupportedFileExtenstions.Contains(entry.Extension.ToLower()))
            .ToList();

            return dirContent;
        }

        public static bool CheckIfDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
