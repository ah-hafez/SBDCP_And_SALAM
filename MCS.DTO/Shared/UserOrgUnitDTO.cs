using System.Collections.Generic;

namespace MCS.DTO
{
    public class UserOrgUnitDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<LocalizationDTO> LoclizationName { get; set; }
        public bool IsSelected { get; set; }
        public int ManagerId { get; set; }

    }
}