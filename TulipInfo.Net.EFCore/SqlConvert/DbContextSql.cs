using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public class DbContextSql
    {
        public DbContextSql(string sql, IEnumerable<object> paramValues)
        {
            this.Sql = sql;
            this.ParamValues = paramValues;
        }
        public string Sql { get; private set; }
        public IEnumerable<object> ParamValues { get; private set; }
    }
}
