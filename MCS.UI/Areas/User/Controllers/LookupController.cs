

using DocumentFormat.OpenXml.Drawing.Charts;
using DocumentFormat.OpenXml.Spreadsheet;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TXTextControl;
using ZXing;

namespace MCS.UI.Areas.User.Controllers
{

    public class LookupController : BaseController
    {
        // GET: User/Lookup
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult Template()
        {
            try
            {
                Session["OfficeOnlineFileGuid"] = Guid.NewGuid();
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                TemplateViewModel formViewModel = new TemplateViewModel();
                int outboundDraft = TransactionCategories.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategories, SessionInfo.CultureShortName);
                List<Models.Lookups.TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups().Where(x => x.Id == (int)TransactionCategories.DraftOutbound).ToList();
                transactionCategoryVMs.ForEach(x => x.IsSelected = true);

                formViewModel.AddTemplate.TransactionCategories = transactionCategoryVMs;
                formViewModel.EditTemplate.TransactionCategories = transactionCategoryVMs;

                GetResult<List<FormDTO>> formDTOs =
                    HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}&OrgunitId={2}", GridHelper.PageSize, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<TemplateVM> formVMs = TemplateMapper.Map(formDTOs.Result);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                if (formVMs == null)
                {
                    formVMs = new List<TemplateVM>();
                    formDTOs.RowsCount = 0;
                }

                IAjaxGrid grid = (AjaxGrid<TemplateVM>)new AjaxGridFactory().CreateAjaxGrid(formVMs, 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("~/Areas/User/Views/Lookups/Template/Index.cshtml", formViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult AddTemplate(TemplateAddVM formAddVM)
        {
            try
            {
                string message = string.Empty;
                bool noTransCatSelected = true;

                formAddVM.OrgUnitIds = new List<int>();
                formAddVM.OrgUnitIds.Add(SessionInfo.OrgUnitId);

                foreach (var item in formAddVM.TransactionCategories)
                {
                    if (item.IsSelected)
                    {
                        noTransCatSelected = false;
                        break;
                    }
                }

                if (noTransCatSelected)
                {
                    foreach (var item in formAddVM.TransactionCategories)
                    {
                        item.IsSelected = true;
                    }
                }
                HttpPostedFileBase file;

                string FilePrefix;
                if (SystemConfigurations.MultiTenantEnabled)
                {
                    FilePrefix = ((TenantInfo)SessionInfo.GetObjectFromSession(Constants.TenantKey)).Id + "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile;
                }
                else
                {
                    FilePrefix = "_" + SessionInfo.CurrentUser.Id + "_" + Constants.ExplanationFile + "_";
                }

                string path = StringUtility.ValidateFileNames(SystemConfigurations.ExternalCopiesAttachmentPath) + FilePrefix;
                var fullPath = StringUtility.ValidateFileNames($"{path}{formAddVM.FileName}");
                byte[] fileContent = System.IO.File.ReadAllBytes(fullPath);
                string fileExtenstion = GetAttchementMimeType(formAddVM.FileName);





                // string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                //string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                if (fileContent != null && fileContent.Length > 0)
                {


                    formAddVM.FormContentVM = new TemplateContentVM
                    {
                        Content = fileContent,

                    };

                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.SelectTemplete");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }



                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostForm", TemplateMapper.Map(formAddVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<List<FormDTO>> formDTOs =
                   HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<TemplateVM>)new AjaxGridFactory().CreateAjaxGrid(TemplateMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_GridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult EditTemplate(TemplateEditVM formEditVM)
        {
            try
            {

                string message = string.Empty;
                bool noTransCatSelected = true;

                formEditVM.OrgUnitIds = new List<int>();
                formEditVM.OrgUnitIds.Add(SessionInfo.OrgUnitId);

                if (formEditVM.OrgUnitIds.Count > 1)
                {
                    formEditVM.OrgUnitIds.Remove(formEditVM.OrgUnitIds.FirstOrDefault(o => o == 0));
                }
                foreach (var item in formEditVM.TransactionCategories)
                {
                    if (item.IsSelected)
                    {
                        noTransCatSelected = false;
                        break;
                    }
                }

                if (noTransCatSelected)
                {
                    foreach (var item in formEditVM.TransactionCategories)
                    {
                        item.IsSelected = true;
                    }
                }



                // string officeOnlineFileGuid = Session["OfficeOnlineFileGuid"].ToString();
                if (!string.IsNullOrWhiteSpace(formEditVM.FileContent))
                {
                    formEditVM.FormContentVM = new TemplateContentVM
                    {
                        Content = Convert.FromBase64String(formEditVM.FileContent)
                    };
                }
                else
                {
                    message = DbRes.TValidation("User.Transaction.SelectTemplete");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                string textSig = "Sig";
                string textMark = "Mark";
                //using (TXTextControl.ServerTextControl tx = new TXTextControl.ServerTextControl())
                //{
                //    tx.Create();

                //    tx.Load(Convert.FromBase64String(formEditVM.FileContent), TXTextControl.BinaryStreamType.WordprocessingML);
                //    var sigFrame = tx.TextFrames.GetItem(textSig);
                //    var markFrame = tx.TextFrames.GetItem(textMark);

                //    if (sigFrame == null)
                //    {
                //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.SigFramIsMandatory.ToString());
                //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //    }

                //    if (markFrame == null)
                //    {
                //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, StatusCode.MarkFramIsMandatory.ToString());
                //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //    }


                //}


                BaseResult baseResult = null;
                baseResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutForm", TemplateMapper.Map(formEditVM)).Result;


                if (baseResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, baseResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
     HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                GetResult<List<FormDTO>> formDTOs =
                    HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<TemplateVM>)new AjaxGridFactory().CreateAjaxGrid(TemplateMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_GridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult DeleteTemplate(string ids)
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

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeleteForms?ids={0}", ids)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<FormDTO>> formDTOs =
                 HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(string.Format("api/Admin/GetForms?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), 1, false, formDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result), 1, formDTOs.RowsCount.Value, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.DeleteSucceeded");

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_GridPartial.cshtml", grid),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult GetTemplate(string id)
        {
            try
            {
                Session["OfficeOnlineFileGuid"] = Guid.NewGuid().ToString();
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<FormEditDTO> formEditDTO =
                    HttpClientWrapper<GetResult<FormEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFormById?formId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (formEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.UpdateSucceeded");

                TemplateEditVM formEditVM = TemplateMapper.Map(formEditDTO.Result);

                GetResult<List<OrgUnitDTO>> orgUnitByIdsDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByIds?orgUnitIds={0}&cultureName={1}", string.Join(",", formEditVM.OrgUnitIds), SessionInfo.CultureShortName)).Result;

                //GetOrgUnitsNodesByIds
                if (formEditVM.OrgUnitIds == null || formEditVM.OrgUnitIds.Count() == 0)
                {
                    formEditVM.AllOrgUnitsSelected = true;
                }
                else
                {
                    formEditVM.OrgUnitsKeyValue = new Dictionary<int, string>();
                    foreach (var item in orgUnitByIdsDTOs.Result)
                    {
                        formEditVM.OrgUnitsKeyValue.Add(item.Id, item.Name);
                    }
                }

                formEditVM.TransactionCategories = MergeTransactionCategoryLookups(formEditVM.TransactionCategories);
                var resultlist = formEditVM.TransactionCategories.Where(x => x.Id == (int)(TransactionCategories.DraftOutbound)).ToList();
                resultlist.ForEach(x => x.IsSelected = true);
                formEditVM.TransactionCategories = resultlist;



                return Json(new { AllOrgUnitsSelected = formEditVM.AllOrgUnitsSelected, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_EditPartial.cshtml", formEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult CopyTemplate(string id)
        {
            try
            {
                Session["OfficeOnlineFileGuid"] = Guid.NewGuid().ToString();
                string message = string.Empty;

                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                if (cultureDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["Culture"] = CultureMapper.Map(cultureDTOs.Result);

                GetResult<FormEditDTO> formEditDTO =
                    HttpClientWrapper<GetResult<FormEditDTO>>.GetItemRequest(String.Format("api/Admin/GetFormById?formId={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (formEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, formEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Form.UpdateSucceeded");

                TemplateAddVM formEditVM = TemplateMapper.MapToCopy(formEditDTO.Result);

                GetResult<List<OrgUnitDTO>> orgUnitByIdsDTOs =
                      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnitsByIds?orgUnitIds={0}&cultureName={1}", string.Join(",", formEditVM.OrgUnitIds), SessionInfo.CultureShortName)).Result;


                formEditVM.AllOrgUnitsSelected = false;
                formEditVM.TransactionCategories = MergeTransactionCategoryLookups(formEditVM.TransactionCategories);
                var resultlist = formEditVM.TransactionCategories.Where(x => x.Id == (int)(TransactionCategories.DraftOutbound)).ToList();
                resultlist.ForEach(x => x.IsSelected = true);
                formEditVM.TransactionCategories = resultlist;



                return Json(new { AllOrgUnitsSelected = formEditVM.AllOrgUnitsSelected, Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_AddPartial.cshtml", formEditVM), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        public ActionResult UpdateTemplateGrid(int? page)
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
                StringBuilder result = new StringBuilder();

                result.Append(GridHelper.GetGridParameters());
                result.Append("&OrgUnitId=").Append(SessionInfo.OrgUnitId);
                string parameters = result.ToString();
                GetResult<List<FormDTO>> formDTOs = HttpClientWrapper<GetResult<List<FormDTO>>>.GetItemRequest(String.Format("api/Admin/GetForms?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                if (formDTOs.StatusCode != StatusCode.Ok)
                {

                }

                //var grid = new AjaxGridFactory().CreateAjaxGrid(FormMapper.Map(formDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, formDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<TemplateVM>)new AjaxGridFactory().CreateAjaxGrid(TemplateMapper.Map(formDTOs.Result), page ?? 1, formDTOs.RowsCount.Value, page.HasValue, GridHelper.PageSize);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Lookups/Template/_GridPartial.cshtml", grid) /*grid.ToJson("_FormGridPartial", this), grid.HasItems*/ }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        private List<TransactionCategoryVM> GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategories, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();

            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    transactionCategoryVMs.Add(new TransactionCategoryVM()
                    {
                        Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                        Text = lookupVM.Text,
                    });
                }
            }

            return transactionCategoryVMs;
        }
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        private byte[] GetFileAsByteArray(HttpPostedFileBase files)
        {

            using (MemoryStream ms = new MemoryStream())
            {
                files.InputStream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        [CustomAuthorizationAttribute(UserClaims.GeneralPermissions.ManageTemplate)]
        private List<TransactionCategoryVM> MergeTransactionCategoryLookups(List<TransactionCategoryVM> transactionCategoryVMs)
        {
            List<TransactionCategoryVM> localizeTransactionCategoryVMs = GetTransactionCategoryLookups();

            foreach (TransactionCategoryVM transactionCategoryVM in transactionCategoryVMs)
            {
                if (localizeTransactionCategoryVMs.Where(l => l.Id == transactionCategoryVM.Id &&
                    transactionCategoryVM.IsSelected == true).SingleOrDefault() != null)
                {
                    localizeTransactionCategoryVMs.Where(l => l.Id == transactionCategoryVM.Id &&
                        transactionCategoryVM.IsSelected == true).SingleOrDefault().IsSelected = true;
                }
            }

            return localizeTransactionCategoryVMs;
        }
    }


}