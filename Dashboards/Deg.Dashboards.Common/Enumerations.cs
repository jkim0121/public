using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Deg.Dashboards.Common
{
    [DataContract]
    [Flags]
    public enum Markets : ulong
    {
        [EnumMember]
        NA      = 0x0000000000000000,
        [EnumMember]
        PJM     = 0x0001000000000000,
        [EnumMember]
        MISO    = 0x0002000000000000,
        [EnumMember]
        ERCOT   = 0x0004000000000000,
        [EnumMember]
        SPP     = 0x0008000000000000,
        [EnumMember]
        CAISO   = 0x0010000000000000,
        [EnumMember]
        NYISO   = 0x0020000000000000,
        [EnumMember]
        ISONE   = 0x0040000000000000,
    }

    [DataContract]
    [Flags]
    public enum DataPoints : ulong
    {
        [Description("5 min LMP")]
        [EnumMember]
        _5minLmp    = 0x0000000000000001,
        [Description("DA LMP")]
        [EnumMember]
        _DALmp      = 0x0000000000000002,
        [Description("Hourly LMP")]
        [EnumMember]
        _HourlyLmp  = 0x0000000000000004,
        [Description("SPP")]
        [EnumMember]
        _15minSPP   = 0x0000000000000008,
        [Description("HASP")]
        [EnumMember]
        _15minHASP  = 0x0000000000000010,
        [Description("Dispatch Rates")]
        [EnumMember]
        _15secDisp = 0x0000000000000020,
    }

    [DataContract]
    public enum Components
    {
        [EnumMember]
        WebServer,
        [EnumMember]
        DataCollectionBroker,
        [EnumMember]
        DatabaseManager,
        [EnumMember]
        DataCollector,
    }
}
