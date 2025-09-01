using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class TemplateEditVM
    {
        public int Id { get; set; }
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Form.DepartmentId")]
        public IList<int> DepartmentIds { get; set; }

        [CustomDisplayName("Admin.Form.FormId")]
        public TemplateContentVM FormContentVM { get; set; }

        [CustomDisplayName("Admin.Form.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.Form.OrgUnitId")]
        public IList<int> OrgUnitIds { get; set; }

        public string SelectedOrgUnitsIds { get; set; }

        public bool AllOrgUnitsSelected { get; set; }
        public Dictionary<int, string> OrgUnitsKeyValue { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public int? LockedBy { get; set; }
        public int Status { get; set; }
        public string FileContent { get; set; }

    }
}