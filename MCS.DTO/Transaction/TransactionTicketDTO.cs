using System;

namespace MCS.DTO
{
    public  class TransactionTicketDTO
    {
        public string BarcodeValue { get; set; }
        public int SequenceNumber { get; set; }
        public long Number { get; set; }
        public DateTime Date { get; set; }
        //public string Description { get; set; }
    }
}
