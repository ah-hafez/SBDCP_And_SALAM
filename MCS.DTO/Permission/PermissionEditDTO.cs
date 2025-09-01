using System.Collections.Generic;

namespace MCS.DTO
{
    public class PermissionEditDTO
    {
        public int Id { get; set; }
        public List<LookupLocalizationDTO> Names { get; set; }
    }
}
