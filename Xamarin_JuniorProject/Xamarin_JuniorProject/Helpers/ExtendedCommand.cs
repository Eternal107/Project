using System;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Xamarin_JuniorProject.Helpers
{
    public class ExtendedCommand : ICommand
    {
        private Func<object, Task> _command;
        private Func<object, bool> _canExecute;

        public static ExtendedCommand Create(Func<Task> func, Func<bool> canExecute = null)
        {
            return new ExtendedCommand
            {
                _command = obj => func(),
                _canExecute = obj => canExecute == null || canExecute()
            };
        }

        public static ExtendedCommand Create<T>(Func<T, Task> func, Func<T, bool> canExecute = null)
        {
            return new ExtendedCommand
            {
                _command = obj => func((T)obj),
                _canExecute = obj => canExecute == null || canExecute((T)obj)
            };
        }

        #region == ICommand Implementation --

        public event EventHandler CanExecuteChanged;

        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        public async void Execute(object parameter)
        {
            await _command(parameter);
        }

        #endregion
    }
}
