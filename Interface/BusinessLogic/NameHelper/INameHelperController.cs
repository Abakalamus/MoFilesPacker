namespace FilesBoxing.Interface.BusinessLogic.NameHelper
{
    public interface INameHelperController
    {
        int Year { get;}
        int Month { get;}
        void UpdateYearMonthInfo(int newYear, int newMonth);
        string ConvertToTransformedValue(string source);
        string ConvertToTransformedValueForMo(string source, string codeMo);
    }
}