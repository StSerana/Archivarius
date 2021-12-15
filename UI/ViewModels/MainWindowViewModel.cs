using Avalonia.Controls;
using Avalonia.Interactivity;
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
        public ExplorerEntity SelectedEntity { get; set; }

        public MainWindowViewModel()
        {
            var content = FileSystem.GetCurrentDirectoryContent();

            CurrentDirectoryContent = new ObservableCollection<ExplorerEntity>();

            content.ForEach(g => CurrentDirectoryContent.Add(new ExplorerEntity(g.IsDirectory ? "folder" : "file", g.Name)));
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
