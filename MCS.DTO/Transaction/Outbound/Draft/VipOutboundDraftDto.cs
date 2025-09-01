using MCS.Common;
using MCS.DTO.Transaction.Vip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class VipOutboundDraftDto
    {
        public int Id { get; set; }
        public TransactionCategory Type
        {
            get { return TransactionCategory.DraftOutbound; }
        }
        public List<VIPTransactionAssignmentDto> Assignments { get; set; }

        public PublicFollowupDto PublicFollowUps { get; set; }
        public PrivateFollowupDto PrivateFollowUps { get; set; }
        public string Notes { get; set; }
        public int? ExplanationConfedentialityForAssignmentPaperId { get; set; }
        public string ExplanationForAssignmentPaper { get; set; }
        public Dictionary<int, string> ProccessDescriptions { get; set; }
        public string MainDocumentData { get; set; }
        public string OldMainDocumentData { get; set; }
        public bool IsSigned { get; set; }



    }
}
