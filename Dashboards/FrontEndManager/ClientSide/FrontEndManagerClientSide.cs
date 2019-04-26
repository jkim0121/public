using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;
using System.Collections.Concurrent;
using System.Configuration;

using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    public partial class FrontEndManagerService : IFrontEndManagerClientSide
    {
        internal DashBoardsDatabaseDataContext Database
        {
            get;
            private set;
        } = new DashBoardsDatabaseDataContext(ConfigurationManager.ConnectionStrings["UserDatabase"].ConnectionString);

        public static ConcurrentDictionary<Guid, IDataPushServerCallBack> CallBackChannels
        {
            get;
        } = new ConcurrentDictionary<Guid, IDataPushServerCallBack>();

        public static ConcurrentDictionary<Guid, List<Tuple<ulong, string[]>>> Registrations
        {
            get;
        } = new ConcurrentDictionary<Guid, List<Tuple<ulong, string[]>>>();

        public void SubscribeWebServer(IPAddress ipAddress, string machineName)
        {
            lock (this)
            {
                try
                {
                    if (_components.All(item => string.Compare(item.MachineName, machineName, true) != 0))
                    {
                        _components.Add(new ServerComponent
                        {
                            Address = ipAddress,
                            MachineName = machineName,
                            Type = Components.WebServer,
                        });
                    }
                }
                catch (Exception ex)
                {
                    _log.Fatal(ex);
                }
            }
        }

        public Guid RegisterSession(string username, string password, IPAddress clientAddress, string agent, IPAddress serverAddress)
        {
            try
            {
                var entry = Database.tbl_dashboardusers.FirstOrDefault(row => string.Compare(username, row.username, true) == 0 && password == row.password);
                if (entry != null)
                {
                    var sessionID = Guid.NewGuid();
                    var session = new Session
                    {
                        Agent = agent,
                        ClientAddress = clientAddress.ToString(),
                        UserName = username,
                        SessionID = sessionID,
                        WebServerAddress = serverAddress.ToString(),
                        IsInternal = false,
                    };

                    ServerManager.Sessions.TryAdd(sessionID, session);

                    return sessionID;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return Guid.Empty;
        }

        public Guid RegisterNTLMSession(string username, IPAddress clientAddress, string agent, IPAddress serverAddress)
        {
            try
            {
                if (Database.tbl_dashboardusers.Any(row => string.Compare(username, row.username, true) == 0))
                {
                    var sessionID = Guid.NewGuid();
                    var session = new Session
                    {
                        Agent = agent,
                        ClientAddress = clientAddress.ToString(),
                        UserName = username,
                        SessionID = sessionID,
                        WebServerAddress = serverAddress.ToString(),
                        IsInternal = true,
                    };

                    ServerManager.Sessions.TryAdd(sessionID, session);

                    return sessionID;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return Guid.Empty;
        }


        public void RemoveSession(Guid sessionID)
        {
            var session = default(Session);
            ServerManager.Sessions.TryRemove(sessionID, out session);
        }

        public void RegisterDataPoint(Guid sessionID, ulong dataPointType, string[] parameter)
        {
            if (Registrations.ContainsKey(sessionID))
            {
                var existingPoint = false;
                var currentRegistration = Registrations[sessionID].FirstOrDefault(item => item.Item1 == dataPointType);
                if (currentRegistration != null && parameter != null && parameter.Length > 0)
                {
                    if (currentRegistration.Item2.Length == parameter.Length)
                    {
                        existingPoint = true;
                        for (var i = 0; i < parameter.Length; i++)
                        {
                            if (string.Compare(currentRegistration.Item2[i], parameter[i], true) != 0)
                            {
                                existingPoint = false;
                                break;
                            }
                        }
                    }
                }

                if (existingPoint == false)
                {
                    Registrations[sessionID].Add(new Tuple<ulong, string[]>(dataPointType, parameter));
                }
            }
            else
            {
                Registrations.TryAdd(sessionID, new List<Tuple<ulong, string[]>>(new[] { new Tuple<ulong, string[]>(dataPointType, parameter), }));
            }
        }
    }
}
