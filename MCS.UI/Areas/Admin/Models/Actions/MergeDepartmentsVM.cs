using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Framework.Localization;
using MCS.Common.CustomAttributes;

namespace MCS.UI.Areas.Admin.Models.Actions
{
    public class MergeDepartmentsVM
    {
        public int Id { get; set; }
        [CustomDisplayName("Admin.Actions.MergeEntities.MergedEntityTitle")]
        [CustomRequired("Admin.Actions.MergeEntities.MergedEntityValidation")]
        public int MergedEntityId { get; set; }
        [CustomDisplayName("Admin.Actions.MergeEntities.BaseEntityTitle")]
        [CustomRequired("Admin.Actions.MergeEntities.BaseEntityValidation")]
        public int BaseEntityId { get; set; }
        [CustomRequired("Admin.Actions.MergeEntities.ManagerValidation")]
        public int ManagerId { get; set; }
        public List<LocalizationVM> NewEntityNames { get; set; }
    }
}