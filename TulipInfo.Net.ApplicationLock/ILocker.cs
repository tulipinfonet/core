using System;

namespace TulipInfo.Net.ApplicationLock
{
    public interface ILocker
    {
        LockContext Lock(string key);
        LockContext Lock(string key, string identifier);
        LockContext Lock(string key, string identifier, string customData);
        LockContext TryLock(string key);
        LockContext TryLock(string key, string identifier);
        LockContext TryLock(string key, string identifier, string customData);
        void UnLock(string key, string identifier);
    }
}
