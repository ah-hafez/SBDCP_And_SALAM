using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Internal
{
    public class VIPEditOutboundInternalBasicInfoVM : BasicInfoBaseVM
    {
        public long Number { get; set; }  //رقم المعاملة//



        [CustomDisplayName("User.Transaction.BasicInfo.Type")]
        [CustomRequired("User.Transaction.BasicInfo.TypeRequired")]
        public int LetterTypeId { get; set; }  //نوع الخطاب الصادر الداخلي//

        [CustomDisplayName("User.Transaction.ConfidentialityLevel")]
        [CustomRequired("User.Transaction.ConfidentialityRequired")]
        public int ConfidentialityLevelId { get; set; }   //مستوى السريه//
        public string CreatedDateH { get; set; }
        public string ConfidentialityLevelText { get; set; }   //مستوى السريه//
        public string EntityName { get; set; }
        public string Subject { get; set; }
        public int PriorityLevelId { get; set; }   //مستوى السريه//
        public string PriorityLevelText { get; set; }   //مستوى السريه//
        public string LetterNumber { get; set; }
        public bool IsElcOutBound { get; set; }
        public long? OutBoundDraftNumber { get; set; }
        [CustomDisplayNameAttribute("User.Editor.ToFollowUp")]
        public bool ToFollowUp { get; set; }
        public string PriorityLeveText { get; set; }
        public string Summary { get; set; }
    }
}