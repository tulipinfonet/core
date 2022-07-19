using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.ApplicationLock
{
    public class LockOptions
    {
        public int TimeOutInMilliSecond { get; set; } = 10*1000;
    }
}
