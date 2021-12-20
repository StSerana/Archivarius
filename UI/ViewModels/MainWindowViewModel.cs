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

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        public ObservableCollection<ArchivariusEntity> CurrentDirectoryContent { get; }
        private string currentDirectoryPath;
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
        }

        private void ChangeCurrentDirectory(string newPath)
        {            
            if (FileSystem.CheckIfDirectoryExists(newPath))
            {
                CurrentDirectoryPath = newPath;
                UpdateCurrentDirectoryContent();
                this.RaisePropertyChanged("CurrentDirectoryPath");
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
