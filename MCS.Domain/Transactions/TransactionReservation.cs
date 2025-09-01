using System;
using System.Collections.Generic;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionReservation : EntityBase
    {
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Count { get; set; }
        public string Reason { get; set; }
        public int TransactionCategoryId { get; set; }


        public virtual UserProfile User { get; set; }
        public virtual OrgUnit Entity { get; set; }
        public virtual Lookup TransactionCategory { get; set; }
        public virtual IList<Transaction> Transactions { get; set; }
        public string LetterNumber { get; set; }
    }
}