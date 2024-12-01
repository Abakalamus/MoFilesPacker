using System;
using System.Collections.Generic;
using System.Linq;
using FilesBoxing.Interface.BusinessLogic.NameHelper;

namespace FilesBoxing.Class.BusinessLogic.NameHelper
{
    public class NameHelperController : INameHelperController
    {
        private readonly IPackageNameHelper _nameHelper;

        public int Year { get; private set; }
        public int Month { get; private set; }
        private IList<IAnchorValue> _periodAnchors;
        private readonly int _idAnchorCodeMo;
        public NameHelperController(IPackageNameHelper nameHelper, int year, int month)
        {
            _nameHelper = nameHelper;
            if (!NameHelperContainsRequireAnchors())
                throw new ApplicationException("NameHelper не содержит всех необходимых якорей!");
            _idAnchorCodeMo = _nameHelper.AnchorCollection.First(x => x.FieldName == "CODE_MO").Id;
            UpdateYearMonthInfo(year, month);

            bool NameHelperContainsRequireAnchors()
            {
                var tempYear = _nameHelper.GetAnchorInfoByFieldName("YEAR");
                var tempMonth = _nameHelper.GetAnchorInfoByFieldName("MONTH");
                var tempCodeMo = _nameHelper.GetAnchorInfoByFieldName("CODE_MO");
                return tempYear != null && tempMonth != null && tempCodeMo != null;
            }
        }
        public void UpdateYearMonthInfo(int newYear, int newMonth)
        {
            Year = newYear;
            Month = newMonth;
            _periodAnchors = GetPeriodAchorsValues();
        }
        public string GetTransformedValue(string source)
        {
            return _nameHelper.GetTransformedValue(source, _periodAnchors);
        }
        public string GetTransformedValueForMo(string source, string codeMo)
        {
            var tempSource = source;
            if (string.IsNullOrEmpty(tempSource))
                tempSource = _nameHelper.GetPackageFileDefaultName();
            return _nameHelper.GetTransformedValue(tempSource, GetFullAnchorCollection(codeMo));
        }
        private IList<IAnchorValue> GetPeriodAchorsValues()
        {
            return new List<IAnchorValue>
                {
                   _nameHelper.GetAsAnchorValue(_nameHelper.GetAnchorInfoByFieldName("YEAR").Id, Year.ToString()),
                   _nameHelper.GetAsAnchorValue(_nameHelper.GetAnchorInfoByFieldName("MONTH").Id, Month.ToString()),
                };
        }
        private ICollection<IAnchorValue> GetFullAnchorCollection(string codeMo)
        {
            var baseAnchorsCopy = new List<IAnchorValue>(_periodAnchors)
            {
                _nameHelper.GetAsAnchorValue(_idAnchorCodeMo, codeMo)
            };
            return baseAnchorsCopy;
        }
    }
}