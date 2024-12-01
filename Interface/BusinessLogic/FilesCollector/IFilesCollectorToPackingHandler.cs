using FilesBoxing.Interface.Visual;

using System;
using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic.FilesCollector
{
    public interface IFilesCollectorToPackingHandler
    {
        EventHandler<ISearchFileMoInfo> OnFilesSearchComplite { get; set; }
        EventHandler<IProcessHandleMoInfo> OnMoPackingComplite { get; set; }
        DirectoryInfo SaveDirectory { get; }
        ICollection<IFileDirectoryInfo> DirectoryInfoCollection { get; set; }
        void CreatePackageFileForMoFiles(IReadOnlyCollection<IMoWithName> collectionInfo, byte countTask);
        void CreatePackageFileForMoFiles(string codeMo, string archiveName);

    }
}