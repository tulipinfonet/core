using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.ApplicationLock
{
    public class LockContext : IDisposable
    {
        public bool IsSuccess { get; private set; }
        public LockData LockedData { get; private set; }
        private ILocker LockerInstance { get; set; }

        public LockContext(bool isSuccess, LockData data, ILocker lockerInstance)
        {
            this.IsSuccess = isSuccess;
            this.LockedData = data;
            this.LockerInstance = lockerInstance;
        }

        public void Dispose()
        {
            if (this.IsSuccess && this.LockerInstance != null && this.LockedData!=null)
            {
                this.LockerInstance.UnLock(this.LockedData.Key, this.LockedData.Identifier);
            }
        }
    }
}
