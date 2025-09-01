using System;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionVisitTicketVM
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
        public BarcodeVM barcodeVM { get; set; }
        public string Subject { get; set; }
    }
}