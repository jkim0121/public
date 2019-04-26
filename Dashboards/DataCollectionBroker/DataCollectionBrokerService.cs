using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceProcess;

namespace Deg.DataCollectionBroker
{
    public class DataCollectionBrokerService : ServiceBase
    {
        public DataCollectionBrokerService()
        {
            ServiceName = "Dashboards - DataCollectionBroker";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = true;
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);
        }
    }
}
