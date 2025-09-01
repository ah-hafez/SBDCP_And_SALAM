using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionSubjectClassification : EntityBase, IAuditable
    {
        public int SubjectClassificationId { get; set; }
        public virtual SubjectClassification SubjectClassification { get; set; }
        public int TransactionId { get; set; }
      
    }
}
