using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction.Inbound
{
    public class VipOutboundInternalUpdateVM
    {

        public int Id { get; set; }
        public TransactionCategory Type
        {
            get { return TransactionCategory.Inbound; }
        }
        public List<VIPTransactionAssignmentVM> AssignmentVMs { get; set; } = new List<VIPTransactionAssignmentVM>();

        public PublicFollowupVM PublicFollowUps { get; set; }
        public PrivateFollowupVM PrivateFollowUps { get; set; }
        public string Notes { get; set; }
        public int? ExplanationConfedentialityForAssignmentPaperId { get; set; }
        public string ExplanationForAssignmentPaper { get; set; }
        public int InboundId { get; set; }
        public string hdnMainDocTokenId { get; set; }
        public DocumentVM DocumentVM { get; set; }
        public bool IsConfirmed { get; set; }
        public string Summary { get; set; }

    }
}