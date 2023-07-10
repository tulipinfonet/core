using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Dynamic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net.MySql
{
    public class OracleClient
    {
        protected ILogger Logger { get; private set; }
        protected MySqlDatabaseOptions DbOptions { get; private set; }

        public OracleClient(ILogger<OracleClient> logger,
            IOptions<MySqlDatabaseOptions> dbOptions)
        {
            this.Logger = logger;
            this.DbOptions = dbOptions.Value;
        }

        #region CreateConnection
        public MySqlConnection CreateConnection()
        {
            return MySqlHelper.CreateConnection(this.DbOptions.ConnectionString);
        }
        #endregion

        #region BeginTransaction

        #region Sync
        public MySqlTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public MySqlTransaction BeginTransaction(IsolationLevel level)
        {
            MySqlConnection conn = this.CreateConnection();
            return MySqlHelper.BeginTransaction(conn);
        }
        #endregion

        #region Async
        public async Task<MySqlTransaction> BeginTransactionAsync()
        {
            return await BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }
        public Task<MySqlTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            MySqlConnection conn = this.CreateConnection();
            return MySqlHelper.BeginTransactionAsync(conn, level);
        }
        #endregion

        #endregion

        #region ExecuteNonQuery

        #region Sync
        public int ExecuteNonQuery(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQuery(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQuery(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQueryProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private int ExecuteNonQuery(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteNonQuery(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<int> ExecuteNonQueryAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public Task<int> ExecuteNonQueryAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public Task<int> ExecuteNonQueryProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<int> ExecuteNonQueryAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteNonQueryAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteReader
        #region Sync
        public MySqlDataReader ExecuteReader(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public MySqlDataReader ExecuteReader(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReader(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReaderProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public MySqlDataReader ExecuteReaderProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReaderProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        private MySqlDataReader ExecuteReader(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteReader(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<MySqlDataReader> ExecuteReaderAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<MySqlDataReader> ExecuteReaderAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<MySqlDataReader> ExecuteReaderAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<MySqlDataReader> ExecuteReaderProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<MySqlDataReader> ExecuteReaderProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<MySqlDataReader> ExecuteReaderProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<MySqlDataReader> ExecuteReaderAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteReaderAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteScalar
        #region sync
        public object? ExecuteScalar(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalar(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalar(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalarProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private object? ExecuteScalar(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteScalar(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<object?> ExecuteScalarAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<object?> ExecuteScalarAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteScalarAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSet(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSet(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSetProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private DataSet ExecuteDataSet(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDataSet(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region ExecuteDynamic
        #region Sync
        public IEnumerable<ExpandoObject> ExecuteDynamic(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamic(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamic(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamic(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProc(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProc(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProc(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProcAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProcAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return MySqlHelper.ExecuteDynamicProcAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteList
        #region Sync
        public IEnumerable<T> ExecuteList<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteList<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteList<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteList<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProc<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProc<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProc<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<T>> ExecuteListAsync<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListAsync<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListAsync<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProcAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProcAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return MySqlHelper.ExecuteListProcAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
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
