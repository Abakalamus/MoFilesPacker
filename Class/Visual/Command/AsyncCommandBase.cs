using FilesBoxing.Interface.Visual;
using System.Threading.Tasks;
using System.Windows.Input;
using System;

namespace FilesBoxing.Class.Visual.Command
{
    public abstract class AsyncCommandBase : IAsyncCommand
    {
        public abstract bool CanExecute(object parameter);
        public abstract Task ExecuteAsync(object parameter);
        public async void Execute(object parameter)
        {
            await ExecuteAsync(parameter);
        }
        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }
        protected void RaiseCanExecuteChanged()
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}