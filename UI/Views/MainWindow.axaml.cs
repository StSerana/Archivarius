using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;

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
            DataContext = viewModel;

            //колхоз для того чтобы передать событие во вьюмодель. по логике вещей, должно делаться в разметке, но не делается.
            this.FindControl<DataGrid>("CurrentDirectoryContentGrid").Tapped += (s, a) =>
            {
                viewModel.OnCurrentDirectoryGridTap(s, a);
            };

            this.FindControl<Button>("GoUpButton").Tapped += (s, a) =>
            {
                viewModel.OnGoUpButtonTap(s, a);
            };

            viewModel.PropertyChanged += (s, a) =>
            {
                
            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }    
}
