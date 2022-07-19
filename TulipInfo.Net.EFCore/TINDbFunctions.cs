using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.EFCore
{
    public static class TINDbFunctions
    {
        //https://docs.microsoft.com/en-us/ef/core/querying/user-defined-function-mapping
        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="searchText"></param>
        /// <param name="searchMode">For MySql, the value could be: IN NATURAL LANGUAGE MODE,WITH QUERY EXPANSION,IN BOOLEAN MODE</param>
        /// <returns></returns>
        public static int FullTextSearch(string columnName, string searchText)
            => throw new InvalidOperationException("This is a database function mapping");
    }
}
