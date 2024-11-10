using System.IO;
using FilesBoxing.Interface;

namespace FilesBoxing.Class
{
    public class Settings : ISettings
    {
        public DirectoryInfo TempDirectory { get; set; }
        public DirectoryInfo OutputDirectory { get; set; }

        public void FillBySource()
        {
            TempDirectory = new DirectoryInfo("Temp");
            OutputDirectory = new DirectoryInfo("Output");
        }
    }
}