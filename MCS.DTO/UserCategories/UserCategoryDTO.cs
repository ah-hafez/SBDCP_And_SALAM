using System.Collections.Generic;

namespace MCS.DTO
{
    public class UserCategoryDTO
    {
        public int Id { get; set; }

        public string CategoryText { get; set; }

        public List<LocalizationDTO> Categories { get; set; }

        public string PermissionText { get; set; }

        public bool IsSelected { get; set; }
    }
}
