using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class BaseFollowupDto
    {
        public int ProccessId { get; set; }
        public int PeriodId { get; set; }
        public DateTime? DateTo { get; set; }
        public string DateToH { get; set; }
        public bool IsImportant { get; set; }
        public int FollowUpStatusId { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreationDateHj { get; set; }
        public bool Active { get; set; }
        public int CreatingUserId { get; set; }
        public int CreatingEntityId { get; set; }
        public int FollowUpTypeId { get; set; }
        public DateTime FollowUpExpireDate { get; set; }
        public string FollowUpExpireDateHj { get; set; }
        public int FollowUpEntityId { get; set; }
        public int TransactionId { get; set; }
        public int? FollowUpUserId { get; set; }




    }
}
