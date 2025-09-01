using Newtonsoft.Json;
using NPOI.HSSF.UserModel;
using NPOI.POIFS.FileSystem;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.Groups;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.Admin.Models.User;
using MCS.UI.Controls;
using CustomGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserLookup = MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.Admin.Models.OrgUnit;
using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class UserController : AdminControllerBase
    {
        [HttpGet]
        public ActionResult Index()
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookup.CultureMapper.Map(cultureDTOs.Result);

                List<LocalizationDTO> localizationDTOList = new List<LocalizationDTO>();

                //UserViewModel userViewModel = new UserViewModel();
                GetResult<List<UserProfileDTO>> userProfileDTOs;

                //if (id.HasValue)
                //{
                //    userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}&Filters[0].ColumnName=Id&Filters[0].Type=Equals&Filters[0].Value={2}", GridHelper.PageSize, SessionInfo.CultureShortName, id.Value)).Result;
                //}
                //else
                //{
                userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                //}

                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileVMs == null)
                {
                    userProfileVMs = new List<UserProfileVM>();
                    userProfileDTOs.RowsCount = 0;
                }

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), 1, userProfileDTOs.RowsCount.Value, false, UIHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(grid);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult PendingRegestration()
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookup.CultureMapper.Map(cultureDTOs.Result);

                List<LocalizationDTO> localizationDTOList = new List<LocalizationDTO>();

                //UserViewModel userViewModel = new UserViewModel();
                GetResult<List<UserProfileDTO>> userProfileDTOs;

                //if (id.HasValue)
                //{
                //    userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}&Filters[0].ColumnName=Id&Filters[0].Type=Equals&Filters[0].Value={2}", GridHelper.PageSize, SessionInfo.CultureShortName, id.Value)).Result;
                //}
                //else
                //{
                userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetPendingRegestrationUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                //}

                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                if (userProfileVMs == null)
                {
                    userProfileVMs = new List<UserProfileVM>();
                    userProfileDTOs.RowsCount = 0;
                }

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), 1, userProfileDTOs.RowsCount.Value, false, UIHelper.PageSize);

                ViewData["GridData"] = grid;

                return View(grid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult PendingRoleRequest()
        {
            try
            {
                string message = string.Empty;
                GetResult<List<UserPendingGroupDTO>> userPendingGroupDTOs;
                userPendingGroupDTOs = HttpClientWrapper<GetResult<List<UserPendingGroupDTO>>>.GetItemRequest(string.Format("api/Common/GetuserPendingGroup?CultureName={0}", SessionInfo.CultureShortName)).Result;
                List<UserPendingGroupVM> userPendingGroupVMs = UserPendingGroupMapper.Map(userPendingGroupDTOs.Result);
                return View(userPendingGroupVMs);
            }
            catch (Exception)
            {
                throw;
            }
        }



        [HttpGet]
        public ActionResult ApproveRoleRequest(int Id)
        {
            try
            {
                GetResult<UserGroupDTO> userGroupDTO = HttpClientWrapper<GetResult<UserGroupDTO>>.GetItemRequest(string.Format("api/Common/ApproveRoleRequest?Id={0}&CultureName={1}", Id, SessionInfo.CultureShortName)).Result;
                return Json(new { result = userGroupDTO.Result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult RejectRoleRequest(int Id)
        {
            try
            {
                GetResult<bool> userGroupDTO = HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Common/RejectRoleRequest?Id={0}", Id)).Result;
                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult AddUserProfile()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookup.CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}&includeUserDefinedGroups={1}", SessionInfo.CultureShortName, false)).Result;

                TreeViewModel tree = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                ViewData["PermissionsGroups"] = tree;

                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

                //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

                GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
                   .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                List<GroupVM> GroupVMs = GroupMapper.Map(groups.Result).ToList();

                ViewData["Roles"] = GetGroupsAutoCompleteDataSource(GroupVMs);

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.DefaultRole)).Result;
                AddUserProfileVM addUserProfileVM = new AddUserProfileVM();
                addUserProfileVM.RoleId = Convert.ToInt32(SettingValue.Result.Value);
                addUserProfileVM.TransactionProcessingPeriod = 3;
                return View("_UserManagementAddPartial", addUserProfileVM);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[HttpGet]
        //[AllowAnonymous]
        //public ActionResult RequestUserProfile()
        //{
        //    try
        //    {
        //        GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

        //        ViewData["Culture"] = UserLookup.CultureMapper.Map(cultureDTOs.Result);

        //        GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}&includeUserDefinedGroups={1}", SessionInfo.CultureShortName, false)).Result;

        //        TreeViewModel tree = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

        //        ViewData["PermissionsGroups"] = tree;

        //        //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest("api/Admin/GetOrgUnits?cultureName=" + SessionInfo.CultureShortName).Result;

        //        //ViewData["OrgUnitData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);

        //        GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
        //           .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

        //        List<GroupVM> GroupVMs = GroupMapper.Map(groups.Result).ToList();

        //        ViewData["Roles"] = GetGroupsAutoCompleteDataSource(GroupVMs);

        //        GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.DefaultRole)).Result;
        //        AddUserProfileVM addUserProfileVM = new AddUserProfileVM();
        //        addUserProfileVM.RoleId = Convert.ToInt32(SettingValue.Result.Value);
        //        addUserProfileVM.TransactionProcessingPeriod = 5;
        //        return View("_UserRequestManagementAddPartial", addUserProfileVM);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}



        [HttpGet]
        public ActionResult GetGroupPermissionsByGroupId(int groupId)
        {
            GetResult<PermissionGroupDTO> getResult = HttpClientWrapper<GetResult<PermissionGroupDTO>>
                  .GetItemRequest(string.Format("api/Admin/GetGroupPermissionsByGroupId?groupId={0}&cultureName={1}", groupId, SessionInfo.CultureShortName)).Result;

            PermissionGroupVM permissionGroupVM = PermissionMapper.MapPermissionGroup(getResult.Result);

            TreeViewModel tree = BuildTree(new List<PermissionGroupVM> { permissionGroupVM }, null);

            ViewData["PermissionsGroups"] = tree;

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/Permissions/_PermissionTreePartial.cshtml", new AddGroupVM()), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUserProfile(AddUserProfileVM addUserProfileVM)
        {
            try
            {
                string message = string.Empty;

                if (addUserProfileVM.Email == null)
                {
                    addUserProfileVM.Email = addUserProfileVM.UserName + "@modmil.gov.sa";
                }
                if (addUserProfileVM.UserNationalId == null)
                {
                    addUserProfileVM.UserNationalId = "0000000000";

                }
                if (addUserProfileVM.PhoneNumber == null)
                {
                    addUserProfileVM.PhoneNumber = "0000000000";

                }
                if (addUserProfileVM.AllowMobile == false)
                {
                    addUserProfileVM.UserMobileClassId = null;
                }
                if (addUserProfileVM.SelectedOrgUnitsIds != null)
                {
                    addUserProfileVM.OrgUnits = addUserProfileVM.SelectedOrgUnitsIds.Split(',').Select(int.Parse).ToList();
                    //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                }
                if (addUserProfileVM.UserGroups != null)
                {
                    addUserProfileVM.UserGroupsList = addUserProfileVM.UserGroups.Split(',').Select(int.Parse).ToList();
                    //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                }
                addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/PostUser?&cultureName={0}&resetPasswordUrl={1}",
                    SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(ControllerContext, "User")), UserProfileMapper.Map(addUserProfileVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    if (postResult.StatusCode == MCS.Common.StatusCode.UserNameAlreadyExist)
                    {
                        GetResult<UserProfileDTO> userProfileDTO = HttpClientWrapper<GetResult<UserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserByUserName?userName={0}", addUserProfileVM.UserName)).Result;
                        if (userProfileDTO.Result.IsDeleted == true)
                        {
                            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                            return Json(new { MessageText = message, MessageType = MessageType.Warning, UserId = userProfileDTO.Result.Id }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken()]
        //public ActionResult RequestUserProfile(AddUserProfileVM addUserProfileVM)
        //{
        //    try
        //    {
        //        string message = string.Empty;
        //        addUserProfileVM.PendingRegestration = true;
        //        if (addUserProfileVM.Email == null)
        //        {
        //            addUserProfileVM.Email = addUserProfileVM.UserName + "@yasser.gov.sa";
        //        }
        //        if (addUserProfileVM.SelectedOrgUnitsIds != null)
        //        {
        //            addUserProfileVM.OrgUnits = addUserProfileVM.SelectedOrgUnitsIds.Split(',').Select(int.Parse).ToList();
        //            //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
        //        }
        //        if (addUserProfileVM.UserGroups != null)
        //        {
        //            addUserProfileVM.UserGroupsList = addUserProfileVM.UserGroups.Split(',').Select(int.Parse).ToList();
        //            //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
        //        }
        //        addUserProfileVM.OrgUnits = new List<int>();
        //        addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
        //        PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/PostUser?&cultureName={0}&resetPasswordUrl={1}",
        //            SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(ControllerContext, "User")), UserProfileMapper.Map(addUserProfileVM)).Result;

        //        if (postResult.StatusCode != StatusCode.Ok)
        //        {
        //            if (postResult.StatusCode == MCS.Common.StatusCode.UserNameAlreadyExist)
        //            {
        //                GetResult<UserProfileDTO> userProfileDTO = HttpClientWrapper<GetResult<UserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserByUserName?userName={0}", addUserProfileVM.UserName)).Result;
        //                if (userProfileDTO.Result.IsDeleted == true)
        //                {
        //                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

        //                    return Json(new { MessageText = message, MessageType = MessageType.Warning, UserId = userProfileDTO.Result.Id }, JsonRequestBehavior.AllowGet);

        //                }
        //            }
        //            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

        //            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
        //        }

        //        GetResult<List<UserProfileDTO>> userProfileDTOs =
        //            HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

        //        if (userProfileDTOs.StatusCode != StatusCode.Ok)
        //        {
        //            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

        //            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
        //        }

        //        IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);

        //        message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.AddSucceeded");

        //        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}




        [HttpPost]
        public ActionResult DeleteUsers(string ids)
        {
            try
            {
                string message = string.Empty;

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteUsers?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Warning }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), UsersUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult DeleteUserAndMoveTransaction(string ids)
        {
            try
            {
                string message = string.Empty;

                var logInUserId = SessionInfo.CurrentUser.Id;
                int id = Convert.ToInt32(ids);
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/MoveAllUserTransactions?UserId={0}", id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteUsers?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Warning }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), UsersUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult CheckIfUserCanDeleted(string ids)
        {
            try
            {
                string message = string.Empty;

                GetResult<bool> putResult = HttpClientWrapper<GetResult<bool>>.PostRequest(String.Format("api/Admin/CheckIfNotUsedUser?id={0}", ids), null).Result;

                if (putResult.Result == true)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "سوف يتم نقل المعاملات جميعها ثم حذف المستخدم");
                    return Json(new { MessageText = message, MessageType = MessageType.Error, id = Convert.ToInt32(ids) }, JsonRequestBehavior.AllowGet);
                }

                else
                {
                    message = "";
                    return Json(new { MessageText = message, MessageType = StatusCode.Ok, id = Convert.ToInt32(ids) }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetUserOtherDepartment(int id)
        {
            GetResult<OrgUnitDTO> orgUnit = HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(String.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementOtherDepartmentsItem", orgUnit.Result), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserGroup(int id)
        {
            GetResult<EditGroupDTO> groupEditDTO = HttpClientWrapper<GetResult<EditGroupDTO>>.GetItemRequest(String.Format("api/Admin/GetGroupByID?groupId={0}", id)).Result;
            UserGroupDTO userGroupDTO = new UserGroupDTO();
            userGroupDTO.GroupId = groupEditDTO.Result.Id;
            userGroupDTO.UserName = groupEditDTO.Result.Name.Localizations[0].Text;
            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserRolesItem.cshtml", userGroupDTO), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetUserProfile(int id)
        {
            try
            {
                string message = string.Empty;

                GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", id)).Result;

                EditUserProfileVM editUserProfileVM = UserProfileMapper.Map(userProfileEditDTO.Result);
                editUserProfileVM.OrgUnitList.RemoveAll(o => o.Id == editUserProfileVM.MainOrgUnitId);

                if (userProfileEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
                     .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", 10000, SessionInfo.CultureShortName)).Result;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                List<GroupVM> GroupVMs = GroupMapper.Map(groups.Result).ToList();

                ViewData["Roles"] = GetGroupsAutoCompleteDataSource(GroupVMs);

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                foreach (User.Models.Lookups.LookupVM lookup in LookupsHelper.GetLookupItems(LookupCategory.UserMobileClass, SessionInfo.CultureShortName).Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = lookup.EnumReference.ToString(),
                        Label = lookup.Text
                    });
                }

                ViewData["UserMobileClass"] = JsonConvert.SerializeObject(dataSource);


                return View("_UserManagementEditPartial", UserProfileMapper.Map(userProfileEditDTO.Result));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditUserProfile(EditUserProfileVM userEditProfileVM)
        {
            try
            {
                string message = string.Empty;
                userEditProfileVM.OrgUnits = new List<int>();
                //if (userEditProfileVM.OrgUnitList != null)
                //{
                //    userEditProfileVM.OrgUnits = userEditProfileVM.OrgUnitList.Select(o => o.Id).ToList();


                //}
                // userEditProfileVM.OrgUnits.Add(userEditProfileVM.MainOrgUnitId.Value);


                if (userEditProfileVM.SelectedOrgUnitsIds != null)
                {
                    userEditProfileVM.OrgUnits = userEditProfileVM.SelectedOrgUnitsIds.Split(',').Select(int.Parse).ToList();
                    //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                }
                if (userEditProfileVM.AllowMobile == false)
                {
                    userEditProfileVM.UserMobileClassId = null;
                    //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                }
                if (userEditProfileVM.UserGroupsList != null)
                {
                    userEditProfileVM.UserGroupsData = userEditProfileVM.UserGroupsList.Split(',').Select(int.Parse).ToList();
                    //addUserProfileVM.OrgUnits.Add(addUserProfileVM.MainOrgUnitId.Value);
                }
                userEditProfileVM.OrgUnits.Add(userEditProfileVM.MainOrgUnitId.Value);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutUser", UserProfileMapper.Map(userEditProfileVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                         HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]

        public ActionResult RemoveSignaturePassword(EditUserProfileVM userEditProfileVM)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/UserProfile/RemoveSignaturePassword?userId={0}", userEditProfileVM.Id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserProfile.Succeeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        public string GetListTransactionParameters(int? pageValue)
        {
            StringBuilder result = new StringBuilder();
            string filter = Request.Form["filter"];
            string sortColumnName = Request.Form["gridColumn"];
            string dir = Request.Form["dir"];
            string pageIndex = pageValue.HasValue ? pageValue.Value.ToString() : Request.Form["page"];
            string searchColumn = Request.Form["searchColumn"];
            string fromDate = Request.Form["fromDate"];
            string toDate = Request.Form["toDate"];
            string pageSize = Request.Form["pageSize"];
            string isDeleted = Request.Form["isDeleted"];
            result.Append("CultureName=").Append(SessionInfo.CultureShortName);
            FilterType filterType;
            if (!string.IsNullOrEmpty(filter))
            {
                string[] filterData = filter.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < filterData.Length; i++)
                {
                    string[] data = filterData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    string filterValue = data.Count() == 3 ? data[2] : string.Empty;
                    string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                    if (!Enum.TryParse(data[1], true, out filterType))
                    {
                        filterType = FilterType.Equals;
                    }
                    if (Convert.ToInt32(data[1]) == 2)
                    {
                        filterType = FilterType.Contains;
                    }
                    result.Append("&Filters[").Append(i).Append("].ColumnName=")
                          .Append(columnName[0]).Append("&Filters[").Append(i)
                          .Append("].Type=").Append(filterType).Append("&Filters[")
                          .Append(i).Append("].Value=").Append(filterValue);
                }
            }
            if (!string.IsNullOrEmpty(searchColumn))
            {
                string[] searchData = searchColumn.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < searchData.Length; i++)
                {
                    string[] data = searchData[i].Split(new[] { "__" },
                    StringSplitOptions.RemoveEmptyEntries);
                    result.Append("&SearchColunms[").Append(i).Append("].ColunmName=")
                          .Append(data[0]).Append("&SearchColunms[").Append(i)
                          .Append("].ColunmValue=").Append(data[1]);
                }
            }
            if (!string.IsNullOrEmpty(fromDate))
            {
                result.Append("&FromDate=").Append(fromDate);
            }
            if (!string.IsNullOrEmpty(toDate))
            {
                result.Append("&ToDate=").Append(toDate);
            }
            if (!string.IsNullOrEmpty(sortColumnName))
            {
                string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                if (sortData.Length > 1)
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
                else
                {
                    result.Append("&OrderBy=").Append(sortData[0]);
                }
            }
            if (!string.IsNullOrEmpty(pageSize))
            {
                result.Append("&PageSize=").Append(pageSize);
            }
            else
            {
                result.Append("&PageSize=").Append(UIHelper.PageSize);
            }
            if (dir == "1")
            {
                result.Append("&Ascending=").Append(true);
            }
            else
            {
                result.Append("&Ascending=").Append(false);
            }
            if (!string.IsNullOrEmpty(pageIndex))
            {
                int page = Convert.ToInt32(pageIndex);
                result.Append("&PageIndex=").Append(page);
            }
            else
            {
                result.Append("&PageIndex=").Append(1);
            }
            if (isDeleted == "true")
            {
                result.Append("&isDeleted=").Append(true);
            }
            else
            {
                result.Append("&isDeleted=").Append(false);
            }
            return result.ToString();
        }
        [HttpPost]
        public ActionResult UsersProfileSearch(int? page)
        {
            string parameters = GetListTransactionParameters(page);
            GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                   .GetItemRequest(string.Format("api/Admin/GetUsersProfiles?{0}", parameters)).Result;
            List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
            if (userProfileVMs != null)
            {
                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), page ?? 1, userProfileDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/UsersGridPartial.cshtml", grid), JsonRequestBehavior.AllowGet });
            }
            else
            {
                return Json(new { MessageText = DbRes.TResource("Admin.User.NotFound"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult UpdateGridUserProfile(int? page)
        {
            try
            {
                string data = GridHelper.GetGridParameters();
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(String.Format("api/Admin/GetUsersProfiles?{0}&CultureName={1}", data, SessionInfo.CultureShortName)).Result;
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), page ?? 1, userProfileDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/UsersGridPartial.cshtml", grid), JsonRequestBehavior.AllowGet });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult UpdateGridPendingUserRegestrationProfile(int? page)
        {
            try
            {
                string data = GridHelper.GetGridParameters();
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(String.Format("api/Admin/GetUsersProfiles?{0}&CultureName={1}", data, SessionInfo.CultureShortName)).Result;
                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), page ?? 1, userProfileDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/PendingUsersRegestrationGridPartial.cshtml", grid), JsonRequestBehavior.AllowGet });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult UpdateGridOrgUserProfile(int? page, int orgUnitKey)
        {
            try
            {
                string data = GridHelper.GetGridParameters();
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                .GetItemRequest(string.Format("api/Admin/GetUsersByOrgUnitId?PageIndex={0}&PageSize={1}&CultureName={2}&orgUnitId={3}",
                page, GridHelper.PageSize, SessionInfo.CultureShortName, orgUnitKey)).Result;
                List<OrgUnitUserVM> userProfileVMs = OrgUnitMapper.MapToOrgUnitUser(userProfileDTOs.Result);
                MCS.GridMvc.Ajax.GridExtensions.IAjaxGrid grid = (MCS.GridMvc.Ajax.GridExtensions.AjaxGrid<OrgUnitUserVM>)new MCS.GridMvc.Ajax.GridExtensions.AjaxGridFactory().CreateAjaxGrid(userProfileVMs.ToList(), page ?? 1, userProfileDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/OrgUnitStructure/OrgUsersGridPartial.cshtml", grid), JsonRequestBehavior.AllowGet });

            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult CheckUserName(string username)
        {
            try
            {
                username = StringUtility.ValidateUserNameInput(username);
                List<ActiveDirectoryUserVM> allUsers = GetAllActiveUsers(username);

                allUsers.OrderBy(s => s.UserName).ToList();

                return Json(new { UsersList = allUsers }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetActiveDirectoryUsers(List<ActiveDirectoryUserVM> users, int type, string username)
        {
            try
            {
                ViewData["Type"] = type;
                ViewData["UserName"] = username;

                if (users == null)
                {
                    users = new List<ActiveDirectoryUserVM>();
                }

                IAjaxGrid grid = (AjaxGrid<ActiveDirectoryUserVM>)new AjaxGridFactory().CreateAjaxGrid(users.Where(u => u.IsActive).OrderBy(u => u.UserName).AsQueryable(), 1, false, users.Count);

                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ActiveDirectoryUsersPartial", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridActiveDirectoryUser(string username, int? page)
        {
            try
            {
                username = StringUtility.ValidateUserNameInput(username);
                List<ActiveDirectoryUserVM> allUsers = GetAllActiveUsers(username);

                var grid = new AjaxGridFactory().CreateAjaxGrid(allUsers.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, allUsers.Count, true);

                return Json(new { Html = grid.ToJson("_ActiveDirectoryUsersGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ImportUsers()
        {
            try
            {
                List<AddUserProfileDTO> users = new List<AddUserProfileDTO>();

                var userDTOType = typeof(AddUserProfileDTO);

                foreach (string file in Request.Files)
                {
                    HttpPostedFileBase postedFile = Request.Files[file];
                    ISheet sheet;

                    if (postedFile.FileName.EndsWith(".xlsx"))
                    {
                        XSSFWorkbook xssfwb = new XSSFWorkbook(postedFile.InputStream);

                        sheet = xssfwb.GetSheetAt(0);
                    }
                    else if (postedFile.FileName.EndsWith(".xls"))
                    {
                        POIFSFileSystem filesys = new POIFSFileSystem(postedFile.InputStream);
                        HSSFWorkbook hssfwb = new HSSFWorkbook(filesys);

                        sheet = hssfwb.GetSheetAt(0);
                    }
                    else //not excel 
                    {
                        return Json(new { MessageText = DbRes.TResource("Admin.User.InvalidFile"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                    List<CultureDTO> cultures = cultureDTOs.Result;

                    for (int row = 1; row <= sheet.LastRowNum; row++)
                    {
                        if (sheet.GetRow(row) != null) //null is when the row only contains empty cells 
                        {
                            AddUserProfileDTO user = new AddUserProfileDTO();

                            user.Names = new List<LocalizationDTO>();

                            for (int i = 0; i < cultures.Count; i++)
                            {
                                if (sheet.GetRow(0) != null && sheet.GetRow(0).Cells[i] != null)
                                {
                                    var propName = GetPropertyName<AddUserProfileDTO, object>(y => y.Names);

                                    PropertyInfo propertyInfo = userDTOType.GetProperty(propName);

                                    if (propertyInfo != null &&
                                        typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType) &&
                                        propertyInfo.PropertyType.IsGenericType &&
                                        propertyInfo.PropertyType.GetGenericArguments().Length == 1)
                                    {
                                        LocalizationDTO localization = new LocalizationDTO();

                                        if (propertyInfo.PropertyType.GetGenericArguments().Count() > 0)
                                        {
                                            IList<PropertyInfo> innerProperties = propertyInfo.PropertyType.GetGenericArguments()[0].GetProperties();

                                            if (innerProperties != null &&
                                                innerProperties[1] != null &&
                                                innerProperties[2] != null)
                                            {
                                                innerProperties[1].SetValue(localization, Convert.ChangeType(sheet.GetRow(row).Cells[i].ToString(), innerProperties[1].PropertyType));
                                                innerProperties[2].SetValue(localization, Convert.ChangeType(cultures[i].Id, innerProperties[2].PropertyType));
                                            }

                                            user.Names.Add(localization);
                                        }
                                    }
                                }
                            }

                            for (int col = cultures.Count; col < sheet.GetRow(row).Cells.Count; col++)
                            {
                                if (sheet.GetRow(0) != null && sheet.GetRow(0).Cells[col] != null)
                                {
                                    PropertyInfo propertyInfo = userDTOType.GetProperty(sheet.GetRow(0).Cells[col].ToString());

                                    if (propertyInfo != null)
                                    {
                                        propertyInfo.SetValue(user, Convert.ChangeType(sheet.GetRow(row).Cells[col].ToString(), propertyInfo.PropertyType));
                                    }
                                }
                            }

                            users.Add(user);
                        }
                    }
                }

                return PostUsers(UserProfileMapper.Map(users));
            }
            catch (Exception)
            {
                return Json(new { MessageText = DbRes.TResource("Admin.User.InvalidFile"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        static string GetPropertyName<TObject, TResult>(Expression<Func<TObject, TResult>> exp)
        {
            // extract property name
            return (((MemberExpression)(exp.Body)).Member).Name;
        }

        public ActionResult PostUsers(List<AddUserProfileVM> userProfileAddVMs)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/PostUsers?cultureName={0}&resetPasswordUrl={1}", SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(ControllerContext)), UserProfileMapper.Map(userProfileAddVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersProfiles?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userProfileDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userProfileDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserProfileVM>)new AjaxGridFactory().CreateAjaxGrid(UserProfileMapper.Map(userProfileDTOs.Result).AsQueryable(), 1, false, userProfileDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult OpenAddUserTitle(string mode)
        {
            try
            {
                ViewData["Mode"] = mode;

                LookupDTO lookupDTO = new LookupDTO();

                lookupDTO.CategoryId = (int)LookupCategory.Title;
                lookupDTO.IsActive = true;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                lookupDTO.Localizations = cultureDTOs.Result.Select(c => new LookupLocalizationDTO()
                {
                    CultureId = c.Id,
                    CultureName = c.ShortName
                }).ToList();

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AddUserTitleDialogPartial", LookupMapper.Map(lookupDTO)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUserTitle(LookupVM lookupVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest($"api/Lookups/PostLookupItem?cultureName={SessionInfo.CultureShortName}",
                    LookupMapper.Map(lookupVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.Title, SessionInfo.CultureShortName);

                List<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (lookups.Result != null)
                {
                    dataSource = lookups.Result.Select(l => new AutoCompleteDataSource()
                    {

                        Value = l.Id.ToString(),
                        Label = l.Text
                    }).ToList();
                }

                message = DbRes.TResource("Admin.UserManagement.AddTitleSuccessMsg");

                return Json(new { Items = JsonConvert.SerializeObject(dataSource), NewItem = postResult.Id, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddPermission(PermissionVM permissionVM)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostPermission", PermissionMapper.Map(permissionVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;
                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementTreePermissions", new AddUserProfileVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditPermission(PermissionEditVM permissionEditVM)
        {
            try
            {
                string message = string.Empty;

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutPermission", PermissionMapper.Map(permissionEditVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;
                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementTreePermissions", new AddUserProfileVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeletePermission(int permissionId)
        {
            try
            {
                string message = string.Empty;

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeletePermission?permissionId={0}", permissionId)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<PermissionGroupDTO>> permissionGroupDTOs = HttpClientWrapper<GetResult<List<PermissionGroupDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllPermissionsGroups?cultureName={0}", SessionInfo.CultureShortName)).Result;
                ViewData["PermissionsGroups"] = BuildTree(PermissionMapper.Map(permissionGroupDTOs.Result), null);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Permissions.Success");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserManagementTreePermissions", new AddUserProfileVM()), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult ResendEmail(int userId)
        {
            try
            {
                string message = string.Empty;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Admin/ReSendNotificationEmail?userId={0}&cultureName={1}&resetPasswordUrl={2}", userId, SessionInfo.CultureShortName, UrlHelper.GetResetPasswordUrl(ControllerContext)), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.Success");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActivateUser(int userId, bool isActive)
        {
            try
            {
                string message = string.Empty;

                EditUserProfileDTO userEditProfileDTO = new EditUserProfileDTO()
                {
                    Id = userId,
                    IsActive = isActive
                };

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Admin/ActivateUser?userId={0}&isActive={1}", userId, isActive), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.UpdateSucceeded");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult Users()
        {

            return View();
        }

        private TreeViewModel BuildTree(List<PermissionGroupVM> permissionGroupVMs, string rootName)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            tree.RootNode = new TreeNode { Id = 0, Name = rootName };

            for (int i = 0; i < permissionGroupVMs.Count; i++)
            {
                TreeNode groupNode = new TreeNode()
                {
                    Id = permissionGroupVMs[i].Id,
                    ParentId = 0, // it is group, so, it is root with no parent 
                    Name = permissionGroupVMs[i].Text,
                    IsUserDefined = permissionGroupVMs[i].IsUserDefined
                };

                foreach (PermissionVM permission in permissionGroupVMs[i].Permissions)
                {
                    TreeNode permissionNode = new TreeNode()
                    {
                        Id = permission.Id,
                        ParentId = permissionGroupVMs[i].Id,
                        Name = permission.Text,
                        IsSelected = permission.IsSelected,
                        IsUserDefined = permission.IsUserDefined
                    };

                    groupNode.Childs.Add(permissionNode);
                }

                tree.RootNode.Childs.Add(groupNode);
            }

            nodes.Add(tree.RootNode);

            return tree;
        }

        private string GetUserCategories()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<UserCategoryDTO>> userCategoryDTOs =
              HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllUsersCategories?cultureName={0}", SessionInfo.CultureShortName)).Result;

            if (userCategoryDTOs.Result != null)
            {
                foreach (UserCategoryDTO userCategoryDTO in userCategoryDTOs.Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = UserCategoryMapper.Map(userCategoryDTO).Id.ToString(),
                        Label = UserCategoryMapper.Map(userCategoryDTO).CategoryText
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        //private TreeViewModel BuildOrgUnitsTree(List<OrgUnitDTO> orgUnitDTOs)
        //{
        //    TreeViewModel tree = new TreeViewModel();
        //    List<TreeNode> nodes = new List<TreeNode>();

        //    tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

        //    OrgUnitDTO root = orgUnitDTOs.Where(o => o.ParentId == -1).SingleOrDefault();

        //    if (root != null)
        //    {
        //        orgUnitDTOs.Where(o => o.ParentId == root.Id).ToList().ForEach(d =>
        //        {
        //            tree.RootNode.Childs.Add(AddChilds(orgUnitDTOs, d));
        //        });
        //    }

        //    return tree;
        //}

        //private TreeNode AddChilds(List<OrgUnitDTO> orgUnitDTOs, OrgUnitDTO orgUnitDTO)
        //{
        //    TreeNode treeNode = new TreeNode()
        //    {
        //        DepartmentNumber = orgUnitDTO.Number.ToString(),
        //        IsSelected = orgUnitDTO.IsSelected,
        //        Selectable = true,
        //        Name = orgUnitDTO.Name,
        //        Id = orgUnitDTO.Id
        //    };

        //    orgUnitDTOs.Where(o => o.ParentId == orgUnitDTO.Id).ToList().ForEach(d =>
        //    {
        //        treeNode.Childs.Add(AddChilds(orgUnitDTOs, d));
        //    });

        //    return treeNode;
        //}

        private List<ActiveDirectoryUserVM> GetAllActiveUsers(string username)
        {
            string ldapConnectionString = string.Format("LDAP://{0}/{1}", SystemConfigurations.LDAPServerName, SystemConfigurations.LDAPDomainName);
            string domainUserName = SystemConfigurations.LDAPUserName;
            string domainUserPassword = SystemConfigurations.LDAPPassword;
            System.DirectoryServices.DirectoryEntry group = new System.DirectoryServices.DirectoryEntry(ldapConnectionString, domainUserName, domainUserPassword);

            return GetGroupUsers(group, username);
        }

        private List<ActiveDirectoryUserVM> GetGroupUsers(System.DirectoryServices.DirectoryEntry groupToGet, string username)
        {
            DirectorySearcher directorySearcher = new DirectorySearcher(groupToGet);
            directorySearcher.Filter = string.Format("(&(objectCategory=person)(anr={0}))", username);

            SearchResultCollection searchResult = directorySearcher.FindAll();
            int counter = 0;
            List<ActiveDirectoryUserVM> users = new List<ActiveDirectoryUserVM>();
            foreach (SearchResult memberEntry in searchResult)
            {
                ActiveDirectoryUserVM user = new ActiveDirectoryUserVM();

                if (memberEntry.Properties.Contains("samaccountname"))
                {
                    user.UserName = (String)memberEntry.Properties["samaccountname"][0];
                }
                if (memberEntry.Properties.Contains("displayName"))
                {
                    user.EmployeeName = (String)memberEntry.Properties["displayName"][0];
                }
                if (memberEntry.Properties.Contains("mail"))
                {
                    user.Email = (String)memberEntry.Properties["mail"][0];
                }
                if (memberEntry.Properties.Contains("mobile"))
                {
                    user.PhoneNumber = (String)memberEntry.Properties["mobile"][0];
                }
                if (memberEntry.Properties.Contains("userAccountControl"))
                {
                    user.IsActive = Convert.ToBoolean(memberEntry.Properties["userAccountControl"][0]);
                }
                if (memberEntry.Properties.Contains("title"))
                {
                    user.Title = Convert.ToString(memberEntry.Properties["title"][0]);
                }
                if (user.UserName != null && users.Any(u => u.UserName.ToLower() == user.UserName.ToLower()) == false)
                {
                    counter += 1;
                    user.Id = counter;
                    users.Add(user);
                }
            }

            return users;
        }
        private string GetGroupsAutoCompleteDataSource(List<GroupVM> groupVMs)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();


                if (groupVMs != null && groupVMs.Count() > 0)
                {
                    foreach (GroupVM groupVM in groupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = groupVM.Id.ToString(),
                            Label = groupVM.LocalName
                        });
                    }
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult ActivateDeactivateUser(int id)
        {
            try
            {
                string message = string.Empty;
                GetResult<UserProfileDTO> getResult = HttpClientWrapper<GetResult<UserProfileDTO>>
                    .GetItemRequest(string.Format("api/Admin/ActivateDeactivateUser?UserId={0}&CultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                UserProfileVM userProfileVM = UserProfileMapper.Map(getResult.Result);
                message = userProfileVM.IsActive ? "تم تفعيل المستخدم بنجاح" : "تم إلغاء تفعيل المستخدم بنجاح";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/UserBoxPartial.cshtml", userProfileVM), MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ApproveRequestedUser(int id)
        {
            try
            {
                string message = string.Empty;
                GetResult<UserProfileDTO> getResult = HttpClientWrapper<GetResult<UserProfileDTO>>
                    .GetItemRequest(string.Format("api/Admin/ApproveRequestedUser?UserId={0}&CultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                UserProfileVM userProfileVM = UserProfileMapper.Map(getResult.Result);
                message = userProfileVM.IsActive ? "تم تفعيل المستخدم بنجاح" : "تم إلغاء تفعيل المستخدم بنجاح";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/UserBoxPartial.cshtml", userProfileVM), MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult RejectRequestedUser(int Id)
        {
            try
            {
                GetResult<bool> userResult = HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/Admin/RejectRequestedUser?UserId={0}", Id)).Result;
                return Json(new { result = userResult.Result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }


        [HttpGet]
        public ActionResult ActivateDeleteUser(int id)
        {
            try
            {
                string message = string.Empty;
                GetResult<UserProfileDTO> getResult = HttpClientWrapper<GetResult<UserProfileDTO>>
                    .GetItemRequest(string.Format("api/Admin/ActivateDeleteUser?UserId={0}&CultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                UserProfileVM userProfileVM = UserProfileMapper.Map(getResult.Result);
                message = "تم تفعيل المستخدم بنجاح";

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/User/UserBoxPartial.cshtml", userProfileVM), MessageType = MessageType.Information, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult CheckUserNameExists(string userName)
        {
            try
            {
                string message = string.Empty;
                GetResult<int?> getResult = HttpClientWrapper<GetResult<int?>>
                    .GetItemRequest(string.Format("api/Admin/CheckUserNameExists?userName={0}&CultureName={1}", userName, SessionInfo.CultureShortName)).Result;

                if (getResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (getResult.Result.HasValue)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Confirm.Exists");
                    return Json(new { MessageType = MessageType.Information, MessageText = message, UserId = getResult.Result.Value }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageType = MessageType.Warning, MessageText = message, UserId = -1 }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetUsersWithGroups(string GroupId = null)
        {
            try
            {
                GetResult<List<UserGroupDTO>> userGroupDTOList = HttpClientWrapper<GetResult<List<UserGroupDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetUsersWithGroups?GroupId={0}", GroupId)).Result;

                List<UserGroupVM> userGroupVMList = UsersWithGroupsMapper.Map(userGroupDTOList.Result);

                var grid = (CustomGrid.AjaxGrid<UserGroupVM>)new CustomGrid.AjaxGridFactory()
                    .CreateAjaxGrid(userGroupVMList.ToList(), 1, userGroupVMList.ToList().Count, false, 50);// GridHelper.PageSize


                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));



                ViewData["GridData"] = grid;
                if (GroupId == null)
                {
                    return View();

                }
                else
                {

                    var partialPath = "~/Areas/Admin/Views/Lookups/_UsersWithGroups.cshtml";


                    return Json(new
                    {
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, partialPath, grid),
                        Count = 0,
                        PageSize = 0,
                        MessageText = ""
                    }, JsonRequestBehavior.AllowGet);

                    //return Json(new
                    //{
                    //    Html = grid.ToJson("_UsersWithGroups", this),
                    //    grid.HasItems
                    //}, JsonRequestBehavior.AllowGet);


                }



            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetUsersWithGroups()
        {
            try
            {
                GetResult<List<UserGroupDTO>> userGroupDTOList = HttpClientWrapper<GetResult<List<UserGroupDTO>>>.GetItemRequest("api/Admin/GetUsersWithGroups").Result;

                List<UserGroupVM> userGroupVMList = UsersWithGroupsMapper.Map(userGroupDTOList.Result);

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserGroupVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userGroupVMList.ToList(), 1, userGroupVMList.ToList().Count, false, 50);// GridHelper.PageSize


                ViewData["GridData"] = grid;

                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetUpdateUsersWithGroups(int? page)
        {
            try
            {
                GetResult<List<UserGroupDTO>> userGroupDTOList = HttpClientWrapper<GetResult<List<UserGroupDTO>>>.GetItemRequest("api/Admin/GetUsersWithGroups").Result;

                List<UserGroupVM> userGroupVMList = UsersWithGroupsMapper.Map(userGroupDTOList.Result);

                CustomGrid.IAjaxGrid grid = (CustomGrid.AjaxGrid<UserGroupVM>)new CustomGrid.AjaxGridFactory().CreateAjaxGrid(userGroupVMList.ToList(), page ?? 1, userGroupVMList.ToList().Count, page.HasValue, GridHelper.PageSize);



                ViewData["GridData"] = grid;



                return Json(new { Html = grid.ToJson("_UsersWithGroups.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetUserReport()
        {
            try
            {
                GetResult<List<UserProfileDTO>> usersDTOList = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                    .GetItemRequest(String.Format("api/Admin/GetUsers")).Result;

                List<UserProfileVM> userVMList = UserProfileMapper.Map(usersDTOList.Result);

                var grid = (CustomGrid.AjaxGrid<UserProfileVM>)new CustomGrid.AjaxGridFactory()
                    .CreateAjaxGrid(userVMList.ToList(), 1, userVMList.ToList().Count, false, 50);// GridHelper.PageSize


                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));

                ViewData["GridData"] = grid;

                return View("~/Areas/Admin/Views/UserProfile/_usersReport.cshtml", grid);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}