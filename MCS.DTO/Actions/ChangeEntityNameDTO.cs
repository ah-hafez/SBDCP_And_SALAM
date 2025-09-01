using System.Collections.Generic;

namespace MCS.DTO
{
    public class ChangeEntityNameDTO
    {
        public int EntityFromId { get; set; }
        public int EntityToId { get; set; }
        public List<LocalizationDTO> EntityFromLocalizations { get; set; } 
        public List<LocalizationDTO> EntityToLocalizations { get; set; } 

    }
}
