using System;
using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFilesCollector
    {
        IEnumerable<IFileDirectoryInfo> FileDirectoryInfo { get; set; }
        IEnumerable<string> UniqueEntities { get; set; }
        Dictionary<string, IEnumerable<string>> GetFilesForEntities();
        EventHandler<string> OnLogging { get; set; }
    }

    public interface IFilesCollectorNew
    {
        IEnumerable<IFileDirectoryInfo> FileDirectoryInfo { get; set; }
        IEnumerable<string> GetFilesForPattern(string pattern);
    }
}