using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic.NameHelper
{
    public interface INameHelper
    {
        IEnumerable<IFieldNameAnchor> AnchorCollection { get; }
        IAnchorValue ConvertToAnchorValue(int id, string value);
        IFieldNameAnchor AnchorInfoByFieldName(string fieldName);
        string TransformValue(string source, ICollection<IAnchorValue> anchorValues);
    }

    public interface IPackageNameHelper : INameHelper
    {
        string CreatePackageFileDefaultName();
    }
}
