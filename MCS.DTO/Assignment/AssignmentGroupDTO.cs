using System.Collections.Generic;

namespace MCS.DTO
{
    public class AssignmentGroupDTO
    {
        public int Id { get; set; }
        public string LocalName { get; set; }
        public List<LocalizationDTO> GroupName { get; set; }
        public List<AssignmentGroupDetailDTO> GroupDetails { get; set; }
    }
}
