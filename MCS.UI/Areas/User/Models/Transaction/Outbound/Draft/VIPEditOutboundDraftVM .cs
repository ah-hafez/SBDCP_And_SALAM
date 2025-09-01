using System;
using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Transaction.Outbound.Draft;
using MCS.UI.Areas.User.Models.Transaction.Outbound.External;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class VIPEditOutboundDraftVM : VIPTransactionVM
    {

        public override TransactionCategory Type
        {
            get { return TransactionCategory.InternalOutbound; }
        }
        public EditorType EditorType { get; set; }
        public VIPEditOutboundExternalBasicInfoVM OutboundDraftBasicInfo { get; set; } = new VIPEditOutboundExternalBasicInfoVM();
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);



    }
}