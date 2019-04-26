using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    public class ERCOT5minLMP : CommonDataControl
    {
        private ERCOT5minLMPDataContext _dataContext;
        private Markets _market;
        private DataPoints _dataPoint;
        private int tempCount;

        public ERCOT5minLMP(string metadataString)
        {
            _dataContext = new ERCOT5minLMPDataContext(metadataString)
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 1 * 60
            };

            isoTodb = null;
            dbToiso = null;
            _dataPoint = DataPoints._5minLmp;
            _market = Markets.ERCOT;
        }

        public List<LocationValuePoint> GetData(DateTime startTime, DateTime? endDate)
        {
            var startTimeUTC = isoTodb(startTime);
            var endDateUTC = endDate == null ? DateTime.UtcNow : isoTodb((DateTime)endDate);

            var data = endDate == null ?
                      _dataContext.GetERCOT5minLMPStartStop(startTimeUTC, null) : _dataContext.GetERCOT5minLMPStartStop(startTimeUTC, endDateUTC);

            if (data != null)
            {
                return data.Select(x => new LocationValuePoint()
                {
                    Market = _market,
                    DataPoint = _dataPoint,
                    CreatedAt = ((DateTime)x.timepoint_cpt),
                    Location = x.name,
                    Time = ((DateTime)x.timepoint_cpt),
                    Value = (double)x.lmp
                }).ToList();
            }

            return new List<LocationValuePoint>();
        }
        public List<LocationValuePoint> GetLatestData(int count)
        {
            var maxTime = DateTime.Parse(_dataContext.GetMax5minLMPTimepoint().First().Column1.Value.ToString());
            var data = _dataContext.GetERCOT5minLMPStartStop(maxTime.AddMinutes(count * -5), maxTime);

            if (data != null)
            {
                return data.Select(x => new LocationValuePoint()
                {
                    Market = _market,
                    DataPoint = _dataPoint,
                    CreatedAt = ((DateTime)x.timepoint_cpt),
                    Location = x.name,
                    Time = ((DateTime)x.timepoint_cpt),
                    Value = (double)x.lmp
                }).ToList();
            }
            return new List<LocationValuePoint>();
        }
        public List<String> GetLocations()
        {
            var data = _dataContext.GetERCOTLocations();
            return data.Select(x => x.name).ToList();
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
                    tempCount = lvPoint.Count;
                    return lvPoint;
                }

                if (!HasData())
                {
                    return new List<LocationValuePoint>();
                }
                var data = (tempCount > 1000 || tempCount == 0) ? _dataContext.GetERCOT5minLMPStartStop((CurrentTimestamp.AddMinutes(5)), null) : _dataContext.GetERCOT5minLMPStartStop((CurrentTimestamp), null);
                if (data != null)
                {
                    var lvPoint = data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = ((DateTime)x.timepoint_cpt),
                        Location = x.name,
                        Time = ((DateTime)x.timepoint_cpt),
                        Value = (double)x.lmp
                    }).ToList();

                    if (lvPoint.Count > tempCount)
                    {
                        CurrentTimestamp = lvPoint.Max(x => x.Time);
                    }
                    else
                    {
#if DEBUG

                        Debug.WriteLine(string.Format("5min New Tick - {0}  count - {1}", _market.ToString(), 0));
#endif
                        if (tempCount > 1000) tempCount = 0;
                        return new List<LocationValuePoint>();
                    }

                    tempCount = lvPoint.Count;
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
