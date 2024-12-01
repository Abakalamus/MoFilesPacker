using FilesBoxing.Interface.BusinessLogic.NameHelper;

namespace FilesBoxing.Class.BusinessLogic.NameHelper
{
    internal class NameAnchor : INameAnchor
    {
        public int Id { get; }
        public string Anchor { get; }
        public string FieldName { get; }

        public NameAnchor(int id, string anchor, string fieldName)
        {
            Id = id;
            Anchor = anchor;
            FieldName = fieldName;
        }
    }
}
