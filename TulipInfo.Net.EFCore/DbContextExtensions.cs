using Microsoft.EntityFrameworkCore;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TulipInfo.Net.EFCore
{
    public static class DbContextExtensions
    {
        public static int DeleteDirectly<EntityType>(this DbContext dbContext,
            Expression<Func<EntityType, bool>> filterExp)
        {
            DbContextSqlConvert sqlConvert = new DbContextSqlConvert(dbContext);
            var dbsql = sqlConvert.ConvertToDeleteSql(filterExp);
            return dbContext.Database.ExecuteSqlRaw(dbsql.Sql, dbsql.ParamValues);
        }

        public static Task<int> DeleteDirectlyAsync<EntityType>(this DbContext dbContext,
            Expression<Func<EntityType, bool>> filterExp)
        {
            DbContextSqlConvert sqlConvert = new DbContextSqlConvert(dbContext);
            var dbsql = sqlConvert.ConvertToDeleteSql(filterExp);
            return dbContext.Database.ExecuteSqlRawAsync(dbsql.Sql, dbsql.ParamValues);
        }

        public static int UpdateDirectly<EntityType>(this DbContext dbContext,
            Expression<Func<EntityType, bool>> filterExp,
            Expression<Func<EntityType>> selectFieldsExp)
        {
            DbContextSqlConvert sqlConvert = new DbContextSqlConvert(dbContext);
            var dbsql = sqlConvert.ConvertToUpdateSql(filterExp, selectFieldsExp);
            return dbContext.Database.ExecuteSqlRaw(dbsql.Sql, dbsql.ParamValues);
        }

        public static Task<int> UpdateDirectlyAsync<EntityType>(this DbContext dbContext,
            Expression<Func<EntityType, bool>> filterExp,
            Expression<Func<EntityType>> selectFieldsExp)
        {
            DbContextSqlConvert sqlConvert = new DbContextSqlConvert(dbContext);
            var dbsql = sqlConvert.ConvertToUpdateSql(filterExp, selectFieldsExp);
            return dbContext.Database.ExecuteSqlRawAsync(dbsql.Sql, dbsql.ParamValues);
        }
    }
}
