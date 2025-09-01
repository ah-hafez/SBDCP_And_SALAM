using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;


namespace MCS.UI.Wrappers
{
    public interface IUserProfileApi
    {
        [Get("/api/UserProfile/GetUsersByOrgUnitId")]
        Task<GetResult<List<UserProfileDTO>>> GetUsersByOrgUnitId(string cultureName, int orgUnitId);

        [Get("/api/UserProfile/GetAssignmentGroupById")]
        Task<GetResult<VIPTransactionAssignmentDTO>> GetAssignmentGroupById(int groupId, string cultureName);

        [Get("/api/UserProfile/GetUserAssignmentGroups")]
        Task<GetResult<List<VIPTransactionAssignmentDTO>>> GetUserAssignmentGroups(int userId, string cultureName);

        [Post("/api/UserProfile/PostAssignmentGroup")]
        Task<PostResult> PostAssignmentGroup(VIPTransactionAssignmentDTO assignmentGroupDTO, string cultureName);

        [Put("/api/UserProfile/ChangePassword")]
        Task<PutResult> ChangePassword(string oldPassword, string newPassword);

        [Get("/api/UserProfile/GetPriorities")]
        Task<GetResult<List<PriorityDTO>>> GetPriorities(string cultureName);

        [Post("/api/UserProfile/PostUserPreference")]
        Task<PostResult> PostUserPreference(UserPreferenceDTO userPreferenceDTO);

        [Get("/api/UserProfile/GetUserPreference")]
        Task<GetResult<UserPreferenceDTO>> GetUserPreference(int userId, string cultureName);

        [Put("/api/UserProfile/PutUserPreference")]
        Task<PutResult> PutUserPreference(UserPreferenceDTO userPreferenceDTO);

        [Put("/api/UserProfile/PutUserDelegation")]
        Task<PutResult> DeleteDelegationsPutUserDelegation(EditUserDelegationDTO editUserDelegationDTO);

        [Delete("/api/UserProfile/DeleteDelegations")]
        Task<DeleteResult> DeleteDelegations(string ids);

        [Get("/api/UserProfile/GetUserDelegationById")]
        Task<GetResult<EditUserDelegationDTO>> GetUserDelegationById(int id, string cultureName);

        [Get("/api/UserProfile/GetUserDelegations")]
        Task<GetResult<List<UserDelegationDTO>>> GetUserDelegations(int preferenceId);

        [Get("/api/UserProfile/GetUserDelegationsByUserId")]
        Task<GetResult<List<UserDelegationDTO>>> GetUserDelegationsByUserId(int userId, int? statusId);
        [Post("/api/UserProfile/PostUserDelegations")]
        Task<PostResult> PostUserDelegations(int userId, List<UserDelegationDTO> userDelegationDTOs);
        [Get("/api/UserProfile/GetOrgUnitManager")]
        Task<GetResult<UserProfileDTO>> GetOrgUnitManager(int orgUnitId, string cultureName);
    }
}
