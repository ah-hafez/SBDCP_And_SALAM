using System.Collections.Generic;

namespace MCS.DTO
{
    public class ExternalPartyDTO
    {
        public int Id { get; set; }
        public int? ParentId { get; set; }
        public string Number { get; set; }
        public List<LocalizationDTO> Name { get; set; }
        public string LocalName { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsSelected { get; set; }
        public bool HasChilds { get; set; }
        public string Lineage { get; set; }
        public bool YasserRegistered { get; set; }
        public string Email { get; set; }
    }
}
