using System.IO;

namespace FilesBoxing.Interface
{
    public interface IFactory
    {
        IFileBoxing GetBoxingHandler(DirectoryInfo directoryForArchive);
        IFilesCollector GetFilesCollector();
    }
}