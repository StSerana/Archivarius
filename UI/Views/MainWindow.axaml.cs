using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System.ComponentModel;
using System.Runtime.CompilerServices;
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
            this.FindControl<DataGrid>("CurrentDirectoryContentGrid").Tapped += (s, a) =>
            {
                viewModel.OnCurrentDirectoryGridTap(s, a);
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
