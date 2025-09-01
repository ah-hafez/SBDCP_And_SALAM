using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class SuggestedTopicVM
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGroup { get; set; }
        public bool IsSelected { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public int? ParentId { get; set; }
        public SuggestedTopicVM Parent { get; set; }

        [CustomDisplayName("Admin.SuggestedTopicDTO.OrgUnits")]
        public List<int> OrgUnits { get; set; }
        public List<SuggestedTopicVM> Childs { get; set; }
        public string LocalName { get; set; }
    }
}