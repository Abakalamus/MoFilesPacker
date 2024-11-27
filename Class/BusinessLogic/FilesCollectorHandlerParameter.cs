using FilesBoxing.Interface.BusinessLogic;
using System.Collections.Generic;

namespace FilesBoxing.Class.BusinessLogic
{
    internal class FilesCollectorHandlerParameter : IFilesCollectorHandlerParameter
    {
        public int Year { get; }
        public int Month { get; }
        public IEnumerable<string> MoCollection { get; }
        public IEnumerable<IFileDirectoryInfo> DirectoryInfoCollection { get; }
        public string NameArchiveTemplate { get; }

        public FilesCollectorHandlerParameter(int year, int month, IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchiveTemplate)
        {
            Year = year;
            Month = month;
            MoCollection = moCollection;
            DirectoryInfoCollection = directoryInfoCollection;
            NameArchiveTemplate = nameArchiveTemplate;
        }
    }
}
