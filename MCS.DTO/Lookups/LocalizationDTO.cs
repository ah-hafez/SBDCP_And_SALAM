using System.ComponentModel.DataAnnotations;

namespace MCS.DTO
{
    public class LocalizationDTO
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Text { get; set; }

        public int CultureId { get; set; }

        public string CultureName { get; set; }
    }
}
