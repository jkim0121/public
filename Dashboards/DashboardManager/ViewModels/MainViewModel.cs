using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Configuration;

using Deg.Dashboards.Common;

namespace Deg.DashboardManager
{
    public class MainViewModel : ViewModelBase
    {
        private ChannelFactory<IFrontEndManagerAdminSide> _forwardFactory;
        private IFrontEndManagerAdminSide _forwardChannel;

        public MainViewModel()
        {
            try
            {
                _forwardFactory = new ChannelFactory<IFrontEndManagerAdminSide>(ConfigurationManager.AppSettings["MainEndPointName"]);
                _forwardChannel = _forwardFactory.CreateChannel();
            }
            catch (Exception ex)
            {

            }

            UserSessionViewModel = new UserSessionViewModel(this);
            ComponentViewModel = new ComponentViewModel(this);
        }

        public ComponentViewModel ComponentViewModel
        {
            get; private set;
        }

        public UserSessionViewModel UserSessionViewModel
        {
            get; private set;
        }

        internal IFrontEndManagerAdminSide Channel
        {
            get
            {
                return _forwardChannel;
            }
        }
    }
}
