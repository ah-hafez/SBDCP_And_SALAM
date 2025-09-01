using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class EditInboundVM : TransactionVM
    {
        public EditInboundBasicInfoVM InboundBasicInfoEdit { get; set; }

        public override TransactionCategory Type
        {
            get { return TransactionCategory.Inbound; }
        }
        public int ModifiedByUserId { get; set; }
        public EditorType EditorType { get; set; }
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
        public string defaultTabId { get; set; }

    }
}