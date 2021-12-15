using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class ArchivariusEntity
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Extension { get; private set; }
        public bool IsDirectory { get; private set; }

        public ArchivariusEntity(string name, string path, string extension = null)
        {
            Name = name;
            Path = path;
            Extension = extension;
            IsDirectory = extension == null;
        }
    }
}
