using System;
using System.Collections.Generic;
using System.Linq;

namespace Deg.Dashboards.Common
{
    public interface IIsoDataCollector
    {
        string Iso { get; }
        string Source { get; }
        bool UseDaylightSavingTime { get; }
        TimeZone TimeZone { get; }
        string CertificatePath { get; set; }
    }
}
