using System.Collections.Generic;

namespace MCS.DTO
{
    public class DeliveryReportInfoDTO
    {
        public string OrgUnitName { get; set; }
        public string ReportNumber { get; set; }
        public string DateH { get; set; }
        public string RootOrgUnitName { get; set; }
        public string UserName { get; set; }
        public IList<DeliveryReportTransactionInfoDTO> DeliveryReportTransactions { get; set; }
        public string ConfidentialityName { get; set; }
        public string TransactionTypeName { get; set; }
    }
}
