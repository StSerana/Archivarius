using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace WindowMode.Models
{
    public class FileSystem
    {
        public readonly static List<string> SupportedFileExtenstions = new List<string>() 
        {
            ".txt", ".jpg", ".archivarius" //add jpeg/png?
        };

        private const string settingsFileName = "settings.json";

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
        
        public static void SaveSettings(Settings settings)
        {
            CheckSettingsFile();

            var rawSettings = JsonSerializer.Serialize(settings, typeof(Settings));
            File.WriteAllText(settingsFileName, rawSettings);
        }

        public static Settings GetSettings()
        {
            CheckSettingsFile();

            var rawSettings = File.ReadAllText(settingsFileName);
            var settings = (Settings) JsonSerializer.Deserialize(rawSettings, typeof(Settings));

            return settings;
        }


        /// <summary>
        /// creates empty settings file if not exists
        /// </summary>
        private static void CheckSettingsFile()
        {
            if (!File.Exists(settingsFileName))
            {
                var emptySettings = JsonSerializer.Serialize(new Settings(), typeof(Settings));

                File.WriteAllText(settingsFileName, emptySettings);
            }
        }

        public static bool CheckIfDirectoryExists(string path)
        {
            return Directory.Exists(path);
        }
    }
}
