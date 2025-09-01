using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Draft
{
    public class EditOutboundDraftVM : TransactionVM
    {

        public EditOutboundDraftBasicInfoVM OutboundDraftBasicInfo { get; set; } = new EditOutboundDraftBasicInfoVM();

        public override TransactionCategory Type
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        public int ModifiedByUserId { get; set; }

        public EditorType EditorType { get; set; }

        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);

    }
}