using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace Deg.DashboardManager
{
    /// <summary>
    /// Interaction logic for UserSesssionView.xaml
    /// </summary>
    public partial class UserSesssionView : UserControl
    {
        private ListCollectionView _collectionView;
        public UserSesssionView()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            var viewModel = DataContext as UserSessionViewModel;
            _collectionView = new ListCollectionView(viewModel.Sessions);
            _listViewSessions.ItemsSource = _collectionView;

            viewModel.ListUpdated += () => Dispatcher.Invoke(() => _collectionView.Refresh());
        }
    }
}
