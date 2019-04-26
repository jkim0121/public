using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deg.DataCollectionBroker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            System.ServiceProcess.ServiceBase.Run(new DataCollectionBrokerService());
        }
    }
}
