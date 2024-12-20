﻿using FilesBoxing.Class.BusinessLogic.EventHandlerParameter;
using FilesBoxing.Interface.BusinessLogic.FilesCollector;
using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Class.BusinessLogic.FilesCollector
{
    public class EventHandlerHelper : IFilesCollectorToPackingEventHandlerHelper
    {
        public ISearchFileMoInfo CreateSearchInfoParameter(string codeMo, byte filesCount)
        {
            return new SearchFileMoInfo(codeMo, filesCount);
        }
        public IProcessHandleMoInfo CreateProcessHandleParameterNoArchive(string codeMo)
        {
            return new ProcessHandleMoInfo(codeMo, false);
        }
        public IProcessHandleMoInfo CreateProcessHandleParameterCreateProcessHandleParameteArchiveComplite(string codeMo)
        {
            return new ProcessHandleMoInfo(codeMo, true);
        }
    }
}