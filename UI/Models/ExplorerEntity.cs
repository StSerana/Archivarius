using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI.Models
{
    public class ExplorerEntity
    {
        public string Type { get; set; }
        public string Name { get; set; }

        public ExplorerEntity(string type, string name)
        {
            Type = type;
            Name = name;
        }
    }
}
