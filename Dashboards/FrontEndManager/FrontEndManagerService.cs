using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Threading;
using System.Threading.Tasks;
using System.ServiceProcess;
using System.Net;
using System.ServiceModel;
using System.ComponentModel;
using System.Windows.Input;

using log4net;

using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public partial class FrontEndManagerService : ServiceBase
    {
        private static ILog _log = LogManager.GetLogger(typeof(FrontEndManagerService));

        private static FrontEndManagerService _service;

        private ServiceHost _serviceHost;
        private ServiceHost _pushHost;

        [STAThread]
        static void Main(string[] args)
        {
            _service = new FrontEndManagerService();

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

        public FrontEndManagerService()
        {
            ServiceName = "Dashboards - FrontEndManager";
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
                _serviceHost = new ServiceHost(typeof(FrontEndManagerService));
                _serviceHost.Open();

                _pushHost = new ServiceHost(typeof(DataPushManager));
                _pushHost.Open();

                IsRunning = true;
            }
            catch (Exception ex)
            {
                _log.Fatal(ex);
            }
        }

        protected override void OnStop()
        {
            base.OnStop();

            try
            {
                if (_serviceHost != null)
                {
                    _serviceHost.Close();
                }

                IsRunning = false;
            }
            finally
            {
                _serviceHost = null;
            }

        }
    }
}
