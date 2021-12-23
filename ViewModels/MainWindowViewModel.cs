using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UI.Models;
using ReactiveUI;
using System.Runtime.CompilerServices;
using Archivarius;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<ArchivariusEntity> CurrentDirectoryContent { get; }
        public bool IsDirectoryEmpty { get => CurrentDirectoryContent?.Count == 0; }
        private string currentDirectoryPath;
        private Archivarius.Archivarius api = new Archivarius.Archivarius();
        public string CurrentDirectoryPath 
        { 
            get => currentDirectoryPath;
            set
            {
                this.RaiseAndSetIfChanged(ref currentDirectoryPath, value);
            } 
        }

        public MainWindowViewModel()
        {            
            CurrentDirectoryContent = new ObservableCollection<ArchivariusEntity>();
            CurrentDirectoryPath = Directory.GetCurrentDirectory();
            
            UpdateCurrentDirectoryContent();
        }

        

        private void UpdateCurrentDirectoryContent()
        {
            CurrentDirectoryContent.Clear();

            FileSystem.GetDirectoryContent(CurrentDirectoryPath).ForEach(item =>
            {
                CurrentDirectoryContent.Add(item);
            });
        }

        public void OnCurrentDirectoryGridTap(object? sender, RoutedEventArgs args)
        {
            var dataGrid = (DataGrid)sender;
            var index = dataGrid.SelectedIndex; // tap index

            var item = CurrentDirectoryContent[index];      
            
            Debug.WriteLine(item.Name);

            if (item.IsDirectory)
                ChangeCurrentDirectory(item.Path);
            else if (item.IsArchive) { //change to smtg more reliable
                api.Decompress(item.DirectoryPath, item.Name);

                UpdateCurrentDirectoryContent();
            }
            else
            {
                api.Compress(item.DirectoryPath, item.Name);

                UpdateCurrentDirectoryContent();
            }
        }

        private void ChangeCurrentDirectory(string newPath)
        {            
            if (FileSystem.CheckIfDirectoryExists(newPath))
            {
                CurrentDirectoryPath = newPath;
                UpdateCurrentDirectoryContent();
                this.RaisePropertyChanged("CurrentDirectoryPath");
                this.RaisePropertyChanged("IsDirectoryEmpty");
            }
            else
            {
                //add handle (show err msg?)
            }
        }

        public void OnGoUpButtonTap(object? sender, RoutedEventArgs args)
        {
            ChangeCurrentDirectory(Path.GetFullPath(Path.Combine(CurrentDirectoryPath, @"../")));
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
