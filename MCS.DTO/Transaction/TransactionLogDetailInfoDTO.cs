using System;

namespace MCS.DTO
{
    public class TransactionLogDetailInfoDTO
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
    }
}
