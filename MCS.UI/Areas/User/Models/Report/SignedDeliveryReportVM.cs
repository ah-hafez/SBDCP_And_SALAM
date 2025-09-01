using System;

namespace MCS.UI.Areas.User.Models.Report
{
    public class SignedDeliveryReportVM
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int? DocumentId { get; set; }
        public int Id { get; set; }
        public string Number { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }

    }
}