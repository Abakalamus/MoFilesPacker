using FilesBoxing.Interface.BusinessLogic.FileNameHelper;
using System.Collections.Generic;
using System.Linq;
using System;

namespace FilesBoxing.Class.BusinessLogic.FileNameHelper
{
    internal class PackageFileNameHelper : IPackageFileNameHelper
    {
        private const string ExceptionNotFoundAnchor = "Коллекция содержит идентификатор якоря, не зарегистрированный в системе";
        private const string ExceptionNotUniqueAnchorId = "Не допускается использование нескольких якорей с одинаковым идентификатором!";
        public IEnumerable<INameAnchor> AnchorCollection { get; }

        public string GetTransformedValue(string source, ICollection<IAnchorValue> anchorValues)
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

        public IAnchorValue GetAsAnchorValue(int id, string value)
        {
            var anchorFromCollection = AnchorCollection.FirstOrDefault(x => x.Id == id);
            return anchorFromCollection == null
                ? throw new ApplicationException(ExceptionNotFoundAnchor)
                : (IAnchorValue)new AnchorValue(anchorFromCollection.Id, value);
        }

        public PackageFileNameHelper(IEnumerable<INameAnchor> anchorsCollection)
        {
            var anchorCollection = anchorsCollection.ToList();
            if (anchorCollection.GroupBy(x => x.Id).Where(x => x.Count() > 1).Select(x => x.Key).Any())
                throw new ApplicationException(ExceptionNotUniqueAnchorId);
            AnchorCollection = anchorCollection;
        }
        public string GetPackageFileDefaultName()
        {
            var year = GetAnchorInfoByFieldName("YEAR");
            var month = GetAnchorInfoByFieldName("MONTH");
            var codeMO = GetAnchorInfoByFieldName("CODE_MO");
            return $"Данные реестра {(year == null ? "NOT_FOUND_YEAR" : year.Anchor)} {(month == null ? "NOT_FOUND_MONTH" : month.Anchor)} для {(codeMO == null ? "NOT_FOUND_CODE_MO" : codeMO.Anchor)}";
        }

        public INameAnchor GetAnchorInfoByFieldName(string fieldName)
        {
            return AnchorCollection.First(x => x.FieldName == fieldName);
        }
    }
}
