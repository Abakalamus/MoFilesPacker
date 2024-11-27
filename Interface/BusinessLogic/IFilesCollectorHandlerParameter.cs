using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic
{
    public  interface IFilesCollectorHandlerParameter
    {
        int Year { get; }
        int Month { get; }
        IEnumerable<string> MoCollection { get; }
        IEnumerable<IFileDirectoryInfo> DirectoryInfoCollection { get; }
        string NameArchiveTemplate { get; }
    }
}
