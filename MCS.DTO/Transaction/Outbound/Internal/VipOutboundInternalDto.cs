using MCS.Common;
using MCS.DTO.Transaction.Vip;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.DTO
{
    public class VipOutboundInternalDto
    {
        public int Id { get; set; }
        public TransactionCategory Type
        {
            get { return TransactionCategory.InternalOutbound; }
        }
        public List<VIPTransactionAssignmentDto> Assignments { get; set; }

        public PublicFollowupDto PublicFollowUps { get; set; }
        public PrivateFollowupDto PrivateFollowUps { get; set; }
        public string Notes { get; set; }
        public int? ExplanationConfedentialityForAssignmentPaperId { get; set; }
        public string ExplanationForAssignmentPaper { get; set; }
        public Dictionary<int, string> ProccessDescriptions { get; set; }
        public DocumentDTO DocumentDTO { get; set; }
        public string Summary { get; set; }

    }
}
