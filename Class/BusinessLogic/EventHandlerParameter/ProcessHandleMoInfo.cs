using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Class.BusinessLogic.EventHandlerParameter
{
    public class ProcessHandleMoInfo : IProcessHandleMoInfo
    {
        public string CodeMo { get; }
        public bool? IsPackageFileCreated { get; set; }
        public ProcessHandleMoInfo(string codeMo, bool isPackageFileCreated)
        {
            CodeMo = codeMo;
            IsPackageFileCreated = isPackageFileCreated;
        }
    }
}