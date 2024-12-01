using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Interface.BusinessLogic.FilesCollector
{
    public interface IFilesCollectorToPackingEventHandlerHelper
    {
        ISearchFileMoInfo CreateSearchInfoParameter(string codeMo, byte filesCount);
        IProcessHandleMoInfo CreateProcessHandleParameterNoArchive(string codeMo);
        IProcessHandleMoInfo CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(string codeMo);
    }
}