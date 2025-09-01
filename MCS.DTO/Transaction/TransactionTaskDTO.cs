using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionTaskDTO
    {
        public List<TaskAddDTO> TaskDTOs { get; set; }
        public int TransactionId { get; set; }
    }
}
