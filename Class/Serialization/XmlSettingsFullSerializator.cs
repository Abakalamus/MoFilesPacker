using FilesBoxing.Class.Settings;
using FilesBoxing.Interface.Settings;

using LibraryKurguzov.Class.Xml;

using System;
using System.IO;

namespace FilesBoxing.Class.Serialization
{
    public class XmlSettingsFullSerializator : IXmlSettingsFullSerializator
    {
        public void SerializeAsFile(ISettingsFileBoxingParameters model, string filePath)
        {
            try
            {
                var convertedData = new XmlSettingsFileBoxingParameters(model);
                XmlHandler.SerializeXmlObject(convertedData, filePath);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"Ошибка формирования файла настроек", ex);
            }
        }

        public ISettingsFileBoxingParameters DeserializeFromFile(string filePath)
        {
            try
            {
                return XmlHandler.DeserializeXmlObject<XmlSettingsFileBoxingParameters>(filePath);
            }
            catch (Exception ex)
            {
                var fileName = Path.GetFileName(filePath);
                throw new ApplicationException($"Ошибка чтения файла [{fileName}] содержащего настройки программы", ex);
            }
        }
    }
}