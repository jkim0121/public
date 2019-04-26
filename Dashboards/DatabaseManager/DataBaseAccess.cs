using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Deg.Dashboards.Common;
using System.Configuration;

namespace Deg.DatabaseManager
{
    public partial class DatabaseManagerService : IDatabaseManagerClientSide
    {
        public List<ValuePoint> GetData(Markets market, DataPoints dataPoint, DateTime startTime, DateTime? endDate)
        {
            switch (market)
            {
                case Markets.PJM:
                    switch (dataPoint)
                    {
                        case DataPoints._15secDisp:
                            return DataBaseMonitor._dispRateEdata.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._DALmp:
                            return DataBaseMonitor._pjmDA.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._pjmRT.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._pjm5minLMP.GetData(startTime, endDate).Cast<ValuePoint>().ToList();                      
                    }
                    break;
                case Markets.MISO:
                    switch (dataPoint)
                    {
                        case DataPoints._DALmp:
                            return DataBaseMonitor._misoDA.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._misoRT.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._miso5minLMP.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                    }
                    break;
                case Markets.ERCOT:
                    switch (dataPoint)
                    {
                        case DataPoints._DALmp:
                            return DataBaseMonitor._ercotDA.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._ercotRT.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._ercot5minLMP.GetData(startTime, endDate).Cast<ValuePoint>().ToList();
                    }
                    break;
            }
            return null;
        }
        public List<ValuePoint> GetLatestData(Markets market, DataPoints dataPoint, int count)
        {
            switch (market)
            {
                case Markets.PJM:
                    switch (dataPoint)
                    {
                        case DataPoints._15secDisp:
                            return DataBaseMonitor._dispRateEdata.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._DALmp:
                            return DataBaseMonitor._pjmDA.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._pjmRT.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._pjm5minLMP.GetLatestData(count).Cast<ValuePoint>().ToList();
                    }
                    break;
                case Markets.MISO:
                    switch (dataPoint)
                    {
                        case DataPoints._DALmp:
                            return DataBaseMonitor._misoDA.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._misoRT.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._miso5minLMP.GetLatestData(count).Cast<ValuePoint>().ToList();
                    }
                    break;
                case Markets.ERCOT:
                    switch (dataPoint)
                    {
                        case DataPoints._DALmp:
                            return DataBaseMonitor._ercotDA.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._HourlyLmp:
                            return DataBaseMonitor._ercotRT.GetLatestData(count).Cast<ValuePoint>().ToList();
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._ercot5minLMP.GetLatestData(count).Cast<ValuePoint>().ToList();
                    }
                    break;
            }
            return null;
        }
        public List<string> GetLocations(Markets market, DataPoints dataPoint)
        {
           switch(market)
            {
                case Markets.PJM:
                    switch (dataPoint)
                    {
                        case DataPoints._15secDisp:
                            return DataBaseMonitor._dispRateEdata.GetLocations();
                        case DataPoints._HourlyLmp:
                        case DataPoints._DALmp:
                            return DataBaseMonitor._pjmRT.GetLocations(Markets.PJM);
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._pjm5minLMP.GetLocations();                  
                    }
                    break;
                case Markets.MISO:
                    switch(dataPoint)
                    {
                        case DataPoints._HourlyLmp:
                        case DataPoints._DALmp:
                            return DataBaseMonitor._misoRT.GetLocations(Markets.MISO);
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._miso5minLMP.GetLocations();
                    }
                    break;
                case Markets.ERCOT:
                    switch(dataPoint)
                    {
                        case DataPoints._HourlyLmp:
                        case DataPoints._DALmp:
                            return DataBaseMonitor._ercotRT.GetLocations(Markets.ERCOT);
                        case DataPoints._5minLmp:
                            return DataBaseMonitor._ercot5minLMP.GetLocations();
                    }
                    break;
            }
            return new List<string>();
        }

    }

    public partial class DataBaseMonitor
    {
        private static string metadataString = string.Format(ConfigurationManager.AppSettings["DataConnection"], ConfigurationManager.AppSettings["DataServer"], ConfigurationManager.AppSettings["DataUser"], ConfigurationManager.AppSettings["DataPass"]);
        
        public static PJMDispRatesEdata _dispRateEdata = new PJMDispRatesEdata(metadataString);
        public static PJMDispRatesWebPage _dispRateWebpage = new PJMDispRatesWebPage(metadataString);
        public static ISOStats _pjmRT = new ISOStats(metadataString, Markets.PJM, DataPoints._HourlyLmp);
        public static ISOStats _pjmDA = new ISOStats(metadataString, Markets.PJM, DataPoints._DALmp);
        public static ISOStats _misoRT = new ISOStats(metadataString, Markets.MISO, DataPoints._HourlyLmp);
        public static ISOStats _misoDA = new ISOStats(metadataString, Markets.MISO, DataPoints._DALmp);
        public static ISOStats _ercotRT = new ISOStats(metadataString, Markets.ERCOT, DataPoints._HourlyLmp);
        public static ISOStats _ercotDA = new ISOStats(metadataString, Markets.ERCOT, DataPoints._DALmp);
        public static PJM5MinLMP _pjm5minLMP = new PJM5MinLMP(metadataString);
        public static MISO5MinLMP _miso5minLMP = new MISO5MinLMP(metadataString);
        public static ERCOT5minLMP _ercot5minLMP = new ERCOT5minLMP(metadataString);

        public void PollDatabase(object state)
        {
            Parallel.Invoke(
                    () => PushData(Markets.PJM, DataPoints._15secDisp, _dispRateEdata.RefreshData()),
                    () => PushData(Markets.PJM, DataPoints._15secDisp, _dispRateWebpage.RefreshData()),
                    () => PushData(Markets.PJM, DataPoints._DALmp, _pjmDA.RefreshData()),
                    () => PushData(Markets.PJM, DataPoints._HourlyLmp, _pjmRT.RefreshData()),
                    () => PushData(Markets.PJM, DataPoints._5minLmp, _pjm5minLMP.RefreshData()),
                    () => PushData(Markets.MISO, DataPoints._HourlyLmp, _misoRT.RefreshData()),
                    () => PushData(Markets.MISO, DataPoints._DALmp, _misoDA.RefreshData()),
                    () => PushData(Markets.ERCOT, DataPoints._DALmp, _miso5minLMP.RefreshData()),
                    () => PushData(Markets.ERCOT, DataPoints._HourlyLmp, _ercotRT.RefreshData()),
                    () => PushData(Markets.ERCOT, DataPoints._DALmp, _ercotDA.RefreshData()),
                    () => PushData(Markets.ERCOT, DataPoints._DALmp, _ercot5minLMP.RefreshData())
                );
        }

        private void PushData(Markets market, DataPoints dataPoint, List<LocationValuePoint> valuePoint)
        {
            if (valuePoint.Count > 0)
            {
                _channel.PushData(market, dataPoint, valuePoint.Cast<ValuePoint>().ToList());
            }
        }
    }
}
