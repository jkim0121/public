using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [ServiceContract]
    public interface IDatabaseManagerClientSide
    {
        [OperationContract]
        List<ValuePoint> GetData(Markets market, DataPoints dataPoint, DateTime startTime, DateTime? endDate);

        [OperationContract]
        List<ValuePoint> GetLatestData(Markets market, DataPoints dataPoint, int count);

        [OperationContract]
        List<string> GetLocations(Markets market, DataPoints dataPoint);
    }
}
