using System.Collections.Generic;

namespace FilesBoxing.Interface.DataBase
{
    public interface IDataBaseController
    {
        IEnumerable<string> GetCodesMo();
    }

}