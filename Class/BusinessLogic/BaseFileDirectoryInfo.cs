using FilesBoxing.Interface.BusinessLogic;

using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Class.BusinessLogic
{
    public class BaseFileDirectoryInfo : IFileDirectoryInfo
    {
        public DirectoryInfo ParentFileDirectory { get; }
        public string ExtensionFile { get; }
        public IEnumerable<int> IdUsingGroups { get; }
        public BaseFileDirectoryInfo(DirectoryInfo parentFileDirectory, string extensionFile, IEnumerable<int> idUsingGroups)
        {
            ParentFileDirectory = parentFileDirectory;
            ExtensionFile = extensionFile;
            IdUsingGroups = idUsingGroups;
        }

        public BaseFileDirectoryInfo(IFileDirectoryInfo source)
        {
            ParentFileDirectory = source.ParentFileDirectory;
            ExtensionFile = source.ExtensionFile;
            IdUsingGroups = source.IdUsingGroups;
        }
    }
}