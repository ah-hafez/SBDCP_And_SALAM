using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class Barcode : EntityBase
    {
        public string Value { get; set; }
        public int ReferenceId { get; set; }
        public int ReferenceTypeId { get; set; }
        public virtual Lookup ReferenceType { get; set; }
    }
}
