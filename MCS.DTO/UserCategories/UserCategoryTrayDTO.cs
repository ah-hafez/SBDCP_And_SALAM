using System.Collections.Generic;

namespace MCS.DTO
{
    public class UserCategoryTrayDTO
    {

        public int Id { get; set; }

        public List<LocalizationDTO> Categories { get; set; }

        public string CategoryText { get; set; }

        public List<TrayDTO> Trays { get; set; }
    }
}
