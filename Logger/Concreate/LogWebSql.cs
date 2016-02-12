using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using WebLogger.Abstract;
using WebLogger.Abstract.Interface;
using WebLogger.Enum;

namespace WebLogger.Concreate
{

    public sealed class LogWebSql : LogWebBase<String>, ILogReader
    {
        private Lazy<SqlConnection> _conection;
        private string _query;
        private string _queryRead;
        private string _queryDelete;

        public LogWebSql()
        {
            _query = "insert into Log (message,exceptionType,stackTrace,exceptionMsg,httpMethod,path,urlReferrer,userAgent,isAuthenticated,type) " +
                                  "values (@message,@exceptionType,@stackTrace,@exceptionMsg,@httpMethod,@path,@urlReferrer,@userAgent,@isAuthenticated,@type)";
            _queryRead = "SELECT * " +
                         "FROM (SELECT *, ROW_NUMBER() OVER (ORDER BY [date] desc) AS [ROW_NUMBER] " +
                         "FROM [Log]) as tmp,(SELECT COUNT(*) as total FROM [Log]) as tot " +
                         "WHERE tmp.[ROW_NUMBER] BETWEEN @start AND @end";

            _queryDelete = "delete from [Log] where logId=@id";

            _conection = new Lazy<SqlConnection>(GetConnection);
        }
        public LogWebSql(string sqlInsert)
        {
            _query = sqlInsert;
            _conection = new Lazy<SqlConnection>(GetConnection);
        }
        private SqlConnection GetConnection()
        {
            var str = ConfigurationManager.ConnectionStrings["ShopContext"].ConnectionString;
            var sql = new SqlConnection(str);
            sql.Open();
            return sql;
        }

        #region write log

        protected override void Execute(TypeLog typeLog, string messageLog, Exception exception)
        {
            using (var command = new SqlCommand(_query, _conection.Value))
            {
                var message = new SqlParameter("@message", SqlDbType.NVarChar)
                {
                    Value = (object)messageLog ?? DBNull.Value
                };
                IncludeException(command, exception);
                var httpMethod = new SqlParameter("@httpMethod", SqlDbType.NVarChar)
                {
                    Value = (object)HttpMethod ?? DBNull.Value
                };
                var path = new SqlParameter("@path", SqlDbType.NVarChar) { Value = (object)Path ?? DBNull.Value };
                var urlReferrer = new SqlParameter("@urlReferrer", SqlDbType.NVarChar)
                {
                    Value = (object)UrlReferrer ?? DBNull.Value
                };
                var userAgent = new SqlParameter("@userAgent", SqlDbType.NVarChar)
                {
                    Value = (object)UserAgent ?? DBNull.Value
                };
                var isAuthenticated = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Value = IsAuthenticated };

                var type = new SqlParameter("@type", SqlDbType.TinyInt) { Value = (int)typeLog };

                command.Parameters.AddRange(new[] { message, httpMethod, path, urlReferrer, userAgent, isAuthenticated, type });
                command.ExecuteNonQuery();
            }
        }

        private void IncludeException(SqlCommand command, Exception exception)
        {
            SqlParameter exc = new SqlParameter("@exceptionType", SqlDbType.NVarChar);
            SqlParameter st = new SqlParameter("@stackTrace", SqlDbType.NVarChar);
            SqlParameter msg = new SqlParameter("@exceptionMsg", SqlDbType.NVarChar);
            if (exception != null)
            {
                while (exception.InnerException != null)
                {
                    exception = exception.InnerException;
                }
                exc.Value = exception.GetType().ToString();
                st.Value = (object)exception.StackTrace ?? DBNull.Value;
                msg.Value = exception.Message;
            }
            else
            {
                exc.Value = DBNull.Value;
                st.Value = DBNull.Value;
                msg.Value = DBNull.Value;
            }
            command.Parameters.AddRange(new[] { exc, st, msg });
        }

        #endregion


        #region read log

        public IEnumerable<object> LogRead()
        {
            yield break;
        }

        public dynamic LogReadPage(int page, int perPage)
        {
            var skip = (page - 1) * perPage;
            var take = skip + perPage;
            var start = new SqlParameter("@start", SqlDbType.Int) { Value = skip };
            var end = new SqlParameter("@end", SqlDbType.Int) { Value = take };

            using (var command = new SqlCommand(_queryRead, _conection.Value))
            {
                command.Parameters.AddRange(new[] { start, end });
                using (var reader = command.ExecuteReader())
                {
                    var list = new List<dynamic>();
                    while (reader.Read())
                    {
                        list.Add(SqlDataReaderToExpando(reader));
                    }
                    reader.Dispose();
                    reader.Close();
                    var group = list.GroupBy(s => new { s.total }).FirstOrDefault();

                    dynamic d = new ExpandoObject();
                    d.totalPagesCount = Math.Ceiling(Convert.ToDouble(group.Key.total) / perPage);
                    d.items = group.Select(c => c).ToList();

                    return d;
                }
            }
        }

        private dynamic ToExpando(dynamic item)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            foreach (var prop in item.GetType().GetProperties())
            {
                if (prop.Name == "total") continue;
                expandoObject.Add(prop.Name, prop.GetValue(item));
            }

            return expandoObject;
        }

        private dynamic SqlDataReaderToExpando(SqlDataReader reader)
        {
            var expandoObject = new ExpandoObject() as IDictionary<string, object>;

            for (var i = 0; i < reader.FieldCount; i++)
                expandoObject.Add(reader.GetName(i), reader[i]);

            return expandoObject;
        }

        #endregion

        public override bool Remove(int id)
        {
            var idPar = new SqlParameter("@id", SqlDbType.BigInt) { Value = id };
            using (var command = new SqlCommand(_queryDelete, _conection.Value))
            {
                command.Parameters.Add(idPar);
                return command.ExecuteNonQuery() > 0;
            }
        }

        private bool _disposed;

        public override void Dispose()
        {
            if (!_disposed && _conection.IsValueCreated &&
                _conection.Value.State != ConnectionState.Closed)
            {
                try
                {
                    _conection.Value.Dispose();
                    _conection.Value.Close();
                    _disposed = true;
                    GC.SuppressFinalize(this);
                }
                catch (SqlException e)
                {
                    throw new Exception("Dispose LogSql", e);
                }
            }

        }


        ~LogWebSql()
        {
            if (!_disposed)
            {
                Dispose();
            }
        }

    }
}