using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class HubRQUID : EntityBase
    {
        public long TransactionNumber { get; set; }
        public string RQUID { get; set; }
    }
}
