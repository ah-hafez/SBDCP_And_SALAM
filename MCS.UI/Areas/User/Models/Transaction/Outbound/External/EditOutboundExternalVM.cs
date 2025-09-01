using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.External
{
    public class EditOutboundExternalVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.ExternalOutbound; }
        }
        public EditOutboundExternalBasicInfoVM OutboundExternalBasicInfo { get; set; } = new EditOutboundExternalBasicInfoVM();
        public int ModifiedByUserId { get; set; }
        public EditorType EditorType { get; set; }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
        public DocumentVM OldDocumentVM { get; set; }


    }
}