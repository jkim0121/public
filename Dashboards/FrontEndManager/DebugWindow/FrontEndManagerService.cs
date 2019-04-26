using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deg.Dashboards.Common;

namespace Deg.FrontEndManager
{
    public partial class FrontEndManagerService : INotifyPropertyChanged
    {
        private void InitializeDebugWindow()
        {
            StartServiceCommand = new DelegateCommand(o => _service.OnStart(new string[] { }));
            StopServiceCommand = new DelegateCommand(o => _service.OnStop());
            PushDataCommand = new DelegateCommand(o =>
            {
                DataPushManager.Push((ulong)Markets.PJM | (ulong)SelectedDataPoint, new List<ValuePoint>(new[] { new ValuePoint { Market = Markets.PJM, Value = _dataToPush, } }), SelectedSessionID);
            });
        }

        internal bool _isRunning = false;
        public bool IsRunning
        {
            get
            {
                return _isRunning;
            }
            set
            {
                _isRunning = value;
                if (PropertyChanged != null)
                {
                    PropertyChanged(this, new PropertyChangedEventArgs("IsRunning"));
                }
            }
        }

        public ICommand StartServiceCommand
        {
            get; private set;
        }

        public ICommand StopServiceCommand
        {
            get; private set;
        }

        public ICommand PushDataCommand
        {
            get; private set;
        }

        public ICollection<Guid> SessionIDCollection
        {
            get
            {
                return ServerManager.Sessions.Keys;
            }
        }
        public Guid SelectedSessionID
        {
            get; set;
        }

        public DataPoints SelectedDataPoint
        {
            get; set;
        }

        private double _dataToPush;
        public double DataToPush
        {
            set
            {
                _dataToPush = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
