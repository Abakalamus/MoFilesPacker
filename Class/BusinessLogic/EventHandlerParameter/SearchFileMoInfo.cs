using FilesBoxing.Interface.Visual;

namespace FilesBoxing.Class.BusinessLogic.EventHandlerParameter
{
    public class SearchFileMoInfo : ISearchFileMoInfo
    {
        public string CodeMo { get; }
        public byte? CountFiles { get; set; }
        public SearchFileMoInfo(string codeMo, byte countFiles)
        {
            CodeMo = codeMo;
            CountFiles = countFiles;
        }
    }
}