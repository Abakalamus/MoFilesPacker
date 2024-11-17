using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

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
                const string sql = "select distinct MCOD from nsi.F032 where 1=1";
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
            IEnumerable<string> GetCodesMoFromDataTable(DataTable source)
            {
                return (from DataRow row in source.Rows select row["MCOD"].ToString()).ToList();
            }
        }
    }
}