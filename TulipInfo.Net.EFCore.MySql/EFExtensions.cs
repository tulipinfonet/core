using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using TulipInfo.Net.EFCore;

namespace Microsoft.EntityFrameworkCore
{
    public static class EFExtensions
    {
        public static void RegisterMySqlDbFunctions(this ModelBuilder modelBuilder)
        {
            modelBuilder.HasDbFunction(
                typeof(TINDbFunctions).GetMethod(nameof(TINDbFunctions.FullTextSearch), new[] { typeof(string), typeof(string) }))
                .HasTranslation(
                args =>
                {
                    string columnName = "";
                    var columnsExp = args.FirstOrDefault() as SqlConstantExpression;
                    if (columnsExp != null)
                    {
                        columnName = columnsExp.Value.ToString();
                    }

                    return new SqlFunctionExpression($"MATCH ({columnName}) AGAINST",
                        new SqlExpression[]{
                               args.Skip(1).FirstOrDefault()
                        },
                        false,
                        new bool[]{
                               false
                        },
                        typeof(Int32),
                        new IntTypeMapping("int", DbType.Int32));
                }
                );
        }
    }
}
