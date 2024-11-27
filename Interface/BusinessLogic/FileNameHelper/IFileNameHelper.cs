using System.Collections.Generic;

namespace FilesBoxing.Interface.BusinessLogic.FileNameHelper
{
    public interface IFileNameHelper
    {
        IEnumerable<INameAnchor> AnchorCollection { get; }
        IAnchorValue GetAsAnchorValue(int id, string value);
        INameAnchor GetAnchorInfoByFieldName(string fieldName);
        string GetTransformedValue(string source, ICollection<IAnchorValue> anchorValues);
    }

    public interface IPackageFileNameHelper : IFileNameHelper
    {
        string GetPackageFileDefaultName();
    }
}
