using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction.Outbound.Internal
{
    public class EditOutboundInternalVM : TransactionVM
    {
        public override TransactionCategory Type
        {
            get { return TransactionCategory.InternalOutbound; }
        }
        public EditorType EditorType { get; set; }
        public EditOutboundInternalBasicInfoVM OutboundInternalBasicInfoEdit { get; set; } = new EditOutboundInternalBasicInfoVM();
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);

        public string defaultTabId { get; set; }
        public int? ActionId { get; set; }

        public bool? AssignmentPaperMainSource { get; set; }
        public bool? MainPlaceHolderAssignmentPaperCopy { get; set; }
    }
}