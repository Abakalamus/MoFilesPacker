using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFilesCollectorToBoxingHandler
    {
        DirectoryInfo SaveFilesDirectory { get; }
        void CreateBoxingFileForMoCollectionFiles(IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection,
            string nameArchive);
    }
}