using FilesBoxing.Class.DataBase;
using FilesBoxing.Interface.DataBase;
using FilesBoxing.Interface.Factory;

namespace FilesBoxing.Class.Factory
{
    public class FullFileBoxingHandler : IFullFactoryFileBoxingHandler
    {
        public IFactoryFileBoxingHandler GetFileBoxingFactory()
        {
            return new FactoryFileBoxingHandler();
        }
        //public ISettingsFileBoxing GetSettingsFileBoxing(string filePathSettings)
        //{
        //    return new SettingsFileBoxing(filePathSettings, GetSettingsXmlFullSerializator());

        //    IXmlSettingsFullSerializator GetSettingsXmlFullSerializator()
        //    {
        //        return new XmlSettingsFullSerializator();
        //    }
        //}
        public IDataBaseController GetDataBaseController(string connectionString)
        {
            return new DataBaseController(connectionString);
        }
    }
}