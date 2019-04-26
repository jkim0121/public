using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.DashboardManager
{
    public class UserSessionViewModel : ViewModelBase
    {
        private MainViewModel _parent;
        internal event Action ListUpdated;

        public UserSessionViewModel(MainViewModel parent)
        {
            _parent = parent;
            Sessions.AddRange(_parent.Channel.GetActiveSessions());

            if (ListUpdated != null)
            {
                ListUpdated();
            }
        }

        public List<Session> Sessions
        {
            get; private set;
        } = new List<Session>();
    }
}
