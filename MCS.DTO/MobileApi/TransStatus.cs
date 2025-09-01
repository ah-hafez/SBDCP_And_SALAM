using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApi.Domain
{
    public class TransStatus
    {
        public int TransId { get; set; }

        public int StatusId { get; set; }

        public int ProcessId { get; set; }

        public string Reason { get; set; }
    }
}
