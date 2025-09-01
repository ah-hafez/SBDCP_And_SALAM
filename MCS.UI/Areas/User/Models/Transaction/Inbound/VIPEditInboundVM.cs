using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class VIPEditInboundVM : TransactionVM
    {
        public VIPEditInboundBasicInfoVM InboundBasicInfoEdit { get; set; }

        public override TransactionCategory Type
        {
            get { return TransactionCategory.Inbound; }
        }
        public int ModifiedByUserId { get; set; }
        public EditorType EditorType { get; set; }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
        public string defaultTabId { get; set; }
        public List<VIPTransactionAssignmentVM> AssignmentVM { get; set; } = new List<VIPTransactionAssignmentVM>();
        public bool? IsEnableAssignBack { get; set; }
        public string Notes { get; set; }
        public string Summary { get; set; } //الملخص//
        public string RemindDateH { get; set; }
        public DateTime? RemindDate { get; set; }
    }
}