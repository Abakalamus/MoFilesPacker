using FilesBoxing.Interface.BusinessLogic.NameHelper;

using System;
using System.Collections.Generic;
using System.Linq;

namespace FilesBoxing.Class.BusinessLogic.NameHelper
{
    internal class PackageNameHelper : IPackageNameHelper
    {
        private const string ExceptionNotFoundAnchor = "Коллекция содержит идентификатор якоря, не зарегистрированный в системе";
        private const string ExceptionNotUniqueAnchorId = "Не допускается использование нескольких якорей с одинаковым идентификатором!";
        public IEnumerable<IFieldNameAnchor> AnchorCollection { get; }

        public string TransformValue(string source, ICollection<IAnchorValue> anchorValues)
        {

            var result = source;
            if (!anchorValues.All(x => AnchorCollection.Select(c => c.Id).Contains(x.Id)))
                throw new ApplicationException(ExceptionNotFoundAnchor);
            foreach (var anchor in anchorValues)
            {
                var foundAnchor = AnchorCollection.First(x => x.Id == anchor.Id);
                result = result.Replace(foundAnchor.Anchor, anchor.Value);
            }
            return result;
        }
        public IAnchorValue ConvertToAnchorValue(int id, string value)
        {
            var anchorFromCollection = AnchorCollection.FirstOrDefault(x => x.Id == id);
            return anchorFromCollection == null
                ? throw new ApplicationException(ExceptionNotFoundAnchor)
                : (IAnchorValue)new AnchorValue(anchorFromCollection.Id, value);
        }
        public PackageNameHelper(IEnumerable<IFieldNameAnchor> anchorsCollection)
        {
            var anchorCollection = anchorsCollection.ToList();
            if (anchorCollection.GroupBy(x => x.Id).Where(x => x.Count() > 1).Select(x => x.Key).Any())
                throw new ApplicationException(ExceptionNotUniqueAnchorId);
            AnchorCollection = anchorCollection;
        }
        public string CreatePackageFileDefaultName()
        {
            var year = AnchorInfoByFieldName("YEAR");
            var month = AnchorInfoByFieldName("MONTH");
            var codeMO = AnchorInfoByFieldName("CODE_MO");
            return $"Данные реестра {(year == null ? "NOT_FOUND_YEAR" : year.Anchor)} {(month == null ? "NOT_FOUND_MONTH" : month.Anchor)} для {(codeMO == null ? "NOT_FOUND_CODE_MO" : codeMO.Anchor)}";
        }
        public IFieldNameAnchor AnchorInfoByFieldName(string fieldName)
        {
            return AnchorCollection.First(x => x.FieldName == fieldName);
        }
    }
}
