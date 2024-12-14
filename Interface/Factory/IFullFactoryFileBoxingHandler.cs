using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;
using FilesBoxing.Interface.BusinessLogic.NameHelper;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Visual;

using System.Collections.Generic;

namespace FilesBoxing.Interface.Factory
{
    public interface IFullFactoryFileBoxingHandler
    {
        IDataBaseController DataBaseController(string connectionString);
        IFilesCollectorToPackingHandler FilesCollectorToBoxingHandler(string tempDirectoryPath);
        IMoProcessInfo NewMoProcessInfo(string codeMo);
        INameHelperController NameHelperController(int year, int month);
        IUserInfoGetter NewUserInfoGetter();
        IMoWithName NewMoWithName(string codeMo, string name);
        IFileDirectoryInfo NewDirectoryInfo(string directoryPath, string extensionFile, IEnumerable<int> usingGroups);
    }
}