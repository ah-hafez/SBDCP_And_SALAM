using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;


namespace MCS.UI.Wrappers
{
    public interface ICommonApi
    {
        #region Culture

        [Get("/api/Common/GetCultures")]
        Task<GetResult<List<CultureDTO>>> GetCultures();

        #endregion Culture

        #region OrgUnits

        [Get("/api/Common/GetOrgUnits")]
        Task<GetResult<List<OrgUnitDTO>>> GetOrgUnits(string cultureName, int? orgUnitId = null);

        #endregion OrgUnits

        #region External Parties

        [Get("/api/Common/GetExternalParties")]
        Task<GetResult<List<ExternalPartyDTO>>> GetExternalParties(int? parentId, string cultureName);

        [Get("/api/Common/GetExternalPartyNodes")]
        Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartyNodes(int? nodeId, string cultureName);

        [Get("/api/Common/GetExternalPartiesBySearchCriteria")]
        Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesBySearchCriteria();

        [Get("/api/Common/GetExternalPartiesByParentId")]
        Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesByParentId(int? parentId, string cultureName);

        [Get("/api/Common/GetExternalPartiesByLetterId")]
        Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesByLetterId(int letterId, int? parentId, string cultureName);

        [Get("/api/Common/GetManagersByPartyId")]
        Task<GetResult<List<ManagerDTO>>> GetManagersByPartyId(int? parentId, string cultureName);

        [Post("/api/Common/PostParty")]
        Task<PostResult> PostParty(ExternalPartyAddDTO partyAddDTO);

        [Put("/api/Common/PutParty")]
        Task<PutResult> PutParty(ExternalPartyEditDTO externalPartyEditDTO);

        [Get("/api/Common/GetExternalParty")]
        Task<GetResult<ExternalPartyEditDTO>> GetExternalParty(int id);

        [Post("/api/Common/PostExternalPartyManager")]
        Task<PostResult> PostExternalPartyManager(ManagerAddDTO managerAddDTO);

        [Put("/api/Common/PutExternalPartyManager")]
        Task<PutResult> PutExternalPartyManager(ManagerEditDTO managerEditDTO);

        [Get("/api/Common/GetExternalPartyManagerById")]
        Task<GetResult<ManagerEditDTO>> GetExternalPartyManagerById(int externalPartyManagerId);

        [Get("/api/Common/GetExternalPartyManagers")]
        Task<GetResult<List<ManagerDTO>>> GetExternalPartyManagers();

        #endregion External Parties

        #region Actions

        [Get("/api/Common/GetAllActions")]
        Task<GetResult<List<ActionDTO>>> GetAllActions(string cultureName);

        #endregion Actions

        #region Permission

        [Get("/api/Common/GetPermissionsByGroupId")]
        Task<GetResult<List<PermissionDTO>>> GetPermissionsByGroupId(string cultureName);

        #endregion Permission

        #region collaboration

        [Get("/api/Common/GetIntitialChatHistory")]
        Task<GetResult<List<ConversationDTO>>> GetIntitialChatHistory(int toUserId, int pageSize, string cultureName);

        [Get("/api/Common/GetChatHistory")]
        Task<GetResult<List<ConversationDTO>>> GetChatHistory(int toUserId, int pageSize, int startId, string cultureName);

        [Get("/api/Common/GetChatNotifications")]
        Task<GetResult<ChatNotificationsInfoDTO>> GetChatNotifications();

        [Get("/api/Common/GetCollaborationUsers")]
        Task<GetResult<List<CollaborationUserInfoDTO>>> GetCollaborationUsers(string cultureName);

        #endregion collaboration

    }
}