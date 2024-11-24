using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFileDirectoryInfo
    {
        DirectoryInfo ParentFileDirectory { get; }
        bool IsEnabled { get; }
        string ExtensionFile { get; }
        IEnumerable<int> IdUsingGroups { get; }
    }
}