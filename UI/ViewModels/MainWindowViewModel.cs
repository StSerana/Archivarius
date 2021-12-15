using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using UI.Models;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ExplorerEntity> CurrentDirectoryContent { get; }

        public MainWindowViewModel()
        {
            var content = FileSystem.GetCurrentDirectoryContent();

            CurrentDirectoryContent = new ObservableCollection<ExplorerEntity>();

            content.ForEach(g => CurrentDirectoryContent.Add(new ExplorerEntity(g.IsDirectory ? "folder" : "file", g.Name)));
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
