using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    public class ISOStats : CommonDataControl
    {
        private Markets _market;
        private DataPoints _dataPoint;
        private ISOStatsDataContext _dataContext;
        private bool isWorking;
        public ISOStats(string metadataString, Markets market, DataPoints dataPoint)
        {
            _market = market;
            _dataPoint = dataPoint;
            _dataContext = new ISOStatsDataContext(metadataString)
            {
                ObjectTrackingEnabled = false,
                CommandTimeout = 1 * 60
            };

            switch (market)
            {
                case Markets.MISO:
                    isoTodb = ISOTime.ESTToUTC;
                    dbToiso = ISOTime.UTCToEST;
                    break;
                case Markets.PJM:
                case Markets.NYISO:
                case Markets.ISONE:
                    isoTodb = ISOTime.EPTToUTC;
                    dbToiso = ISOTime.UTCToEPT;
                    break;
                case Markets.CAISO:
                    isoTodb = ISOTime.PPTToUTC;
                    dbToiso = ISOTime.UTCToPPT;
                    break;
                case Markets.ERCOT:
                case Markets.SPP:
                    isoTodb = ISOTime.CPTToUTC;
                    dbToiso = ISOTime.UTCToCPT;
                    break;
                default:
                    throw new NotImplementedException("ISO Not Implemented");
            }
        }
        public List<LocationValuePoint> GetData(DateTime startTime, DateTime? endDate)
        {
            var startTimeUTC = isoTodb(startTime);
            var endDateUTC = endDate == null ? DateTime.UtcNow : isoTodb((DateTime)endDate);

            var statType = _dataPoint == DataPoints._DALmp ? "DA LMP" : _dataPoint == DataPoints._HourlyLmp ? "RT LMP" : string.Empty;

            if (_dataPoint == DataPoints._HourlyLmp)
            {
                var data = endDate == null ?
                    _dataContext.GetRTLMPStartStop(_market.ToString(), startTimeUTC, null) : _dataContext.GetRTLMPStartStop(_market.ToString(), startTimeUTC, endDateUTC);

                if (data != null)
                {
                    return data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = dbToiso(x.Timepoint),
                        Location = x.Location,
                        Time = dbToiso(x.Timepoint),
                        Value = (double)x.VALUE
                    }).ToList();
                }
            }
            else if (_dataPoint == DataPoints._DALmp)
            {
                var data = endDate == null ?
                    _dataContext.GetDALMPStartStop(_market.ToString(), startTimeUTC, null) : _dataContext.GetDALMPStartStop(_market.ToString(), startTimeUTC, endDateUTC);

                if (data != null)
                {
                    return data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = dbToiso(x.Timepoint),
                        Location = x.Location,
                        Time = dbToiso(x.Timepoint),
                        Value = (double)x.VALUE
                    }).ToList();
                }

            }

            return new List<LocationValuePoint>();
        }
        public List<LocationValuePoint> GetLatestData(int count)
        {
            var statType = _dataPoint == DataPoints._DALmp ? "DA LMP" : _dataPoint == DataPoints._HourlyLmp ? "RT LMP" : string.Empty;
            var maxTime = DateTime.Parse(_dataContext.GetMAXStatTime(_market.ToString(), statType).First().Column1.Value.ToString());

            if (_dataPoint == DataPoints._HourlyLmp)
            {
                var data = _dataContext.GetRTLMPStartStop(_market.ToString(), maxTime.AddHours(count * -1), maxTime);

                if (data != null)
                {
                    return data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = dbToiso(x.Timepoint),
                        Location = x.Location,
                        Time = dbToiso(x.Timepoint),
                        Value = (double)x.VALUE
                    }).ToList();
                }
            }
            else if (_dataPoint == DataPoints._DALmp)
            {
                var data = _dataContext.GetDALMPStartStop(_market.ToString(), maxTime.AddHours(count * -1), maxTime);
                if (data != null)
                {
                    return data.Select(x => new LocationValuePoint()
                    {
                        Market = _market,
                        DataPoint = _dataPoint,
                        CreatedAt = dbToiso(x.Timepoint),
                        Location = x.Location,
                        Time = dbToiso(x.Timepoint),
                        Value = (double)x.VALUE
                    }).ToList();
                }
            }

            return new List<LocationValuePoint>();
        }
        public List<LocationValuePoint> RefreshData()
        {
            if (isWorking)
            {
                return new List<LocationValuePoint>();
            }
            else
            {
                isWorking = true;
            }
            try
            {
                if (CurrentTimestamp == DateTime.MinValue)
                {
                    var lvPoint = GetLatestData(1);
                    CurrentTimestamp = lvPoint.Max(x => x.Time);
                    isWorking = false;
                    return lvPoint;
                }

                if (!HasData(_dataPoint))
                {
                    isWorking = false;
                    return new List<LocationValuePoint>();
                }

                var statType = _dataPoint == DataPoints._DALmp ? "DA LMP" : _dataPoint == DataPoints._HourlyLmp ? "RT LMP" : string.Empty;

                if (_dataPoint == DataPoints._HourlyLmp)
                {
                    var data = _dataContext.GetRTLMPStartStop(_market.ToString(), isoTodb(CurrentTimestamp.AddHours(1)), null);

                    if (data != null)
                    {
                        var lvPoint = data.Select(x => new LocationValuePoint()
                        {
                            Market = _market,
                            DataPoint = _dataPoint,
                            CreatedAt = dbToiso(x.Timepoint),
                            Location = x.Location,
                            Time = dbToiso(x.Timepoint),
                            Value = (double)x.VALUE
                        }).ToList();

                        if (lvPoint.Count > 0)
                            CurrentTimestamp = lvPoint.Max(x => x.Time);
                        isWorking = false;
                        return lvPoint;
                    }
                }
                else if (_dataPoint == DataPoints._DALmp)
                {
                    var data = _dataContext.GetDALMPStartStop(_market.ToString(), isoTodb(CurrentTimestamp.AddHours(1)), null);

                    if (data != null)
                    {
                        var lvPoint = data.Select(x =>
                                new LocationValuePoint()
                                {
                                    Market = _market,
                                    DataPoint = _dataPoint,
                                    CreatedAt = dbToiso(x.Timepoint),
                                    Location = x.Location,
                                    Time = dbToiso(x.Timepoint),
                                    Value = (double)x.VALUE
                                }).ToList();

                        if (lvPoint.Count > 0)
                            CurrentTimestamp = lvPoint.Max(x => x.Time);
                        isWorking = false;

                        return lvPoint;
                    }
                }
            }
            catch { }
            isWorking = false;
            return new List<LocationValuePoint>();
        }
        private bool HasData(DataPoints dataPoint)
        {
            ////This function will determine if a new data is available for the given datapoint. 
            bool hasData = true;
            switch (dataPoint)
            {
                case DataPoints._HourlyLmp:

                    if (CurrentTimestamp.AddHours(1) >= dbToiso(DateTime.UtcNow) || DateTime.Now.Second % 20 != 0)
                    {
                        hasData = false;
                    }
                    else
                    {
                        hasData = true;
                    }
                    break;
                case DataPoints._DALmp:
                    if (CurrentTimestamp >= dbToiso(DateTime.UtcNow))
                    {
                        hasData = false;
                    }
                    else
                    {
                        hasData = true;
                    }
                    break;

            }

            return hasData;
        }
        public List<String> GetLocations(Markets market)
        {
            var data = _dataContext.GetISOLocations(market.ToString());
            return data.Select(x => x.Location).ToList();
        }
    }
}
