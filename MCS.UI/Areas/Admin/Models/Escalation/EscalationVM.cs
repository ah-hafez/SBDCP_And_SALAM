using System.Collections.Generic;
using System.Linq;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.Admin.Models.Escalation
{
    public class EscalationVM 
    {
        public int Id { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.ActionId")]
        public string EscalationAction { get; set; }//الإجراء//
        [CustomDisplayName("User.Transaction.Assignment.ActionId")]
        [CustomRequired("User.Transaction.Copies.ActionRequired")]
        public int EscalationActionId { get; set; }
        [CustomDisplayName("Admin.Setting.EscalationTo")]
        public string EscalationTo { get; set; }//تصعيد إلى//
        [CustomDisplayName("Admin.Setting.EscalationTo")]
        [CustomRequired("Admin.Setting.EscalationToRequired")]
        public int EscalationToId { get; set; }
        public int TransactionCategory { get; set; }
        public string TransCategoryIdEncrypt { get; set; }//نوع المعاملة//
        public string TransactionCategoryName { get; set; }

        [CustomDisplayName("Admin.Escalation.NumberofDays")]
        [CustomRequired("Admin.Escalation.NumberofDaysRequired")]
        [CustomRange(("Admin.Escalation.NumberofDaysRange"), 0, 500)]
        public int EscalationAfterDays { get; set; }
        public string Priority { get; set; }//الأهميه//
        [CustomDisplayName("User.Transaction.PriorityLevel")]
        [CustomRequired("User.Transaction.PriorityRequired")]
        public int PriorityId { get; set; }
        public List<EscalationVM> EscalationsGrid { get; set; } = (AjaxGrid<EscalationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<EscalationVM>(), 1, 0, false);

    }
}