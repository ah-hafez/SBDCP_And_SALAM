using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class CommonApiHelper
    {
        public ICommonApi CommonApiClient { get; }
        public CommonApiHelper()
        {
            CommonApiClient = ClientFactory.GetClient<ICommonApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async  Task<GetResult<List<CultureDTO>>> GetCultures()
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetCultures();
            return result;
        }

        public static async Task<GetResult<List<OrgUnitDTO>>> GetOrgUnits(string cultureName, int? orgUnitId = null)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetOrgUnits(cultureName, orgUnitId);
            return result;
        }
        
        public static async Task<GetResult<List<ExternalPartyDTO>>> GetExternalParties(int? parentId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalParties(parentId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartyNodes(int? nodeId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartyNodes(nodeId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesBySearchCriteria()
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartiesBySearchCriteria( );
            return result;
        }

        public static async Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesByParentId(int? parentId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartiesByParentId(parentId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<ExternalPartyDTO>>> GetExternalPartiesByLetterId(int letterId, int? parentId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartiesByLetterId(letterId, parentId, cultureName);
            return result;
        }

        public static async Task<GetResult<List<ManagerDTO>>> GetManagersByPartyId(int? parentId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetManagersByPartyId(parentId, cultureName);
            return result;
        }

        public static async Task<PostResult> PostParty(ExternalPartyAddDTO partyAddDTO)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.PostParty(partyAddDTO );
            return result;
        }

        public static async Task<PutResult> PutParty(ExternalPartyEditDTO externalPartyEditDTO)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.PutParty(externalPartyEditDTO);
            return result;
        }

        public static async Task<GetResult<ExternalPartyEditDTO>> GetExternalParty(int id)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalParty(id);
            return result;
        }

        public static async Task<PostResult> PostExternalPartyManager(ManagerAddDTO managerAddDTO)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.PostExternalPartyManager(managerAddDTO);
            return result;
        }

        public static async Task<PutResult> PutExternalPartyManager(ManagerEditDTO managerEditDTO)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.PutExternalPartyManager(managerEditDTO);
            return result;
        }

        public static async Task<GetResult<ManagerEditDTO>> GetExternalPartyManagerById(int externalPartyManagerId)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartyManagerById(externalPartyManagerId);
            return result;
        }

        public static async Task<GetResult<List<ManagerDTO>>> GetExternalPartyManagers()
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetExternalPartyManagers();
            return result;
        }



        public static async Task<GetResult<List<ActionDTO>>> GetAllActions(string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetAllActions(cultureName);
            return result;
        }


        public static async Task<GetResult<List<PermissionDTO>>> GetPermissionsByGroupId(string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetPermissionsByGroupId(cultureName);
            return result;
        }


        public static async Task<GetResult<List<ConversationDTO>>> GetIntitialChatHistory(int toUserId, int pageSize, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetIntitialChatHistory(toUserId, pageSize, cultureName);
            return result;
        }

        public static async Task<GetResult<List<ConversationDTO>>> GetChatHistory(int toUserId, int pageSize, int startId, string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetChatHistory(toUserId, pageSize, startId, cultureName);
            return result;
        }
        public static async Task<GetResult<ChatNotificationsInfoDTO>> GetChatNotifications()
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetChatNotifications();
            return result;
        }

        public static async Task<GetResult<List<CollaborationUserInfoDTO>>> GetCollaborationUsers(string cultureName)
        {
            var client = new CommonApiHelper();
            var result = await client.CommonApiClient.GetCollaborationUsers(cultureName);
            return result;
        }

    }
}