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
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
            DataContext = new ViewModels.MainWindowViewModel();

            this.FindControl<DataGrid>("CurrentDirectoryContentGrid").Tapped += (s, a) =>
            {
                var dataGrid = (DataGrid)s;

                var index = dataGrid.SelectedIndex; // tap index


            };
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }    
}
