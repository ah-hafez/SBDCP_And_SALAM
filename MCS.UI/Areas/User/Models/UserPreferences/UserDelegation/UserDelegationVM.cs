using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Permission;

namespace MCS.UI.Areas.User.Models.UserPreferences.UserDelegation
{
    public class UserDelegationVM : EntityBase
    {
        public int Id { get; set; }
        public string FromDateH { get; set; }
        public string FromDateG { get; set; }
        public string ToDateH { get; set; }
        public string ToDateG { get; set; }
        public string OrgUnit { get; set; }
        public string DirectedTo { get; set; }
        public string Priority { get; set; }
        public string Confidentiality { get; set; }
        public string SourceType { get; set; }

      
        [CustomRequired("User.UserDelegation.FromDateRequired")]
        [CustomDateTimeCompareAttribute("CurrentDate", Operation.GreaterThanOrEqual, "User.UserDelegation.DateNowCompare")]
        public DateTime FromDate { get; set; }

        public DateTime? CurrentDate { get; set; } = DateTime.Now;

        [CustomDateTimeCompareAttribute("FromDate", Operation.GreaterThan, "User.UserDelegation.DateCompare")]
        [CustomRequired("User.UserDelegation.ToDateRequired")]
        public DateTime ToDate { get; set; }

        [CustomDisplayName("User.UserDelegation.Unit")]
        [CustomRequired("User.UserDelegation.OrgUnitRequired")]
        public int OrgUnitId { get; set; }
        [CustomRequired("User.UserDelegation.DirectToRequired")]
        public int DirectedToId { get; set; }
        public int? PriorityId { get; set; }
        public int? ConfidentialityId { get; set; }
        public int? SourceTypesId { get; set; }
        public int UserPreferenceId { get; set; }
        public string RejectionReason { get; set; }
        public int StatusId { get; set; }
        public string Status { get; set; }
        public bool ReceiveCopy { get; set; }

        public bool ShowTransaction { get; set; }
        

        public AjaxGrid<UserDelegationVM> DelegationListGrid { get; set; } = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserDelegationVM>(), 1, 0, false);
        public AjaxGrid<UserDelegationVM> ApprovedDelegationListGrid { get; set; } = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserDelegationVM>(), 1, 0, false);
        public AjaxGrid<UserDelegationVM> ManagerDelegationListGrid { get; set; } = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserDelegationVM>(), 1, 0, false);
        public AjaxGrid<UserDelegationVM> MyDelegationListGrid { get; set; } = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(new List<UserDelegationVM>(), 1, 0, false);


        [CustomDisplayName("User.Form.TransactionCategories")]
        public List<TransactionCategoryVM> TransactionCategories { get; set; }
        public string SelectedTransactionCategoriesText { get; set; }
        public string SelectedTransactionCategoriesIdList { get; set; }
        public string DirectedFrom { get; set; }
        public List<PermissionVM> ConfidentialityLevels { get; set; }
        public string SelectedConfidentialityLevelsText { get; set; }
        public string SelectedConfidentialityLevelsIdList { get; set; }
        public string TransacionCategoryIds { get; set; }
        public string TransacionConfidentialityIds { get; set; }
    }

}