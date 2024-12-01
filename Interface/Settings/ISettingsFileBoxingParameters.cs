using FilesBoxing.Interface.BusinessLogic;

using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.Settings
{
    public interface ISettingsFileBoxingParameters
    {
        string ConnectionString { get; }
        DirectoryInfo TempDirectory { get; }
        DirectoryInfo OutputDirectory { get; }
        int DefaultGroupId { get; }
        IEnumerable<string> CodeMoCollection { get; set; }
        IEnumerable<ITypeGroupingSettings> UsingGroups { get; }
        IEnumerable<IFileDirectoryInfo> FileDirectoriesInfo { get; }
    }
}