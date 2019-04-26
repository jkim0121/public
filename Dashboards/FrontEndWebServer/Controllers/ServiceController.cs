using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.WebSockets;
using System.Runtime.Serialization.Json;
using System.IO;
using System.ServiceModel;
using System.Configuration;
using Microsoft.Owin.Host.SystemWeb;
using Microsoft.AspNet.Identity;

using log4net;

using Deg.Dashboards.Common;

namespace Deg.FrontEndWebServer.Controllers
{
    [RoutePrefix("service")]
    public class ServiceController : ApiController, IDataPushServerCallBack
    {
        private DuplexChannelFactory<IDataPushServer> _pushFactory;
        private IDataPushServer _pushChannel;
        private WebSocket _socket;

        private ILog _log = LogManager.GetLogger(typeof(ServiceController));

        // GET: Service
        [Route("subscribe")]
        [HttpGet]
        public HttpResponseMessage Subscribe()
        {
            if (HttpContext.Current.IsWebSocketRequest)
            {
                HttpContext.Current.AcceptWebSocketRequest(MapWebSocket);
            }

            return Request.CreateResponse(HttpStatusCode.SwitchingProtocols);
        }

        [Route("markettime/{market}")]
        [HttpGet]
        public string MarketTime(string market)
        {
            var dateTime = string.Empty;
            try
            {
                // TODO: The routine will move to FrontEndManager to get unique time for all web servers
                if (string.IsNullOrWhiteSpace(market) == false)
                {
                    var localTime = default(DateTime);
                    var utcTime = DateTime.UtcNow;

                    switch (market.ToLower())
                    {
                        case "pjm":
                        case "nyiso":
                        case "iso-ne":
                            localTime = new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Local).AddHours(-5);
                            localTime = GetDaylightSavingTime(localTime);
                            break;
                        case "miso":
                            localTime = new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Local).AddHours(-5);
                            break;
                        case "ercot":
                        case "spp":
                            localTime = new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Local).AddHours(-6);
                            localTime = GetDaylightSavingTime(localTime);
                            break;
                        case "caiso":
                            localTime = new DateTime(utcTime.Year, utcTime.Month, utcTime.Day, utcTime.Hour, utcTime.Minute, utcTime.Second, DateTimeKind.Local).AddHours(-8);
                            localTime = GetDaylightSavingTime(localTime);
                            break;
                    }

                    if (localTime != default(DateTime))
                    {
                        dateTime = localTime.ToString("s");
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }

            return dateTime;
        }

        [Route("registerdatapoint/{sessionID:Guid}/{datapoint}/{parameters}")]
        [HttpGet]
        public void RegisterDataPoint(Guid sessionID, string datapoint, string parameters)
        {
            if (string.Compare(parameters, "no", true) == 0)
            {
                parameters = string.Empty;
            }

            WebShared.Instance.RegisterDataPoint(sessionID, Convert.ToUInt64(datapoint, 16), string.IsNullOrWhiteSpace(parameters) ? new string[0] : parameters.Trim('[', ']').Split(','));
        }

        [Route("marketdatabyrange/{sessionID:Guid}/{datapoint}/{starttime:datetime}/{endtime:datetime}")]
        [HttpGet]
        public string MarketDataByRange(Guid sessionID, string datapoint, DateTime starttime, DateTime? endtime)
        {
            var jsonString = "{}";

            if (WebShared.Instance.Sessions.Any(item => item.Value == sessionID))
            {
                var channel = default(IDatabaseManagerClientSide);

                try
                {
                    channel = GetDatabaseManagerChannel();
                    var id = Convert.ToUInt64(datapoint, 16);
                    var market = (Markets)(id & 0xffff000000000000);
                    var dataPoint = (DataPoints)(id & 0x0000ffffffffffff);
                    var data = channel.GetData(market, dataPoint, starttime, endtime);

                    if (data != null && data.Count > 0)
                    {
                        jsonString = string.Format("[ {0} ]", string.Join(",", data.Select(item => item.ToJson()).ToArray()));
                    }
                }
                catch (Exception ex)
                {
                    _log.Error(ex);
                }
                finally
                {
                    if (channel != null)
                    {
                        try
                        {
                            (channel as IServiceChannel).Close();
                        }
                        catch { }
                        
                    }
                }
            }

            return jsonString;
        }

        [Route("marketdatalatest/{sessionID:Guid}/{datapoint}/{count:int:min(1)}")]
        [HttpGet]
        public string MarketDataLatest(Guid sessionID, string datapoint, int? count)
        {
            var channel = default(IDatabaseManagerClientSide);
            var jsonValue = "[]";

            try
            {
                if (WebShared.Instance.Sessions.Any(item => item.Value == sessionID))
                {
                    channel = GetDatabaseManagerChannel();
                    var id = Convert.ToUInt64(datapoint, 16);
                    var market = (Markets)(id & 0xffff000000000000);
                    var dataPoint = (DataPoints)(id & 0x0000ffffffffffff);
                    var values = channel.GetLatestData(market, dataPoint, count.GetValueOrDefault(1));

                    if (dataPoint == DataPoints._15secDisp)
                    {
                        var arrangedValues = values.Cast<LocationValuePoint>().OrderBy(item => item.Location).OrderBy(item => item.Time);
                        jsonValue = string.Format("[{0}]", string.Join(",", arrangedValues.Select(item => item.ToJson()).ToArray()));
                    }
                    else
                    {
                        jsonValue = string.Format("[{0}]", string.Join(",", values.Select(item => item.ToJson()).ToArray()));
                    }

                    
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            finally
            {
                if (channel != null)
                {
                    try
                    {
                        (channel as IServiceChannel).Close();
                    }
                    catch { }

                }
            }

            return jsonValue;
        }

        [Route("tradablenodes/{sessionID:Guid}/{datapoint}")]
        [HttpGet]
        public string TradableNodes(Guid sessionID, string datapoint)
        {
            var nodes = new List<string>();
            var channel = default(IDatabaseManagerClientSide);

            try
            {
                if (WebShared.Instance.Sessions.Any(item => item.Value == sessionID))
                {
                    channel = GetDatabaseManagerChannel();
                    var id = Convert.ToUInt64(datapoint, 16);
                    var market = (Markets)(id & 0xffff000000000000);
                    var dataPoint = (DataPoints)(id & 0x0000ffffffffffff);
                    nodes = channel.GetLocations(market, dataPoint);
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
            finally
            {
                if (channel != null)
                {
                    try
                    {
                        (channel as IServiceChannel).Close();
                    }
                    catch { }

                }
            }

            return string.Format("[ {0} ]", string.Join(",", nodes.Select(node => string.Format("'{0}'", node))).ToArray());
        }

        [Route("loadlayout/{sessionID:Guid}")]
        [HttpGet]
        public string LoadLayout(Guid sessionID)
        {
            var jsonString = "{}";

            try
            {
                if (WebShared.Instance.Sessions.Any(item => item.Value == sessionID))
                {

                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            return jsonString;
        }

        public async Task MapWebSocket(AspNetWebSocketContext context)
        {
            try
            {
                _socket = context.WebSocket;
                var buffer = new byte[1024];

                while (_socket.State == WebSocketState.Open)
                {
                    try
                    {
                        var accepted = (bool?)false;
                        var result = await _socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        switch (result.MessageType)
                        {
                            case WebSocketMessageType.Close:
                                await _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "final", CancellationToken.None);
                                _socket = null;

                                // Automatic Logout here
                                _pushChannel.UnregisterPush();
                                accepted = true;
                                break;
                            case WebSocketMessageType.Text:
                                try
                                {
                                    var text = (new String(Encoding.UTF8.GetString(buffer).TakeWhile(x => x != '\0').ToArray())).Trim('\"');
                                    if (text.ToLower().StartsWith("ping") == false)
                                    {
                                        var sessionID = new Guid(text);
                                        if (_pushChannel != null)
                                        {
                                            _pushChannel = null;
                                        }

                                        if (_pushFactory == null)
                                        {
                                            _pushFactory = new DuplexChannelFactory<IDataPushServer>(this, ConfigurationManager.AppSettings["PushEndPointName"]);
                                            _pushChannel = _pushFactory.CreateChannel();
                                            _pushChannel.RegisterPush(sessionID);
                                        }
                                    }

                                    accepted = true;
                                }
                                catch (Exception ex)
                                {
                                    _log.Info(ex);
                                }

                                break;
                            default:
                                break;
                        }

                        if (result.MessageType != WebSocketMessageType.Close && accepted.HasValue)
                        {
                            await _socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(accepted.Value == true ? "ack" : "nak")), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                    }
                    catch (WebSocketException ex)
                    {
                        _log.Info(ex);
                    }
                    catch (Exception ex)
                    {
                        _log.Fatal(ex);
                        break;
                    }
                }
            }
            finally
            {
                if (_socket != null)
                {
                    _socket.Dispose();
                }
            }
        }

        public DateTime GetDaylightSavingTime(DateTime time)
        {
            if (time.Date.IsDaylightSavingTime() == false && time.AddDays(1).IsDaylightSavingTime())
            {
                return time.Hour >= 2 ? time.AddHours(1) : time;
            }
            else if (time.Date.IsDaylightSavingTime() && time.Date.AddDays(1).IsDaylightSavingTime() == false)
            {
                return time.Hour == 3 ? time.AddHours(-1) : time;
            }
            else if (time.IsDaylightSavingTime())
            {
                return time.AddHours(1);
            }

            return time;
        }

        public void PushData(ulong dataPointType, List<ValuePoint> datapoints)
        {
            try
            {
                if (datapoints.Count > 0)
                {
#if DEBUG
                    var test = string.Format("[ {0} ]", string.Join(",", datapoints.Select(item => item.ToJson()).ToArray()));
                    var data = datapoints.Count > 1 ? test : datapoints.First().ToJson();
#else
                    var data = datapoints.Count > 1 ? string.Format("[ {0} ]", string.Join(",", datapoints.Select(item => item.ToJson()).ToArray())) : datapoints.First().ToJson();
#endif
                    data = string.Format("{{ \"datapoint\": \"0x{0:X16}\", \"points\": {1} }}", dataPointType, data);
                    if (string.IsNullOrWhiteSpace(data) == false)
                    {
                        var result = _socket.SendAsync(new ArraySegment<byte>(Encoding.UTF8.GetBytes(data)), WebSocketMessageType.Text, true, CancellationToken.None);

                        if (result.Exception != null)
                        {
                            foreach (var ex in result.Exception.InnerExceptions)
                            {
                                _log.Error(ex);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }
        }

        public void CloseSession(Guid sessionID)
        {
            try
            {
                var authentication = HttpContext.Current.GetOwinContext().Authentication;
                if (authentication != null)
                {
                    authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie, DefaultAuthenticationTypes.ExternalCookie);
                }

                if (_socket != null)
                {
                    _socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "ack", CancellationToken.None);
                }

                Task.Run(() =>
                {
                    var sessions = WebShared.Instance.Sessions;
                    var username = sessions.FirstOrDefault(item => item.Value == sessionID).Key;
                    if (string.IsNullOrWhiteSpace(username) == false)
                    {
                        WebShared.Instance.Sessions.TryRemove(username, out sessionID);
                    }
                });

                _socket.Dispose();
            }
            catch (Exception ex)
            {
                _log.Info(ex);
            }
            finally
            {
                _socket = null;
            }
        }

        private IDatabaseManagerClientSide GetDatabaseManagerChannel()
        {
            try
            {
                var factory = new ChannelFactory<IDatabaseManagerClientSide>(ConfigurationManager.AppSettings["DatabaseEndPointName"]);
                return factory.CreateChannel();
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }

            return null;
        }
    }
}