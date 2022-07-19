using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.ApplicationLock
{
    public class LockData
    {
        public LockData(string key)
            : this(key, "", "", DateTime.UtcNow)
        {

        }

        public LockData(string key, string identifier)
            : this(key, identifier, "", DateTime.UtcNow)
        {

        }

        public LockData(string key, string identifier, string customData)
            : this(key, identifier, customData, DateTime.UtcNow)
        {
        }

        public LockData(string key, string identifier, string customData, DateTime lockedUtcTime)
        {
            this.Key = key;
            this.Identifier = identifier;
            this.CustomData = customData;
            this.LockedUtcTime = lockedUtcTime;
        }

        public string Key { get; private set; }
        public string Identifier { get; private set; }
        public string CustomData { get; private set; }
        public DateTime LockedUtcTime { get; private set; }
    }
}
