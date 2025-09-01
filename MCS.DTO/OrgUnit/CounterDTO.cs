using System.Collections.Generic;

namespace MCS.DTO
{
    public class CounterDTO
    {
        public int Id { get; set; }
        public int Year { get; set; }
        public int OwnerEntityId { get; set; }
        public bool IsGeneral { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public List<CounterDetailDTO> CounterDetails { get; set; }
    }
}
