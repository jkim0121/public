using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using log4net;

using Deg.Dashboards.Common;
using System.ServiceModel;

namespace Deg.FrontEndManager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession)]

    public class DataPushManager : IDataPushServer
    {
        private static ILog _log = LogManager.GetLogger(typeof(DataPushManager));
        private IDataPushServerCallBack _callbackChannel;

        public Guid SessionID
        {
            get;
            private set;

        }

        public void RegisterPush(Guid sessionID)
        {
            try
            {
                while (_callbackChannel != null)
                {
                    _callbackChannel = null;
                    FrontEndManagerService.CallBackChannels.TryRemove(sessionID, out _callbackChannel);
                }

                _callbackChannel = OperationContext.Current.GetCallbackChannel<IDataPushServerCallBack>();
                var channel = _callbackChannel as IServiceChannel;
                if (channel.State != CommunicationState.Opened)
                {
                    channel.Open();
                }

                FrontEndManagerService.CallBackChannels.TryAdd(sessionID, _callbackChannel);
                SessionID = sessionID;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        public void UnregisterPush()
        {
            try
            {
                var callbackChannel = default(IDataPushServerCallBack);
                FrontEndManagerService.CallBackChannels.TryRemove(SessionID, out callbackChannel);

                Task.Run(() => callbackChannel.CloseSession(SessionID));

            }
            catch (Exception ex)
            {

            }
        }

        public static void Push(ulong dataPointType, List<ValuePoint> datapoints, Guid? sessionID = null)
        {
            try
            {
                if (datapoints.Count > 0)
                {
                    if (sessionID.HasValue) // This block has to be used only with Debug Console
                    {
                        var channel = default(IDataPushServerCallBack);
                        FrontEndManagerService.CallBackChannels.TryGetValue(sessionID.GetValueOrDefault(), out channel);
                        if (channel != null)
                        {
                            channel.PushData(dataPointType, datapoints);
                        }
                    }
                    else
                    {
                        FrontEndManagerService.CallBackChannels.Values.AsParallel().ForAll(channel =>
                        {
                            var targets = FrontEndManagerService.Registrations.Where(x => x.Value.Any(y => y.Item1 == dataPointType));
                            foreach (var target in targets)
                            {
                                if (ServerManager.Sessions.ContainsKey(target.Key))
                                {
                                    var locationGroup = target.Value.FirstOrDefault(x => x.Item1 == dataPointType);
                                    if (locationGroup.Item2 != null && locationGroup.Item2.Length > 0 && datapoints.First() is LocationValuePoint)
                                    {
                                        datapoints = datapoints.Where(x => locationGroup.Item2.Contains((x as LocationValuePoint).Location)).ToList();
                                    }
                                    channel.PushData(dataPointType, datapoints);
                                }
                            }
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }
    }
}
