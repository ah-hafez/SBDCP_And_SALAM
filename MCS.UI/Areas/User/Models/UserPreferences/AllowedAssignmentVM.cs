using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MCS.Common;
using MCS.Common.CustomAttributes;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences.UserDelegation;

namespace MCS.UI.Areas.User.Models.UserPreferences
{

    public class AllowedAssignmentVM
    {
        public int? Id { get; set; } 
        public int? UserId { get; set; }
        public int ToUserId { get; set; }
        [CustomDisplayName("User.Transaction.AssignmentDetail.OrgUnitId")]
        [CustomRequired("User.Transaction.AssignmentDetail.OrgUnitIdRequired")]
        public int EntityId { get; set; }
        public UserProfileVM User { get; set; }
        public UserProfileVM ToUser { get; set; }
        public OrgUnitVM Entity { get; set; }
        public List<AllowedAssignmentVM> AllowedAssignmentList { get; set; }
    }
}