using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [ServiceContract]
    public interface IFrontEndManagerDatabaseSide
    {
        [OperationContract]
        void PushData(Markets market, DataPoints dataPoint, List<ValuePoint> points);
    }
}
