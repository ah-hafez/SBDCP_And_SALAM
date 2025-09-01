using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.External
{
    public class VIPEditOutboundExternalVM : VIPTransactionVM
    {

        public override TransactionCategory Type
        {
            get { return TransactionCategory.ExternalOutbound; }
        }
        public EditorType EditorType { get; set; }
        public VIPEditOutboundExternalBasicInfoVM OutboundExternalBasicInfoEdit { get; set; } = new VIPEditOutboundExternalBasicInfoVM();
        public int? ActionId { get; set; }

        public bool? AssignmentPaperMainSource { get; set; }
        public bool? MainPlaceHolderAssignmentPaperCopy { get; set; }
        public bool? IsEnableAssignBack { get; set; }
        public List<VIPTransactionAssignmentVM> AssignmentVM { get; set; } = new List<VIPTransactionAssignmentVM>();
        public string RemindDateH { get; set; }
        public DateTime? RemindDate { get; set; }
        public string DocumentBase64String { get; set; }
        

    }
}