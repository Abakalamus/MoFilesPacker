namespace FilesBoxing.Interface
{
    public interface IXmlSettingsFullSerializator
    {
        void SerializeAsFile(ISettingsFileBoxingParameters model, string filePath);
        ISettingsFileBoxingParameters DeserializeFromFile(string filePath);
    }
}