using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TulipInfo.Net.MySql;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddSqlClient<MySqlClientType>(this IServiceCollection services,
                Action<DatabaseOptions>? optAction=null)
                where MySqlClientType :  MySqlClient
        {
            if (optAction == null)
            {
                optAction = (opt) =>
                {
                };
            }
            services.Configure<DatabaseOptions>(optAction);
            services.AddSingleton<MySqlClientType>();

            return services;
        }
    }
}
