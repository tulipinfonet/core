using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipInfo.Net.Oracle;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDefaultOracleClient(this IServiceCollection services,
                Action<OracleDatabaseOptions>? optAction = null)
        {
            return services.AddOracleClient<OracleClient, OracleDatabaseOptions>(optAction);
        }

        public static IServiceCollection AddOracleClient<SqlClientType, DatabaseOptionsType>(this IServiceCollection services,
                Action<DatabaseOptionsType>? optAction = null)
                where SqlClientType : OracleClient
                where DatabaseOptionsType : OracleDatabaseOptions
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
