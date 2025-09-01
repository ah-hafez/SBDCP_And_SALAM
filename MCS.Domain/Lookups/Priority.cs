using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Priority : LookupBase, ILocalizeEntity
    {
        public bool HasDate { get; set; }
        public int LateForEntity { get; set; }
        public int LateForUser { get; set; }
        public int Sort { get; set; }
        public bool HasPriorityExceptions { get; set; }
        public int ProcessPeriod { get; set; }
        public virtual List<PriorityException> PriorityExceptions { get; set; }
    }
}
