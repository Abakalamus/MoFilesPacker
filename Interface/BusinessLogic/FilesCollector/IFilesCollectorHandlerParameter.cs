using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic.FilesCollector
{
    public interface IFilesCollectorHandlerParameter
    {
        int Year { get; }
        int Month { get; }
        IEnumerable<string> MoCollection { get; }
        IEnumerable<IFileDirectoryInfo> DirectoryInfoCollection { get; }
        string NameArchiveTemplate { get; }
    }
    public interface IFilesCollectorHandlerParameter2
    {
        IEnumerable<IFileDirectoryInfo> DirectoryInfoCollection { get; }
        string NameArchiveTemplate { get; }
    }
}
