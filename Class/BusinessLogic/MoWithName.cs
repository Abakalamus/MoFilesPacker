using FilesBoxing.Interface.BusinessLogic;

namespace FilesBoxing.Class.BusinessLogic
{
    public class MoWithName : IMoWithName
    {
        public string CodeMo { get; }
        public string Name { get; }
        public MoWithName(string codeMo, string name)
        {
            CodeMo = codeMo;
            Name = name;
        }
    }
}