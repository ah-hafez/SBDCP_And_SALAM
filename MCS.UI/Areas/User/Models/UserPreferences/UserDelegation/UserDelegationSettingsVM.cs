using System;
using System.Collections.Generic;
using MCS.Common.CustomAttributes;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.UserPreferences.UserDelegation
{
    public class UserDelegationSettingsVM
    {
        public UserDelegationSettingsVM()
        {
            userDelegations = new List<UserDelegationVM>();
            userApprovedDelegates = new UserDelegationVM();
            userManagerDelegates = new UserDelegationVM();
        }

        public bool IsManager { get; set; }
        public List<UserDelegationVM> userDelegations { get; set; }
        public UserDelegationVM userCurrentDelegates { get; set; }
        public UserDelegationVM userApprovedDelegates { get; set; }
        public UserDelegationVM userManagerDelegates { get; set; }
        public UserDelegationVM MyDelegates { get; set; } = new UserDelegationVM();

    }
}