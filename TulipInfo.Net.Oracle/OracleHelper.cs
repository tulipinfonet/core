using Microsoft.Extensions.Logging;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TulipInfo.Net.Oracle
{
    public static class OracleHelper
    {
        public static OracleConnection CreateConnection(string connectionString)
        {
            return new OracleConnection(connectionString);
        }

        #region BeginTransaction
        #region sync
        public static OracleTransaction BeginTransaction(string connectionString)
        {
            return BeginTransaction(connectionString, IsolationLevel.ReadCommitted);
        }

        public static OracleTransaction BeginTransaction(string connectionString, IsolationLevel level)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return BeginTransaction(conn, level);
        }

        public static OracleTransaction BeginTransaction(OracleConnection conn)
        {
            return BeginTransaction(conn, IsolationLevel.ReadCommitted);
        }

        public static OracleTransaction BeginTransaction(OracleConnection conn, IsolationLevel level)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            return conn.BeginTransaction(level);
        }
        #endregion

        #region async
        public static Task<OracleTransaction> BeginTransactionAsync(string connectionString)
        {
            return BeginTransactionAsync(connectionString, IsolationLevel.ReadCommitted);
        }
        public static Task<OracleTransaction> BeginTransactionAsync(string connectionString, IsolationLevel level)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return BeginTransactionAsync(conn, level);
        }

        public static Task<OracleTransaction> BeginTransactionAsync(OracleConnection conn)
        {
            return BeginTransactionAsync(conn, IsolationLevel.ReadCommitted);
        }

        public static async Task<OracleTransaction> BeginTransactionAsync(OracleConnection conn, IsolationLevel level)
        {
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }
            var tran = await conn.BeginTransactionAsync(level);
            return (tran as OracleTransaction)!;
        }
        #endregion

        #endregion

        #region ExecuteNonQuery

        #region sync
        public static int ExecuteNonQuery(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static int ExecuteNonQuery(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static int ExecuteNonQuery(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static int ExecuteNonQueryProc(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public static int ExecuteNonQueryProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQuery(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static int ExecuteNonQueryProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQuery(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static int ExecuteNonQuery(OracleConnection conn, OracleTransaction? tran, 
            string commandText, CommandType commandType, 
            OracleParameter[]? dbParam = null, 
            int commandTimeout = 0, 
            ILogger? logger = null)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
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
        public static async Task<int> ExecuteNonQueryAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static Task<int> ExecuteNonQueryAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static Task<int> ExecuteNonQueryAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static async Task<int> ExecuteNonQueryProcAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }

        public static Task<int> ExecuteNonQueryProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteNonQueryAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static Task<int> ExecuteNonQueryProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteNonQueryAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<int> ExecuteNonQueryAsync(OracleConnection conn, OracleTransaction? tran, string commandText, CommandType commandType, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
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
        public static OracleDataReader ExecuteReader(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout,null, CommandBehavior.Default| CommandBehavior.CloseConnection);
        }
        public static OracleDataReader ExecuteReader(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static OracleDataReader ExecuteReader(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static OracleDataReader ExecuteReaderProc(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout,null, CommandBehavior.Default | CommandBehavior.CloseConnection);
        }
        public static OracleDataReader ExecuteReaderProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReader(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static OracleDataReader ExecuteReaderProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReader(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static OracleDataReader ExecuteReader(OracleConnection conn, OracleTransaction? tran,
            string commandText, CommandType commandType, OracleParameter[]? dbParam = null,
            int commandTimeout = 0, ILogger? logger = null, CommandBehavior behavior= CommandBehavior.Default)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            if (logger != null)
            {
                logger.LogDebug("ExecuteReader");
            }
            return cmd.ExecuteReader(behavior);
        }
        #endregion

        #region async
        public static Task<OracleDataReader> ExecuteReaderAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout,null, CommandBehavior.Default | CommandBehavior.CloseConnection);
        }
        public static Task<OracleDataReader> ExecuteReaderAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReaderAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }

        public static Task<OracleDataReader> ExecuteReaderAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReaderAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static Task<OracleDataReader> ExecuteReaderProcAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = CreateConnection(connectionString);
            return ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout, null, CommandBehavior.Default | CommandBehavior.CloseConnection);
        }
        public static Task<OracleDataReader> ExecuteReaderProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteReaderAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static Task<OracleDataReader> ExecuteReaderProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteReaderAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static async Task<OracleDataReader> ExecuteReaderAsync(OracleConnection conn, 
            OracleTransaction? tran, string commandText, CommandType commandType, 
            OracleParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null, CommandBehavior behavior= CommandBehavior.Default)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
            if (conn.State != ConnectionState.Open)
            {
                await conn.OpenAsync();
            }

            if (logger != null)
            {
                logger.LogDebug("ExecuteReaderAsync");
            }
            return ((await cmd.ExecuteReaderAsync(behavior)) as OracleDataReader)!;
        }
        #endregion

        #endregion

        #region ExecuteScalar

        #region sync
        public static object? ExecuteScalar(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static object? ExecuteScalar(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static object? ExecuteScalar(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static object? ExecuteScalarProc(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static object? ExecuteScalarProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteScalar(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static object? ExecuteScalarProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteScalar(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static object? ExecuteScalar(OracleConnection conn, OracleTransaction? tran, 
            string commandText, CommandType commandType, OracleParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
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
        public static async Task<object?> ExecuteScalarAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static async Task<object?> ExecuteScalarAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarProcAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static async Task<object?> ExecuteScalarProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return await ExecuteScalarAsync(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return await ExecuteScalarAsync(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static async Task<object?> ExecuteScalarAsync(OracleConnection conn, OracleTransaction? tran, 
            string commandText, CommandType commandType, OracleParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout, logger);
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
        public static DataSet ExecuteDataSet(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
            }
        }
        public static DataSet ExecuteDataSet(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSet(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.Text, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSetProc(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            using (OracleConnection conn = CreateConnection(connectionString))
            {
                return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
            }
        }
        public static DataSet ExecuteDataSetProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            OracleConnection conn = tran.Connection!;
            return ExecuteDataSet(conn, tran, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }
        public static DataSet ExecuteDataSetProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0)
        {
            return ExecuteDataSet(conn, null, commandText, CommandType.StoredProcedure, dbParam, commandTimeout);
        }

        public static DataSet ExecuteDataSet(OracleConnection conn, OracleTransaction? tran, 
            string commandText, CommandType commandType, OracleParameter[]? dbParam = null, 
            int commandTimeout = 0, ILogger? logger = null)
        {
            OracleCommand cmd = PrepareCommand(conn, tran, commandText, commandType, dbParam, commandTimeout,logger);
            if (conn.State != ConnectionState.Open)
                conn.Open();

            if (logger != null)
            {
                logger.LogDebug("ExecuteDataSet");
            }

            DataSet result = new DataSet();
            OracleDataAdapter adp = new OracleDataAdapter(cmd);
            adp.Fill(result);

            return result;
        }
        #endregion

        #endregion

        #region ExecuteDynamic
        #region Sync
        public static IEnumerable<ExpandoObject> ExecuteDynamic(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0,ILogger? logger=null)
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamic(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static IEnumerable<ExpandoObject> ExecuteDynamic(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReaderProc(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static IEnumerable<ExpandoObject> ExecuteDynamicProc(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        #endregion

        #region Async
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteDynamic(sdr, logger);
        }

        public static async Task<IEnumerable<ExpandoObject>> ExecuteDynamicProcAsync(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null)
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteDynamic(sdr, logger);
            }
        }
        #endregion

        private static IEnumerable<ExpandoObject> ExecuteDynamic(OracleDataReader sdr,ILogger? logger)
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
        public static IEnumerable<T> ExecuteList<T>(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReader(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr,logger);
            }
        }
        public static IEnumerable<T> ExecuteList<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = ExecuteReader(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static IEnumerable<T> ExecuteList<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReader(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }

        public static IEnumerable<T> ExecuteListProc<T>(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReaderProc(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static IEnumerable<T> ExecuteListProc<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = ExecuteReaderProc(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static IEnumerable<T> ExecuteListProc<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = ExecuteReaderProc(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        #endregion

        #region Async
        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = await ExecuteReaderAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static async Task<IEnumerable<T>> ExecuteListAsync<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(string connectionString, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(connectionString, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(OracleTransaction tran, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            var sdr = await ExecuteReaderProcAsync(tran, commandText, dbParam, commandTimeout);
            return ExecuteList<T>(sdr, logger);
        }

        public static async Task<IEnumerable<T>> ExecuteListProcAsync<T>(OracleConnection conn, string commandText, OracleParameter[]? dbParam = null, int commandTimeout = 0, ILogger? logger = null) where T : new()
        {
            using (var sdr = await ExecuteReaderProcAsync(conn, commandText, dbParam, commandTimeout))
            {
                return ExecuteList<T>(sdr, logger);
            }
        }
        #endregion
        private static IEnumerable<T> ExecuteList<T>(OracleDataReader sdr, ILogger? logger) where T : new()
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

        private static OracleCommand PrepareCommand(OracleConnection conn,
            OracleTransaction? tran,
            string commandText,
            CommandType commandType,
            OracleParameter[]? dbParam = null,
            int commandTimeout = 0,
            ILogger? logger = null)
        {
            if (logger != null)
            {
                logger.LogDebug("PrepareCommand");
            }

            OracleCommand cmd = conn.CreateCommand();
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
