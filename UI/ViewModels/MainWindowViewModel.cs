using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ExplorerEntity> CurrentDirectoryContent { get; }

        public MainWindowViewModel()
        {
            CurrentDirectoryContent = new ObservableCollection<ExplorerEntity>()
            {
                new ExplorerEntity("Folder", "Downloads"),
                new ExplorerEntity("Folder", "Desktop"),
                new ExplorerEntity("File", "Script.js"),
            };
        }

        public string Greeting => "Welcome to Avalonia!";
    }

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
