using FilesBoxing.Interface.Visual;
using System;

namespace FilesBoxing.Interface.BusinessLogic
{
    public interface IFilesCollectorToPackingEventHandlerHelper
    {
        ISearchFileMoInfo CreateSearchInfoParameter(string codeMo, byte filesCount);
        IProcessHandleMoInfo CreateProcessHandleParameterNoArchive(string codeMo);
        IProcessHandleMoInfo CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(string codeMo);
    }
}