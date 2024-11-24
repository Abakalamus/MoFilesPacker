namespace FilesBoxing.Interface.Settings
{
    public interface IFileSettings
    {
        string FilePathSettings { get; }
        void ResreshDataFromFile();
        void SaveCurrentSettingToFile();
    }
    public interface ISettingsFileBoxing : IFileSettings
    {
        ISettingsFileBoxingParameters ProgramSettings { get; }
    }
}