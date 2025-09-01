using System;
using System.Collections.Generic;

namespace MCS.DTO.Transaction
{
    public class TransactionReservationDTO
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int EntityId { get; set; }
        public int Count { get; set; }
        public int TransactionCategoryId { get; set; }
        public string Reason { get; set; }
        public string EntityName { get; set; }
        public string UserName { get; set; }
        public string TransactionCategoryName { get; set; }
        public DateTime DateTime { get; set; }
        public IList<TransactionReservedDTO> Transactions { get; set; }
    }

    public class TransactionReservedDTO
    {
        public int Id { get; set; }
        public long Number { get; set; }
        public int Year { get; set; }
        public string Type { get; set; }
    }
}
