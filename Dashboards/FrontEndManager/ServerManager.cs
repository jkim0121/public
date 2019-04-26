using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    public class ServerManager
    {
        public static ConcurrentDictionary<Guid, Session> Sessions = new ConcurrentDictionary<Guid, Session>();
    }
}
