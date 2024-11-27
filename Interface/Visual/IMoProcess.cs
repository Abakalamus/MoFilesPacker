namespace FilesBoxing.Interface.Visual
{
    public interface IMoInfo
    {
        string CodeMo { get; }
    }

    public interface ISearchFileMoInfo : IMoInfo
    {
        byte? CountFiles { get; set; }
    }

    public interface IProcessHandleMoInfo : IMoInfo, IPackageCreatedInfo
    { }

    public interface IPackageCreatedInfo
    {
        bool? IsPackageFileCreated { get; set; }
    }
}