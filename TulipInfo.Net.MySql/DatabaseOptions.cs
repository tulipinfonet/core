using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.MySql
{
    public class DatabaseOptions
    {
        public string ConnectionString { get; set; } = String.Empty;
        /// <summary>
        /// Command timeout in seconds
        /// </summary>
        public int CommandTimeout { get; set; }
    }
}
