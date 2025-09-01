using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Transaction;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.UserProfile;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;

namespace MCS.UI.Areas.User.Controllers
{
    [CustomViewEngines.AlternateViewEnginePath("Transaction")]
    public class ReservationController : BaseController
    {
        // GET: User/Reservation
        [HttpGet]
        public ActionResult Index(bool isFromSearch = false)
        {
            //string parameters = GridHelper.GetGridParameters();
            string parameters = GetListTransactionParameters(null);
            TransactionReservationVM transactionReservationVM = new TransactionReservationVM();

            GetResult<List<TransactionReservationDTO>> reservationsDTOs =
                HttpClientWrapper<GetResult<List<TransactionReservationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionReservations?orgUnitId={0}&userId={1}&{2}", null, null, parameters)).Result;

            List<TransactionReservationVM> transactionReservationVMs = TransactionReservationMapper.Map(reservationsDTOs.Result);
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
            ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            ViewData["TransactionCategory"] = InboundOutboundLabelForAutoComplete();
            transactionReservationVM.Reservations = (AjaxGrid<TransactionReservationVM>)new AjaxGridFactory().CreateAjaxGrid(transactionReservationVMs, 1, reservationsDTOs.RowsCount.Value, false);
            if (isFromSearch)
            {
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_TransactionReservationGridPartial.cshtml", transactionReservationVM.Reservations) }, JsonRequestBehavior.AllowGet);
            }
            return View("~/Areas/User/Views/Transaction/TransactionReservationDetails.cshtml", transactionReservationVM);
        }
        [HttpGet]
        public ActionResult GetAllUsers(string searchQuery, int? entityId)
        {
            List<UserProfileVM> userProfileVMs = base.GetAllUserProfiles(entityId, searchQuery);

            var users = base.ConvertUsersListToDataSource(userProfileVMs);

            if (searchQuery == null)
            {
                return Json(new { USERS = users, Count = userProfileVMs.Count() }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { USERS = userProfileVMs, Count = userProfileVMs.Count() }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReservationFilter(bool isFromSearch = false)
        {
            string parameters = GetListTransactionParameters(null);
            TransactionReservationVM transactionReservationVM = new TransactionReservationVM();

            GetResult<List<TransactionReservationDTO>> reservationsDTOs =
                HttpClientWrapper<GetResult<List<TransactionReservationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionReservations?orgUnitId={0}&userId={1}&{2}", null, null, parameters)).Result;

            List<TransactionReservationVM> transactionReservationVMs = TransactionReservationMapper.Map(reservationsDTOs.Result);
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                       HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
            ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            transactionReservationVM.Reservations = (AjaxGrid<TransactionReservationVM>)new AjaxGridFactory().CreateAjaxGrid(transactionReservationVMs, 1, reservationsDTOs.RowsCount.Value, false);
            if (isFromSearch)
            {
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/_TransactionReservationGridPartial.cshtml", transactionReservationVM.Reservations) }, JsonRequestBehavior.AllowGet);

            }
            return View("~/Areas/User/Views/Transaction/TransactionReservationDetails.cshtml", transactionReservationVM);
        }

        //GET: User/Reservation/Add
        public ActionResult Add()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                                HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);

                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                TransactionReservationVM transactionReservationVM = new TransactionReservationVM();
                transactionReservationVM.UserId = SessionInfo.CurrentUser.Id;
                return View("~/Areas/User/Views/Transaction/TransactionReservationAdd.cshtml", transactionReservationVM);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveTransactionReservation(TransactionReservationVM transactionReservationVM)
        {
            GetResult<SettingDTO> SettingValue = null;
            if (transactionReservationVM.TransactionCategoryId == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
            {
                SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.MaxOutboundNumberCanBooked)).Result;
            }
            else
            {
                SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.GeneralSettings.MaxInboundNumberCanBooked)).Result;
            }
            var settingVM = SettingMapper.Map(SettingValue.Result);
            int MaxAllowedReservation = Convert.ToInt32(settingVM.Value);
            if (Convert.ToInt32(transactionReservationVM.Count.ToString()) > MaxAllowedReservation /*SystemConfigurations.MaxAllowedReservation*/)
            {
                return Json(new { MessageText = DbRes.TValidation("User.Transaction.Reservation.MaxAllowedCount") + MaxAllowedReservation.ToString(), MessageType = MessageType.Warning });
            }
            TransactionReservationDTO transactionReservationDTO = TransactionReservationMapper.Map(transactionReservationVM);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Transaction/PostTransactionReservation", transactionReservationDTO).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }

        [HttpGet]
        public ActionResult GetReservedTransactions(int reservationId)
        {
            var reservationsDTOs = HttpClientWrapper<GetResult<List<TransactionReservedDTO>>>
                .GetItemRequest(string.Format("api/Transaction/GetReservedTransaction?reservationId={0}", reservationId)).Result;

            List<TransactionReservedVM> reservedVM = TransactionReservationMapper.Map(reservationsDTOs.Result);

            TransactionReservedVM transactionReservedVM = new TransactionReservedVM();
            transactionReservedVM.Transactions = (AjaxGrid<TransactionReservedVM>)new AjaxGridFactory().CreateAjaxGrid(reservedVM, 1, reservationsDTOs.Result.Count, false);

            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Transaction/TransactionReservationDialog.cshtml", transactionReservedVM)
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult UpdateGridReservations(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<TransactionReservationDTO>> reservationsDTOs =
                    HttpClientWrapper<GetResult<List<TransactionReservationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTransactionReservations?orgUnitId={0}&userId={1}&{2}", null, null, parameters)).Result;

                List<TransactionReservationVM> transactionReservationVMs = TransactionReservationMapper.Map(reservationsDTOs.Result);
                IAjaxGrid grid = (AjaxGrid<TransactionReservationVM>)new AjaxGridFactory().CreateAjaxGrid(transactionReservationVMs, page.HasValue ? page.Value : 1, reservationsDTOs.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/Transaction/_TransactionReservationGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public string GetUsersByOrgUnitId(int? id)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;

                List<UserProfileVM> userProfileVMS = UserProfileMapper.Map(userProfileDTOs.Result);

                if (userProfileVMS != null)
                {
                    foreach (UserProfileVM userProfileVM in userProfileVMS)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = userProfileVM.Id.ToString(),
                            Label = userProfileVM.LocalName
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
            return result.ToString();
        }

        public string InboundOutboundLabelForAutoComplete()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>
                {
                    new AutoCompleteDataSource()
                    {
                        Label = DbRes.TResource("User.SubjectSearch.Inbound"),
                        Value = (TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)).ToString()
                    },

                    new AutoCompleteDataSource()
                    {
                        Label = DbRes.TResource("User.SubjectSearch.Outbound"),
                        Value = (TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)).ToString()
                    }
                };

                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}