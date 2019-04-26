using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [ServiceContract]
    public interface IFrontEndManagerAdminSide
    {
        [OperationContract]
        List<Session> GetActiveSessions();

        [OperationContract]
        List<ServerComponent> GetRegistrants();

        [OperationContract]
        void ForceSignOut(Guid sessionID);
    }
}
