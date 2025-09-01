
using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Counter : EntityBase
    {
        public bool IsGeneral { get; set; }
        public string Year { get; set; }
        public bool ResetByYear { get; set; }
        public int OwnerEntityId { get; set; }
        public virtual LocalizationIdentifier Description { get; set; }
        public virtual IList<CounterDetail> CounterDetails { get; set; }
    }
}
