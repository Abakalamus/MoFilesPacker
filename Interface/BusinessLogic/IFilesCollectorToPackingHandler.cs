using FilesBoxing.Interface.Visual;

using System;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFilesCollectorToPackingHandler
    {
        EventHandler<ISearchFileMoInfo> OnFilesSearchComplite { get; set; }
        EventHandler<IProcessHandleMoInfo> OnMoPackingComplite { get; set; }
        DirectoryInfo SaveFilesDirectory { get; }
        void CreatePackageFileForMoFiles(IFilesCollectorHandlerParameter parameter);
    }
}