using System.Collections.Generic;

namespace MCS.DTO
{
    public class PermissionDTO
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public bool IsSelected { get; set; }
        public string Text { get; set; }
        public List<LookupLocalizationDTO> Names { get; set; }
        public int groupId { get; set; }
        public bool IsUserDefined { get; set; }

    }
}
