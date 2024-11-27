using System.Collections.Generic;
using System.IO;

using FilesBoxing.Interface.BusinessLogic.FileNameHelper;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFileUsingGroupInfo
    {
        string ExtensionFile { get; }
        IEnumerable<int> IdUsingGroups { get; }
    }

    public interface IFileDirectoryInfo : IFileUsingGroupInfo
    {
        DirectoryInfo ParentFileDirectory { get; }
        // bool IsEnabled { get; }
    }

    public interface IFileDirectoryInfoUpdater
    {
        int Year { get; set; }
        int Month { get; set; }
        IEnumerable<IFileDirectoryInfo> GetUpdatedByPeriod(IEnumerable<IFileDirectoryInfo> source);
    }

    public class BaseFileDirectoryInfo : IFileDirectoryInfo
    {
        public DirectoryInfo ParentFileDirectory { get; }
        public string ExtensionFile { get; }
        public IEnumerable<int> IdUsingGroups { get; }
        public BaseFileDirectoryInfo(DirectoryInfo parentFileDirectory, string extensionFile, IEnumerable<int> idUsingGroups)
        {
            ParentFileDirectory = parentFileDirectory;
            ExtensionFile = extensionFile;
            IdUsingGroups = idUsingGroups;
        }

        public BaseFileDirectoryInfo(IFileDirectoryInfo source)
        {
            ParentFileDirectory = source.ParentFileDirectory;
            ExtensionFile = source.ExtensionFile;
            IdUsingGroups = source.IdUsingGroups;
        }
    }

    public class FileDirectoryInfoUpdater : IFileDirectoryInfoUpdater
    {
        private readonly IFileNameHelper _fileNameHelper;
        public int Year { get; set; }
        public int Month { get; set; }
        public FileDirectoryInfoUpdater(IFileNameHelper helper, int year, int month)
        {
            _fileNameHelper = helper;
            Year = year;
            Month = month;
        }
        public IEnumerable<IFileDirectoryInfo> GetUpdatedByPeriod(IEnumerable<IFileDirectoryInfo> source)
        {
            var result = new List<IFileDirectoryInfo>();
            var periodAnchors = GetPeriodAchorsValues();
            foreach (var info in source)
            {
                var newPath = _fileNameHelper.GetTransformedValue(info.ParentFileDirectory.FullName, periodAnchors);
                result.Add(new BaseFileDirectoryInfo(new DirectoryInfo(newPath), info.ExtensionFile, info.IdUsingGroups));
            }
            return result;

            IList<IAnchorValue> GetPeriodAchorsValues()
            {
                return new List<IAnchorValue>
                {
                    _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("YEAR").Id, Year.ToString()),
                    _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("MONTH").Id, Month.ToString()),
                };
            }
        }
    }
    //public interface IExtendedFileDirecoryInfo : IFileUsingGroupInfo
    //{
    //    ITemplatedDirectory TemplatedDirectoryInfo { get; }
    //}

    //public interface ITemplatedDirectory
    //{
    //    string TemplateDirectory { get; }
    //    DirectoryInfo ResultDirectory { get; }
    //    void UpdateResultDirectory(int year, int month);
    //}
    //public class TemplatedDirectory : ITemplatedDirectory
    //{
    //    private readonly IFileNameHelper _fileNameHelper;
    //    public string TemplateDirectory { get; set; }
    //    public DirectoryInfo ResultDirectory { get; set; }
    //    public TemplatedDirectory(IFileNameHelper fileNameHelper, string templateDirectory)
    //    {
    //        _fileNameHelper = fileNameHelper;
    //        TemplateDirectory = templateDirectory;
    //    }
    //    public void UpdateResultDirectory(int year, int month)
    //    {
    //        var newPath = _fileNameHelper.GetTransformedValue(TemplateDirectory, GetPeriodAchorsValues());
    //        ResultDirectory = new DirectoryInfo(newPath);

    //        IList<IAnchorValue> GetPeriodAchorsValues()
    //        {
    //            return new List<IAnchorValue>
    //            {
    //                _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("YEAR").Id, year.ToString()),
    //                _fileNameHelper.GetAsAnchorValue(_fileNameHelper.GetAnchorInfoByFieldName("MONTH").Id, month.ToString()),
    //            };
    //        }
    //    }
    //}

    //public class ExtendedFileDirecoryInfo : IExtendedFileDirecoryInfo
    //{
    //    public ITemplatedDirectory TemplatedDirectoryInfo { get; }

    //    public string ExtensionFile { get; }

    //    public IEnumerable<int> IdUsingGroups { get; }

    //    public ExtendedFileDirecoryInfo(IFileDirectoryInfo source)
    //    {
    //        ExtensionFile = source.ExtensionFile;
    //        IdUsingGroups = source.IdUsingGroups;
    //        var a = source.ParentFileDirectory;
    //    }
    //}
}