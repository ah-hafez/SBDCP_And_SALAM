using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;  
using System.Text;
using MCS.Framework.Persistence;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User;
using CustomGridMvc = MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.Admin.Models.ReleaseNotes;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Mappers;
using MCS.UI.Areas.Admin.Models.AdminAudit;
using MCS.DTO.AdminAudit;
using DocumentFormat.OpenXml.Spreadsheet;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class AdminAuditController : AdminControllerBase
    {
        public ActionResult Index()
        {
            try
            {
                
                GetResult<List<AuditLogDTO>> AuditLogDTOs =
                                 HttpClientWrapper<GetResult<List<AuditLogDTO>>>.GetItemRequest(string.Format(
                                     "api/AuditLog/GetAuditLog?cultureName={0}&IsForPrint={1}&searchCriteria={2}",
                                    SessionInfo.CultureShortName,  true, "")).Result;
                AuditLogVM adminAudit = JsonConvert.DeserializeObject<AuditLogVM>(AuditLogDTOs.Result.FirstOrDefault().AuditData);

                List<AdminAuditVM> AdminAudits = new List<AdminAuditVM>();
                ////AdminAudits = AdminAudits.Where(x => x.Changes.Where(c => c.NewValue != c.OriginalValue)).ToList();
                //AdminAudits.Add(adminAudit);
                ViewData["PaginationData"] = new Pagination { Page = 1, PageSize = UIHelper.PageSize, TotalCount = 1 };
                //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
                ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
                ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();
                return View((CustomGridMvc.AjaxGrid<AdminAuditVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(AdminAudits, 1, 1, false, UIHelper.PageSize));
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //[HttpPost]
        //public ActionResult UpdateAdminLogGrid(int transactionId, string sortType, int? searchData, int? page)
        //{
        //    string parameters = GetListTransactionParameters(page ?? 1);
        //    parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
        //    List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

        //    CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, false, UIHelper.PageSize);

        //    int GridSize = UIHelper.PageSize;
        //    ViewData["PaginationData"] = new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount };
        //    ViewData["TransactionId"] = transactionId;
        //    //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
        //    ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
        //    ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

        //    return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_LoggingTablePartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        //}

        //[HttpGet]
        //public ActionResult GetTransactionLogGrid(int transactionId)
        //{
        //    string parameters = GetListTransactionParameters(null);
        //    List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

        //    CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, false, UIHelper.PageSize);

        //    ViewData["PaginationData"] = new Pagination { Page = 1, PageSize = UIHelper.PageSize, TotalCount = itemsCount };
        //    ViewData["TransactionId"] = transactionId;
        //    //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
        //    ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
        //    ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

        //    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/Admin/Views/AdminAudit/_AdminAuditPartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        //}
        //public List<TransactionLogDetailInfoVM> GetTransactionLogInfo( bool IsForPrint, string parameters, out int itemsCount)
        //{
        //    GetResult<List<TransactionLogDetailInfoVM>> adminAuditDetails =
        //                           HttpClientWrapper<GetResult<List<TransactionLogDetailInfoVM>>>.GetItemRequest(string.Format(
        //                               "api/AuditLog/GetAuditLogDetailsInfo?{0}&cultureName={2}&IsForPrint={3}",
        //                             parameters, SessionInfo.CultureShortName, IsForPrint)).Result;

        //    itemsCount = adminAuditDetails.RowsCount ?? 0;
        //    List<TransactionLogDetailInfoVM> transactionLogInfoVMs = TransactionLogInfoMapper.Map(adminAuditDetails.Result);
        //    // commented until the SP finished
        //    return transactionLogInfoVMs;
        //}
        //private string GetListTransactionParameters(int? pageValue)
        //{
        //    StringBuilder result = new StringBuilder();
        //    string filter = Request.Form["filter"];
        //    string sortColumnName = Request.Form["gridColumn"];
        //    string dir = Request.Form["dir"];
        //    string pageIndex = pageValue.HasValue ? pageValue.Value.ToString() : Request.Form["page"];
        //    string searchColumn = Request.Form["searchColumn"];
        //    string fromDate = Request.Form["fromDate"];
        //    string toDate = Request.Form["toDate"];
        //    GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
        //    var settingVM = Areas.User.Mappers.Shared.SettingMapper.Map(SettingValue.Result);
        //    string pageSize = settingVM.Value;
        //    result.Append("CultureName=").Append(SessionInfo.CultureShortName);
        //    FilterType filterType;
        //    if (!string.IsNullOrEmpty(filter))
        //    {
        //        string[] filterData = filter.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = 0; i < filterData.Length; i++)
        //        {
        //            string[] data = filterData[i].Split(new[] { "__" },
        //            StringSplitOptions.RemoveEmptyEntries);
        //            string filterValue = data.Count() == 3 ? data[2] : string.Empty;
        //            string[] columnName = data[0].Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
        //            if (!Enum.TryParse(data[1], true, out filterType))
        //            {
        //                filterType = FilterType.Equals;
        //            }
        //            if (Convert.ToInt32(data[1]) == 2)
        //            {
        //                filterType = FilterType.Contains;
        //            }
        //            result.Append("&Filters[").Append(i).Append("].ColumnName=")
        //                  .Append(columnName[0]).Append("&Filters[").Append(i)
        //                  .Append("].Type=").Append(filterType).Append("&Filters[")
        //                  .Append(i).Append("].Value=").Append(filterValue);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(searchColumn))
        //    {
        //        string[] searchData = searchColumn.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
        //        for (int i = 0; i < searchData.Length; i++)
        //        {
        //            string[] data = searchData[i].Split(new[] { "__" },
        //            StringSplitOptions.RemoveEmptyEntries);
        //            result.Append("&SearchColunms[").Append(i).Append("].ColunmName=")
        //                  .Append(data[0]).Append("&SearchColunms[").Append(i)
        //                  .Append("].ColunmValue=").Append(data[1]);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(fromDate))
        //    {
        //        result.Append("&FromDate=").Append(fromDate);
        //    }
        //    if (!string.IsNullOrEmpty(toDate))
        //    {
        //        result.Append("&ToDate=").Append(toDate);
        //    }
        //    if (!string.IsNullOrEmpty(sortColumnName))
        //    {
        //        string[] sortData = sortColumnName.Split(new[] { "_" }, StringSplitOptions.RemoveEmptyEntries);
        //        if (sortData.Length > 1)
        //        {
        //            result.Append("&OrderBy=").Append(sortData[0]);
        //        }
        //        else
        //        {
        //            result.Append("&OrderBy=").Append(sortData[0]);
        //        }
        //    }
        //    if (!string.IsNullOrEmpty(pageSize))
        //    {
        //        result.Append("&PageSize=").Append(pageSize);
        //    }
        //    else
        //    {
        //        result.Append("&PageSize=").Append(UIHelper.PageSize);
        //    }
        //    if (dir == "1")
        //    {
        //        result.Append("&Ascending=").Append(true);
        //    }
        //    else
        //    {
        //        result.Append("&Ascending=").Append(false);
        //    }
        //    if (!string.IsNullOrEmpty(pageIndex))
        //    {
        //        int page = Convert.ToInt32(pageIndex);
        //        result.Append("&PageIndex=").Append(page);
        //    }
        //    else
        //    {
        //        result.Append("&PageIndex=").Append(1);
        //    }
        //    return result.ToString();
        //}

        //[HttpPost]
        //public ActionResult TransactionLogGridEventHandler(int transactionId, string sortType, int? searchData, int? page)
        //{
        //    string parameters = GetListTransactionParameters(page ?? 1);
        //    parameters += (sortType != null && sortType != "") ? "&OrderBy=" + sortType + "&SearchData=" + searchData : "";
        //    List<TransactionLogDetailInfoVM> transactionLogDetailInfoVMs = GetTransactionLogInfo(transactionId, false, parameters, out int itemsCount);

        //    CustomGridMvc.IAjaxGrid LogGrid = (CustomGridMvc.AjaxGrid<TransactionLogDetailInfoVM>)new CustomGridMvc.AjaxGridFactory().CreateAjaxGrid(transactionLogDetailInfoVMs, 1, itemsCount, true, UIHelper.PageSize);

        //    int GridSize = UIHelper.PageSize;
        //    ViewData["PaginationData"] = new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount };
        //    ViewData["TransactionId"] = transactionId;
        //    //ViewData["AllUsers"] = base.ConvertUsersListToDataSource(base.GetAllUserProfiles(null, null));
        //    ViewData["LogType"] = GetLookupsAudingActionCodes(LookupCategory.AuditingActionCode);
        //    ViewData["SortByDate"] = TransactionHelper.GetByDateAscDesc();

        //    return Json(new { PaginationData = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_TransactionLogPaginationPartial.cshtml", new Pagination { Page = page ?? 1, PageSize = GridSize, TotalCount = itemsCount }), Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/TransactionCertificate/_LoggingTablePartial.cshtml", LogGrid) }, JsonRequestBehavior.AllowGet);

        //}

        private string GetLookupsAudingActionCodes(LookupCategory lookupCategory)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                List<LookupVM> lookupVMs = LookupsHelper.GetLookupItems(LookupCategory.AuditingActionCode, SessionInfo.CultureShortName).Result.ToList();

                if (lookupVMs != null)
                {
                    foreach (LookupVM lookupVM in lookupVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = lookupVM.Id.ToString(),
                            Label = lookupVM.Text
                        });
                    }
                }

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}