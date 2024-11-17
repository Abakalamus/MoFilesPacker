using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface
{
    public interface IFileSettings
    {
        string FilePathSettings { get; }
        void ResreshDataFromFile();
        void SaveCurrentSettingToFile();
    }
    public interface ISettingsFileBoxing : IFileSettings
    {
        ISettingsFileBoxingParameters ProgramSettings { get; }
    }
    public interface ISettingsFileBoxingParameters
    {
        string ConnectionString { get; }
        DirectoryInfo TempDirectory { get; }
        DirectoryInfo OutputDirectory { get; }
        IEnumerable<string> CodeMoCollection { get; set; }
        IEnumerable<IFileDirectoryInfo> FileDirectoriesInfo { get; set; }
        string FileNameArchive { get; }
    }
}