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
using Oracle.ManagedDataAccess;
using Oracle.ManagedDataAccess.Client;

namespace TulipInfo.Net.Oracle
{
    public class OracleClient
    {
        protected ILogger Logger { get; private set; }
        protected OracleDatabaseOptions DbOptions { get;private set; }

        public OracleClient(ILogger<OracleClient> logger,
            IOptions<OracleDatabaseOptions> dbOptions)
        {
            this.Logger = logger;
            this.DbOptions = dbOptions.Value;
        }

        #region CreateConnection
        public OracleConnection CreateConnection()
        {
            return OracleHelper.CreateConnection(this.DbOptions.ConnectionString);
        }
        #endregion

        #region BeginTransaction

        #region Sync
        public OracleTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public OracleTransaction BeginTransaction(IsolationLevel level)
        {
            OracleConnection conn = this.CreateConnection();
            return OracleHelper.BeginTransaction(conn);
        }
        #endregion

        #region Async
        public async Task<OracleTransaction> BeginTransactionAsync()
        {
            return await BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }
        public Task<OracleTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            OracleConnection conn = this.CreateConnection();
            return OracleHelper.BeginTransactionAsync(conn, level);
        }
        #endregion

        #endregion

        #region ExecuteNonQuery

        #region Sync
        public int ExecuteNonQuery(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQuery(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQuery(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = this.CreateConnection())
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public int ExecuteNonQueryProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public int ExecuteNonQueryProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private int ExecuteNonQuery(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteNonQuery(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<int> ExecuteNonQueryAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public Task<int> ExecuteNonQueryAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryProcAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public Task<int> ExecuteNonQueryProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public Task<int> ExecuteNonQueryProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<int> ExecuteNonQueryAsync(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteNonQueryAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteReader
        #region Sync
        public OracleDataReader ExecuteReader(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public OracleDataReader ExecuteReader(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public OracleDataReader ExecuteReader(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public OracleDataReader ExecuteReaderProc(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public OracleDataReader ExecuteReaderProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public OracleDataReader ExecuteReaderProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        private OracleDataReader ExecuteReader(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteReader(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<OracleDataReader> ExecuteReaderAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<OracleDataReader> ExecuteReaderAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<OracleDataReader> ExecuteReaderAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<OracleDataReader> ExecuteReaderProcAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<OracleDataReader> ExecuteReaderProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteReaderAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<OracleDataReader> ExecuteReaderProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<OracleDataReader> ExecuteReaderAsync(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteReaderAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteScalar
        #region sync
        public object? ExecuteScalar(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalar(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalar(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public object? ExecuteScalarProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public object? ExecuteScalarProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private object? ExecuteScalar(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteScalar(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public async Task<object?> ExecuteScalarAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public async Task<object?> ExecuteScalarProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public async Task<object?> ExecuteScalarProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private Task<object?> ExecuteScalarAsync(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteScalarAsync(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion
        #endregion

        #region ExecuteDataSet
        public DataSet ExecuteDataSet(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSet(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSet(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection())
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public DataSet ExecuteDataSetProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public DataSet ExecuteDataSetProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private DataSet ExecuteDataSet(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDataSet(conn, tran, commandText, commandType, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region ExecuteDynamic
        #region Sync
        public IEnumerable<ExpandoObject> ExecuteDynamic(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamic(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamic(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamic(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProc(this.CreateConnection(),  commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProc(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProc(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProcAsync(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProcAsync(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return OracleHelper.ExecuteDynamicProcAsync(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #endregion

        #region ExecuteList
        #region Sync
        public IEnumerable<T> ExecuteList<T>(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteList<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteList<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteList<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteList<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProc<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProc<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public IEnumerable<T> ExecuteListProc<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProc<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        #endregion

        #region Async
        public Task<IEnumerable<T>> ExecuteListAsync<T>(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListAsync<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListAsync<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProcAsync<T>(this.CreateConnection(), commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }
        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProcAsync<T>(tran, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
        }

        public Task<IEnumerable<T>> ExecuteListProcAsync<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            return OracleHelper.ExecuteListProcAsync<T>(conn, commandText, dbParam, GetTimeoutInSeconds(commandTimeout), this.Logger);
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
