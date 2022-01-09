﻿using System.Collections.Generic;

namespace WindowMode.Models
{
    public class ArchivariusEntity
    {
        public string Name { get; private set; }
        public string Path { get; private set; }
        public string Extension { get; private set; }
        public bool IsDirectory { get; private set; }
        public bool IsArchive { get; private set; }
        public EntityType Type { get; private set; }
        public string TypeTranslation { get; private set; }
        public string DirectoryPath { get; private set; }

        public ArchivariusEntity(string name, string path, string extension = null)
        {
            Name = name;
            Path = path;
            Extension = extension;
            IsDirectory = extension == null;
            IsArchive = new List<string>{".huf", ".lzw"}.Contains(extension);
            DirectoryPath = System.IO.Path.GetDirectoryName(Path);

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

    public enum EntityType
    {
        Archive,
        File,
        Directory
    }
}