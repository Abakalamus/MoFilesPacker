namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IUserInfoGetter
    {
        string GetDirectoryOutputPathByUserChoise(string defaultPath);
        bool IsUserWantOpenInExplorerPath(string path);
    }
}