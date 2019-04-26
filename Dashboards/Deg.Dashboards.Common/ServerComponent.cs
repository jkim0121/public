using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [DataContract]
    public class ServerComponent
    {
        [DataMember]
        public Components Type
        {
            get; set;
        }

        [DataMember]
        public IPAddress Address
        {
            get; set;
        }

        [DataMember]
        public string MachineName
        {
            get; set;
        }
    }
}
