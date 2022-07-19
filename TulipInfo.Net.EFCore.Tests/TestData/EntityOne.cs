using System;
using System.Collections.Generic;
using System.Text;

namespace TulipInfo.Net.EFCore.Tests
{
    public class EntityOne
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public int Age { get; set; }
        public DateTime CreatedDateTime { get; set; }
        public bool IsActive { get; set; }
    }
}
