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
using MySqlConnector;

namespace TulipInfo.Net.MySql
{
    public class MySqlClient
    {
        protected ILogger Logger { get; private set; }
        protected DatabaseOptions DbOptions { get;private set; }

        public MySqlClient(ILogger<MySqlClient> logger,
            IOptions<DatabaseOptions> dbOptions)
        {
            this.Logger = logger;
            this.DbOptions = dbOptions.Value;
        }

        #region CreateConnection
        public MySqlConnection CreateConnection()
        {
            return this.CreateConnection(this.DbOptions.ConnectionString);
        }
        public MySqlConnection CreateConnection(string connectionString)
        {
            return new MySqlConnection(connectionString);
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
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn.BeginTransaction(level);
        }
        public MySqlTransaction BeginTransaction(string connectionString)
        {
            return BeginTransaction(connectionString, IsolationLevel.ReadCommitted);
        }

        public MySqlTransaction BeginTransaction(string connectionString, IsolationLevel level)
        {
            MySqlConnection conn = this.CreateConnection(connectionString);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn.BeginTransaction(level);
        }
        #endregion

        #region Async
        public async Task<MySqlTransaction> BeginTransactionAsync()
        {
            return await BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task<MySqlTransaction> BeginTransactionAsync(IsolationLevel level)
        {
            MySqlConnection conn = this.CreateConnection();
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            var tran= await conn.BeginTransactionAsync(level);
            return (tran as MySqlTransaction)!;
        }
        public async Task<MySqlTransaction> BeginTransactionAsync(string connectionString)
        {
            return await BeginTransactionAsync(connectionString, IsolationLevel.ReadCommitted);
        }
        public async Task<MySqlTransaction> BeginTransactionAsync(string connectionString, IsolationLevel level)
        {
            MySqlConnection conn = this.CreateConnection(connectionString);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            var tran = await conn.BeginTransactionAsync(level);
            return (tran as MySqlTransaction)!;
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
        public int ExecuteNonQuery(string connectionString,string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection(connectionString))
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
        public int ExecuteNonQueryProc(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection(connectionString))
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
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam,commandTimeout);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            Logger.LogDebug("ExecuteNonQuery");
            return cmd.ExecuteNonQuery();
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
        public async Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public async Task<int> ExecuteNonQueryAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection())
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public async Task<int> ExecuteNonQueryProcAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = this.CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public async Task<int> ExecuteNonQueryProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return await ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public async Task<int> ExecuteNonQueryProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        private async Task<int> ExecuteNonQueryAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            Logger.LogDebug("ExecuteNonQueryAsync");
            return await cmd.ExecuteNonQueryAsync();
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
        public MySqlDataReader ExecuteReader(string connectionString,string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection(connectionString);
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReader(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public MySqlDataReader ExecuteReader(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReaderProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public MySqlDataReader ExecuteReaderProc(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection(connectionString);
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
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            Logger.LogDebug("ExecuteReader");
            return cmd.ExecuteReader();
        }
        #endregion

        #region Async
        public async Task<MySqlDataReader> ExecuteReaderAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection();
            return await ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public async Task<MySqlDataReader> ExecuteReaderAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection(connectionString);
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
        public async Task<MySqlDataReader> ExecuteReaderProcAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlConnection conn = CreateConnection(connectionString);
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
        private async Task<MySqlDataReader> ExecuteReaderAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            Logger.LogDebug("ExecuteReaderAsync");
            return await cmd.ExecuteReaderAsync();
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
        public object? ExecuteScalar(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
        public object? ExecuteScalarProc(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            Logger.LogDebug("ExecuteScalar");
            var dbResult = cmd.ExecuteScalar();
            if (dbResult == DBNull.Value)
                return null;
            return dbResult;
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
        public async Task<object?> ExecuteScalarAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
        public async Task<object?> ExecuteScalarProcAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
        private async Task<object?> ExecuteScalarAsync(MySqlConnection conn, MySqlTransaction? tran, string commandText, CommandType commandType, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            Logger.LogDebug("ExecuteScalarAsync");
            var dbResult = await cmd.ExecuteScalarAsync();
            if (dbResult == DBNull.Value)
                return null;
            return dbResult;
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
        public DataSet ExecuteDataSet(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
        public DataSet ExecuteDataSetProc(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (MySqlConnection conn = CreateConnection(connectionString))
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
            MySqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            Logger.LogDebug("ExecuteDataSet");
            DataSet result = new DataSet();
            DbDataAdapter adp = new MySqlDataAdapter(cmd);
            adp.Fill(result);

            return result;
        }
        #endregion

        #region ExecuteDynamic
        #region Sync
        public IEnumerable<ExpandoObject> ExecuteDynamic(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using(var sdr = ExecuteReader(commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public IEnumerable<ExpandoObject> ExecuteDynamic(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr);
        }

        public IEnumerable<ExpandoObject> ExecuteDynamic(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = ExecuteReaderProc(commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = ExecuteReaderProc(connectionString,commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public IEnumerable<ExpandoObject> ExecuteDynamicProc(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr);
        }

        public IEnumerable<ExpandoObject> ExecuteDynamicProc(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        #endregion

        #region Async
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderAsync(commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr);
        }

        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderProcAsync(commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr);
        }

        public async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr);
            }
        }
        #endregion

        private IEnumerable<ExpandoObject> ExecuteDynamic(MySqlDataReader sdr)
        {
            Logger.LogDebug("ExecuteDynamic");
            List<ExpandoObject> result = new List<ExpandoObject>();
            while(sdr.Read())
            {
                IDictionary<string, object?> item = new ExpandoObject() as IDictionary<string, object?>;
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    string key = sdr.GetName(i);
                    object value = sdr.GetValue(i);

                    if (sdr.GetValue(i).GetType() != typeof(DBNull))
                        item[key] = value;
                    else
                        item[key] = null;
                }
                result.Add((ExpandoObject)item);
            }

            return result;
        }
        #endregion

        #region ExecuteList
        #region Sync
        public IEnumerable<T> ExecuteList<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReader(commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public IEnumerable<T> ExecuteList<T>(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public IEnumerable<T> ExecuteList<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr);
        }

        public IEnumerable<T> ExecuteList<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }

        public IEnumerable<T> ExecuteListProc<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReaderProc(commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public IEnumerable<T> ExecuteListProc<T>(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReaderProc(connectionString,commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public IEnumerable<T> ExecuteListProc<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr);
        }

        public IEnumerable<T> ExecuteListProc<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        #endregion

        #region Async
        public async Task<IEnumerable<T>> ExecuteListAsync<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public async Task<IEnumerable<T>> ExecuteListAsync<T>(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public async Task<IEnumerable<T>> ExecuteListAsync<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr);
        }

        public async Task<IEnumerable<T>> ExecuteListAsync<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }

        public async Task<IEnumerable<T>> ExecuteListProcAsync<T>(string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public async Task<IEnumerable<T>> ExecuteListProcAsync<T>(string connectionString, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        public async Task<IEnumerable<T>> ExecuteListProcAsync<T>(MySqlTransaction tran, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr);
        }

        public async Task<IEnumerable<T>> ExecuteListProcAsync<T>(MySqlConnection conn, string commandText, MySqlParameter[]? dbParam = null, int commandTimeout = 0) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr);
            }
        }
        #endregion
        private IEnumerable<T> ExecuteList<T>(MySqlDataReader sdr) where T : new()
        {
            Logger.LogDebug("ExecuteList");
            List<T> result = new List<T>();
            int index = 0;
            PropertyInfo[] propList = null!;
            while (sdr.Read())
            {
                if (index == 0)
                {
                    Type objType = typeof(T);
                    propList = new PropertyInfo[sdr.FieldCount];
                    for (int i = 0; i < sdr.FieldCount; i++)
                    {
                        string propertyName = sdr.GetName(i);
                        PropertyInfo? prop = objType.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.SetProperty | BindingFlags.IgnoreCase);
                        if (prop != null)
                        {
                            propList[i] = prop;
                        }
                    }

                    index++;
                }

                T item = new T();
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    object? value = null;
                    if(sdr.IsDBNull(i)==false)
                    {
                        value = sdr.GetValue(i);
                    }

                    PropertyInfo prop = propList[i];
                    if (prop != null)
                    {
                        prop.SetValue(item, value);
                    }
                }
                result.Add(item);
            }

            return result;
        }
        #endregion
        private MySqlCommand PrepareCommand(MySqlConnection conn, 
            MySqlTransaction? tran,
            string commandText,
            CommandType commandType,
            MySqlParameter[]? dbParam=null,
            int commandTimeout = 0)
        {
            Logger.LogDebug("PrepareCommand");

            MySqlCommand cmd = conn.CreateCommand();
            if (this.DbOptions.CommandTimeout > 0)
            {
                cmd.CommandTimeout = this.DbOptions.CommandTimeout;
            }
            if (commandTimeout > 0)
            {
                cmd.CommandTimeout = commandTimeout;
            }

            cmd.CommandText = commandText;
            Logger.LogDebug(commandText);

            cmd.CommandType = commandType;
            Logger.LogDebug(commandType.ToString());

            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            if (dbParam != null)
            {
                cmd.Parameters.AddRange(dbParam);
                foreach (var p in dbParam)
                {
                    Logger.LogDebug("ParamName:" + (p.ParameterName ?? "")
                        + ";Direction:" + p.Direction
                        + ";Size:" + p.Size
                        + ";Precision:" + p.Precision
                        + ";Scale:" + p.Scale
                        + ";Value:" + ((p.Value == null || p.Value == DBNull.Value) ? "null" : p.Value.ToString())
                        );

                }
            }
            return cmd;
        }

    }
}
