using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFileUsingGroupInfo
    {
        string ExtensionFile { get; }
        IEnumerable<int> IdUsingGroups { get; }
    }

    public interface IFileDirectoryInfo : IFileUsingGroupInfo
    {
        DirectoryInfo ParentFileDirectory { get; }
    }
}