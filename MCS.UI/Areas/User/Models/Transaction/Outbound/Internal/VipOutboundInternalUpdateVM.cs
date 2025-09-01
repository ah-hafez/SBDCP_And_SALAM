using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class VipOutboundDraftUpdateVM
    {

        public int Id { get; set; }
        public TransactionCategory Type
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        public List<VIPTransactionAssignmentVM> AssignmentVMs { get; set; } = new List<VIPTransactionAssignmentVM>();

        public PublicFollowupVM PublicFollowUps { get; set; }
        public PrivateFollowupVM PrivateFollowUps { get; set; }
        public string Notes { get; set; }
        public int? ExplanationConfedentialityForAssignmentPaperId { get; set; }
        public string ExplanationForAssignmentPaper { get; set; }
        public int OutboundDraftId { get; set; }
        public string DocumentBase64String { get; set; }
        public bool IsSigned { get; set; }
        public bool IsDecisionDraft { get; set; }
        public string OldDocumentBase64String { get; set; }
        public string hdnMainDocToken { get; set; }

        public bool IsConfirmed { get; set; }



    }
}