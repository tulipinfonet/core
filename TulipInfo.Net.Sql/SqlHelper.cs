using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net.Sql
{
    public static class SqlHelper
    {
        public static SqlConnection CreateConnection(string connectionString)
        {
            return new SqlConnection(connectionString);
        }

        #region BeginTransaction
        #region sync
        public static SqlTransaction BeginTransaction(string connectionString)
        {
            return BeginTransaction(connectionString, IsolationLevel.ReadCommitted);
        }

        public static SqlTransaction BeginTransaction(string connectionString, IsolationLevel level)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return BeginTransaction(conn, level);
        }

        public static SqlTransaction BeginTransaction(SqlConnection conn)
        {
            return BeginTransaction(conn, IsolationLevel.ReadCommitted);
        }

        public static SqlTransaction BeginTransaction(SqlConnection conn, IsolationLevel level)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn.BeginTransaction(level);
        }
        #endregion

        #region async
        public static Task<SqlTransaction> BeginTransactionAsync(string connectionString)
        {
            return BeginTransactionAsync(connectionString, IsolationLevel.ReadCommitted);
        }
        public static Task<SqlTransaction> BeginTransactionAsync(string connectionString, IsolationLevel level)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return BeginTransactionAsync(conn, level);
        }

        public static Task<SqlTransaction> BeginTransactionAsync(SqlConnection conn)
        {
            return BeginTransactionAsync(conn, IsolationLevel.ReadCommitted);
        }

        public static async Task<SqlTransaction> BeginTransactionAsync(SqlConnection conn, IsolationLevel level)
        {
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            var tran = await conn.BeginTransactionAsync(level);
            return (tran as SqlTransaction)!;
        }
        #endregion

        #endregion

        #region ExecuteNonQuery

        #region sync
        public static int ExecuteNonQuery(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static int ExecuteNonQuery(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static int ExecuteNonQuery(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static int ExecuteNonQueryProc(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public static int ExecuteNonQueryProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static int ExecuteNonQueryProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static int ExecuteNonQuery(SqlConnection conn, SqlTransaction? tran, 
            string commandText, CommandType commandType, 
            SqlParameter[]? dbParam = null, 
            int commandTimeout = 0, 
            ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            if (logger != null)
            {
                logger.LogDebug("ExecuteNonQuery");
            }
            return cmd.ExecuteNonQuery();
        }
        #endregion

        #region asyn
        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static Task<int> ExecuteNonQueryAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static Task<int> ExecuteNonQueryAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static async Task<int> ExecuteNonQueryProcAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public static Task<int> ExecuteNonQueryProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static Task<int> ExecuteNonQueryProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<int> ExecuteNonQueryAsync(SqlConnection conn, SqlTransaction? tran, string commandText, CommandType commandType, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
                await conn.OpenAsync();

            if (logger != null)
            {
                logger.LogDebug("ExecuteNonQueryAsync");
            }

            return await cmd.ExecuteNonQueryAsync();
        }
        #endregion

        #endregion

        #region ExecuteReader

        #region sync
        public static SqlDataReader ExecuteReader(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static SqlDataReader ExecuteReader(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static SqlDataReader ExecuteReaderProc(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static SqlDataReader ExecuteReaderProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static SqlDataReader ExecuteReaderProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static SqlDataReader ExecuteReader(SqlConnection conn, SqlTransaction? tran,
            string commandText, CommandType commandType, SqlParameter[]? dbParam = null,
            int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            if (logger != null)
            {
                logger.LogDebug("ExecuteReader");
            }
            return cmd.ExecuteReader();
        }
        #endregion

        #region async
        public static Task<SqlDataReader> ExecuteReaderAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static Task<SqlDataReader> ExecuteReaderAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReaderAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static Task<SqlDataReader> ExecuteReaderAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static Task<SqlDataReader> ExecuteReaderProcAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = CreateConnection(connectionString);
            return ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static Task<SqlDataReader> ExecuteReaderProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteReaderAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static Task<SqlDataReader> ExecuteReaderProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static async Task<SqlDataReader> ExecuteReaderAsync(SqlConnection conn, 
            SqlTransaction? tran, string commandText, CommandType commandType, 
            SqlParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            if (logger != null)
            {
                logger.LogDebug("ExecuteReaderAsync");
            }
            return await cmd.ExecuteReaderAsync();
        }
        #endregion

        #endregion

        #region ExecuteScalar

        #region sync
        public static object? ExecuteScalar(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static object? ExecuteScalar(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static object? ExecuteScalar(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static object? ExecuteScalarProc(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static object? ExecuteScalarProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static object? ExecuteScalarProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static object? ExecuteScalar(SqlConnection conn, SqlTransaction? tran, 
            string commandText, CommandType commandType, SqlParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }

            if (logger != null)
            {
                logger.LogDebug("ExecuteScalar");
            }

            var dbResult = cmd.ExecuteScalar();
            if (dbResult == DBNull.Value)
                return null;
            return dbResult;
        }
        #endregion

        #region Async
        public static async Task<object?> ExecuteScalarAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static async Task<object?> ExecuteScalarAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarProcAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static async Task<object?> ExecuteScalarProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarAsync(SqlConnection conn, SqlTransaction? tran, 
            string commandText, CommandType commandType, SqlParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            if (logger != null)
            {
                logger.LogDebug("ExecuteScalarAsync");
            }

            var dbResult = await cmd.ExecuteScalarAsync();
            if (dbResult == DBNull.Value)
                return null;
            return dbResult;
        }
        #endregion

        #endregion

        #region ExecuteDataSet

        #region sync
        public static DataSet ExecuteDataSet(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static DataSet ExecuteDataSet(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSet(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSetProc(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (SqlConnection conn = CreateConnection(connectionString))
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static DataSet ExecuteDataSetProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            SqlConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSetProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static DataSet ExecuteDataSet(SqlConnection conn, SqlTransaction? tran, 
            string commandText, CommandType commandType, SqlParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            SqlCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout,logger);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            if (logger != null)
            {
                logger.LogDebug("ExecuteDataSet");
            }

            DataSet result = new DataSet();
            SqlDataAdapter adp = new SqlDataAdapter(cmd);
            adp.Fill(result);

            return result;
        }
        #endregion

        #endregion

        #region ExecuteDynamic
        #region Sync
        public static IEnumerable<ExpandoObject> ExecuteDynamic(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0,ILogger? logger=null)
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamic(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static IEnumerable<ExpandoObject> ExecuteDynamic(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReaderProc(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        #endregion

        #region Async
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        #endregion

        private static IEnumerable<ExpandoObject> ExecuteDynamic(SqlDataReader sdr,ILogger? logger)
        {
            if (logger != null)
            {
                logger.LogDebug("ExecuteDynamic");
            }
            List<ExpandoObject> result = new List<ExpandoObject>();
            while (sdr.Read())
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
        public static IEnumerable<T> ExecuteList<T>(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr,logger);
            }
        }
        public static IEnumerable<T> ExecuteList<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static IEnumerable<T> ExecuteList<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }

        public static IEnumerable<T> ExecuteListProc<T>(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReaderProc(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static IEnumerable<T> ExecuteListProc<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static IEnumerable<T> ExecuteListProc<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        #endregion

        #region Async
        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(string connectionString, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(SqlTransaction tran, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(SqlConnection conn, string commandText, SqlParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        #endregion
        private static IEnumerable<T> ExecuteList<T>(SqlDataReader sdr, ILogger? logger) where T : new()
        {
            if (logger != null)
            {
                logger.LogDebug("ExecuteList");
            }

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
                    if (sdr.IsDBNull(i) == false)
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

        private static SqlCommand PrepareCommand(SqlConnection conn,
            SqlTransaction? tran,
            string commandText,
            CommandType commandType,
            SqlParameter[]? dbParam = null,
            int commandTimeout = 0,
            ILogger? logger = null)
        {
            if (logger != null)
            {
                logger.LogDebug("PrepareCommand");
            }

            SqlCommand cmd = conn.CreateCommand();
            if (commandTimeout > 0)
            {
                cmd.CommandTimeout = commandTimeout;
            }

            cmd.CommandText = commandText;

            if (logger != null)
            {
                logger.LogDebug(commandText);
            }

            cmd.CommandType = commandType;

            if (logger != null)
            {
                logger.LogDebug(commandType.ToString());
            }

            if (tran != null)
            {
                cmd.Transaction = tran;
            }
            if (dbParam != null)
            {
                cmd.Parameters.AddRange(dbParam);
                foreach (var p in dbParam)
                {
                    if (logger != null)
                    {
                        logger.LogDebug("ParamName:" + (p.ParameterName ?? "")
                        + ";Direction:" + p.Direction
                        + ";Size:" + p.Size
                        + ";Precision:" + p.Precision
                        + ";Scale:" + p.Scale
                        + ";Value:" + ((p.Value == null || p.Value == DBNull.Value) ? "null" : p.Value.ToString())
                        );
                    }
                }
            }
            return cmd;
        }
    }
}
