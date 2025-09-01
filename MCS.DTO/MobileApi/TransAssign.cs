using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MobileApi.Domain
{
    public class TransAssign
    {
        public int PersonId { get; set; }

        public int EntityId { get; set; }

        public int ProcessId { get; set; }

        public string Remarks { get; set; }
    }
}
