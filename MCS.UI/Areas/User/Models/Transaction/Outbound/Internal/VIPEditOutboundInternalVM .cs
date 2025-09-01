using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Internal
{
    public class VIPEditOutboundInternalVM : VIPTransactionVM
    {

        public override TransactionCategory Type
        {
            get { return TransactionCategory.InternalOutbound; }
        }
        public EditorType EditorType { get; set; }
        public VIPEditOutboundInternalBasicInfoVM OutboundInternalBasicInfoEdit { get; set; } = new VIPEditOutboundInternalBasicInfoVM();
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
        public int? ActionId { get; set; }

        public bool? AssignmentPaperMainSource { get; set; }
        public bool? MainPlaceHolderAssignmentPaperCopy { get; set; }
        public bool? IsEnableAssignBack { get; set; }
        public List<VIPTransactionAssignmentVM> AssignmentVM { get; set; } = new List<VIPTransactionAssignmentVM>();
        public string RemindDateH { get; set; }
        public DateTime? RemindDate { get; set; }
        public string SavedTransactionAssignment { get; set; }
    }
}