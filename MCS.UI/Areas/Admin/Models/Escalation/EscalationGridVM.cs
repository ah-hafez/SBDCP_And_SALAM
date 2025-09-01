
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Escalation
{
    public class EscalationGridVM
    {
        public int Id { get; set; }
        public int Key { get; set; }
        [CustomDisplayName("User.Transaction.Assignment.Reason")]
        public string EscalationAction { get; set; }//الإجراء//
        [CustomDisplayName("User.Transaction.Assignment.Reason")]
        public int EscalationActionId { get; set; }
        [CustomDisplayName("Admin.Setting.EscalationTo")]
        public string EscalationTo { get; set; }//تصعيد إلى//
        [CustomDisplayName("Admin.Setting.EscalationTo")]
        public int EscalationToId { get; set; }

        [CustomDisplayName("Admin.Escalation.NumberofDays")]
        public int EscalationAfterDays { get; set; }
        public string Priority { get; set; }//الأهميه//
        [CustomDisplayName("User.Transaction.PriorityLevel")]
        public int PriorityId { get; set; }

    }
}