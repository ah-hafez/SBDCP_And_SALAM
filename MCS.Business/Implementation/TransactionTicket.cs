using System;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionTicket
    {
        public Barcode barcode { get; set; }
        public int SequenceNumber { get; set; }
        public long Number { get; set; }
        public DateTime Date { get; set; }
    }
}
