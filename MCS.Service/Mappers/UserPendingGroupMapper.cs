using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserPendingGroupMapper
    {
        public static UserPendingGroup Map(UserPendingGroupDTO userPendingGroupDTO)
        {
            if (userPendingGroupDTO == null)
                return null;

            UserPendingGroup userPendingGroup = new UserPendingGroup()
            {
                Id = userPendingGroupDTO.Id,
                GroupId = userPendingGroupDTO.GroupId,
                UserId = userPendingGroupDTO.UserId,
               
            };

            return userPendingGroup;
        }


        public static List<UserPendingGroupDTO> Map(IList<UserPendingGroup> userPendingGroups)
        {
            if (userPendingGroups == null)
            {
                return null;
            }

                List<UserPendingGroupDTO> userPendingGroupDTOs = userPendingGroups
                .Select(userPendingGroupItem => new UserPendingGroupDTO()
                {
                    Id = userPendingGroupItem.Id,
                    UserId = userPendingGroupItem.Id,
                    GroupId = userPendingGroupItem.Id,
                    UserName = userPendingGroupItem.User.LocalName,
                    GroupName = userPendingGroupItem.Group.Name,
                   

                }).ToList();

            return userPendingGroupDTOs;
         
            }


    }
}