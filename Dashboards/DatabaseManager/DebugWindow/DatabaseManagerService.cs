using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Deg.Dashboards.Common;

namespace Deg.DatabaseManager
{
    public partial class DatabaseManagerService : INotifyPropertyChanged
    {
        private void InitializeDebugWindow()
        {
            StartServiceCommand = new DelegateCommand(o => _service.OnStart(new string[] { }));
            StopServiceCommand = new DelegateCommand(o => _service.OnStop());
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

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
