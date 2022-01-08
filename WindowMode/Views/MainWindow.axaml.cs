using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using UI.ViewModels;

namespace UI.Views
{
    public partial class MainWindow : Window
    {
        private ViewModels.MainWindowViewModel viewModel = new ViewModels.MainWindowViewModel();

        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif

            //������ ��� ���� ����� �������� ������� �� ���������. �� ������ �����, ������ �������� � ��������, �� �� ��������.
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

        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
