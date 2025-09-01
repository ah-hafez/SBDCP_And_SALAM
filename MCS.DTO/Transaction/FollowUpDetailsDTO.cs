using System;

namespace MCS.DTO
{
    public class FollowUpDetailsDTO
    {
        public int Id { get; set; }
        public int TransactionFollowUpId { get; set; }
        public string Notes { get; set; }
        public TransactionFollowUpDTO FollowUp { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
