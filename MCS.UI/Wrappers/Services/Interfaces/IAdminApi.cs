using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.UI.Wrappers
{
    public interface IAdminApi
    {
        [Get("/api/admin/GetUserById")]
        Task<GetResult<EditUserProfileDTO>> GetUserById(int userId);

        [Get("/api/admin/GetUsersProfiles")]
        Task<GetResult<List<UserProfileDTO>>> GetUsersProfiles(SearchCriteria searchCriteria);

        [Get("/api/admin/GetAllUsers")]
        Task<GetResult<List<UserProfileDTO>>> GetAllUsers(string cultureName);

        [Get("/api/admin/GetUsersByPermissionId")]
        Task<GetResult<List<UserProfileDTO>>> GetUsersByPermissionId(string cultureName, int permissionId);

        [Get("/api/admin/GetUsersByTrayId")]
        Task<GetResult<List<UserProfileDTO>>> GetUsersByTrayId(string cultureName, int trayId);

        [Post("/api/admin/PostUsers")]
        Task<PostResult> PostUsers(string cultureName, string resetPasswordUrl, List<AddUserProfileDTO> AddUserProfileDTOs);

        [Post("/api/admin/PostUser")]
        Task<PostResult> PostUser(string cultureName, string resetPasswordUrl, AddUserProfileDTO AddUserProfileDTO);

        [Put("/api/admin/PutUser")]
        Task<PutResult> PutUser(EditUserProfileDTO EditUserProfileDTO);

        [Put("/api/admin/ActivateUser")]
        Task<PutResult> ActivateUser(int userId, bool isActive);

        [Delete("/api/admin/DeleteUsers")]
        Task<DeleteResult> DeleteUsers(string ids);

        [Get("/api/admin/GetUserPermissionGroups")]
        Task<GetResult<List<PermissionGroupDTO>>> GetUserPermissionGroups(int userId, string cultureName);

        [Get("/api/admin/GetUserPermissionGroups")]
        Task<GetResult<List<PermissionDTO>>> GetAllUserPermissions(int userId, string cultureName);

        [Post("/api/admin/ReSendNotificationEmail")]
        Task<PostResult> ReSendNotificationEmail(int userId, string cultureName, string resetPasswordUrl);

        [Get("/api/admin/GetAllUserTrays")]
        Task<GetResult<List<TrayDTO>>> GetAllUserTrays(int userId);

        [Post("/api/admin/PostPriority")]
        Task<PostResult> PostPriority(PriorityAddDTO priorityAddDTO);

        [Put("/api/admin/PutPriority")]
        Task<PutResult> PutPriority(PriorityEditDTO priorityEditDTO);

        [Delete("/api/admin/DeletePriorities")]
        Task<DeleteResult> DeletePriorities(string ids);

        [Get("/api/admin/GetPriorityById")]
        Task<GetResult<PriorityEditDTO>> GetPriorityById(int priorityId, string cultureName);

        [Get("/api/admin/GetPriorities")]
        Task<GetResult<List<PriorityDTO>>> GetPriorities(SearchCriteria searchCriteria);

        [Post("/api/admin/PostLink")]
        Task<PostResult> PostLink(LinkAddDTO linkAddDTO);

        [Put("/api/admin/PutLink")]
        Task<PutResult> PutLink(LinkEditDTO linkEditDTO);

        [Delete("/api/admin/DeleteLinks")]
        Task<DeleteResult> DeleteLinks(string ids);

        [Get("/api/admin/GetLinkById")]
        Task<GetResult<LinkEditDTO>> GetLinkById(int linkId, string cultureName);

        [Get("/api/admin/GetLinks")]
        Task<GetResult<LinkDTO>> GetLinks(SearchCriteria searchCriteria);

        [Post("/api/admin/PostForm")]
        Task<PostResult> PostForm(FormAddDTO formAddDTO);

        [Put("/api/admin/PutForm")]
        Task<PutResult> PutForm(FormEditDTO formEditDTO);

        [Delete("/api/admin/DeleteForms")]
        Task<DeleteResult> DeleteForms(string ids);

        [Get("/api/admin/GetFormById")]
        Task<GetResult<FormEditDTO>> GetFormById(int formId, string cultureName);

        [Get("/api/admin/GetForms")]
        Task<GetResult<List<FormDTO>>> GetForms(SearchCriteria searchCriteria);

        [Post("/api/admin/PostLetterType")]
        Task<PostResult> PostLetterType(LetterTypeAddDTO letterTypeAddDTO);

        [Put("/api/admin/PutLetterType")]
        Task<PutResult> PutLetterType(LetterTypeEditDTO letterTypeEditDTO);

        [Delete("/api/admin/DeleteLetterTypes")]
        Task<DeleteResult> DeleteLetterTypes(string ids);

        [Get("/api/admin/GetLetterTypeById")]
        Task<GetResult<LetterTypeEditDTO>> GetLetterTypeById(int letterTypeId, string cultureName);

        [Get("/api/admin/GetLetterTypes")]
        Task<GetResult<List<LetterTypeDTO>>> GetLetterTypes(SearchCriteria searchCriteria);

        [Post("/api/admin/PostAttachmentType")]
        Task<PostResult> PostAttachmentType(AttachmentTypeAddDTO attachmentTypeAddDTO);

        [Put("/api/admin/PutUser")]
        Task<PutResult> PutAttachmentType(AttachmentTypeEditDTO attachmentTypeEditDTO);

        [Delete("/api/admin/DeleteAttachmentTypes")]
        Task<DeleteResult> DeleteAttachmentTypes(string ids);

        [Get("/api/admin/GetAttachmentTypeById")]
        Task<GetResult<AttachmentTypeEditDTO>> GetAttachmentTypeById(int attachmentTypeId, string cultureName);

        [Get("/api/admin/GetAttachmentTypes")]
        Task<GetResult<List<AttachmentTypeDTO>>> GetAttachmentTypes(SearchCriteria searchCriteria);

        [Post("/api/admin/PostTransactionType")]
        Task<PostResult> PostTransactionType(TransactionTypeAddDTO transactionTypeAddDTO);

        [Put("/api/admin/PutUser")]
        Task<PutResult> PutTransactionType(TransactionTypeEditDTO transactionTypeEditDTO);

        [Delete("/api/admin/DeleteTransactionTypes")]
        Task<RemoveObjectResult<List<int>>> DeleteTransactionTypes(string ids);

        [Get("/api/admin/GetTransactionTypeById")]
        Task<GetResult<TransactionTypeEditDTO>> GetTransactionTypeById(int transactionTypeId, string cultureName);

        [Get("/api/admin/GetTransactionTypes")]
        Task<GetResult<List<TransactionTypeDTO>>> GetTransactionTypes(SearchCriteria searchCriteria);


        [Post("/api/admin/PostPermission")]
        Task<PostResult> PostPermission(PermissionDTO permissionDTO);

        [Put("/api/admin/PutUser")]
        Task<PutResult> PutPermission(PermissionEditDTO permissionEditDTO);

        [Delete("/api/admin/DeletePermission")]
        Task<DeleteResult> DeletePermission(int permissionId);

        [Get("/api/admin/GetPermissionByID")]
        Task<GetResult<PermissionEditDTO>> GetPermissionByID(int permissionId);

        [Put("/api/admin/UpdatePermissionsName")]
        Task<PutResult> UpdatePermissionsName(List<PermissionGroupDTO> permissionGroupDTOs, string cultureName);

        [Get("/api/admin/GetAllPermissions")]
        Task<GetResult<List<PermissionDTO>>> GetAllPermissions(string cultureName);

        [Get("/api/admin/GetPermissionsGroups")]
        Task<GetResult<List<PermissionGroupDTO>>> GetPermissionsGroups(List<PermissionGroupName> permissionGroupNames, string cultureName);

        [Get("/api/admin/GetPermissionsGroups")]
        Task<GetResult<List<PermissionGroupDTO>>> GetPermissionsGroups(SearchCriteria searchCriteria);

        [Get("/api/admin/GetAllPermissionsGroups")]
        Task<GetResult<List<PermissionGroupDTO>>> GetAllPermissionsGroups(string cultureName, bool includeUserDefinedGroups = true);

        [Get("/api/admin/GetAllUserDefinedGroups")]
        Task<GetResult<List<GroupDTO>>> GetAllUserDefinedGroups(SearchCriteria searchCriteria);

        [Get("/api/admin/GetGroupByID")]
        Task<GetResult<EditGroupDTO>> GetGroupByID(int groupId);

        [Post("/api/admin/PostGroup")]
        Task<PostResult> PostGroup(AddGroupDTO addGroupDTO);

        [Put("/api/admin/PutGroup")]
        Task<PutResult> PutGroup(EditGroupDTO EditGroupDTO);

        [Delete("/api/admin/DeleteGroups")]
        Task<DeleteResult> DeleteGroups(string ids);

        [Post("/api/admin/PostUserCategory")]
        Task<PostResult> PostUserCategory(AddUserCategoryDTO userCategoryAddDTO);

        [Put("/api/admin/PutUser")]
        Task<PutResult> PutUserCategory(EditUserCategoryDTO editUserCategoryDTO);

        [Delete("/api/admin/DeleteUserCategories")]
        Task<RemoveObjectResult<List<int>>> DeleteUserCategories(string ids);

        [Get("/api/admin/GetUserCategoryById")]
        Task<GetResult<EditUserCategoryDTO>> GetUserCategoryById(int userCategoryId);

        [Get("/api/admin/GetUserCategories")]
        Task<GetResult<List<UserCategoryDTO>>> GetUserCategories(SearchCriteria searchCriteria);

        [Get("/api/admin/GetAllUsersCategoriesTrays")]
        Task<GetResult<List<UserCategoryTrayDTO>>> GetAllUsersCategoriesTrays(string cultureName);

        [Get("/api/admin/GetAllUsersCategories")]
        Task<GetResult<List<UserCategoryDTO>>> GetAllUsersCategories(string cultureName);

        [Get("/api/admin/GetUserCategoryTrays")]
        Task<GetResult<List<TrayDTO>>> GetUserCategoryTrays(int userCategoryId);

        [Put("/api/admin/PutUsersCategoriesTrays")]
        Task<PutResult> PutUsersCategoriesTrays(List<UserCategoryTrayDTO> usersCategoriesTraysDTO);

        [Post("/api/admin/PostAction")]
        Task<PostResult> PostAction(AddActionDTO addActionDTO);

        [Put("/api/admin/PutAction")]
        Task<PutResult> PutAction(EditActionDTO actionEditDTO);

        [Delete("/api/admin/DeleteActions")]
        Task<RemoveObjectResult<List<int>>> DeleteActions(string ids);

        [Get("/api/admin/GetActions")]
        Task<GetResult<List<ActionDTO>>> GetActions(SearchCriteria searchCriteria, string cultureName);

        [Get("/api/admin/GetActionById")]
        Task<GetResult<ActionDTO>> GetActionById(int id);

        [Post("/api/admin/PostOrgUnitStructure")]
        Task<PostResult> PostOrgUnitStructure(string cultureName, OrgUnitStructureDesignDTO orgUnitStructureDesignDTO);

        [Get("/api/admin/GetOrgUnitStructure")]
        Task<GetResult<OrgUnitStructureDesignDTO>> GetOrgUnitStructure(string cultureName);

        [Get("/api/admin/GetOrgUnits")]
        Task<GetResult<List<OrgUnitDTO>>> GetOrgUnits(string cultureName, int? orgUnitId = null);

        [Get("/api/admin/GetOrgUnitsUsedInTransaction")]
        Task<GetResult<List<int>>> GetOrgUnitsUsedInTransaction(string orgUnitIds);

        [Delete("/api/admin/DeleteExternalPartyManagers")]
        Task<RemoveObjectResult<List<int>>> DeleteExternalPartyManagers(string ids);

        [Delete("/api/admin/DeletePartites")]
        Task<RemoveObjectResult<List<int>>> DeletePartites(string ids);

        [Put("/api/admin/PutTray")]
        Task<PutResult> PutTray(EditTrayDTO editTrayDTO);

        [Put("/api/admin/traysDTO")]
        Task<PutResult> PutTrays(List<TrayDTO> traysDTO);

        [Get("/api/admin/GetAllTrays")]
        Task<GetResult<List<TrayDTO>>> GetAllTrays(string cultureName);

        [Get("/api/admin/GetTrayById")]
        Task<GetResult<EditTrayDTO>> GetTrayById(int trayId);

        [Get("/api/admin/GetTrays")]
        Task<GetResult<List<EditTrayDTO>>> GetTrays(SearchCriteria searchCriteria);

        [Post("/api/admin/PostDesign")]
        Task<PostResult> PostDesign(BarcodeDesignerDTO designDTO);

        [Get("/api/admin/GetBarcodeDesign")]
        Task<GetResult<BarcodeDesignerDTO>> GetBarcodeDesign(bool isGeneral, int typeId);

        [Get("/api/admin/GetBarcodeDesignByOrgUnitId")]
        Task<GetResult<BarcodeDesignerDTO>> GetBarcodeDesignByOrgUnitId(int orgUnitId, int typeId);

        [Post("/api/admin/suggestedTopicDTOs")]
        Task<PostResult> PostSuggestedTopics(List<SuggestedTopicDTO> suggestedTopicDTOs);

        [Get("/api/admin/GetSuggestedTopics")]
        Task<GetResult<List<SuggestedTopicDTO>>> GetSuggestedTopics();

        [Post("/api/admin/PostSubjectClassifications")]
        Task<PostResult> PostSubjectClassifications(List<SubjectClassificationDTO> subjectClassificationDTO);

        [Get("/api/admin/GetSubjectClassifications")]
        Task<GetResult<List<SubjectClassificationDTO>>> GetSubjectClassifications();

        [Post("/api/admin/ReleaseNotesAdd")]
        Task<PostResult> ReleaseNotesAdd(ReleaseNotesDTO releaseNotesDTO);

        [Put("/api/admin/ReleaseNotesUpdate")]
        Task<PutResult> ReleaseNotesUpdate(ReleaseNotesDTO releaseNotesDTO);

        [Delete("/api/admin/ReleaseNotesDelete")]
        Task<RemoveObjectResult<List<int>>> ReleaseNotesDelete(string ids);


    }
}
