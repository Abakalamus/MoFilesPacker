using FilesBoxing.Interface;

using System.IO;

namespace FilesBoxing.Class
{
    public class FileDirectoryInfo : IFileDirectoryInfo
    {
        public DirectoryInfo ParentFileDirectory { get; set; }
        public bool IsEnabled { get; set; }
        public string ExtensionFile { get; set; }

        public FileDirectoryInfo(DirectoryInfo parentFileDirectory, bool isEnabled, string extensionFile)
        {
            ParentFileDirectory = parentFileDirectory;
            IsEnabled = isEnabled;
            ExtensionFile = extensionFile;
        }
        public FileDirectoryInfo(DirectoryInfo parentFileDirectory, string extensionFile) : this(parentFileDirectory, true, extensionFile)
        {}
    }
}