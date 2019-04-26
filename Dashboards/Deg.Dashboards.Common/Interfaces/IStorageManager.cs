using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;

namespace Deg.Dashboards.Common
{
    [ServiceContract]
    public interface IStorageManager
    {
        [OperationContract]
        void RegisterCollector(IIsoDataCollector collector);
        [OperationContract]
        void ReportError(IIsoDataCollector collector, Exception ex);
    }
}
