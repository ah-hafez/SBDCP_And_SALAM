using System;

namespace MCS.DTO
{
    public  class TransactionVisitTicketDTO
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public string CompanyName { get; set; }
        public string TransactionDateH { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionNumber { get; set; }
        public string Entity { get; set; }
        public string InboundNumber { get; set; }
        public string InboundDestination { get; set; }
        public string ToEntityName { get; set; }
        public string VisitTicketHtmlDesign { get; set; }
        public int TicketDesignWidth { get; set; }
        public int TicketDesignHeight { get; set; }
        public BarcodeDTO barcodeDTO { get; set; }
        public string Subject { get; set; }
    }
}
