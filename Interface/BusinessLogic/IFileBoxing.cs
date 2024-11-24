using System.Collections.Generic;
using System.IO;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFileBoxing
    {
        void BoxFiles(IEnumerable<string> files, string archiveName);
    }

    public interface IFileBoxingToDirectory : IFileBoxing
    {
        DirectoryInfo DirectoryForBoxingFile { get; }
    }
}