using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;
using MCS.UI.TenantsAdmin.Mappers;
using MCS.UI.TenantsAdmin.Models;
using MCS.UI.TenantsAdmin;
using MCS.UI.TenantsAdmin.Models.Tenant;
using System.IO;
using MCS.Common.ApiControllerResults;
using MCS.UI.TenantsAdmin.Wrappers;
using MCS.Common;
using MCS.UI.TenantsAdmin.Models.LookupsVM;
using System.Threading.Tasks;
using MCS.DTO.Tenants;
using MCS.Framework.Controls;
using Newtonsoft.Json;
using System.Security.Cryptography.X509Certificates;

namespace MCS.UI.TenantsAdmin.Controllers
{
    [CustomAuthorizationAttribute]
    public class TenantController : BaseController
    {
        private const string filterDataDelimeter = "__";
        public ActionResult Index()
        {
            try
            {
                TenantViewModel tenantViewModel = new TenantViewModel();
                SearchCriteria searchCriteria = new SearchCriteria
                {
                    PageIndex = 1,
                    PageSize = 100,
                    CultureName = SessionInfo.CultureShortName
                };
                ViewData["Culture"] = FillStaticCulture();

                GetResult<List<TenantDTO>> getResult = HttpClientWrapper<GetResult<List<TenantDTO>>>
                                            .PostRequest("api/tenant/search", searchCriteria).Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<TenantVM> tenantVMs = TenantMapper.Map(getResult.Result);

                if (tenantVMs == null)
                {
                    tenantVMs = new List<TenantVM>();
                }

                IAjaxGrid grid = (AjaxGrid<TenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, tenantVMs.Count);

                ViewData["GridData"] = grid;

                return View(tenantViewModel);
            }
            catch (BusinessException ex)
            {
                string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddTenant(AddTenantVM AddTenantVM)
        {
            string message = string.Empty;

            try
            {
                var AddTenantDTO = new TenantDTO
                {
                    DelegatedEmail = AddTenantVM.DelegatedEmail,
                    DelegatedMobile = AddTenantVM.DelegatedMobile,
                    DelegatedUserName = AddTenantVM.DelegatedUserName,
                    FromDate = AddTenantVM.FromDate,
                    FromDateH = AddTenantVM.FromDateH,
                    HostName = AddTenantVM.HostName,
                    OrgUnitsCount = AddTenantVM.OrgUnitsCount,
                    ToDate = AddTenantVM.ToDate,
                    ToDateH = AddTenantVM.ToDateH,
                    UsersCount = AddTenantVM.UsersCount
                };

                AddTenantDTO.DelegatedName = new TenantLocalizationIdentifierDTO();
                AddTenantDTO.DelegatedName.Localizations = new List<TenantLocalizationDTO>();
                AddTenantVM.DelegatedName?.ForEach(a =>
                {
                    AddTenantDTO.DelegatedName.Localizations.Add(new TenantLocalizationDTO
                    {
                        CultureId = a.CultureId,
                        Id = a.Id,
                        Text = a.Text,
                    });
                });

                AddTenantDTO.Name = new TenantLocalizationIdentifierDTO();
                AddTenantDTO.Name.Localizations = new List<TenantLocalizationDTO>();
                AddTenantVM.Names?.ForEach(a =>
                {
                    AddTenantDTO.Name.Localizations.Add(new TenantLocalizationDTO
                    {
                        CultureId = a.CultureId,
                        Id = a.Id,
                        Text = a.Text
                    });
                });
                if (Request.Files != null)
                {
                    using (MemoryStream memoryStreamLogo = new MemoryStream())
                    {
                        if (Request.Files["LogoFileAdd"].ContentLength > 0)
                        {
                            Request.Files["LogoFileAdd"].InputStream.CopyTo(memoryStreamLogo);
                            AddTenantDTO.Logo = memoryStreamLogo.ToArray();
                        }
                    }
                    using (MemoryStream memoryStreamCertificate = new MemoryStream())
                    {
                        if (Request.Files["YesserCertificateFileAdd"].ContentLength > 0)
                        {
                            Request.Files["YesserCertificateFileAdd"].InputStream.CopyTo(memoryStreamCertificate);
                            AddTenantDTO.YesserCertificate = memoryStreamCertificate.ToArray();
                        }
                    }
                };


                PostObjectResult<TenantDTO> addResult = HttpClientWrapper<PostObjectResult<TenantDTO>>
                    .PostRequest($"api/tenant/save?cultureName={SessionInfo.CultureShortName}", AddTenantDTO).Result;

                if (addResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, addResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int rowsCount = 0;
                SearchCriteria searchCriteria = new SearchCriteria();
                searchCriteria.PageIndex = 1;
                searchCriteria.PageSize = 10;
                searchCriteria.CultureName = SessionInfo.CultureShortName;
                GetResult<List<TenantDTO>> getResult = HttpClientWrapper<GetResult<List<TenantDTO>>>.PostRequest("api/tenant/search", searchCriteria).Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<TenantVM> tenantVMs = TenantMapper.Map(getResult.Result);
                if (tenantVMs == null)
                {
                    tenantVMs = new List<TenantVM>();
                    rowsCount = 0;
                }
                IAjaxGrid grid = (AjaxGrid<TenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, rowsCount);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Tenant.AddSucceeded");
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TenantGridPartial", grid),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Error
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditTenant(EditTenantVM EditTenantVM)
        {
            string message = string.Empty;

            try
            {
                var EditTenantDTO = new TenantDTO
                {
                    Id = EditTenantVM.Id,
                    DelegatedEmail = EditTenantVM.DelegatedEmail,
                    DelegatedMobile = EditTenantVM.DelegatedMobile,
                    DelegatedUserName = EditTenantVM.DelegatedUserName,
                    FromDate = EditTenantVM.FromDate,
                    FromDateH = EditTenantVM.FromDateH,
                    HostName = EditTenantVM.HostName,
                    OrgUnitsCount = EditTenantVM.OrgUnitsCount,
                    ToDate = EditTenantVM.ToDate,
                    ToDateH = EditTenantVM.ToDateH,
                    UsersCount = EditTenantVM.UsersCount
                };
                EditTenantDTO.DelegatedName = new TenantLocalizationIdentifierDTO();
                EditTenantDTO.DelegatedName.Localizations = new List<TenantLocalizationDTO>();
                EditTenantVM.DelegatedName.ForEach(a =>
                {
                    EditTenantDTO.DelegatedName.Localizations.Add(new TenantLocalizationDTO { CultureId = a.CultureId, Id = a.Id, Text = a.Text });
                });
                EditTenantDTO.Name = new TenantLocalizationIdentifierDTO();
                EditTenantDTO.Name.Localizations = new List<TenantLocalizationDTO>();
                EditTenantVM.Names.ForEach(a =>
                {
                    EditTenantDTO.Name.Localizations.Add(new TenantLocalizationDTO { CultureId = a.CultureId, Id = a.Id, Text = a.Text });
                });

                if (Request.Files != null)
                {
                    using (MemoryStream memoryStreamLogo = new MemoryStream())
                    {
                        if (Request.Files["LogoFileEdit"].ContentLength > 0)
                        {
                            Request.Files["LogoFileEdit"].InputStream.CopyTo(memoryStreamLogo);
                            EditTenantDTO.Logo = memoryStreamLogo.ToArray();
                        }
                    }
                    using (MemoryStream memoryStreamCertificate = new MemoryStream())
                    {
                        if (Request.Files["YesserCertificateFileEdit"].ContentLength > 0)
                        {
                            Request.Files["YesserCertificateFileEdit"].InputStream.CopyTo(memoryStreamCertificate);
                            EditTenantDTO.YesserCertificate = memoryStreamCertificate.ToArray();
                        }
                    }
                }

                PostObjectResult<TenantDTO> editResult = HttpClientWrapper<PostObjectResult<TenantDTO>>
                    .PostRequest($"api/tenant/update", EditTenantDTO).Result;

                if (editResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, editResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int rowsCount = 0;
                SearchCriteria searchCriteria = new SearchCriteria();
                searchCriteria.PageIndex = 1;
                searchCriteria.PageSize = 10;
                searchCriteria.CultureName = SessionInfo.CultureShortName;
                GetResult<List<TenantDTO>> getResult = HttpClientWrapper<GetResult<List<TenantDTO>>>.PostRequest("api/tenant/search", searchCriteria).Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<TenantVM> tenantVMs = TenantMapper.Map(getResult.Result);
                if (tenantVMs == null)
                {
                    tenantVMs = new List<TenantVM>();
                    rowsCount = 0;
                }
                IAjaxGrid grid = (AjaxGrid<TenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, rowsCount);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Tenant.UpdateSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TenantGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteTenant(string ids)
        {
            string message = string.Empty;
            try
            {
                IList<int> tenantIds = ids.Split(',').Select(int.Parse).ToList();
                var deleteResult = HttpClientWrapper<PostObjectResult<List<int>>>.PostRequest("api/tenant/delete", tenantIds).Result;

                if (deleteResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int rowsCount = 0;
                SearchCriteria searchCriteria = new SearchCriteria();
                searchCriteria.PageIndex = 1;
                searchCriteria.PageSize = 10;
                searchCriteria.CultureName = SessionInfo.CultureShortName;
                GetResult<List<TenantDTO>> getResult = HttpClientWrapper<GetResult<List<TenantDTO>>>.PostRequest("api/tenant/search", searchCriteria).Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<TenantVM> tenantVMs = TenantMapper.Map(getResult.Result);
                if (tenantVMs == null)
                {
                    tenantVMs = new List<TenantVM>();
                }
                IAjaxGrid grid = (AjaxGrid<TenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, rowsCount);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Tenant.DeleteSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TenantGridPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult GetTenant(int id)
        {
            string message = string.Empty;
            try
            {
                ViewData["Culture"] = FillStaticCulture();

                GetResult<TenantDTO> getResult = HttpClientWrapper<GetResult<TenantDTO>>.GetItemRequest($"api/tenant/byId/{id}").Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                EditTenantVM editTenantVM = TenantMapper.Map(getResult.Result);
                if (editTenantVM.Logo != null)
                {
                    ViewData["LogoFile"] = Convert.ToBase64String(editTenantVM.Logo);
                }

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TenantEditPartial", editTenantVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ActivateTenant(int tenantId, bool isActive)
        {
            string message = string.Empty;
            try
            {
                TenantDTO tenantDTO = new TenantDTO
                {
                    Id = tenantId,
                    IsActive = isActive
                };

                var activateResult = HttpClientWrapper<PostObjectResult<List<int>>>.PostRequest("api/tenant/activate", tenantDTO).Result;
                if (activateResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, activateResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateTenantGrid(int? page)
        {
            try
            {
                ITenantBL tenantBL = new TenantBL();
                int rowsCount = 0;

                SearchCriteria searchCriteria = GetSearchCriteria();

                List<Tenant> tenants = tenantBL.GetTenants(searchCriteria, SessionInfo.CultureShortName, out rowsCount).ToList();

                List<TenantDTO> tenantDTOs = TenantMapper.Map(tenants);
                List<TenantVM> tenantVMs = TenantMapper.Map(tenantDTOs);

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, rowsCount);

                return Json(new { Html = grid.ToJson("_TenantGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SendTanentResetPasswordEmail(int tenantId)
        {
            string message = string.Empty;

            try
            {
                var tenantDTO = new TenantDTO { Id = tenantId };
                var resetPasswordResult = HttpClientWrapper<PostObjectResult<TenantDTO>>
                    .PostRequest($"api/tenant/sendResetPasswordEmail", tenantDTO).Result;

                if (resetPasswordResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, resetPasswordResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = DbRes.TResource("Tenant.SendResetEmailSucceed");
                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region User Tenant
        [HttpGet]
        public ActionResult UserTenant()
        {
            string message = string.Empty;
            var userTenantVM = new UserTenantVM();
            userTenantVM.Mode = Mode.Add;

            ViewData["Tenants"] = GetAllTenant();

            //getAllUserTenants
            var getResult = HttpClientWrapper<GetResult<List<UserTenantDTO>>>.GetItemRequest("api/tenant/getAllUserTenants").Result;
            if (getResult.StatusCode != StatusCode.CodeOK)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<UserTenantVM> tenantVMs = TenantMapper.Map(getResult.Result, SessionInfo.CultureShortName);
            if (tenantVMs == null)
            {
                tenantVMs = new List<UserTenantVM>();
            }

            IAjaxGrid grid = (AjaxGrid<UserTenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, tenantVMs.Count());

            ViewData["UserTenantGridData"] = grid;
            return View(userTenantVM);
        }
        [HttpPost]
        public ActionResult UserTenant(UserTenantVM userTenantVM)
        {
            ViewData["Tenants"] = GetAllTenant();
            string message = string.Empty;

            var userTenantDTO = new UserTenantDTO();
            userTenantDTO.Id = userTenantVM.Id ?? 0;
            userTenantDTO.TenantId = userTenantVM.TenantId;
            userTenantDTO.UserName = userTenantVM.UserName;

            var addEditResult = HttpClientWrapper<PostObjectResult<TenantDTO>>.PostRequest($"api/tenant/AddEditUserTenant", userTenantDTO).Result;

            if (addEditResult.StatusCode != StatusCode.CodeOK)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, addEditResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            //getAllUserTenants
            var getResult = HttpClientWrapper<GetResult<List<UserTenantDTO>>>.GetItemRequest("api/tenant/getAllUserTenants").Result;
            if (getResult.StatusCode != StatusCode.CodeOK)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<UserTenantVM> tenantVMs = TenantMapper.Map(getResult.Result, SessionInfo.CultureShortName);
            if (tenantVMs == null)
            {
                tenantVMs = new List<UserTenantVM>();
            }

            IAjaxGrid grid = (AjaxGrid<UserTenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, tenantVMs.Count());
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Tenant.AddSucceeded");
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserTenantGridPartial", grid),
                MessageText = message,
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetUserTenant(int id)
        {
            string message = string.Empty;
            try
            {
                ViewData["Tenants"] = GetAllTenant();

                GetResult<UserTenantDTO> getResult = HttpClientWrapper<GetResult<UserTenantDTO>>.GetItemRequest($"api/tenant/userTenantById/{id}").Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                UserTenantVM userTenantVM = TenantMapper.Map(getResult.Result);
                userTenantVM.Mode = Mode.Edit;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AddEditUserTenantPartial", userTenantVM),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteUserTenant(int ids)
        {
            string message = string.Empty;
            try
            {
                var deleteResult = HttpClientWrapper<PostObjectResult<UserTenantDTO>>.PostRequest($"api/tenant/DeleteUserTenant?id={ids}", null).Result;

                if (deleteResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //getAllUserTenants
                int rowsCount = 0;
                var getResult = HttpClientWrapper<GetResult<List<UserTenantDTO>>>.GetItemRequest("api/tenant/getAllUserTenants").Result;
                if (getResult.StatusCode != StatusCode.CodeOK)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<UserTenantVM> tenantVMs = TenantMapper.Map(getResult.Result, SessionInfo.CultureShortName);
                if (tenantVMs == null)
                {
                    tenantVMs = new List<UserTenantVM>();
                    rowsCount = 0;
                }

                IAjaxGrid grid = (AjaxGrid<UserTenantVM>)new AjaxGridFactory().CreateAjaxGrid(tenantVMs.AsQueryable(), 1, false, rowsCount);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Tenant.AddSucceeded");
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserTenantGridPartial", grid),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (BusinessException ex)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ex.Message);

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        protected string GetAllTenant()
        {
            try
            {
                var getResult = HttpClientWrapper<GetResult<List<TenantDTO>>>.GetItemRequest("api/tenant/getAllTenants").Result;
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (getResult.Result != null)
                {
                    foreach (var item in getResult.Result)
                    {
                        if (item != null)
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = item.Id.ToString(),
                                Label = item.LocalName
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        private SearchCriteria GetSearchCriteria()
        {
            SearchCriteria searchCriteria = new SearchCriteria();

            string filter = HttpContext.Request.QueryString["grid-filter"];
            string sortColumnName = HttpContext.Request.QueryString["gridColumn"];
            string dir = HttpContext.Request.QueryString["dir"];
            string pageIndex = HttpContext.Request.QueryString["page"];
            string pageSizeText = HttpContext.Request.QueryString["pageSize"];

            searchCriteria.CultureName = SessionInfo.CultureShortName;
            searchCriteria.PageSize = 10;

            FilterType filterType;

            if (filter != null)
            {
                string[] filterData = filter.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                searchCriteria.Filters = new List<Framework.Persistence.Filter>();

                for (int i = 0; i < filterData.Length; i++)
                {
                    string[] data = filterData[i].Split(new[] { filterDataDelimeter },
                    StringSplitOptions.RemoveEmptyEntries);

                    string filterValue = data.Count() == 3 ? data[2] : string.Empty;

                    string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);

                    if (!Enum.TryParse(data[1], true, out filterType))
                    {
                        filterType = FilterType.Equals;
                    }

                    searchCriteria.Filters.Add(new Framework.Persistence.Filter { ColumnName = columnName[0], Type = filterType, Value = filterValue });
                }
            }

            if (sortColumnName != null)
            {
                string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
                searchCriteria.OrderBy = sortData[0];
            }

            if (!string.IsNullOrEmpty(pageSizeText))
            {
                searchCriteria.PageSize = Convert.ToInt32(pageSizeText);
            }

            if (dir == "1")
            {
                searchCriteria.Ascending = true;
            }

            if (!string.IsNullOrEmpty(pageIndex))
            {
                int page = Convert.ToInt32(pageIndex);
                searchCriteria.PageIndex = page;
            }
            else
            {
                searchCriteria.PageIndex = 1;

            }

            return searchCriteria;
        }
        private List<TenantCultureVM> FillStaticCulture()
        {
            List<TenantCultureVM> cultureVMs = new List<TenantCultureVM>();
            TenantCultureVM cultureVM = new TenantCultureVM
            {
                Id = (int)CultureType.Arabic,
                LocalName = "ar",
                ShortName = "ar"
            };
            cultureVMs.Add(cultureVM);
            cultureVM = new TenantCultureVM
            {
                Id = (int)CultureType.English,
                LocalName = "en",
                ShortName = "en"
            };
            cultureVMs.Add(cultureVM);
            return cultureVMs;
        }
        #endregion
    }
}

