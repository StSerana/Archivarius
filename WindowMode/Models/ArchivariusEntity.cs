using System.Collections.Generic;

namespace WindowMode.Models
{
    public class ArchivariusEntity
    {
        public string Path { get; }
        public string Extension { get; }
        public bool IsDirectory { get; }
        public bool IsArchive { get; }
        private EntityType Type { get; }

        public ArchivariusEntity(string name, string path, string extension = null)
        {
            Path = path;
            Extension = extension;
            IsDirectory = extension == null;
            IsArchive = new List<string>{".huf", ".lzw"}.Contains(extension);
            
            if (IsArchive) 
                Type = EntityType.Archive;
            else
                Type = IsDirectory ? EntityType.Directory : EntityType.File;

            switch (Type)
            {
                case EntityType.Archive:
                    break;
                case EntityType.File:
                    break;
                case EntityType.Directory:
                    break;
            }
        }
    }
}
