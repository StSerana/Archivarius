using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using Archivarius;
using Archivarius.Algorithms;
using WindowMode.Models;
using ReactiveUI;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using System.ComponentModel;

namespace WindowMode.ViewModels
{
    public class MainWindowViewModel : ViewModelBase, INotifyPropertyChanged
    {
        
        private string currentDirectoryPath;
        private readonly Archivator archivator;
        public string CurrentDirectoryPath 
        { 
            get => currentDirectoryPath;
            set => this.RaiseAndSetIfChanged(ref currentDirectoryPath, value);
        }

        private readonly Settings settings = FileSystem.GetSettings();

        public MainWindowViewModel(Archivator archivator)
        {
            this.archivator = archivator;
            AvailableAlgorithmTypes = new ObservableCollection<AlgorithmType>(archivator.AlgorithmManager.GetResolvedAlgorithmTypes());
            CurrentDirectoryContent = new ObservableCollection<ArchivariusEntity>();
            CurrentDirectoryPath = settings.DirectoryPath;
            SelectedAlgorithmType = AlgorithmType.Huffman;
            
            UpdateCurrentDirectoryContent();
        }
        
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
        public AlgorithmType SelectedAlgorithmType { get; set; }
        public ObservableCollection<AlgorithmType> AvailableAlgorithmTypes { get; }
        public ObservableCollection<ArchivariusEntity> CurrentDirectoryContent { get; }        
        public bool IsDirectoryEmpty => CurrentDirectoryContent?.Count == 0;
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

                        ExtractButtonIsEnabled = IsEntityArchive(selectedItem);
                        AddToButtonIsEnabled = IsEntityArchive(selectedItem);
                        ViewButtonIsEnabled = selectedItem.IsDirectory;
                        ArchiveButtonIsEnabled = selectedItem.Extension != null && !IsEntityArchive(selectedItem);
                    }

                    this.RaiseAndSetIfChanged(ref dataGridSelectedRowIndex, value);
                }
            }
        }

        private bool IsEntityArchive(ArchivariusEntity entity) => archivator.AlgorithmManager.GetResolvedArchiveExtensions().Contains(entity.Extension);

        

        public void OnExtractPress(object? sender, RoutedEventArgs args)
        {
            var item = CurrentDirectoryContent[DataGridSelectedRowIndex];

            archivator.Decompress(new FileInfo(item.Path));

            UpdateCurrentDirectoryContent();
        }

        public async void OnAddToPress(object? sender, RoutedEventArgs args)
        {
            var archive = CurrentDirectoryContent[DataGridSelectedRowIndex];
            var fileDialog = new OpenFileDialog();
            var textFileFilter = new FileDialogFilter() { Name = "Выберите файл", Extensions = new List<string>() { "txt" } };
            fileDialog.Filters = new List<FileDialogFilter>() { textFileFilter };
            var mainWindow = ((IClassicDesktopStyleApplicationLifetime)Application.Current.ApplicationLifetime).MainWindow;

            var selectedItems = await fileDialog.ShowAsync(mainWindow);

            if (selectedItems == null)
                return;

            foreach (var selectedItemPath in selectedItems)
            {
                archivator.AppendFile(new FileInfo(selectedItemPath), new FileInfo(archive.Path));
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

            archivator.Compress(new FileInfo(item.Path), SelectedAlgorithmType);

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
                settings.DirectoryPath = newPath;
                FileSystem.SaveSettings(settings);
                
                UpdateCurrentDirectoryContent();
                this.RaisePropertyChanged("CurrentDirectoryPath");
                this.RaisePropertyChanged("IsDirectoryEmpty");
            }
            else
            {
                //need to add alert, for now just replace not existing path with the previous one
                CurrentDirectoryPath = settings.DirectoryPath;

                UpdateCurrentDirectoryContent();
                this.RaisePropertyChanged("CurrentDirectoryPath");
                this.RaisePropertyChanged("IsDirectoryEmpty");
            }
        }

        public void OnGoUpButtonTap(object? sender, RoutedEventArgs args)
        {
            ChangeCurrentDirectory(Path.GetFullPath(Path.Combine(CurrentDirectoryPath, @"../")));
        }

        public void OnCurrentDirectoryPathTextboxKeyDown(object? sender, Avalonia.Input.KeyEventArgs args)
        {
            var key = args.Key;

            if (key == Avalonia.Input.Key.Enter)
                ChangeCurrentDirectory(CurrentDirectoryPath);
        }
    }
}
