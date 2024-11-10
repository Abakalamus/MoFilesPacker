using FilesBoxing.Interface;

using System.IO;

namespace FilesBoxing.Class
{
    public class Factory : IFactory
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