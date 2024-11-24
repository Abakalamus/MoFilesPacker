using System;
using System.Collections.Generic;

using FilesBoxing.Interface.DataBase;

using Oracle.ManagedDataAccess.Client;

namespace FilesBoxing.Class.DataBase
{
    public class DataBaseController : IDataBaseController
    {
        private readonly string _connectionString;
        public DataBaseController(string connectionString)
        {
            _connectionString = connectionString;
        }
        public IEnumerable<string> GetCodesMo()
        {
            return GetCodesMoFomDbAsDataTable();

            IEnumerable<string> GetCodesMoFomDbAsDataTable()
            {
                const string sql = "select distinct f.MCOD from nsi.F032 f where f.oktmo_p like '76000000%' AND SYSDATE BETWEEN f.D_BEGIN_OMS AND COALESCE(f.D_END, F.DATEEND, SYSDATE)";
                Exception innerException = null;
                var result = new List<string>();

                using (var connection = new OracleConnection(_connectionString))
                {
                    try
                    {
                        connection.Open();
                        var command = new OracleCommand(sql, connection);
                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                                result.Add(reader.GetString(0));
                        }
                    }
                    catch (Exception ex)
                    {
                        innerException = ex;
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                if (innerException != null)
                    throw new ApplicationException("Ошибка получения данных о МО из базы данных", innerException);
                return result;
            }
        }
    }
}