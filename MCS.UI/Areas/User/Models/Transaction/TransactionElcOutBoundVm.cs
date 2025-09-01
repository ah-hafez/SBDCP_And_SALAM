using MCS.Framework.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.UI.Areas.User.Models.Transaction
{
   public class TransactionElcOutBoundVm 
    {
        public int Id { get; set; }
        public int TransactionId { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; } 
        public int? UserId { get; set; }
        public string UserName { get; set; }
        public bool Ishidden { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
