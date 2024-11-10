using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface
{
    public interface IFileBoxing
    {
        void BoxFiles(ICollection<string> files, string archiveName);
    }

    public interface IFileBoxingToDirectory : IFileBoxing
    {
        DirectoryInfo DirectoryForBoxingFile { get; }
    }
}