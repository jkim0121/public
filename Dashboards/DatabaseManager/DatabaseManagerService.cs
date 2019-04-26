using System;
using System.Windows;
using System.ServiceProcess;
using System.ServiceModel;
using System.Configuration;

using log4net;

using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall)]
    public partial class DatabaseManagerService : ServiceBase
    {
        private static ILog _log = LogManager.GetLogger(typeof(DatabaseManagerService));
        private static DatabaseManagerService _service;

        private ServiceHost _serviceHost;
        private ChannelFactory<IFrontEndManagerDatabaseSide> _forwardFactory;
        private IFrontEndManagerDatabaseSide _forwardChannel;
        private DataBaseMonitor _dbMonitor;

        [STAThread]
        static void Main(string[] args)
        {
            _service = new DatabaseManagerService();
          

            if (Environment.UserInteractive)
            {
                var window = new DebugWindow()
                {
                    DataContext = _service,
                };

                window.Closing += (s, e) =>
                {
                    try
                    {
                        if (_service.IsRunning)
                        {
                            _service.OnStop();
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error(ex);
                    }
                };

                (new Application()).Run(window);
            }
            else
            {
                ServiceBase.Run(_service);
            }
        }

        public DatabaseManagerService()
        {
            ServiceName = "Dashboards - DatabaseManager";
            CanStop = true;
            CanPauseAndContinue = false;
            AutoLog = true;

            if (Environment.UserInteractive)
            {
                InitializeDebugWindow();
            }
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            try
            {
                _serviceHost = new ServiceHost(typeof(DatabaseManagerService));
                _serviceHost.Open();

                _forwardFactory = new ChannelFactory<IFrontEndManagerDatabaseSide>(ConfigurationManager.AppSettings["MainEndPointName"]);
                _forwardChannel = _forwardFactory.CreateChannel();
                _dbMonitor = new DataBaseMonitor(_forwardChannel);
                _dbMonitor.Start();

                IsRunning = true;
            }
            catch (Exception ex)
            {

            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            try
            {
               if (_forwardChannel != null && _forwardChannel is IServiceChannel)
                {
                    var channel = _forwardChannel as IServiceChannel;
                    channel.Abort();
                    channel.Close();
                    _dbMonitor.Stop();
                }

                if (_serviceHost != null)
                {
                    _serviceHost.Close();
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                _forwardChannel = null;
                _serviceHost = null;
            }

            IsRunning = false;
        }
    }
}
