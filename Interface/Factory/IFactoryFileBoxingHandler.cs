using System.IO;
using FilesBoxing.Interface.BusinessLogic;

namespace FilesBoxing.Interface.Factory
{
    public interface IFactoryFileBoxingHandler
    {
        IFileBoxing GetBoxingHandler(DirectoryInfo directoryForArchive);
        IFilesCollector GetFilesCollector();
        IFilesCollectorNew GetFilesCollectorNew();
    }
}