using System.Collections.Generic;

namespace MCS.DTO
{
    public class AddActionDTO
    {
        public List<LocalizationDTO> Description { get; set; }

        public int TypeId { get; set; }

        public bool IsAsCopy { get; set; }
        public int AssignmentTypeId { get; set; }

    }
}
