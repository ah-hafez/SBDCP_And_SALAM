using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class SubjectClassificationDTO
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGroup { get; set; }
        public bool IsSelected { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public string LocalName { get; set; }
        public int? ParentId { get; set; }
        public SubjectClassificationDTO Parent { get; set; }
        
        public List<int> OrgUnits { get; set; }
        public List<SubjectClassificationDTO> Childs { get; set; }
    }
}
