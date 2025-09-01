using System.Collections.Generic;

namespace MCS.DTO
{
    public class CityDTO
    {
        public int Id { get; set; }
        public int CityId { get; set; }
        public List<LocalizationDTO> Description { get; set; }
    }
}
