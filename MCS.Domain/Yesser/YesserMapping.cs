using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class YesserMapping : EntityBase
    {
        public int TypeId { get; set; }
        public string YesserTypeId { get; set; }
        public int CloudTypeId { get; set; }
        public virtual ExternalParty CloudType { get; set; }
        public byte[] Exponent { get; set; }
        public byte[] Modulus { get; set; }
    }
}
