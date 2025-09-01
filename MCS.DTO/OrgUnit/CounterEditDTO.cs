using System.Collections.Generic;

namespace MCS.DTO
{
    public class CounterEditDTO
    {
        public int Id { get; set; }
        public bool IsJoinToGeneralCounter { get; set; }
        public List<CounterDetailDTO> CounterDetails { get; set; }
        public int Year { get; set; }
    }
}
