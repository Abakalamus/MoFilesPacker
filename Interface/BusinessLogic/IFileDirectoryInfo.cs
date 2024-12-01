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

    public interface IFileDirectoryInfoUpdater
    {
        int Year { get; set; }
        int Month { get; set; }
        IEnumerable<IFileDirectoryInfo> GetUpdatedByPeriod(IEnumerable<IFileDirectoryInfo> source);
    }

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