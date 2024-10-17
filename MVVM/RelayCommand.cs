using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace FitTrack.MVVM
{
    public class RelayCommand : ICommand
    {
        // Fält för att hålla referenserna till metoder som definierar vad som ska göras (Execute). //
        private Action<object> execute;

        // Kollar om kommandot kan köras. //
        private Func<object, bool> canExecute;

        // Event som signalerar när kommandots möjlighet att köras har ändrats. //
        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            return canExecute == null || canExecute(parameter);
        }

        public void Execute(object? parameter)
        {
            execute(parameter);
        }
    }
}
