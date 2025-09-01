using MCS.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Business.Implementation
{
    public class BarcodeInfo
    {
        public string Value { get; set; }
        public int ReferenceId { get; set; }
        public int ReferenceTypeId { get; set; }
        public virtual Lookup ReferenceType { get; set; }
        public string EntityName { get; set; }
    }
}
