using System.Collections.Generic;

namespace MCS.DTO
{
    public class PermissionGroupDTO
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public List<PermissionDTO> Permissions { get; set; }
        public bool IsUserDefined { get; set; }
    }
}
