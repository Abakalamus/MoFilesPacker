using System.Collections.Generic;

namespace FilesBoxing.Interface
{
    public interface IFilesCollector
    {
        IEnumerable<IFileDirectoryInfo> FileDirectoryInfo { get; set; }
        IEnumerable<string> UniqueEntities { get; set; }
        Dictionary<string, IEnumerable<string>> GetFilesForEntities();
    }
}