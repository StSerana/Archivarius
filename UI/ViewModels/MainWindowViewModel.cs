using Avalonia.Controls;
using Avalonia.Interactivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using UI.Models;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        public ObservableCollection<ArchivariusEntity> CurrentDirectoryContent { get; }
        public string CurrentDirectoryPath { get; set; }

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
            Debug.WriteLine(CurrentDirectoryContent[index].Name);
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}
