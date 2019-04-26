using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{

    public class MISO5MinLMP : CommonDataControl
    {
        private MISO5minLMPDataContext _dataContext;
        private Markets _market;
        private DataPoints _dataPoint;


        public MISO5MinLMP(string metadataString)
        {
            _dataContext = new MISO5minLMPDataContext(metadataString)
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 1 * 60
            };
            isoTodb = ISOTime.ESTToMPT;
            dbToiso = ISOTime.MPTToEST;
            _market = Markets.MISO;
            _dataPoint = DataPoints._5minLmp;
        }

        public List<LocationValuePoint> GetData(DateTime startTime, DateTime? endDate)
        {
            var startTimeUTC = isoTodb(startTime);
            var endDateUTC = endDate == null ? DateTime.UtcNow : isoTodb((DateTime)endDate);

            var data = endDate == null ?
                      _dataContext.GetMISO5minLMPStartStop(startTimeUTC, null) : _dataContext.GetMISO5minLMPStartStop(startTimeUTC, endDateUTC);

            if (data != null)
            {
                return data.Select(x => new LocationValuePoint()
                {
                    Market = _market,
                    DataPoint = _dataPoint,
                    CreatedAt = dbToiso((DateTime)x.EditTime),
                    Location = x.NodeName,
                    Time = dbToiso((DateTime)x.Timepoint),
                    Value = (double)x.LMP
                }).ToList();
            }

            return new List<LocationValuePoint>();
        }
        public List<LocationValuePoint> GetLatestData(int count)
        {
            var maxTime = DateTime.Parse(_dataContext.GetMISOMaxTimepoint().First().Column1.Value.ToString());
            var data = _dataContext.GetMISO5minLMPStartStop(maxTime.AddMinutes(count * -5), maxTime);

            if (data != null)
            {
                return data.Select(x => new LocationValuePoint()
                {
                    Market = _market,
                    DataPoint = _dataPoint,
                    CreatedAt = dbToiso((DateTime)x.EditTime),
                    Location = x.NodeName,
                    Time = dbToiso((DateTime)x.Timepoint),
                    Value = (double)x.LMP
                }).ToList();
            }
            return new List<LocationValuePoint>();
        }
        public List<String> GetLocations()
        {
            var data = _dataContext.GetMISOLocations();
            return data.Select(x => x.location_name).ToList();
        }
        public List<LocationValuePoint> RefreshData()
        {
            try
            {
                if (CurrentTimestamp == DateTime.MinValue)
                {
                    var lvPoint = GetLatestData(1);
                    CurrentTimestamp = lvPoint.Max(x => x.Time);
                    //isWorking = false;
                    return lvPoint;
                }

                if (!HasData())
                {
                    return new List<LocationValuePoint>();
                }
                var data = _dataContext.GetMISO5minLMPStartStop(isoTodb(CurrentTimestamp.AddMinutes(5)), null);
                if (data != null)
                {
                    var lvPoint = data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = dbToiso((DateTime)x.EditTime),
                        Location = x.NodeName,
                        Time = dbToiso((DateTime)x.Timepoint),
                        Value = (double)x.LMP
                    }).ToList();

                    if (lvPoint.Count > 0)
                        CurrentTimestamp = lvPoint.Max(x => x.Time);
#if DEBUG
                    Debug.WriteLine(string.Format("5min New Tick - {0}  count - {1}", _market.ToString(), lvPoint.Count));
#endif
                    return lvPoint;
                }
            }
            catch { }

            return new List<LocationValuePoint>();
        }
        private bool HasData()
        {
            return DateTime.Now.Second % 10 == 0;
        }
    }
}