using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    /// <summary>
    /// Handles PJM Dispatchrates publishd by WebPage
    /// </summary>
    public class PJMDispRatesWebPage : CommonDataControl
    {
        private PJMDispRates _dataService;
        public PJMDispRatesWebPage(String metadataString)
        {
            _dataService = new PJMDispRates(metadataString)
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 1 * 60
            };

            isoTodb = ISOTime.EPTToUTC;
            dbToiso = ISOTime.UTCToEPT;
        }

        public List<LocationValuePoint> GetData(DateTime startTime, DateTime? endDate)
        {
            var data = _dataService.GetDispRatesWebpageStartStop(startTime, endDate);
            var lvPoint = new List<LocationValuePoint>();
            if (data != null && data.Count () > 0 )
            {
                var grpData = data.GroupBy(x => x.TIMEPOINT);
                foreach (var grp in grpData )
                {
                    int i = 0;
                    foreach (var dRate in grp )
                    {
                        var point = new LocationValuePoint()
                        {
                            Market = Markets.PJM,
                            DataPoint = DataPoints._15secDisp,
                            CreatedAt = dbToiso(dRate.EDITTIME),
                            Location = (++i).ToString(),
                            Time = dRate.TIMEPOINT,
                            Value = dRate.VALUE
                        };
                        lvPoint.Add(point);
                    }
                }
           
            }
            return lvPoint;
        }
        public List<LocationValuePoint> GetLatestData(int count)
        {
            var lvPoint = new List<LocationValuePoint>();

            try
            {
                var data = _dataService.GetDispRatesWebpageTop(count).ToList();

                if (data != null && data.Count() > 0)
                {
                    var grpData = data.GroupBy(x => x.TIMEPOINT);
                    foreach (var grp in grpData)
                    {
                        int i = 0;
                        foreach (var dRate in grp)
                        {
                            var point = new LocationValuePoint()
                            {
                                Market = Markets.PJM,
                                DataPoint = DataPoints._15secDisp,
                                CreatedAt = dbToiso(dRate.EDITTIME),
                                Location = (++i).ToString(),
                                Time = dRate.TIMEPOINT,
                                Value = dRate.VALUE
                            };
                            lvPoint.Add(point);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

            }
            return lvPoint;
        }
        public List<LocationValuePoint> RefreshData()
        {
            try
            {
                if (CurrentTimestamp == DateTime.MinValue)
                {
                    var lvPoint = GetLatestData(1);
                    CurrentTimestamp = lvPoint.Max(x => x.Time);
                    return lvPoint;
                }

                var data = _dataService.GetDispRatesWebpageStartStop(CurrentTimestamp.AddSeconds(5), null).OrderBy(x => x.VALUE).ToList();

                if (data != null && data.Count() > 0)
                {
                    var lvPoint = new List<LocationValuePoint>();

                    foreach (var grp in data.GroupBy(x => x.TIMEPOINT))
                    {
                        int i = 0;
                        foreach (var dRate in grp)
                        {
                            var point = new LocationValuePoint()
                            {
                                Market = Markets.PJM,
                                DataPoint = DataPoints._15secDisp,
                                CreatedAt = dbToiso(dRate.EDITTIME),
                                Location = (++i).ToString(),
                                Time = dRate.TIMEPOINT,
                                Value = dRate.VALUE
                            };
                            lvPoint.Add(point);
                        }
                    }

                    if (lvPoint.Count > 0) CurrentTimestamp = lvPoint.Max(x => x.Time);
#if DEBUG

                    Debug.WriteLine(string.Format("dispRate WebPage New Tick - {0}  count - {1}", Markets.PJM, lvPoint.Count()));
#endif
                    return lvPoint;
                }
            }
            catch (Exception ex)
            {
            }

            return new List<LocationValuePoint>();
        }
    }
}
