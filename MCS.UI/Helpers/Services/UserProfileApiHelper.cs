using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class UserProfileApiHelper
    {


        public IUserProfileApi IUserProfileClient { get; }
        public UserProfileApiHelper()
        {
            IUserProfileClient = ClientFactory.GetClient<IUserProfileApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async  Task<GetResult<List<UserProfileDTO>>> GetUsersByOrgUnitId(string cultureName, int orgUnitId)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUsersByOrgUnitId(cultureName, orgUnitId);
            return result;
        }
        
        public static async Task<GetResult<VIPTransactionAssignmentDTO>> GetAssignmentGroupById(int groupId, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetAssignmentGroupById(groupId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<VIPTransactionAssignmentDTO>>> GetUserAssignmentGroups(int userId, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUserAssignmentGroups(userId, cultureName);
            return result;
        }

        public static async Task<PostResult> PostAssignmentGroup(VIPTransactionAssignmentDTO assignmentGroupDTO, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.PostAssignmentGroup(assignmentGroupDTO, cultureName);
            return result;
        }

        public static async Task<PutResult> ChangePassword(string oldPassword, string newPassword)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.ChangePassword(oldPassword, newPassword);
            return result;
        }

        public static async Task<GetResult<List<PriorityDTO>>> GetPriorities(string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetPriorities(cultureName);
            return result;
        }

        public static async Task<PostResult> PostUserPreference(UserPreferenceDTO userPreferenceDTO)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.PostUserPreference(userPreferenceDTO);
            return result;
        }

        public static async Task<GetResult<UserPreferenceDTO>> GetUserPreference(int userId, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUserPreference(userId, cultureName);
            return result;
        }

        public static async Task<PutResult> PutUserPreference(UserPreferenceDTO userPreferenceDTO)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.PutUserPreference(userPreferenceDTO);
            return result;
        }

        public static async Task<PutResult> DeleteDelegationsPutUserDelegation(EditUserDelegationDTO editUserDelegationDTO)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.DeleteDelegationsPutUserDelegation(editUserDelegationDTO);
            return result;
        }

        public static async Task<DeleteResult> DeleteDelegations(string ids)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.DeleteDelegations(ids);
            return result;
        }

        public static async Task<GetResult<EditUserDelegationDTO>> GetUserDelegationById(int id, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUserDelegationById(id, cultureName);
            return result;
        }

        public static async Task<GetResult<List<UserDelegationDTO>>> GetUserDelegations(int preferenceId)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUserDelegations(preferenceId);
            return result;
        }

        public static async Task<GetResult<List<UserDelegationDTO>>> GetUserDelegationsByUserId(int userId, int? statusId)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetUserDelegationsByUserId(userId, statusId);
            return result;
        }

        public static async Task<PostResult> PostUserDelegations(int userId, List<UserDelegationDTO> userDelegationDTOs)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.PostUserDelegations(userId, userDelegationDTOs);
            return result;
        }

        public static async Task<GetResult<UserProfileDTO>> GetOrgUnitManager(int orgUnitId, string cultureName)
        {
            var client = new UserProfileApiHelper();
            var result = await client.IUserProfileClient.GetOrgUnitManager(orgUnitId, cultureName);
            return result;
        }
    }
}