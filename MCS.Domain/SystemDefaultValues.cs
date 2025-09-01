
using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class SystemDefaultValues : EntityBase
    {
        public int CategoryId { get; set; }
        public int TypeId { get; set; }
        public int? DefaultValueId { get; set; }

    }
}
