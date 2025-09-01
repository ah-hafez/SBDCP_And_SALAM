using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class FormAddVM
    {
        public List<LocalizationVM> Description { get; set; }

        [CustomDisplayName("Admin.Form.DepartmentId")]
        public List<int> DepartmentIds { get; set; }
        
        [CustomDisplayName("Admin.Form.FormId")]
        public FormContentVM FormContentVM { get; set; }

        [CustomDisplayName("Admin.Form.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }

        [CustomDisplayName("Admin.Form.OrgUnitId")]
        public IList<int> OrgUnitIds { get; set; }
        public bool AllOrgUnitsSelected { get; set; }
        public string FileContent { get; set; }
        public string FileName { get; set; }
    }
}