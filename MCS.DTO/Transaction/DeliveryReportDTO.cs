using System.Collections.Generic;

namespace MCS.DTO
{
    public class DeliveryReportDTO
    {
        public string OrgUnitName { get; set; }
        public string ReportNumber { get; set; }
        public string DateH { get; set; }
        public string RootName { get; set; }
        public string UserName { get; set; }

        public List<DeliveryReportTransactionDTO> DeliveryReportTransactions { get; set; }
        public string ConfidentialityName { get; set; }
        public string TransactionTypeName { get; set; }
    }
}   
