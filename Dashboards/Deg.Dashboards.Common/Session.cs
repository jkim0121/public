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
    public class Session
    {
        [DataMember]
        public Guid SessionID { get; set; }

        [DataMember]
        public string ClientAddress { get; set; }

        [DataMember]
        public string WebServerAddress { get; set; }

        [DataMember]
        public string Agent { get; set; }

        [DataMember]
        public string UserName { get; set; }

        [DataMember]
        public bool IsInternal { get; set; }

        [DataMember]
        public Markets MarketAccess { get; set; }
    }
}
