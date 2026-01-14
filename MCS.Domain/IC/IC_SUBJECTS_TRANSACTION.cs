using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class IC_SUBJECTS_TRANSACTION : EntityBase
    {
        public int TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public int IC_SUBJECTId { get; set; }

        public virtual IC_SUBJECT IC_SUBJECTS { get; set; }
        public int? Number { get; set; }
        public string  Description{ get; set; }
        public string  Part{ get; set; }



    }
}
