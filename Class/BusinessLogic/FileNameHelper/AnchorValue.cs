using FilesBoxing.Interface.BusinessLogic.FileNameHelper;

namespace FilesBoxing.Class.BusinessLogic.FileNameHelper
{
    public class AnchorValue : IAnchorValue
    {
        public int Id { get; }

        public string Value { get; }
        public AnchorValue(int id, string value)
        {
            Id = id;
            Value = value;
        }
    }
}
