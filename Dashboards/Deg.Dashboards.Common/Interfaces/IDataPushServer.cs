using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [ServiceContract(CallbackContract = typeof(IDataPushServerCallBack))]
    public interface IDataPushServer
    {
        [OperationContract]
        void RegisterPush(Guid sessionID);
        [OperationContract]
        void UnregisterPush();
    }

    public interface IDataPushServerCallBack
    {
        [OperationContract]
        void PushData(ulong dataPointType, List<ValuePoint> datapoints);
        [OperationContract]
        void CloseSession(Guid sessionID);
    }
}
