namespace FilesBoxing.Interface.BusinessLogic.NameHelper
{
    public interface INameHelperController
    {
        int Year { get;}
        int Month { get;}
        void UpdateYearMonthInfo(int newYear, int newMonth);
        string GetTransformedValue(string source);
        string GetTransformedValueForMo(string source, string codeMo);
    }
}