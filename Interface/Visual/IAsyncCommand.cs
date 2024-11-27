using System.Threading.Tasks;
using System.Windows.Input;

namespace FilesBoxing.Interface.Visual
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object parameter);
    }
}