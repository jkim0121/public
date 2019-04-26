using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    public partial class FrontEndManagerService : IFrontEndManagerAdminSide
    {
        public List<Session> GetActiveSessions()
        {
            return ServerManager.Sessions.Values.ToList();
        }

        public List<ServerComponent> GetRegistrants()
        {
            return _components;
        }

        public void ForceSignOut(Guid sessionID)
        {
            try
            {
                var channel = default(IDataPushServerCallBack);
                CallBackChannels.TryRemove(sessionID, out channel);

                channel.CloseSession(sessionID);
            }
            catch (Exception ex)
            {

            }

        }
    }
}
