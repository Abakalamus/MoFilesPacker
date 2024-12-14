using FilesBoxing.Interface.BusinessLogic.NameHelper;

using System;
using System.Collections.Generic;
using System.Linq;

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
                var tempYear = _nameHelper.AnchorInfoByFieldName("YEAR");
                var tempMonth = _nameHelper.AnchorInfoByFieldName("MONTH");
                var tempCodeMo = _nameHelper.AnchorInfoByFieldName("CODE_MO");
                return tempYear != null && tempMonth != null && tempCodeMo != null;
            }
        }
        public void UpdateYearMonthInfo(int newYear, int newMonth)
        {
            Year = newYear;
            Month = newMonth;
            _periodAnchors = PeriodAchorsValues();
        }
        public string ConvertToTransformedValue(string source)
        {
            return _nameHelper.TransformValue(source, _periodAnchors);
        }
        public string ConvertToTransformedValueForMo(string source, string codeMo)
        {
            var tempSource = source;
            if (string.IsNullOrEmpty(tempSource))
                tempSource = _nameHelper.CreatePackageFileDefaultName();
            return _nameHelper.TransformValue(tempSource, FullAnchorCollection(codeMo));
        }
        private IList<IAnchorValue> PeriodAchorsValues()
        {
            return new List<IAnchorValue>
                {
                   _nameHelper.ConvertToAnchorValue(_nameHelper.AnchorInfoByFieldName("YEAR").Id, Year.ToString()),
                   _nameHelper.ConvertToAnchorValue(_nameHelper.AnchorInfoByFieldName("MONTH").Id, Month.ToString()),
                };
        }
        private ICollection<IAnchorValue> FullAnchorCollection(string codeMo)
        {
            var baseAnchorsCopy = new List<IAnchorValue>(_periodAnchors)
            {
                _nameHelper.ConvertToAnchorValue(_idAnchorCodeMo, codeMo)
            };
            return baseAnchorsCopy;
        }
    }
}