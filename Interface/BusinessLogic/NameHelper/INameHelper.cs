using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic.NameHelper
{
    public interface INameHelper
    {
        IEnumerable<INameAnchor> AnchorCollection { get; }
        IAnchorValue GetAsAnchorValue(int id, string value);
        INameAnchor GetAnchorInfoByFieldName(string fieldName);
        string GetTransformedValue(string source, ICollection<IAnchorValue> anchorValues);
    }

    public interface IPackageNameHelper : INameHelper
    {
        string GetPackageFileDefaultName();
    }
}
