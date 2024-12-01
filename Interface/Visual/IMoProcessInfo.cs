namespace FilesBoxing.Interface.Visual
{
    public interface IMoProcessInfo : ISearchFileMoInfo, IPackageCreatedInfo
    {
        bool IsSelected { get; set; }
    }
}