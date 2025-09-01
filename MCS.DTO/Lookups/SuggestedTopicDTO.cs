using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.DTO
{
    public class SuggestedTopicDTO
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGroup { get; set; }
        public bool IsSelected { get; set; }
        public List<LocalizationDTO> Description { get; set; }
        public int? ParentId { get; set; }
        public SuggestedTopicDTO Parent { get; set; }
        
        public List<int> OrgUnits { get; set; }
        public List<SuggestedTopicDTO> Childs { get; set; }
        public string LocalName { get; set; }
    }
}
