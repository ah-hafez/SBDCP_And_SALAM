using System.Collections.Generic;

namespace MCS.DTO
{
    public class TrayDetailsDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int AllTransactionCount { get; set; }
        public int TodayTransactionCount { get; set; }        
        public List<TransactionTrayInfoDTO> TransactionTrayInfoDTOs { get; set; }
        public bool IsExcluded { get; set; }
        public bool IsVIPUser { get; set; }
    }
}
