using Deg.Dashboards.Common;

namespace Deg.DashboardManager
{
    public class ComponentViewModel : ViewModelBase
    {
        private MainViewModel _parent;

        public ComponentViewModel(MainViewModel parent)
        {
            _parent = parent;
        }
    }
}
