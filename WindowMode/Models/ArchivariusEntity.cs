using System.Collections.Generic;

namespace WindowMode.Models
{
    public class ArchivariusEntity
    {
        public string Name { get; } 
        public string Path { get; }
        public string Extension { get; }
        public bool IsDirectory { get; }
        public bool IsArchive { get; }
        public string TypeTranslation { get; }
        private EntityType Type { get; }

        public ArchivariusEntity(string name, string path, string extension = null)
        {
            Name = name;
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
                    TypeTranslation = "Архив";
                    break;
                case EntityType.File:
                    TypeTranslation = "Файл";
                    break;
                case EntityType.Directory:
                    TypeTranslation = "Папка";
                    break;
            }
        }
    }
}
