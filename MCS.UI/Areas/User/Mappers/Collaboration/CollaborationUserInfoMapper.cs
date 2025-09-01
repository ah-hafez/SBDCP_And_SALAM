using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Collaboration;

namespace MCS.UI.Areas.User.Mappers.Collaboration
{
    public static class CollaborationUserInfoMapper
    {
        public static List<CollaborationUserInfoVM> Map(IList<CollaborationUserInfoDTO> collaborationUserInfoDTOs)
        {
            if (collaborationUserInfoDTOs == null || !collaborationUserInfoDTOs.Any())
            {
                return new List<CollaborationUserInfoVM>();
            }
            List<CollaborationUserInfoVM> collaborationUserInfoVMs = collaborationUserInfoDTOs
                .Select(collaborationUserInfoDTO => new CollaborationUserInfoVM()
                {  
                    UserId = collaborationUserInfoDTO.UserId, 
                    UserName = collaborationUserInfoDTO.UserName,
                    OrgUnitId = collaborationUserInfoDTO.OrgUnitId,
                    NotificationCount = collaborationUserInfoDTO.NotificationCount,
                    Status = collaborationUserInfoDTO.Status
                }).ToList();

            return collaborationUserInfoVMs;
        }
        public static List<CollaborationUserInfoDTO> Map(IList<CollaborationUserInfoVM> collaborationUserInfoVMs)
        {
            if (collaborationUserInfoVMs == null || !collaborationUserInfoVMs.Any())
            {
                return new List<CollaborationUserInfoDTO>();
            }
            List<CollaborationUserInfoDTO> collaborationUserInfoDTOs = collaborationUserInfoVMs
                .Select(collaborationUserInfoVM => new CollaborationUserInfoDTO()
                { 
                    UserId = collaborationUserInfoVM.UserId,
                    UserName = collaborationUserInfoVM.UserName,
                    OrgUnitId = collaborationUserInfoVM.OrgUnitId,
                    NotificationCount = collaborationUserInfoVM.NotificationCount,
                    Status = collaborationUserInfoVM.Status
                }).ToList();

            return collaborationUserInfoDTOs;
        }
    }
}