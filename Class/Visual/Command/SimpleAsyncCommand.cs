using System;
using System.Threading.Tasks;

namespace FilesBoxing.Class.Visual.Command
{
    public class SimpleAsyncCommand : AsyncCommandBase
    {
        private readonly Func<Task> _command;
        public SimpleAsyncCommand(Func<Task> command)
        {
            _command = command;
        }
        public override bool CanExecute(object parameter)
        {
            return true;
        }
        public override Task ExecuteAsync(object parameter)
        {
            return _command();
        }
    }
}