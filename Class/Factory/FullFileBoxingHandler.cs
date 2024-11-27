using FilesBoxing.Class.BusinessLogic;
using FilesBoxing.Class.BusinessLogic.FileNameHelper;
using FilesBoxing.Class.DataBase;
using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FileNameHelper;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;
using FilesBoxing.Interface.Visual;

using System.Collections.Generic;
using System.IO;
using FilesBoxing.Class.Visual;

namespace FilesBoxing.Class.Factory
{
    public class FullFileBoxingHandler : IFullFactoryFileBoxingHandler
    {
        public IDataBaseController GetDataBaseController(string connectionString)
        {
            return new DataBaseController(connectionString);
        }

        public IFilesCollectorToPackingHandler GetFilesCollectorToBoxingHandler(string tempDirectoryPath)
        {
            return new FilesCollectorToPackingHandler(GetBoxingHandler(new DirectoryInfo(tempDirectoryPath)), GetFilesCollector(), GetPackageFileNameHelper(), CreateEventHelper());

            IFileBoxingToDirectory GetBoxingHandler(DirectoryInfo directoryForArchive)
            {
                return new FileBoxing(directoryForArchive);
            }
            IFilesCollector GetFilesCollector()
            {
                return new FilesPatternCollector();
            }

            IFilesCollectorToPackingEventHandlerHelper CreateEventHelper()
            {
                return new EventHandlerHelper();
            }
        }
        public IFilesCollectorHandlerParameter CreateFilesCollectorHandlerParameter(int year, int month, IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchiveTemplate)
        {
            return new FilesCollectorHandlerParameter(year, month, moCollection, directoryInfoCollection, nameArchiveTemplate);
        }
        private static IPackageFileNameHelper GetPackageFileNameHelper()
        {
            return new PackageFileNameHelper(GetAnchors());

            IEnumerable<INameAnchor> GetAnchors()
            {
                return new List<INameAnchor>
                {
                    new NameAnchor(1, GetWrappedValue("CODE_MO"), "CODE_MO"),
                    new NameAnchor(2, GetWrappedValue("YEAR"), "YEAR"),
                    new NameAnchor(3, GetWrappedValue("MONTH"), "MONTH")
                };
            }
            string GetWrappedValue(string source)
            {
                return $"![{source}]!";
            }
        }
        public IMoProcessInfo GetNewMoProcessInfo(string codeMo)
        {
            return new MoProcessInfo(codeMo);
        }
        public IFileDirectoryInfoUpdater GetFileDirectoryInfoUpdater(int year, int month)
        {
            return new FileDirectoryInfoUpdater(GetPackageFileNameHelper(), year, month);
        }
    }
}