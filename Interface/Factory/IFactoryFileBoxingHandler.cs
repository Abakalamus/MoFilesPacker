using System.IO;

namespace FilesBoxing.Interface.Factory
{
    public interface IFactoryFileBoxingHandler
    {
        IFileBoxing GetBoxingHandler(DirectoryInfo directoryForArchive);
        IFilesCollector GetFilesCollector();
    }
}