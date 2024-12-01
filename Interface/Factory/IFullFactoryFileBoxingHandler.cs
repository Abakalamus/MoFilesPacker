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
        IDataBaseController GetDataBaseController(string connectionString);
        IFilesCollectorToPackingHandler GetFilesCollectorToBoxingHandler(string tempDirectoryPath);
        IMoProcessInfo GetNewMoProcessInfo(string codeMo);
        INameHelperController GetNameHelperController(int year, int month);
        IUserInfoGetter GetNewUserInfoGetter();
        IMoWithName GetNewMoWithName(string codeMo, string name);
        IFileDirectoryInfo GetNewDirectoryInfo(string directoryPath, string extensionFile, IEnumerable<int> usingGroups);
        // IFilesCollectorHandlerParameter CreateFilesCollectorHandlerParameter(int year, int month, IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchiveTemplate);
        // IFileDirectoryInfoUpdater GetFileDirectoryInfoUpdater(int year, int month);
        //IFactoryFileBoxingHandler GetFileBoxingFactory();
    }
}