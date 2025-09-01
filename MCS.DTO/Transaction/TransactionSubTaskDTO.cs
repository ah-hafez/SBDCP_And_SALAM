using System.Collections.Generic;

namespace MCS.DTO
{
    public class TransactionSubTaskDTO
    {
        public List<SubTaskAddDTO> SubTasks { get; set; }
        public int TransactionId { get; set; }
        public int ParentId { get; set; }
    }
}
