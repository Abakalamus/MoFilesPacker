using FilesBoxing.Class.Serialization;
using FilesBoxing.Class.Settings;
using FilesBoxing.Interface.Settings;

namespace FilesBoxing.Class.Factory
{
    public class SettingsHelper
    {
        public static ISettingsFileBoxing GetSettingsFileBoxing(string filePathSettings)
        {
            return new SettingsFileBoxing(filePathSettings, GetSettingsXmlFullSerializator());

            IXmlSettingsFullSerializator GetSettingsXmlFullSerializator()
            {
                return new XmlSettingsFullSerializator();
            }
        }
    }
}