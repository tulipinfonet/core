using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Concurrent;

namespace TulipInfo.Net.ApplicationLock
{
    public class MemoryLocker : ILocker
    {
        static ConcurrentDictionary<string, LockData> LockedData = new ConcurrentDictionary<string, LockData>();

        LockOptions _opt = null;
        public MemoryLocker(LockOptions opt)
        {
            _opt = opt;
        }

        public LockContext Lock(string key)
        {
            return Lock(key, "", "");
        }

        public LockContext Lock(string key, string identifier)
        {
            return Lock(key, identifier, "");
        }

        public LockContext Lock(string key, string identifier, string customData)
        {
            bool locked = false;
            DateTime startTime = DateTime.UtcNow;
            LockData data = null;
            while (!locked)
            {
                data = new LockData(key, identifier, customData);
                locked = LockedData.TryAdd(key, data);
                if (locked == false)
                {
                    TryRemoveExpiredLock(key);
                    if ((DateTime.UtcNow - startTime).TotalMilliseconds > _opt.TimeOutInMilliSecond)
                    {
                        LockedData.TryGetValue(key, out data);
                        return new LockContext(false, data, null);
                    }
                }
            }

            return new LockContext(true, data, this);
        }

        public LockContext TryLock(string key)
        {
            return TryLock(key, "", "");
        }

        public LockContext TryLock(string key, string identifier)
        {
            return TryLock(key, identifier, "");
        }

        public LockContext TryLock(string key, string identifier, string customData)
        {
            TryRemoveExpiredLock(key);
            LockData data = new LockData(key, identifier, customData);
            bool locked = LockedData.TryAdd(key, data);
            if (locked == false)
            {
                LockedData.TryGetValue(key, out data);
                return new LockContext(false, data, null);
            }
            return new LockContext(true, data, this);
        }

        public void UnLock(string key, string identifier)
        {
            LockData data;
            if (LockedData.TryGetValue(key, out data))
            {
                if(data.Identifier== identifier)
                {
                    LockedData.TryRemove(key, out _);
                }
            }
        }

        private void TryRemoveExpiredLock(string key)
        {
            LockData data;
            if (LockedData.TryGetValue(key, out data))
            {
                if ((DateTime.UtcNow - data.LockedUtcTime).TotalMilliseconds > _opt.TimeOutInMilliSecond)
                {
                    LockedData.TryRemove(key, out _);
                }
            }
        }
    }
}
