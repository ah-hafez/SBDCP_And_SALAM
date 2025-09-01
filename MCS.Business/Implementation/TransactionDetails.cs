using System;

namespace MCS.Business
{
    public class TransactionDetails
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string DateH { get; set; }
        public long Number { get; set; }
        public string Barcode { get; set; }
        public int Status { get; set; }
    }
}
