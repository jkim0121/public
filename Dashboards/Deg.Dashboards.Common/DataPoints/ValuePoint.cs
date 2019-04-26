using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [DataContract]
    [KnownType(typeof(LocationValuePoint))]
    public class ValuePoint
    {
        [DataMember]
        public Markets Market
        {
            get; set;
        }

        [DataMember]
        public DataPoints DataPoint
        {
            get; set;
        }

        [DataMember]
        public double? Value
        {
            get; set;
        }

        [DataMember]
        public DateTime Time
        {
            get; set;
        }

        [DataMember]
        public DateTime CreatedAt
        {
            get; set;
        }

        public virtual string ToJson()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("\"market\" : \"{0}\", ", Market.ToString());
            sb.AppendFormat("\"typeid\" : \"{0}\", ", DataPoint.ToString("x"));
            sb.AppendFormat("\"date\" : \"{0}\", ", Time.ToString("MM/dd/yyyy"));
            sb.AppendFormat("\"time\" : \"{0}\", ", Time.ToString("HH:mm:ss"));
            sb.AppendFormat("\"created\" : \"{0}\", ", CreatedAt.ToString("s"));
            sb.AppendFormat("\"value\" : \"{0:F2}\" ", Value);
            return string.Format("{{ {0} }}", sb.ToString());
        }
    }

    [DataContract]
    public class LocationValuePoint : ValuePoint
    {
        [DataMember]
        public string Location;

        public override string ToJson()
        {
            return base.ToJson().Replace("}", string.Format(", \"location\" : \"{0}\" }}", Location));
        }
    }
}
