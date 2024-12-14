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
        public IDataBaseController DataBaseController(string connectionString)
        {
            return new DataBaseController(connectionString);
        }
        public IFilesCollectorToPackingHandler FilesCollectorToBoxingHandler(string tempDirectoryPath)
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
        public IMoProcessInfo NewMoProcessInfo(string codeMo)
        {
            return new MoProcessInfo(codeMo);
        }
        public INameHelperController NameHelperController(int year, int month)
        {
            return new NameHelperController(GetPackageFileNameHelper(), year, month);

            IPackageNameHelper GetPackageFileNameHelper()
            {
                return new PackageNameHelper(GetAnchors());

                IEnumerable<IFieldNameAnchor> GetAnchors()
                {
                    return new List<IFieldNameAnchor>
                    {
                        new FieldNameAnchor(1, GetWrappedValue("CODE_MO"), "CODE_MO"),
                        new FieldNameAnchor(2, GetWrappedValue("YEAR"), "YEAR"),
                        new FieldNameAnchor(3, GetWrappedValue("MONTH"), "MONTH")
                    };
                }
                string GetWrappedValue(string source)
                {
                    return $"![{source}]!";
                }
            }
        }
        public IUserInfoGetter NewUserInfoGetter()
        {
            return new UserInfoGetter();
        }
        public IMoWithName NewMoWithName(string codeMo, string name)
        {
            return new MoWithName(codeMo, name);
        }
        public IFileDirectoryInfo NewDirectoryInfo(string directoryPath, string extensionFile, IEnumerable<int> usingGroups)
        {
            return new BaseFileDirectoryInfo(new DirectoryInfo(directoryPath), extensionFile, usingGroups);
        }
    }
}