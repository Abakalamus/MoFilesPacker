using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic.FilesCollector
{
    public interface IFilesCollector
    {
        ICollection<IFileDirectoryInfo> FileDirectoryInfo { get; set; }
        ICollection<string> GetFilesForPattern(string pattern);
    }
}