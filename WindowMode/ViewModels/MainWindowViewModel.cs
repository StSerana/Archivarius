using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using UI.Models;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;

namespace UI.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        private bool extractButtonIsEnabled;
        public bool ExtractButtonIsEnabled 
        {
            get => extractButtonIsEnabled;
            set => this.RaiseAndSetIfChanged(ref extractButtonIsEnabled, value);
        }

        private bool archiveButtonIsEnabled;
        public bool ArchiveButtonIsEnabled
        {
            get => archiveButtonIsEnabled;
            set => this.RaiseAndSetIfChanged(ref archiveButtonIsEnabled, value);
        }

        private bool addToButtonIsEnabled;
        public bool AddToButtonIsEnabled
        {
            get => addToButtonIsEnabled;
            set => this.RaiseAndSetIfChanged(ref addToButtonIsEnabled, value);
        }

        private bool viewButtonIsEnabled;
        public bool ViewButtonIsEnabled
        {
            get => viewButtonIsEnabled;
            set => this.RaiseAndSetIfChanged(ref viewButtonIsEnabled, value);
        }
        public ObservableCollection<ArchivariusEntity> CurrentDirectoryContent { get; }
        public bool IsDirectoryEmpty { get => CurrentDirectoryContent?.Count == 0; }
        private int dataGridSelectedRowIndex;
        public int DataGridSelectedRowIndex
        {
            get => dataGridSelectedRowIndex;
            set 
            {
                if (value != dataGridSelectedRowIndex)
                {
                    if (value == -1)
                    {
                        ExtractButtonIsEnabled = false;
                        AddToButtonIsEnabled = false;
                        ArchiveButtonIsEnabled = false;
                        ViewButtonIsEnabled = false;
                    }
                    else
                    {
                        var selectedItem = CurrentDirectoryContent[value];

                        ExtractButtonIsEnabled = selectedItem.IsArchive;
                        AddToButtonIsEnabled = selectedItem.IsArchive;
                        ViewButtonIsEnabled = selectedItem.IsDirectory;
                        ArchiveButtonIsEnabled = selectedItem.Extension != null && !selectedItem.IsArchive;
                    }

                    this.RaiseAndSetIfChanged(ref dataGridSelectedRowIndex, value);
                }
            }
        }

        private string currentDirectoryPath;
        private Archive api = new Archive(new FileManager(), new AlgorithmLZW(), new AlgorithmHuffman());
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

        public void OnExtractPress(object? sender, RoutedEventArgs args)
        {
            var item = CurrentDirectoryContent[DataGridSelectedRowIndex];

            api.Decompress(item.DirectoryPath, item.Name);

            UpdateCurrentDirectoryContent();
        }

        public async void OnAddToPress(object? sender, RoutedEventArgs args)
        {
            var archive = CurrentDirectoryContent[DataGridSelectedRowIndex];
            var fileDialog = new OpenFileDialog();
            var textFileFilter = new FileDialogFilter() { Name = "��������� �����", Extensions = new List<string>() { "txt" } };
            fileDialog.Filters = new List<FileDialogFilter>() { textFileFilter };
            var mainWindow = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;

            var selectedItems = await fileDialog.ShowAsync(mainWindow);

            if (selectedItems == null)
                return;

            foreach (var selectedItemPath in selectedItems)
            {
                api.AppendFile(archive.DirectoryPath, Path.GetFileName(selectedItemPath), archive.Name);
            }
        }

        public void OnViewPress(object? sender, RoutedEventArgs args)
        {
            var item = CurrentDirectoryContent[DataGridSelectedRowIndex];

            ChangeCurrentDirectory(item.Path);
        }

        public void OnArchivePress(object? sender, RoutedEventArgs args)
        {
            var item = CurrentDirectoryContent[DataGridSelectedRowIndex];

            api.Compress(item.DirectoryPath, item.Name);

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
