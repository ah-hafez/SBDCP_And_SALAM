using System;
using System.ComponentModel.DataAnnotations.Schema;
using MCS.Framework.AuditTrail;
using MCS.Framework.Entities;


namespace MCS.Domain
{
    public class Explanation : EntityBase, IAuditable
    {
        public int TransactionId { get; set; }
        public virtual Transaction Transaction  { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int ExplanationEditorType { get; set; }
        public virtual DocumentInfo Document { get; set; }
        public int PermissionId { get; set; }
        public virtual Permission Permission { get; set; }
        public int FromUserId { get; set; }
        public virtual UserProfile FromUser { get; set; }
        public bool CanBeDeleted { get; set; }
        public bool isCopies { get; set; }

        public bool? CanBeSigned { get; set; }

        [NotMapped]
        public int RowNumber { get; set; }

    }
}
