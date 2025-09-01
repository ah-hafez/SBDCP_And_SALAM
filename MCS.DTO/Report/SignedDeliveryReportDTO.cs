using System;

namespace MCS.DTO
{
    public class SignedDeliveryReportDTO
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public int? DocumentId { get; set; }
        public DocumentDTO Document { get; set; }
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public string NumberDelivery { get; set; }
    }
}
