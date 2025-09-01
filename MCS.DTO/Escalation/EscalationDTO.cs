using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO.Escalation
{
   public class EscalationDTO
    {
        public int Id { get; set; }
        public string EscalationAction { get; set; }//الإجراء//
        public int EscalationActionId { get; set; }
        public string EscalationTo { get; set; }//تصعيد إلى//
        public int EscalationToId { get; set; }
        public int TransactionCategory { get; set; }//نوع المعاملة//
        public string TransactionCategoryName { get; set; }
        public int EscalationAfterDays { get; set; }
        public string Priority { get; set; }//الأهميه//
        public int PriorityId { get; set; }

    }
}
