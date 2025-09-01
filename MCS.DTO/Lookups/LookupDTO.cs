using System.Collections.Generic;

namespace MCS.DTO
{
    public class LookupDTO
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public bool IsActive { get; set; }
        public int Sort { get; set; }     
        public int? EnumReference { get; set; }
        public string Text { get; set; }
        public List<LookupLocalizationDTO> Localizations { get; set; }
    }
}
