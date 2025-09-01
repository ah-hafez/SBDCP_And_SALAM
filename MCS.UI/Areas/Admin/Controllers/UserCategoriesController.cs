using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models.Permission;
using MCS.UI.Areas.Admin.Models.UserCategories;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Controls;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class UserCategoriesController : AdminControllerBase
    {
        private string GetCategories()
        {
            var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.UserCategory, SessionInfo.CultureShortName);

            GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
            if (permissionVMs != null)
            {
                foreach (PermissionVM permissionVM in permissionVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = permissionVM.Id.ToString(),
                        Label = permissionVM.Text
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        public ActionResult Index()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<UserCategoryDTO>> userCategoryDTOs =
                    HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(string.Format("api/Admin/GetUserCategories?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                IAjaxGrid grid = (AjaxGrid<UserCategoryVM>)new AjaxGridFactory().CreateAjaxGrid(UserCategoryMapper.Map(userCategoryDTOs.Result).AsQueryable(), 1, false, userCategoryDTOs.RowsCount.Value);

                ViewData["GridData"] = grid;

                ViewData["UserCategories"] = GetCategories();

                UserCategoriesViewModel userCategoriesViewModel = new UserCategoriesViewModel();

                return View(userCategoriesViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddUserCategory(AddUserCategoryVM addUserCategoryVMs)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostUserCategory", UserCategoryMapper.Map(addUserCategoryVMs)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserCategoryDTO>> userCategoryDTOs =
                   HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(string.Format("api/Admin/GetUserCategories?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userCategoryDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userCategoryDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserCategoryVM>)new AjaxGridFactory().CreateAjaxGrid(UserCategoryMapper.Map(userCategoryDTOs.Result).AsQueryable(), 1, false, userCategoryDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserCategoriesGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditUserCategory(EditUserCategoryVM editUserCategoryVMs)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutUserCategory", UserCategoryMapper.Map(editUserCategoryVMs)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserCategoryDTO>> userCategoryDTOs =
                    HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(string.Format("api/Admin/GetUserCategories?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;


                if (userCategoryDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userCategoryDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserCategoryVM>)new AjaxGridFactory().CreateAjaxGrid(UserCategoryMapper.Map(userCategoryDTOs.Result).AsQueryable(), 1, false, userCategoryDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserCategoriesGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteUserCategories(string ids)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteUserCategories?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserCategoryDTO>> userCategoryDTOs =
                   HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(string.Format("api/Admin/GetUserCategories?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userCategoryDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userCategoryDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserCategoryVM>)new AjaxGridFactory().CreateAjaxGrid(UserCategoryMapper.Map(userCategoryDTOs.Result).AsQueryable(), 1, false, userCategoryDTOs.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserCategoriesGridPartial", grid), UserCategoriesUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetUserCategory(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                ViewData["UserCategories"] = GetCategories();

                GetResult<EditUserCategoryDTO> userCategoryEditDTO =
                    HttpClientWrapper<GetResult<EditUserCategoryDTO>>.GetItemRequest(String.Format("api/Admin/GetUserCategoryById?userCategoryId={0}", id)).Result;

                if (userCategoryEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userCategoryEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.UserCategory.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserCategoryEditPartial", UserCategoryMapper.Map(userCategoryEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateUserCategoriesGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters();

                GetResult<List<UserCategoryDTO>> userCategoryDTOs = HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(String.Format("api/Admin/GetUserCategories?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(UserCategoryMapper.Map(userCategoryDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, userCategoryDTOs.RowsCount.Value);

                return Json(new { Html = grid.ToJson("_UserCategoriesGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}