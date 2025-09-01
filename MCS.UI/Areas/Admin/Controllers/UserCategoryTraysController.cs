using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.Tray;
using MCS.UI.Areas.Admin.Models.UserCategories;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Helpers;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class UserCategoryTraysController : AdminControllerBase
    {
        #region UserCategoryTrays
        public ActionResult Index()
        {
            try
            {
                GetResult<List<UserCategoryTrayDTO>> userCategoryTrayDTOs =
                       HttpClientWrapper<GetResult<List<UserCategoryTrayDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsersCategoriesTrays?cultureName={0}", SessionInfo.CultureShortName)).Result;


                UserCategoryTraysViewModel vm = new UserCategoryTraysViewModel
                {
                    UserCategoryTrays = UserCategoryTrayMapper.Map(userCategoryTrayDTOs.Result),
                };

                return View(vm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Index(UserCategoryTraysViewModel userCategoryTraysViewModel)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutUsersCategoriesTrays", userCategoryTraysViewModel.UserCategoryTrays).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = "", MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }
        #endregion

        #region Trays
        public ActionResult Trays()
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<TrayDTO>> traysList = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(string.Format("api/Admin/GetTrays?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                AjaxGrid<TrayVM> grid = (AjaxGrid<TrayVM>)new AjaxGridFactory().CreateAjaxGrid(TrayMapper.Map(traysList.Result).OrderBy(x => x.sort).AsQueryable(), 1, false, traysList.RowsCount.Value);
                //List<TrayDTO> alltrays = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllTrays?cultureName={0}", SessionInfo.CultureShortName)).Result.Result;

                TraysViewModel vm = new TraysViewModel();
                vm.Trays = grid;
                //vm.AllTrays = alltrays.OrderBy(x => x.sort).ToList();
                return View(vm);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetTray(string id)
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

                GetResult<EditTrayDTO> trayEditDTO =
                   HttpClientWrapper<GetResult<EditTrayDTO>>.GetItemRequest(String.Format("api/Admin/GetTrayById?trayId={0}", id)).Result;

                if (trayEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, trayEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TraysEditPartial", TrayMapper.Map(trayEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTray(EditTrayVM editTrayVM)
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
                ViewData["Culture"] = cultureDTOs.Result;


                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutTray", editTrayVM).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<TrayDTO>> traysList = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(string.Format("api/Admin/GetTrays?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (traysList.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, traysList.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                AjaxGrid<TrayVM> grid = (AjaxGrid<TrayVM>)new AjaxGridFactory().CreateAjaxGrid(TrayMapper.Map(traysList.Result).OrderBy(x => x.sort).AsQueryable(), 1, false, traysList.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TraysGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult UpdateTraySort(List<TrayVM> traysVM)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<CultureVM>> cultureVMs = HttpClientWrapper<GetResult<List<CultureVM>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureVMs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureVMs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                ViewData["Culture"] = cultureVMs.Result;


                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutTrays", TrayMapper.Map(traysVM)).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<TrayDTO>> traysList = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(string.Format("api/Admin/GetTrays?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (traysList.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, traysList.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                AjaxGrid<TrayVM> grid = (AjaxGrid<TrayVM>)new AjaxGridFactory().CreateAjaxGrid(TrayMapper.Map(traysList.Result).OrderBy(x => x.sort).AsQueryable(), 1, false, traysList.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TraysGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateTraysGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {

                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters(); GetResult<List<TrayDTO>> traysDTOs = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(String.Format("api/Admin/GetTrays?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (traysDTOs.StatusCode != StatusCode.Ok)
                {

                }

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(TrayMapper.Map(traysDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, traysDTOs.RowsCount.Value);

                return Json(new { Html = grid.ToJson("_TraysGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult RenderTraysSortPartial()
        {
            try
            {
                List<TrayDTO> alltrays = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllTrays?cultureName={0}", SessionInfo.CultureShortName)).Result.Result;
                List<TrayDTO> traysList = alltrays.OrderBy(x => x.Sort).ToList();
                return Json(new { View = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TraysSortPartial", TrayMapper.Map(traysList)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region User Trays and Permissions

        public ActionResult UsersTraysAndPermissions()
        {
            try
            {
                GetResult<List<UserProfileDTO>> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsers?cultureName={0}", SessionInfo.CultureShortName)).Result;
                GetResult<List<TrayDTO>> trayDTOs = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllTrays?cultureName={0}", SessionInfo.CultureShortName)).Result;
                GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllPermissions?cultureName={0}", SessionInfo.CultureShortName)).Result;

                UsersTraysAndPermissionsViewModel viewModel = new UsersTraysAndPermissionsViewModel();

                viewModel.Users = UserProfileMapper.Map(userProfileDTOs.Result);
                viewModel.Trays = TrayMapper.Map(trayDTOs.Result);
                viewModel.Permissions = PermissionMapper.Map(permissionDTOs.Result);
                if (viewModel.Users == null)
                {
                    viewModel.Users = new List<UserProfileVM>();
                }
                return View(viewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetPermissionsAndTraysByUserId(int id)
        {
            try
            {
                List<TrayDTO> trayDTOs = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllTrays?cultureName={0}", SessionInfo.CultureShortName)).Result.Result;
                List<PermissionDTO> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllPermissions?cultureName={0}", SessionInfo.CultureShortName)).Result.Result;
                List<PermissionDTO> userPermissions = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserPermissions?userId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result.Result;
                List<TrayDTO> userTrays = HttpClientWrapper<GetResult<List<TrayDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUserTrays?userId={0}", id)).Result.Result;

                if (userPermissions != null)
                {
                    foreach (PermissionDTO permissionDTO in userPermissions)
                    {
                        permissionDTOs.Where(p => p.Id == permissionDTO.Id).Single().IsSelected = true;
                    }
                }

                if (userTrays != null)
                {
                    foreach (TrayDTO trayDTO in userTrays)
                    {
                        trayDTOs.Where(t => t.Id == trayDTO.Id).Single().IsSelected = true;
                    }
                }

                permissionDTOs = permissionDTOs.OrderByDescending(p => p.IsSelected).ToList();
                trayDTOs = trayDTOs.OrderByDescending(t => t.IsSelected).ToList();

                string dataPermissions = JsonConvert.SerializeObject(permissionDTOs.Where(p => p.IsSelected));
                string dataTrays = JsonConvert.SerializeObject(trayDTOs.Where(p => p.IsSelected));


                return Json(new
                {
                    TraysHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TraysPartial", TrayMapper.Map(trayDTOs)),
                    PermissionsHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_PermissionsPartial", PermissionMapper.Map(permissionDTOs)),
                    DataPermissions = dataPermissions,
                    DataTrays = dataTrays
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetUsersByPermissionId(int id)
        {
            try
            {
                List<UserProfileDTO> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsers?cultureName=" + SessionInfo.CultureShortName)).Result.Result;
                List<UserProfileDTO> permissionUsers = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersByPermissionId?cultureName={0}&permissionId={1}", SessionInfo.CultureShortName, id)).Result.Result;

                foreach (var item in permissionUsers)
                {
                    userProfileDTOs.Where(p => p.Id == item.Id).Single().IsSelected = true;
                }

                userProfileDTOs = userProfileDTOs.OrderByDescending(p => p.IsSelected).ToList();

                string dataUsers = JsonConvert.SerializeObject(userProfileDTOs.Where(p => p.IsSelected));

                return Json(new
                {
                    UsersHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UsersPartial", UserProfileMapper.Map(userProfileDTOs)),
                    DataUsers = dataUsers
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetUsersByTrayId(int id)
        {
            try
            {
                List<UserProfileDTO> userProfileDTOs = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetAllUsers?cultureName=" + SessionInfo.CultureShortName)).Result.Result;
                List<UserProfileDTO> trayUsers = HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Admin/GetUsersByTrayId?cultureName={0}&trayId={1}", SessionInfo.CultureShortName, id)).Result.Result;

                foreach (UserProfileDTO userProfileDTO in trayUsers)
                {
                    userProfileDTOs.Where(p => p.Id == userProfileDTO.Id).Single().IsSelected = true;
                }

                userProfileDTOs = userProfileDTOs.OrderByDescending(p => p.IsSelected).ToList();

                string dataUsers = JsonConvert.SerializeObject(userProfileDTOs.Where(p => p.IsSelected));

                return Json(new
                {
                    UsersHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UsersPartial", UserProfileMapper.Map(userProfileDTOs)),
                    DataUsers = dataUsers
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SortUsers(string filter, string myList)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                object departmentsData = javaScriptSerializer.Deserialize(myList, typeof(object[]));

                List<UserProfileDTO> userProfileDTOs = new List<UserProfileDTO>();

                userProfileDTOs = userProfileDTOs.OrderBy(c => c.Names.Where(n => n.CultureName == SessionInfo.CultureShortName).Select(l => l.Text).Contains(filter)).ToList();

                return Json(new
                {
                    UsersHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UsersPartial", UserProfileMapper.Map(userProfileDTOs))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }




        [HttpPost]
        public ActionResult ExportToPdf(int id, string name, UsersAndTraysReportType type, string hdnTrays, string hdnUsers, string hdnPermissions)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<UserProfileDTO> userProfileDTOs = new List<UserProfileDTO>();
                List<PermissionDTO> permissionDTOs = new List<PermissionDTO>();
                List<TrayDTO> trayDTOs = new List<TrayDTO>();

                switch (type)
                {
                    case UsersAndTraysReportType.User:
                        userProfileDTOs.Add(new UserProfileDTO { LocalName = name });
                        break;
                    case UsersAndTraysReportType.Permission:
                        permissionDTOs.Add(new PermissionDTO { Text = name });
                        break;
                    case UsersAndTraysReportType.Tray:
                        trayDTOs.Add(new TrayDTO { LocalName = name });
                        break;
                }

                if (!string.IsNullOrEmpty(hdnTrays))
                {
                    trayDTOs = javaScriptSerializer.Deserialize(StringUtility.ValidateGridDataTray(hdnTrays), typeof(List<TrayDTO>)) as List<TrayDTO>;
                }

                if (!string.IsNullOrEmpty(hdnUsers))
                {
                    userProfileDTOs = javaScriptSerializer.Deserialize(StringUtility.ValidateGridData(hdnUsers), typeof(List<UserProfileDTO>)) as List<UserProfileDTO>;
                }

                if (!string.IsNullOrEmpty(hdnPermissions))
                {
                    permissionDTOs = javaScriptSerializer.Deserialize(StringUtility.ValidateGridData(hdnPermissions), typeof(List<PermissionDTO>)) as List<PermissionDTO>;
                }

                UsersTraysAndPermissionsViewModel viewModel = new UsersTraysAndPermissionsViewModel
                {
                    Users = UserProfileMapper.Map(userProfileDTOs),
                    Trays = TrayMapper.Map(trayDTOs),
                    Permissions = PermissionMapper.Map(permissionDTOs)
                };

                ViewData["Type"] = (int)type;

                string htmlText = UIHelper.RenderRazorViewToHtml(ControllerContext, "Report", viewModel);

                //Convert Html to Pdf    
                string handle = Guid.NewGuid().ToString();
                var pdf = PdfHelper.ConvertHtml2PDF(htmlText);

                TempData[handle] = pdf;
                // return resulted pdf document 
                FileResult fileResult = new FileContentResult(pdf, "application/pdf");
                fileResult.FileDownloadName = "UsersTraysAndPermissions.pdf";
                return Json(new
                {
                    FileGuid = handle,
                    FileName = fileResult.FileDownloadName,
                    MessageType = MessageType.Information
                },
                    JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult DownloadDocument(string fileGuid, string fileName)
        {
            if (TempData[fileGuid] != null)
            {
                byte[] pdfContent = TempData[fileGuid] as byte[];
                if (pdfContent == null)
                {
                    return null;
                }
                return File(pdfContent, System.Net.Mime.MediaTypeNames.Application.Pdf, fileName);
            }
            else
            {
                // Problem - Log the error, generate a blank file,
                // redirect to another controller action - whatever fits with your application
                return new EmptyResult();
            }
        }
        #endregion
    }
}