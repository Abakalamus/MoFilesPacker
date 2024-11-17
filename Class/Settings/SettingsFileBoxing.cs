using System.IO;
using FilesBoxing.Interface;

namespace FilesBoxing.Class.Settings
{
    public class SettingsFileBoxing : ISettingsFileBoxing
    {
        private readonly IXmlSettingsFullSerializator _xmlFullSerializator;
        public string FilePathSettings { get; set; }

        public SettingsFileBoxing(string filePath, IXmlSettingsFullSerializator xmlFullSerializator)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException(nameof(filePath));
            FilePathSettings = filePath;
            _xmlFullSerializator = xmlFullSerializator;
        }
        public void ResreshDataFromFile()
        {
            ProgramSettings = _xmlFullSerializator.DeserializeFromFile(FilePathSettings);
        }
        public void SaveCurrentSettingToFile()
        {
            _xmlFullSerializator.SerializeAsFile(ProgramSettings, FilePathSettings);
        }
        public ISettingsFileBoxingParameters ProgramSettings { get; set; }
    }
}