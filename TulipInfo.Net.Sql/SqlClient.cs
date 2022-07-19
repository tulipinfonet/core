using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;

namespace TulipInfo.Net.Sql
{
    public class SqlClient
    {
        protected ILogger Logger { get; private set; }
        protected SqlDatabaseOptions DbOptions { get;private set; }

        public SqlClient(ILogger<SqlClient> logger,
            IOptions<SqlDatabaseOptions> dbOptions)
        {
            this.Logger = logger;
            this.DbOptions = dbOptions.Value;
        }

        #region CreateConnection
        public SqlConnection CreateConnection()
        {
            return SqlHelper.CreateConnection(this.DbOptions.ConnectionString);
        }
        #endregion

        #region BeginTransaction

        #region Sync
        public SqlTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public SqlTransaction BeginTransaction(IsolationLevel level)
        {
            SqlConnection conn = this.CreateConnection();
            return SqlHelper.BeginTransaction(conn);
        }
        #endregion

        #region Async
        public async Task<SqlTransaction> BeginTransactionAsync()
        {
            return await BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }
        public Task<SqlTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            SqlConnection conn = this.CreateConnection();
            return SqlHelper.BeginTransactionAsync(conn, level);
        }
        #endregion

        #endregion

        #region ExecuteNonQuery

        #region Sync
        public int ExecuteNonQuery(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQuery(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQuery(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQueryProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private int ExecuteNonQuery(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteNonQuery(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<int> ExecuteNonQueryAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public Task<int> ExecuteNonQueryAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryProcAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public Task<int> ExecuteNonQueryProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<int> ExecuteNonQueryAsync(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteNonQueryAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteReader
        #region Sync
        public SqlDataReader ExecuteReader(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public SqlDataReader ExecuteReader(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public SqlDataReader ExecuteReader(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public SqlDataReader ExecuteReaderProc(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public SqlDataReader ExecuteReaderProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public SqlDataReader ExecuteReaderProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        private SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteReader(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<SqlDataReader> ExecuteReaderAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<SqlDataReader> ExecuteReaderAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<SqlDataReader> ExecuteReaderProcAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<SqlDataReader> ExecuteReaderProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<SqlDataReader> ExecuteReaderProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<SqlDataReader> ExecuteReaderAsync(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteReaderAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteScalar
        #region sync
        public object? ExecuteScalar(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalar(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalar(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalarProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private object? ExecuteScalar(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteScalar(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<object?> ExecuteScalarAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<object?> ExecuteScalarAsync(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteScalarAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSet(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSet(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSetProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private DataSet ExecuteDataSet(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDataSet(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region ExecuteDynamic
        #region Sync
        public IEnumerable<ExpandoObject> ExecuteDynamic(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamic(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamic(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamic(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProc(this.CreateConnection(),  commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProc(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProc(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProcAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProcAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return SqlHelper.ExecuteDynamicProcAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteList
        #region Sync
        public IEnumerable<T> ExecuteList<T>(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteList<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteList<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteList<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProc<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProc<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProc<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<T>> ExecuteListAsync<T>(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListAsync<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListAsync<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProcAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProcAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return SqlHelper.ExecuteListProcAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
       
        #endregion

        private int GetTimeoutInSeconds(int commandTimeout)
        {
            int timeOut = this.DbOptions.CommandTimeout;
            if (commandTimeout > 0)
            {
                timeOut = commandTimeout;
            }
            return timeOut;
        }
    }
}
