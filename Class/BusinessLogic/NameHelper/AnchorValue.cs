using FilesBoxing.Interface.BusinessLogic.NameHelper;

namespace FilesBoxing.Class.BusinessLogic.NameHelper
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
