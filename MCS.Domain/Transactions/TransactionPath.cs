using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Entities;

namespace MCS.Domain
{
    public class TransactionPath : EntityBase
    {
        public string Name { get; set; }
        public int? UserId { get; set; }
        public int OrgUnitId { get; set; }
        public int TransactionTypeId { get; set; }

        [NotMapped]
        public bool IsReadOnly { get; set; }

        public virtual UserProfile User { get; set; }
        public virtual OrgUnit OrgUnit { get; set; }
        public virtual Lookup TransactionType { get; set; }
        public IList<TransactionPathDetails> TransactionPathDetails { get; set; }

    }
}
