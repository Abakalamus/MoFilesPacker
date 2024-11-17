using System.IO;
using FilesBoxing.Interface;
using FilesBoxing.Interface.Factory;

namespace FilesBoxing.Class.Factory
{
    public class FactoryFileBoxingHandler : IFactoryFileBoxingHandler
    {
        public IFileBoxing GetBoxingHandler(DirectoryInfo directoryForArchive)
        {
            return new FileBoxing(directoryForArchive);
        }

        public IFilesCollector GetFilesCollector()
        {
            return new FilesCollector();
        }
    }
}