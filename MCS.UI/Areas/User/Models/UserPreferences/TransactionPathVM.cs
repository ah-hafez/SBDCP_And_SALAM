using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.UserPreferences
{
    public class TransactionPathVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.Path.Name")]
        [CustomRequired("User.Transaction.Path.NameRequired", AllowEmptyStrings = false)]
        public string Name { get; set; }

        [CustomDisplayName("User.Transaction.Path.Type")]
        [CustomRequired("User.Transaction.Path.TypeRequired")]
        public int TransactionTypeId { get; set; }
        public int? CreatedBy { get; set; }
        public string CreatedByName { get; set; }

        [CustomDisplayName("User.Transaction.Path.OrgUnit")]
        [CustomRequired("User.Transaction.PathDetails.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
        [CustomDisplayName("User.Transaction.Path.User")]
        [CustomRequired("User.Transaction.Path.UserRequired")]
        public int? UserId { get; set; }

        public bool IsReadOnly { get; set; }

        public string TransactionTypeName { get; set; }
        public string OrgUnitName { get; set; }
        public IList<TransactionPathDetailsVM> TransactionPathDetails { get; set; }
        public TransactionPathDetailsVM TransactionPathDetailsVM { get; set; }

        public AjaxGrid<TransactionPathVM> TransactionPathsGrid { get; set; } = (AjaxGrid<TransactionPathVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionPathVM>(), 1, 0, false);
        public AjaxGrid<TransactionPathDetailsVM> TransactionPathDetailsGrid { get; set; } = (AjaxGrid<TransactionPathDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionPathDetailsVM>(), 1, 0, false);

    }

    public class TransactionPathDetailsVM : EntityBase
    {
        public int Id { get; set; }

        [CustomDisplayName("User.Transaction.PathDetails.User")]
        public int? UserId { get; set; }

        [CustomDisplayName("User.Transaction.PathDetails.OrgUnit")]
        [CustomRequired("User.Transaction.PathDetails.OrgUnitRequired")]
        public int EntityId { get; set; }

        [CustomDisplayName("User.Transaction.PathDetails.ActionId")]
        [CustomRequired("User.Transaction.PathDetails.ActionRequired")]
        public int ActionId { get; set; }

        public int Sort { get; set; }

        public bool IsReadOnly { get; set; }

        public string UserName { get; set; }
        public string EntityName { get; set; }
        public string ActionName { get; set; }

        public AjaxGrid<TransactionPathDetailsVM> TransactionPathDetails { get; set; } = (AjaxGrid<TransactionPathDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TransactionPathDetailsVM>(), 1, 0, false);

    }
}