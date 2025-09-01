using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers
{
    public class AdminApiHelper
    {
        public IAdminApi AdminApiClient { get; }
        public AdminApiHelper()
        {
            AdminApiClient = ClientFactory.GetClient<IAdminApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<GetResult<EditUserProfileDTO>> GetUserById(int userId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUserById(userId);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetUsersProfiles(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUsersProfiles(searchCriteria);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetAllUsers(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUsers(cultureName);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetUsersByPermissionId(string cultureName, int permissionId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUsersByPermissionId(cultureName, permissionId);
            return result;
        }
        public static async Task<GetResult<List<UserProfileDTO>>> GetUsersByTrayId(string cultureName, int trayId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUsersByTrayId(cultureName, trayId);
            return result;
        }
        public static async Task<PostResult> PostUsers(string cultureName, string resetPasswordUrl, List<AddUserProfileDTO> AddUserProfileDTOs)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostUsers(cultureName, resetPasswordUrl, AddUserProfileDTOs);
            return result;
        }
        public static async Task<PostResult> PostUser(string cultureName, string resetPasswordUrl, AddUserProfileDTO AddUserProfileDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostUser(cultureName, resetPasswordUrl, AddUserProfileDTO);
            return result;
        }
        public static async Task<PutResult> PutUser(EditUserProfileDTO EditUserProfileDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutUser(EditUserProfileDTO);
            return result;
        }
        public static async Task<PutResult> ActivateUser(int userId, bool isActive)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.ActivateUser(userId, isActive);
            return result;
        }
        public static async Task<DeleteResult> DeleteUsers(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteUsers(ids);
            return result;
        }
        public static async Task<GetResult<List<PermissionGroupDTO>>> GetUserPermissionGroups(int userId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUserPermissionGroups(userId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<PermissionDTO>>> GetAllUserPermissions(int userId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUserPermissions(userId, cultureName);
            return result;
        }
        public static async Task<PostResult> ReSendNotificationEmail(int userId, string cultureName, string resetPasswordUrl)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.ReSendNotificationEmail(userId, cultureName, resetPasswordUrl);
            return result;
        }
        public static async Task<GetResult<List<TrayDTO>>> GetAllUserTrays(int userId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUserTrays(userId);
            return result;
        }
        public static async Task<PostResult> PostPriority(PriorityAddDTO priorityAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostPriority(priorityAddDTO);
            return result;
        }
        public static async Task<PutResult> PutPriority(PriorityEditDTO priorityEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutPriority(priorityEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeletePriorities(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeletePriorities(ids);
            return result;
        }
        public static async Task<GetResult<PriorityEditDTO>> GetPriorityById(int priorityId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetPriorityById(priorityId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<PriorityDTO>>> GetPriorities(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetPriorities(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostLink(LinkAddDTO linkAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostLink(linkAddDTO);
            return result;
        }
        public static async Task<PutResult> PutLink(LinkEditDTO linkEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutLink(linkEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeleteLinks(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteLinks(ids);
            return result;
        }
        public static async Task<GetResult<LinkEditDTO>> GetLinkById(int linkId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetLinkById(linkId, cultureName);
            return result;
        }
        public static async Task<GetResult<LinkDTO>> GetLinks(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetLinks(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostForm(FormAddDTO formAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostForm(formAddDTO);
            return result;
        }
        public static async Task<PutResult> PutForm(FormEditDTO formEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutForm(formEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeleteForms(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteForms(ids);
            return result;
        }
        public static async Task<GetResult<FormEditDTO>> GetFormById(int formId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetFormById(formId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<FormDTO>>> GetForms(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetForms(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostLetterType(LetterTypeAddDTO letterTypeAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostLetterType(letterTypeAddDTO);
            return result;
        }
        public static async Task<PutResult> PutLetterType(LetterTypeEditDTO letterTypeEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutLetterType(letterTypeEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeleteLetterTypes(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteLetterTypes(ids);
            return result;
        }
        public static async Task<GetResult<LetterTypeEditDTO>> GetLetterTypeById(int letterTypeId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetLetterTypeById(letterTypeId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<LetterTypeDTO>>> GetLetterTypes(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetLetterTypes(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostAttachmentType(AttachmentTypeAddDTO attachmentTypeAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostAttachmentType(attachmentTypeAddDTO);
            return result;
        }
        public static async Task<PutResult> PutAttachmentType(AttachmentTypeEditDTO attachmentTypeEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutAttachmentType(attachmentTypeEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeleteAttachmentTypes(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteAttachmentTypes(ids);
            return result;
        }
        public static async Task<GetResult<AttachmentTypeEditDTO>> GetAttachmentTypeById(int attachmentTypeId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAttachmentTypeById(attachmentTypeId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<AttachmentTypeDTO>>> GetAttachmentTypes(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAttachmentTypes(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostTransactionType(TransactionTypeAddDTO transactionTypeAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostTransactionType(transactionTypeAddDTO);
            return result;
        }
        public static async Task<PutResult> PutTransactionType(TransactionTypeEditDTO transactionTypeEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutTransactionType(transactionTypeEditDTO);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> DeleteTransactionTypes(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteTransactionTypes(ids);
            return result;
        }
        public static async Task<GetResult<TransactionTypeEditDTO>> GetTransactionTypeById(int transactionTypeId, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetTransactionTypeById(transactionTypeId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<TransactionTypeDTO>>> GetTransactionTypes(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetTransactionTypes(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostPermission(PermissionDTO permissionDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostPermission(permissionDTO);
            return result;
        }
        public static async Task<PutResult> PutPermission(PermissionEditDTO permissionEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutPermission(permissionEditDTO);
            return result;
        }
        public static async Task<DeleteResult> DeletePermission(int permissionId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeletePermission(permissionId);
            return result;
        }
        public static async Task<GetResult<PermissionEditDTO>> GetPermissionByID(int permissionId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetPermissionByID(permissionId);
            return result;
        }
        public static async Task<PutResult> UpdatePermissionsName(List<PermissionGroupDTO> permissionGroupDTOs, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.UpdatePermissionsName(permissionGroupDTOs, cultureName);
            return result;
        }
        public static async Task<GetResult<List<PermissionDTO>>> GetAllPermissions(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllPermissions(cultureName);
            return result;
        }
        public static async Task<GetResult<List<PermissionGroupDTO>>> GetPermissionsGroups(List<PermissionGroupName> permissionGroupNames, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetPermissionsGroups(permissionGroupNames,cultureName);
            return result;
        }
        public static async Task<GetResult<List<PermissionGroupDTO>>> GetPermissionsGroups(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetPermissionsGroups(searchCriteria);
            return result;
        }
        public static async Task<GetResult<List<PermissionGroupDTO>>> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups = true)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllPermissionsGroups(cultureName, includeUserDefinedGroups);
            return result;
        }
        public static async Task<GetResult<List<GroupDTO>>> GetAllUserDefinedGroups(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUserDefinedGroups(searchCriteria);
            return result;
        }
        public static async Task<GetResult<EditGroupDTO>> GetGroupByID(int groupId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetGroupByID(groupId);
            return result;
        }
        public static async Task<PostResult> PostGroup(AddGroupDTO addGroupDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostGroup(addGroupDTO);
            return result;
        }
        public static async Task<PutResult> PutGroup(EditGroupDTO EditGroupDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutGroup(EditGroupDTO);
            return result;
        }
        public static async Task<DeleteResult> DeleteGroups(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteGroups(ids);
            return result;
        }
        public static async Task<PostResult> PostUserCategory(AddUserCategoryDTO userCategoryAddDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostUserCategory(userCategoryAddDTO);
            return result;
        }
        public static async Task<PutResult> PutUserCategory(EditUserCategoryDTO editUserCategoryDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutUserCategory(editUserCategoryDTO);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> DeleteUserCategories(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteUserCategories(ids);
            return result;
        }
        public static async Task<GetResult<EditUserCategoryDTO>> GetUserCategoryById(int userCategoryId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUserCategoryById(userCategoryId);
            return result;
        }
        public static async Task<GetResult<List<UserCategoryDTO>>> GetUserCategories(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUserCategories(searchCriteria);
            return result;
        }
        public static async Task<GetResult<List<UserCategoryTrayDTO>>> GetAllUsersCategoriesTrays(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUsersCategoriesTrays(cultureName);
            return result;
        }
        public static async Task<GetResult<List<UserCategoryDTO>>> GetAllUsersCategories(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllUsersCategories(cultureName);
            return result;
        }
        public static async Task<GetResult<List<TrayDTO>>> GetUserCategoryTrays(int userCategoryId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetUserCategoryTrays(userCategoryId);
            return result;
        }
        public static async Task<PutResult> PutUsersCategoriesTrays(List<UserCategoryTrayDTO> usersCategoriesTraysDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutUsersCategoriesTrays(usersCategoriesTraysDTO);
            return result;
        }
        public static async Task<PostResult> PostAction(AddActionDTO addActionDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostAction(addActionDTO);
            return result;
        }
        public static async Task<PutResult> PutAction(EditActionDTO actionEditDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutAction(actionEditDTO);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> DeleteActions(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteActions(ids);
            return result;
        }
        public static async Task<GetResult<List<ActionDTO>>> GetActions(SearchCriteria searchCriteria, string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetActions(searchCriteria, cultureName);
            return result;
        }
        public static async Task<GetResult<ActionDTO>> GetActionById(int id)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetActionById(id);
            return result;
        }
        public static async Task<PostResult> PostOrgUnitStructure(string cultureName, OrgUnitStructureDesignDTO orgUnitStructureDesignDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostOrgUnitStructure(cultureName,orgUnitStructureDesignDTO);
            return result;
        }
        public static async Task<GetResult<OrgUnitStructureDesignDTO>> GetOrgUnitStructure(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetOrgUnitStructure(cultureName);
            return result;
        }
        public static async Task<GetResult<List<OrgUnitDTO>>> GetOrgUnits(string cultureName, int? orgUnitId = null)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetOrgUnits(cultureName, orgUnitId);
            return result;
        }
        public static async Task<GetResult<List<int>>> GetOrgUnitsUsedInTransaction(string orgUnitIds)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetOrgUnitsUsedInTransaction(orgUnitIds);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> DeleteExternalPartyManagers(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeleteExternalPartyManagers(ids);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> DeletePartites(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.DeletePartites(ids);
            return result;
        }
        public static async Task<PutResult> PutTray(EditTrayDTO editTrayDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutTray(editTrayDTO);
            return result;
        }
        public static async Task<PutResult> PutTrays(List<TrayDTO> traysDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PutTrays(traysDTO);
            return result;
        }
        public static async Task<GetResult<List<TrayDTO>>> GetAllTrays(string cultureName)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetAllTrays(cultureName);
            return result;
        }
        public static async Task<GetResult<EditTrayDTO>> GetTrayById(int trayId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetTrayById(trayId);
            return result;
        }
        public static async Task<GetResult<List<EditTrayDTO>>> GetTrays(SearchCriteria searchCriteria)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetTrays(searchCriteria);
            return result;
        }
        public static async Task<PostResult> PostDesign(BarcodeDesignerDTO designDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostDesign(designDTO);
            return result;
        }
        public static async Task<GetResult<BarcodeDesignerDTO>> GetBarcodeDesign(bool isGeneral, int typeId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetBarcodeDesign(isGeneral,typeId);
            return result;
        }
        public static async Task<GetResult<BarcodeDesignerDTO>> GetBarcodeDesignByOrgUnitId(int orgUnitId, int typeId)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetBarcodeDesignByOrgUnitId(orgUnitId, typeId);
            return result;
        }
        public static async Task<PostResult> PostSuggestedTopics(List<SuggestedTopicDTO> suggestedTopicDTOs)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostSuggestedTopics(suggestedTopicDTOs);
            return result;
        }
        public static async Task<PostResult> PostSubjectClassifications(List<SubjectClassificationDTO> subjectClassificationDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.PostSubjectClassifications(subjectClassificationDTO);
            return result;
        }
        public static async Task<GetResult<List<SubjectClassificationDTO>>> GetSubjectClassifications()
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.GetSubjectClassifications();
            return result;
        }

        public static async Task<PostResult> ReleaseNotesAdd(ReleaseNotesDTO releaseNotesDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.ReleaseNotesAdd(releaseNotesDTO);
            return result;
        }
        public static async Task<PutResult> ReleaseNotesUpdate(ReleaseNotesDTO releaseNotesDTO)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.ReleaseNotesUpdate(releaseNotesDTO);
            return result;
        }
        public static async Task<RemoveObjectResult<List<int>>> ReleaseNotesDelete(string ids)
        {
            var client = new AdminApiHelper();
            var result = await client.AdminApiClient.ReleaseNotesDelete(ids);
            return result;
        }
    }
}