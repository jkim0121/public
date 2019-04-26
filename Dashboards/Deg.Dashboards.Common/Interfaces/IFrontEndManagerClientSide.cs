using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel;

namespace Deg.Dashboards.Common
{
    [ServiceContract]
    public interface IFrontEndManagerClientSide
    {
        [OperationContract]
        Guid RegisterSession(string username, string password, IPAddress clientAddress, string agent, IPAddress serverAddress);
        [OperationContract]
        Guid RegisterNTLMSession(string username, IPAddress clientAddress, string agent, IPAddress serverAddress);
        [OperationContract]
        void RemoveSession(Guid sessionID);
        [OperationContract]
        void RegisterDataPoint(Guid sessionID, ulong dataPointType, string[] parameter);

        [OperationContract]
        void SubscribeWebServer(IPAddress ipAddress, string machineName);
    }
}
