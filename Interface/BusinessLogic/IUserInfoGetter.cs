namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IUserInfoGetter
    {
        string DirectoryOutputPathByUserChoise(string defaultPath);
        bool IsUserWantOpenInExplorerPath(string path);
    }
}