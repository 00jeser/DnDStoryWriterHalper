using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DnDStoryWriterHalper.Services
{
    public class Command : ICommand
    {
        private Action<object> func;
        public Command(Action<object> func)
        {
            this.func = func;
        }

        private bool _canExecute = true;

        public bool CanCommandExecute
        {
            get => _canExecute;
            set
            {
                _canExecute = value;
                CanExecuteChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        public bool CanExecute(object? parameter)
        {
            return CanCommandExecute;
        }

        public void Execute(object? parameter)
        {
            if(parameter != null)
                func(parameter);
        }

        public event EventHandler? CanExecuteChanged;
    }
}
