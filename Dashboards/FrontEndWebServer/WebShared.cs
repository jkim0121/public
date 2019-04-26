using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.ServiceModel;
using System.Net.WebSockets;
using System.Configuration;

using log4net;

using Deg.Dashboards.Common;
using System.Diagnostics;
using System.Web;
using Deg.FrontEndWebServer.Controllers;

namespace Deg.FrontEndWebServer
{
    public class WebShared : MarshalByRefObject
    {
        private ILog _log;

        private ChannelFactory<IFrontEndManagerClientSide> _forwardFactory;
        private IFrontEndManagerClientSide _forwardChannel;

        private IPAddress _currentAddress;

        public WebShared()
        {
            _log = LogManager.GetLogger(typeof(WebShared));
        }

        public static WebShared Instance
        {
            get
            {
                var domain = Utility.GetAppDomains().FirstOrDefault(item => item.FriendlyName == "Dashboard_Share");
                if (domain == null)
                {
                    domain = AppDomain.CreateDomain("Dashboard_Share");
                    var connector = new WebShared();
                    connector.Connect();
                    domain.SetData("connector", connector);
                }

                return domain.GetData("connector") as WebShared;
            }
        }

        public void Connect()
        { 
            try
            {
                if (_forwardFactory == null)
                {
                    _forwardFactory = new ChannelFactory<IFrontEndManagerClientSide>(ConfigurationManager.AppSettings["MainEndPointName"]);
                    _forwardChannel = CreateForwardChannel();
                    _currentAddress = Dns.GetHostEntry(Dns.GetHostName()).AddressList.Where(o => o.AddressFamily == AddressFamily.InterNetwork).First();
                    _forwardChannel.SubscribeWebServer(_currentAddress, Environment.MachineName);
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);

            }
        }

        private IFrontEndManagerClientSide CreateForwardChannel()
        {
            try
            {
                var channel = _forwardFactory.CreateChannel();

                while (_forwardFactory.State != CommunicationState.Opened)
                {
                    try
                    {
                        channel = _forwardFactory.CreateChannel();
                    }
                    catch (Exception ex)
                    {
                        _log.Info(ex);
                        Thread.SpinWait(1000);
                    }
                }

                return channel;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }

            return null;

        }

        public Guid RegisterSession(string username, string password, IPAddress clientAddress, string agent, bool isInternal)
        {
            try
            {
                if (_forwardChannel != null)
                {
                    var sessionID = isInternal ? _forwardChannel.RegisterNTLMSession(username, clientAddress, agent, _currentAddress) : _forwardChannel.RegisterSession(username, password, clientAddress, agent, _currentAddress);
                    if (sessionID != Guid.Empty)
                    {
                        var oldID = default(Guid);
                        Sessions.TryRemove(username, out oldID);

                        Sessions.TryAdd(username, sessionID);
                        return sessionID;
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return Guid.Empty;
        }

        private ConcurrentDictionary<string, Guid> _sessions = new ConcurrentDictionary<string, Guid>();

        public ConcurrentDictionary<string, Guid> Sessions
        {
            get
            {
                return _sessions;
            }
        }

        public void RegisterDataPoint(Guid sessionID, ulong dataPointType, string[] parameters)
        {
            try
            {
                _forwardChannel.RegisterDataPoint(sessionID, dataPointType, parameters);
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            
        }

    }
}