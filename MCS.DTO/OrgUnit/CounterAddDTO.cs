using System.Collections.Generic;

namespace MCS.DTO
{
    public class CounterAddDTO
    {
        public int Id { get; set; }
        public bool IsJoinToGeneralCounter { get; set; }
        public List<CounterDetailDTO> CounterDetails { get; set; }
    }
}
