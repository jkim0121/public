using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deg.DatabaseManager
{
    public class CommonDataControl
    {
        public Func<DateTime, DateTime> isoTodb;
        public Func<DateTime, DateTime> dbToiso;
        public DateTime CurrentTimestamp { get; set; }
        public DateTime PublishTiemstamp { get; set; }
    }
}
