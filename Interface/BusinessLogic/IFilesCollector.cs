using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFilesCollector
    {
        IEnumerable<IFileDirectoryInfo> FileDirectoryInfo { get; set; }
        IEnumerable<string> GetFilesForPattern(string pattern);
    }
}