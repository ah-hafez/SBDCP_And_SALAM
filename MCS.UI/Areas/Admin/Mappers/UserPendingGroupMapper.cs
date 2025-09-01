using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class UserPendingGroupMapper
    {

        public static List<UserPendingGroupVM> Map(List<UserPendingGroupDTO> UserPendingGroupDTOs)
        {
            if (UserPendingGroupDTOs == null || !UserPendingGroupDTOs.Any())
            { return new List<UserPendingGroupVM>(); }
            List<UserPendingGroupVM> userPendingGroupVMs = UserPendingGroupDTOs
                .Select(b => new UserPendingGroupVM
                {
                    Id = b.Id,
                    UserId = b.UserId,
                    GroupId = b.GroupId,
                    UserName = b.UserName,  
                    GroupName = b.GroupName,    
                    
                }).ToList();
            return userPendingGroupVMs;
        }
      
    
    }
}