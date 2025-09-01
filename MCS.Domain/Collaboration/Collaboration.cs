using System;
using MCS.Framework.Entities;
using MCS.Common;

namespace MCS.Domain
{
    public class Collaboration : EntityBase
    {
        public int? SenderId { get; set; }
        public virtual UserProfile Sender { get; set; }
        public int? ReceiverId { get; set; }
        public virtual UserProfile Receiver { get; set; }
        public string Text { get; set; }
        public int? TransactionId { get; set; }
        public virtual Transaction Transaction { get; set; }
        public Attachment Attachment { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public CollaborationMessageStatus Status { get; set; }
        
    }
}
