using System.IO;

namespace FilesBoxing.Interface
{
    public interface IFileDirectoryInfo
    {
        DirectoryInfo ParentFileDirectory { get; }
        bool IsEnabled { get; }
        string ExtensionFile { get; }
    }
}