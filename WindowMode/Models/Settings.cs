using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowMode.Models
{
    public class Settings
    {
        public string DirectoryPath { get; set; }

        public Settings()
        {
            DirectoryPath = Directory.GetCurrentDirectory();
        }
    }
}
