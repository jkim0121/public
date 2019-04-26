using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    /// <summary>
    /// Handles the logic of getting the Dispatch rates published by edata
    /// </summary>
    public class PJMDispRatesEdata : CommonDataControl
    {
        PJMDispRates _dataService;

        public PJMDispRatesEdata(string metadataString)
        {
            _dataService = new PJMDispRates(metadataString)
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 1 * 60,
            };
            isoTodb = ISOTime.EPTToUTC;
            dbToiso = ISOTime.UTCToEPT;
        }

        public List<LocationValuePoint> GetData(DateTime startTime, DateTime? endDate)
        {
            var data = _dataService.GetDispRatesEdataStartStop(startTime, endDate);

            if (data != null && data.Count() > 0)
            {
                var lvPoint = new List<LocationValuePoint>();
                var grpdData = data.GroupBy(x => x.TIMEPOINT);
                foreach (var grp in grpdData)
                {
                    int i = 0;
                    foreach (var dRate in grp)
                    {
                        var point = new LocationValuePoint()
                        {
                            Market = Markets.PJM,
                            DataPoint = DataPoints._15secDisp,
                            CreatedAt = dbToiso(dRate.EDITTIME),
                            Location = string.Format("{0}:{1}", ++i, dRate.DATAZONE),
                            Time = dRate.TIMEPOINT,
                            Value = dRate.VALUE
                        };
                        lvPoint.Add(point);
                    }
                }

                return lvPoint;
            }
            return new List<LocationValuePoint>();
        }
        public List<LocationValuePoint> GetLatestData(int count)
        {
            var lvPoint = new List<LocationValuePoint>();
            try
            {
                var data = _dataService.GetDispRatesEdataTop(count).ToList();

                if (data != null && data.Count() > 0)
                {
                    var grodData = data.GroupBy(x => x.TIMEPOINT);
                    foreach (var grp in grodData)
                    {
                        int i = 0;
                        foreach (var dRate in grp)
                        {

                            var point = new LocationValuePoint()
                            {
                                Market = Markets.PJM,
                                DataPoint = DataPoints._15secDisp,
                                CreatedAt = dbToiso(dRate.EDITTIME),
                                Location = string.Format("{0}:{1}", ++i, dRate.DATAZONE),
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

                var data = _dataService.GetDispRatesEdataStartStop(CurrentTimestamp.AddSeconds(5), null).OrderBy(x => x.VALUE);

                if (data != null && data.Count() > 0)
                {

                    var lvPoint = new List<LocationValuePoint>();
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
                                Location = string.Format("{0}:{1}", ++i, dRate.DATAZONE),
                                Time = dRate.TIMEPOINT,
                                Value = dRate.VALUE
                            };
                            lvPoint.Add(point);
                        }

                    }



                    if (lvPoint.Count > 0) CurrentTimestamp = lvPoint.Max(x => x.Time);
#if DEBUG

                    Debug.WriteLine(string.Format("dispRate Edata New Tick - {0}  count - {1}", Markets.PJM, lvPoint.Count()));
#endif
                    return lvPoint;
                }
            }
            catch { }

            return new List<LocationValuePoint>();
        }
        public List<string> GetLocations()
        {
            var data = _dataService.GetDispRatesEdataTop(1);
            if (data != null)
            {
                return data.Select(x => x.DATAZONE).ToList();
            }

            return new List<string>();
        }
    }
}