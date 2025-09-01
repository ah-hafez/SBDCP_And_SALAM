using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using System.Collections.Generic;

namespace MCS.UI.Areas.User.Models
{
    public class CopyViewModel
    {
        public EditorType EditorType { get; set; } = EditorType.Scanning;
        public TransactionBasicInfoVM TransactionBasicInfoVM { get; set; }
        public VipCopyTransactionAssignmentVM TransactionAssignmentVM { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public AssignmentPaperVM AssignmentPaper { get; set; }
        public TransactionCopyVM TransactionCopyVM { get; set; } = new TransactionCopyVM();
        public VIPTransactionFollowUpVM TransactionFollowUp { get; set; } = new VIPTransactionFollowUpVM();

    }
}