using FilesBoxing.Enum;

using System.Collections.Generic;

namespace FilesBoxing.Interface
{
    public interface IFilesBoxingController
    {
        int Year { get; set; }
        int Month { get; set; }
        IEnumerable<string> Sources { get; }
        IEnumerable<IFileDirectoryInfo> DataInfo { get; }
        bool UpdateSources(SourceType sourceType);

    }
}