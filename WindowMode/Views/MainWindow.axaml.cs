using Archivarius;
using Archivarius.Utils;
using Archivarius.Utils.Managers;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Ninject;
using WindowMode.ViewModels;

namespace WindowMode.Views
{
    public class MainWindow : Window
    {
        private MainWindowViewModel viewModel;

        public MainWindow()
        {
            var container = ContainerManager.CreateStandardContainer();
            viewModel = new MainWindowViewModel(container.Get<Archivator>());
            
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            this.FindControl<DataGrid>("CurrentDirectoryContentGrid").PointerMoved += (s, a) =>
            {
                
            };

            this.FindControl<Button>("ExtractButton").Tapped += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnExtractPress(s, a);
            };

            this.FindControl<Button>("ArchiveButton").Tapped += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnArchivePress(s, a);
            };

            this.FindControl<Button>("AddToButton").Tapped += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnAddToPress(s, a);
            };

            this.FindControl<Button>("ViewButton").Tapped += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnViewPress(s, a);
            };

            this.FindControl<Button>("GoUpButton").Tapped += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnGoUpButtonTap(s, a);
            };

            this.FindControl<TextBox>("CurrentDirectoryPathTextbox").KeyDown += (s, a) =>
            {
                (DataContext as MainWindowViewModel).OnCurrentDirectoryPathTextboxKeyDown(s, a);
            };

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
