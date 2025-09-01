using System;

namespace MCS.UI.Areas.User.Models.Transaction
{
    public class TransactionTicketVM
    {
        public string BarcodeValue { get; set; }
        public int SequenceNumber { get; set; }
        public long Number { get; set; }
        public DateTime Date { get; set; }
        //public string Description { get; set; }
    }
}