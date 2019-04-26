using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    public partial class DataBaseMonitor
    {
        private readonly TimeSpan _pollingInterval = TimeSpan.FromSeconds(1);
        private IFrontEndManagerDatabaseSide _channel;
        private Timer _timer;

        public DataBaseMonitor(IFrontEndManagerDatabaseSide channel)
        {
            _channel = channel;
            _timer = new Timer(PollDatabase, null, TimeSpan.FromMilliseconds(-1), _pollingInterval);
        }

        public void Start()
        {
            _timer.Change(TimeSpan.FromMilliseconds(0), _pollingInterval);
        }

        public void Stop()
        {
            _timer.Change(TimeSpan.FromMilliseconds(-1), _pollingInterval);
        }
    }
}
