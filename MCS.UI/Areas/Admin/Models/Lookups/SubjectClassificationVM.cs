using System.Collections.Generic;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class SubjectClassificationVM
    {
        public int Id { get; set; }
        public bool IsNew { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsGroup { get; set; }
        public bool IsSelected { get; set; }
        public List<LocalizationVM> Description { get; set; }
        public string LocalName { get; set; }
        public int? ParentId { get; set; }
        public SubjectClassificationVM Parent { get; set; }

        [CustomDisplayName("Admin.SubjectClassificationDTO.OrgUnits")]
        public List<int> OrgUnits { get; set; }
        public List<SubjectClassificationVM> Childs { get; set; }
    }
}