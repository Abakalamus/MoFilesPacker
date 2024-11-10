using System.IO;

namespace FilesBoxing.Interface
{
    public interface ISettings
    {
        DirectoryInfo TempDirectory { get; }
        DirectoryInfo OutputDirectory { get; }

        void FillBySource();
    }
}