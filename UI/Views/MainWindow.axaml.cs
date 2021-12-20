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

            //колхоз для того чтобы передать событие во вьюмодель. по логике вещей, должно делаться в разметке, но не делается.
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
