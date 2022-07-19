using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public class DbContextSqlConvert
    {

        IDbContextTableMapping _dbTableMapping;
        public DbContextSqlConvert(DbContext dbContext)
        {
            _dbTableMapping = new DbContextTableMapping(dbContext);
        }

        public DbContextSql ConvertToDeleteSql<EntityType>(Expression<Func<EntityType, bool>> filterExp)
        {
            var entityTableMapping = _dbTableMapping.GetEntityTableMapping<EntityType>();

            string tableName = entityTableMapping.TableFullName;

            var sqlConvert = new ExpressionFilterConvert<EntityType>(entityTableMapping);

            DbContextSql filterSql = sqlConvert.ConvertToSql(filterExp);
            if (string.IsNullOrWhiteSpace(filterSql.Sql))
            {
                throw new NotSupportedException("Must provide a filter");
            }

            string fullSql = $"Delete from {tableName} Where {filterSql.Sql}";
            return new DbContextSql(fullSql, filterSql.ParamValues);
        }

        public DbContextSql ConvertToUpdateSql<EntityType>(
            Expression<Func<EntityType, bool>> filterExp,
            Expression<Func<EntityType>> selectFieldsExp)
        {
            var entityTableMapping = _dbTableMapping.GetEntityTableMapping<EntityType>();

            string tableName = entityTableMapping.TableFullName;

            //get filter
            var filterConvert = new ExpressionFilterConvert<EntityType>(entityTableMapping);
            DbContextSql filterSql = filterConvert.ConvertToSql(filterExp);
            if (string.IsNullOrWhiteSpace(filterSql.Sql))
            {
                throw new NotSupportedException("Must provide a filter");
            }

            //get fields
            var fieldSelector = new ExpressionFieldSelector<EntityType>(entityTableMapping);
            var columnsToUpdate = fieldSelector.GetFieldAndValues(selectFieldsExp);
            if (columnsToUpdate.Count() == 0)
            {
                throw new ArgumentException("Cannot find any columns to update.");
            }

            //merge sql and param
            string columnsUpdatSql = "";
            object[] updateParamValues = new object[columnsToUpdate.Count()];
            int sqlPlaceIndex = filterSql.ParamValues.Count();
            int columnIndex = 0;
            foreach (var c in columnsToUpdate)
            {
                if (columnIndex > 0)
                {
                    columnsUpdatSql += ",";
                }
                columnsUpdatSql += c.Key + "={" + (sqlPlaceIndex++) + "}";
                updateParamValues[columnIndex] = c.Value;
                columnIndex++;
            }

            string fullSql = $"Update {tableName} Set {columnsUpdatSql} Where {filterSql.Sql}";
            return new DbContextSql(fullSql, filterSql.ParamValues.Concat(updateParamValues));
        }
    }
}
