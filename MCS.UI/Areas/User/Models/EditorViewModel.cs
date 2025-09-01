using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class EditorViewModel
    {
        public EditorType EditorType { get; set; } = EditorType.Scanning;
        public TransactionBasicInfoVM TransactionBasicInfoVM { get; set; }
        public TransactionAssignmentVM TransactionAssignmentVM { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public AssignmentPaperVM AssignmentPaperVM { get; set; }
        public TransactionCopyVM TransactionCopyVM { get; set; } = new TransactionCopyVM();
        public List<TransactionFollowUpVM> FollowUps { get; set; } = (AjaxGrid<TransactionFollowUpVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionFollowUpVM>(), 1, 0, false);
    
        public List<TransactionArchiveVM> Archives { get; set; } = (AjaxGrid<TransactionArchiveVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionArchiveVM>(), 1, 0, false);

    }
}
