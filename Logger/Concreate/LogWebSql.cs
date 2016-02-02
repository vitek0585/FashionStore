using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using WebLogger.Abstract;
using WebLogger.Enum;

namespace WebLogger.Concreate
{

    public sealed class LogWebSql : LogWebBase<String>
    {
        private Lazy<SqlConnection> _conection;
        private string _query;
        public LogWebSql()
        {
            _query = "insert into Log (message,exceptionType,stackTrace,exceptionMsg,httpMethod,path,urlReferrer,userAgent,isAuthenticated,type) " +
                                  "values (@message,@exceptionType,@stackTrace,@exceptionMsg,@httpMethod,@path,@urlReferrer,@userAgent,@isAuthenticated,@type)";

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

        protected override void Execute(TypeLog typeLog, string messageLog, Exception exception)
        {
            var command = new SqlCommand(_query, _conection.Value);
            var message = new SqlParameter("@message", SqlDbType.NVarChar) { Value = (object)messageLog ?? DBNull.Value };
            IncludeException(command, exception);
            var httpMethod = new SqlParameter("@httpMethod", SqlDbType.NVarChar) { Value = (object)HttpMethod ?? DBNull.Value };
            var path = new SqlParameter("@path", SqlDbType.NVarChar) { Value = (object)Path ?? DBNull.Value };
            var urlReferrer = new SqlParameter("@urlReferrer", SqlDbType.NVarChar) { Value = (object)UrlReferrer ?? DBNull.Value };
            var userAgent = new SqlParameter("@userAgent", SqlDbType.NVarChar) { Value = (object)UserAgent ?? DBNull.Value };
            var isAuthenticated = new SqlParameter("@isAuthenticated", SqlDbType.Bit) { Value = IsAuthenticated };

            var type = new SqlParameter("@type", SqlDbType.TinyInt) { Value = (int)typeLog };

            command.Parameters.AddRange(new[] { message, httpMethod, path, urlReferrer, userAgent, isAuthenticated, type });
            command.ExecuteNonQuery();
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

        private bool _disposed;
        public override void Dispose()
        {
            if (!_disposed && _conection.IsValueCreated)
            {
                try
                {
                    _conection.Value.Dispose();
                    _conection.Value.Close();
                }
                catch (SqlException e)
                {
                    throw new Exception("Dispose LogSql", e);
                }
                _disposed = true;
                GC.SuppressFinalize(this);
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