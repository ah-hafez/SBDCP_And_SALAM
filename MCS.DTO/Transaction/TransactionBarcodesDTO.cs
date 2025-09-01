using System;
using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionBarcodesDTO
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public string CompanyName { get; set; }
        public string TransactionDateH { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionNumber { get; set; }
        public List<BarcodeDTO> BarcodeDTOs { get; set; }
        public List<BarcodeDTO> CustomBarcodeDTOs { get; set; }
        public string TransactionBarcodeHtmlDesign { get; set; }
        public string VisitTicketHtmlDesign { get; set; }
        public BarcodeDTO TicketBarcodeDTO { get; set; }
        public List<AttachmentBarcodeDTO> AttachmentBarcodes { get; set; }
        public int TransactionDesignWidth { get; set; }
        public int TransactionDesignHeight { get; set; }
        public int TicketDesignWidth { get; set; }
        public int TicketDesignHeight { get; set; }
        public string TransactionType { get; set; }
        public int TransactionCategory { get; set; }
        public string TransactionAttachmentHtml { get; set; }
        public string Entity { get; set; }
        public string OutboundDestination { get; set; }
        public string OrgUnitSymbol { get; set; }
    }
}
