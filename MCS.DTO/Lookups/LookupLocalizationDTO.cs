using System.ComponentModel.DataAnnotations;

namespace MCS.DTO
{
    public class LookupLocalizationDTO
    {
        public int Id { get; set; }
        public int LookupId { get; set; }

        [Required]
        public string Text { get; set; }

        public int CultureId { get; set; }
        public string CultureName { get; set; }
    }
}
