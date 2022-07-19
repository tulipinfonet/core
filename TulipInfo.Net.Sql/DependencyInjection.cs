using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipInfo.Net.Sql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDefaultSqlClient(this IServiceCollection services,
                Action<SqlDatabaseOptions>? optAction = null)
        {
            return services.AddSqlClient<SqlClient, SqlDatabaseOptions>(optAction);
        }

        public static IServiceCollection AddSqlClient<SqlClientType, DatabaseOptionsType>(this IServiceCollection services,
                Action<DatabaseOptionsType>? optAction = null)
                where SqlClientType : SqlClient
                where DatabaseOptionsType : SqlDatabaseOptions
        {
            if (optAction == null)
            {
                optAction = (opt) =>
                {
                };
            }
            services.Configure<DatabaseOptionsType>(optAction);
            services.AddSingleton<SqlClientType>();

            return services;
        }
    }
}
