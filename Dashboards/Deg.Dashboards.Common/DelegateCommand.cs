using System;
using System.Windows.Input;

namespace Deg.Dashboards.Common
{
    public class DelegateCommand : ICommand
    {
        private Func<object, bool> _canExecuteHandler;
        private Action<object> _executeHandler;
        private bool? _canExecuteOld = null;

        public DelegateCommand(Action<object> executeHandler, Func<object, bool> canExecuteHandler = null)
        {
            _executeHandler = executeHandler;
            _canExecuteHandler = canExecuteHandler;
        }

        public bool CanExecute(object parameter)
        {
            var canExecute = true;
            if (_canExecuteHandler != null)
            {
                canExecute = _canExecuteHandler(parameter);

                if (_canExecuteOld != canExecute && CanExecuteChanged != null)
                {
                    _canExecuteOld = canExecute;
                    CanExecuteChanged(this, EventArgs.Empty);
                }
            }

            return canExecute;
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_executeHandler != null)
            {
                _executeHandler(parameter);
            }
        }
    }
}
