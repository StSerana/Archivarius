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
        public EntityType Type { get; private set; }
        public string TypeTranslation { get; private set; }
        public string DirectoryPath { get; private set; }

        public ArchivariusEntity(string name, string path, string extension = null)
        {
            Name = name;
            Path = path;
            Extension = extension;
            IsDirectory = extension == null;
            DirectoryPath = System.IO.Path.GetDirectoryName(Path);

            if (Extension == ".zip") //у нас же .zip, да?
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

    public enum EntityType
    {
        Archive,
        File,
        Directory
    }
}
