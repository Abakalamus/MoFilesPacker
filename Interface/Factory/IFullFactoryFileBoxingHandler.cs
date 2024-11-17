using FilesBoxing.Interface.DataBase;

namespace FilesBoxing.Interface.Factory
{
    public interface IFullFactoryFileBoxingHandler
    {
        IFactoryFileBoxingHandler GetFileBoxingFactory();
        IDataBaseController GetDataBaseController(string connectionString);
    }
}