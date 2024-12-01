using FilesBoxing.Class.BusinessLogic;
using FilesBoxing.Class.BusinessLogic.FilesCollector;
using FilesBoxing.Class.BusinessLogic.NameHelper;
using FilesBoxing.Class.DataBase;
using FilesBoxing.Class.Visual;
using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;
using FilesBoxing.Interface.BusinessLogic.NameHelper;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;
using FilesBoxing.Interface.Visual;

using System.Collections.Generic;
using System.IO;

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
            return new FilesCollectorToPackingHandler(GetBoxingHandler(new DirectoryInfo(tempDirectoryPath)), GetFilesCollector(), CreateEventHelper());

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
        public IMoProcessInfo GetNewMoProcessInfo(string codeMo)
        {
            return new MoProcessInfo(codeMo);
        }
        public INameHelperController GetNameHelperController(int year, int month)
        {
            return new NameHelperController(GetPackageFileNameHelper(), year, month);

            IPackageNameHelper GetPackageFileNameHelper()
            {
                return new PackageNameHelper(GetAnchors());

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
        }
        public IUserInfoGetter GetNewUserInfoGetter()
        {
            return new UserInfoGetter();
        }
        public IMoWithName GetNewMoWithName(string codeMo, string name)
        {
            return new MoWithName(codeMo, name);
        }
        public IFileDirectoryInfo GetNewDirectoryInfo(string directoryPath, string extensionFile, IEnumerable<int> usingGroups)
        {
            return new BaseFileDirectoryInfo(new DirectoryInfo(directoryPath), extensionFile, usingGroups);
        }
        //public IFileDirectoryInfoUpdater GetFileDirectoryInfoUpdater(int year, int month)
        //{
        //    return new FileDirectoryInfoUpdater(GetPackageFileNameHelper(), year, month);
        //}
        //public IFilesCollectorHandlerParameter CreateFilesCollectorHandlerParameter(int year, int month, IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchiveTemplate)
        //{
        //    return new FilesCollectorHandlerParameter(year, month, moCollection, directoryInfoCollection, nameArchiveTemplate);
        //}
    }
}