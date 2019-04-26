using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    public partial class FrontEndManagerService : IFrontEndManagerDatabaseSide
    {
        public void PushData(Markets market, DataPoints dataPoint, List<ValuePoint> points)
        {
            DataPushManager.Push((ulong)market | (ulong)dataPoint, points);
        }
    }
}
