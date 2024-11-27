using FilesBoxing.Interface.BusinessLogic;
using FilesBoxing.Interface.DataBase;
using System.Collections.Generic;
using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Interface.Factory
{
    public interface IFullFactoryFileBoxingHandler
    {
        //IFactoryFileBoxingHandler GetFileBoxingFactory();
        IDataBaseController GetDataBaseController(string connectionString);
        IFilesCollectorToPackingHandler GetFilesCollectorToBoxingHandler(string tempDirectoryPath);
        IFilesCollectorHandlerParameter CreateFilesCollectorHandlerParameter(int year, int month, IEnumerable<string> moCollection, IEnumerable<IFileDirectoryInfo> directoryInfoCollection, string nameArchiveTemplate);
        IMoProcessInfo GetNewMoProcessInfo(string codeMo);
        IFileDirectoryInfoUpdater GetFileDirectoryInfoUpdater(int year, int month);
    }
}