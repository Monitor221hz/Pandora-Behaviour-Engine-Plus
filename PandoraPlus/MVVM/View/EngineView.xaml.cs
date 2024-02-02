using Pandora.Command;
using Pandora.MVVM.Data;
using Pandora.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Pandora.MVVM.View
{
    /// <summary>
    /// Interaction logic for EngineView.xaml
    /// </summary>
    public partial class EngineView : UserControl
    {
        private EngineViewModel _viewModel;

        public EngineView()
        {
            InitializeComponent();
            
            _viewModel = new EngineViewModel();
            DataContext = _viewModel;
            Loaded += EngineViewLoaded;
        }
        private async void EngineViewLoaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.LoadAsync();
        }
    }
}
