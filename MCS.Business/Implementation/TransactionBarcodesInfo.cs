using System;
using System.Collections.Generic;
using MCS.Business.Implementation;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionBarcodesInfo
    {
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public string CompanyName { get; set; }
        public string TransactionDateH { get; set; }
        public DateTime TransactionDate { get; set; }
        public long TransactionNumber { get; set; }
        public IList<Barcode> Barcodes { get; set; }
        public string TransactionBarcodeHtmlDesign { get; set; }
        public string TransactionAttachmentHtmlDesign { get; set; }
        public string VisitTicketHtmlDesign { get; set; }
        public Barcode TicketBarcode { get; set; }
        public IList<AttachmentBarcode> AttachmentBarcodes { get; set; }
        public int TransactionDesignWidth { get; set; }
        public int TransactionDesignHeight { get; set; }
        public int TicketDesignWidth { get; set; }
        public int TicketDesignHeight { get; set; }
        public string TransactionType { get; set; }
        public int TransactionCategory { get; set; }
        public string Entity { get; set; }
        public string OutboundDestination { get; set; }
        public string OrgUnitSymbol { get; set; }
        public List<BarcodeInfo> CustomBarcodes { get; set; } = new List<BarcodeInfo>();

    }
}
