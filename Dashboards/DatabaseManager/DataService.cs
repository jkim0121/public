using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Linq;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Deg.DatabaseManager
{
    class DataService
    {
   
        private Dictionary<string, DataContext> _database = new Dictionary<string, DataContext>();

        public DataService( string dbServer)
        {
            var metadataString = string.Format(ConfigurationManager.AppSettings["DataConnection"], dbServer, ConfigurationManager.AppSettings["DataUser"], ConfigurationManager.AppSettings["DataPass"]);
        
            _database.Add("PJMDispRatesEdata", new DataDefinitions.PJMDispRatesEdataDataContext(string.Format(metadataString, "PJMDispRatesEdata"))
            {
                ObjectTrackingEnabled = false,             
            });

            _database.Add("PJMDispRatesWebPage", new DataDefinitions.PJMDispRatesWebPageDataContext(string.Format(metadataString, "PJMDispRatesWebPage"))
            {
                ObjectTrackingEnabled = false,
            });

            _database.Add("ISOStats", new DataDefinitions.ISOStatsDataContext(string.Format(metadataString, "ISOStats"))
            {
                ObjectTrackingEnabled = false,
            });


        }

        public string DataBaseName
        {
            get;
            set;
        }

        public Dictionary<string, DataContext> Database
        {
            get
            {
                return _database;
            }
        }
    }
}
