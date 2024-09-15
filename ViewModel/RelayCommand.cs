using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace WPF_Proyect_Food.ViewModel
{
    class RelayCommand : ICommand
    {
        // Lógica que se ejecutará cuando el comando sea invocado.
        private readonly Action<object>? _execute;
        // Funcion que determinara si el comando puede o no ejecutarse.
        private readonly Func<object, bool>? _canExecute;

        // Evento que se dispara si cambia CanExecute
        public event EventHandler? CanExecuteChanged;

        public RelayCommand(Action<object>? execute, Func<object, bool>? canExecute = null)
        {
            _execute = execute;
            _canExecute = canExecute;
        }

        // Determina si el comando puede ejecutarse
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // Se le llama cuando el comando se ejecuta
        public void Execute(object? parameter)
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }

        // Notificar que el estado CanExecute ha cambiado
        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
