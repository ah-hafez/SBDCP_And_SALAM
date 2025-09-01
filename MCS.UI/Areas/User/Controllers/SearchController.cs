using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers.ExternalParties;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Permission;
using MCS.UI.Areas.User.Mappers.Search;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models.ExternalParties;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Search;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;
using MCS.UI;
using MCS.UI.Areas.User.Models.Report;
using System.Data;

namespace MCS.UI.Areas.User.Controllers
{
    [CustomAuthorizationAttribute(UserClaims.Search.DisplaySearch)]
    public class SearchController : BaseController
    {
        public bool HasPermissionSearch
        {
            get
            {
                return SessionInfo.CurrentUser?.Claims.Contains("Search.ShowAllTransactions") == true ? true : false;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Search.DisplaySearch)]
        public ActionResult Index(bool? searchByNumber, string data)
        {
            try
            {

                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllModules) || SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllTransactions))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                      HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetParentOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);

                }
                else
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }

                List<BaseSearchResultVM> searchResultVMs = new List<BaseSearchResultVM>();
                List<InboundSearchResultVM> inboundSearchResultVMs = new List<InboundSearchResultVM>();
                List<OutboundSearchResultVM> outboundSearchResultVMs = new List<OutboundSearchResultVM>();

                //ViewData["InboundSearchGridData"] = inboundSearchResultVMs;

                //ViewData["OutboundSearchGridData"] = new AjaxGridFactory().CreateAjaxGrid(outboundSearchResultVMs.AsQueryable(), 1, false, searchResultVMs.Count());

                //ViewData["GeneralSearchGridData"] = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs.AsQueryable(), 1, false, searchResultVMs.Count());
                ViewData["searchTypeId"] = string.Empty;
                ViewData["searchData"] = string.Empty;

                if (searchByNumber.HasValue && data != null && data != "")
                {
                    if (searchByNumber.Value == true)
                    {
                        ViewData["searchTypeId"] = SearchType.SearchByInboundNumber.LookupIdentity(LookupCategory.SearchType, SessionInfo.CultureShortName);
                    }
                    else
                    {
                        ViewData["searchTypeId"] = SearchType.SearchBySubject.LookupIdentity(LookupCategory.SearchType, SessionInfo.CultureShortName);
                    }
                    ViewData["searchData"] = data;
                }

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                foreach (LookupVM lookup in LookupsHelper.GetLookupItems(LookupCategory.SearchType, SessionInfo.CultureShortName).Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = lookup.EnumReference.ToString(),
                        Label = lookup.Text
                    });
                }

                ViewData["searchTypes"] = JsonConvert.SerializeObject(dataSource);

                AdvancedSearchVM searchVM = new AdvancedSearchVM()
                {
                    InboundSearch = new SearchCriteriaByInboundVM(),
                    OutboundSearch = new SearchCriteriaByOutboundVM(),
                    CreatorSearch = new SearchCriteriaByCreatorVM(),
                    EntitySearch = new SearchCriteriaByEntityNameVM(),
                    OutboundDraftSearch = new SearchCriteriaByOutboundDraftVM(),
                    OutboundInternalSearch = new SearchCriteriaByOutboundInternalVM(),
                    DocumentNumberSearch = new SearchCriteriaByDocumentNumberVM(),
                    RecordNumberSearch = new SearchCriteriaByRecordNumberVM(),
                    AssignmentNoteSearch = new SearchCriteriaByAssignmentNoteVM(),
                    CopyAssignemntSearch = new SearchCriteriaByCopyAssignemntVM(),
                    DailySearch = new SearchCriteriaByDailyVM(),
                    ElcEmployeeSearch = new SearchCriteriaByElcEmployeeVM(),
                    ExternalOutBoundOrManifestNumberSearch = new SearchCriteriaByExternalOutBoundOrManifestNumberVM(),
                    ManifestNumberSearch = new SearchCriteriaByManifestNumberVM(),
                    IdentificationNumber = new SearchCriteriaByMilitaryNumberOrIdentityVM(),
                    NamesSearch = new SearchCriteriaByNamesVM(),
                    SubjectLetterSearch = new SearchCriteriaBySubjectLetterVM(),
                    TransactionNotsSearch = new SearchCriteriaByTransactionNotsVM(),
                    TransactionNumber = new SearchCriteriaByTransactionNumberVM(),
                    OrgUnitId = SessionInfo.OrgUnitId,
                    ExternalPartyCopies = new SearchCriteriaByExternalPartyCopiesVM(),
                };

                return View(searchVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult Search(AdvancedSearchVM advancedSearchVM, int? page)
        {
            try
            {
                string message = string.Empty;
                SearchCriteria searchCriteria = new SearchCriteria();

                searchCriteria.Filters = new List<MCS.Framework.Persistence.Filter>();

                if (advancedSearchVM.OrgUnitId.HasValue)
                {
                    searchCriteria.Filters.Add(
                        AddFilter(SearchFields.OrgUnitId, advancedSearchVM.OrgUnitId.Value.ToString(), FilterType.Equals));
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);


                switch ((SearchType)advancedSearchVM.SearchTypeId)
                {
                    case SearchType.SearchByInboundNumber:
                        {

                            SearchCriteriaByInboundDTO searchCriteriaByInboundDTO = new SearchCriteriaByInboundDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByInboundDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            if (!advancedSearchVM.InboundSearch.Year.HasValue)
                            {
                                advancedSearchVM.InboundSearch.DateFrom = null;
                                advancedSearchVM.InboundSearch.DateTo = null;
                            }

                            if (advancedSearchVM.InboundSearch.DateTo.HasValue)
                            {
                                searchCriteriaByInboundDTO.ToDate = advancedSearchVM.InboundSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.InboundSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByInboundDTO.FromDate = advancedSearchVM.InboundSearch.DateFrom.Value;

                            }
                            searchCriteriaByInboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByInboundDTO.OrderBy = "";
                            searchCriteriaByInboundDTO.PageIndex = page ?? 0;
                            searchCriteriaByInboundDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchCriteriaByInboundDTO.DeliveryMethodId = advancedSearchVM.InboundSearch.DeliveryMethodId;
                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByInboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.InboundSearch.Year != null)
                            {
                                searchCriteriaByInboundDTO.Year = advancedSearchVM.InboundSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByInboundDTO.FromDate.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.FromDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByInboundDTO.ToDate.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.ToDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.InboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.InboundSearch.HourFrom.Value,
                                    (advancedSearchVM.InboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByInboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.FromDate =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByInboundDTO.FromDateTime =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByInboundDTO.Global = true;

                            }

                            if (advancedSearchVM.InboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.InboundSearch.HourTo.Value,
                                    (advancedSearchVM.InboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByInboundDTO.ToDateTime =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByInboundDTO.ToDateTime =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.InboundSearch.Number.HasValue)
                            {
                                searchCriteriaByInboundDTO.Number = advancedSearchVM.InboundSearch.Number.Value;

                            }

                            searchCriteriaByInboundDTO.TransactionTypeId = advancedSearchVM.InboundSearch.TransactionTypeId;
                            searchCriteriaByInboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.InboundSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.SignedById = advancedSearchVM.InboundSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.StatusId = advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            if (advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByInboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByInboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByInboundDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<InboundSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/InboundSearch", searchCriteriaByInboundDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);


                        }

                    case SearchType.SearchByDocumentNumber:
                        {
                            SearchCriteriaByDocumentNumberDTO searchCriteriaByDocumentNumberDTO = new SearchCriteriaByDocumentNumberDTO();
                            searchCriteriaByDocumentNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDocumentNumberDTO.OrderBy = "";
                            searchCriteriaByDocumentNumberDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByDocumentNumberDTO.PageSize = GridHelper.PageSize;

                            if (advancedSearchVM.DocumentNumberSearch.Year != null)
                            {
                                searchCriteriaByDocumentNumberDTO.Year = advancedSearchVM.DocumentNumberSearch.Year;
                            }

                            if (advancedSearchVM.DocumentNumberSearch.DocumentNumber != string.Empty)
                            {
                                searchCriteriaByDocumentNumberDTO.DocumentNumber = advancedSearchVM.DocumentNumberSearch.DocumentNumber;
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDocumentNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            searchCriteriaByDocumentNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByDocumentNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByDocumentNumberDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/DocumentNumberSearch", searchCriteriaByDocumentNumberDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }


                    case SearchType.SearchByEntity:
                        {

                            SearchCriteriaByEntityNameDTO searchCriteriaByEntityNameDTO = new SearchCriteriaByEntityNameDTO();
                            if (advancedSearchVM.EntitySearch.DateTo.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateTo = advancedSearchVM.EntitySearch.DateTo.Value;

                            }
                            if (advancedSearchVM.EntitySearch.DateFrom.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateFrom = advancedSearchVM.EntitySearch.DateFrom.Value;

                            }

                            if (advancedSearchVM.EntitySearch.Number.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.Number = advancedSearchVM.EntitySearch.Number.Value;

                            }
                                searchCriteriaByEntityNameDTO.DocumentNumber = advancedSearchVM.EntitySearch.DocumentNumber;

                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByEntityNameDTO.Global = true;
                            }


                            searchCriteriaByEntityNameDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByEntityNameDTO.OrderBy = "";
                            searchCriteriaByEntityNameDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByEntityNameDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.EntitySearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.EntitySearch.HourFrom.Value,
                                    (advancedSearchVM.EntitySearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByEntityNameDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateFrom =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByEntityNameDTO.FromDateTime =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByEntityNameDTO.Global = true;
                            }
                            if (advancedSearchVM.EntitySearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.EntitySearch.HourTo.Value,
                                    (advancedSearchVM.EntitySearch.MinuteTo.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            searchCriteriaByEntityNameDTO.ExternalPartyId = advancedSearchVM.EntitySearch.ExternalPartyId;



                            searchCriteriaByEntityNameDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByEntityNameDTO.AdvancedSearch.FromPartyId = advancedSearchVM.EntitySearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.DirectedToId = advancedSearchVM.EntitySearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.DirectedToId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.EntitySearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.StatusId = advancedSearchVM.EntitySearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.StatusId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.PriorityId = advancedSearchVM.EntitySearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.PriorityId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.EntitySearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.ConfidentialityId.Value : -1;


                            searchCriteriaByEntityNameDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByEntityNameDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<EntitySearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<EntitySearchResultDTO>>>.PostRequest("api/Search/EntitySearch", searchCriteriaByEntityNameDTO).Result;
                            List<EntitySearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            if (searchResultVMs[0].TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EntitySearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByCreator:
                        {

                            SearchCriteriaByCreatorDTO searchCriteriaByCreatorDTO = new SearchCriteriaByCreatorDTO();
                            if (advancedSearchVM.CreatorSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateTo = advancedSearchVM.CreatorSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CreatorSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateFrom = advancedSearchVM.CreatorSearch.DateFrom.Value;

                            }

                            searchCriteriaByCreatorDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCreatorDTO.OrderBy = "";
                            searchCriteriaByCreatorDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByCreatorDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCreatorDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.CreatorSearch.Number.HasValue)
                            {
                                searchCriteriaByCreatorDTO.Number = advancedSearchVM.CreatorSearch.Number.Value;

                            }
                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.CreatorSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourFrom.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCreatorDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateFrom =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCreatorDTO.FromDateTime =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByCreatorDTO.Global = true;
                            }

                            if (advancedSearchVM.CreatorSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourTo.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByCreatorDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CreatorSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.FromPartyId = advancedSearchVM.CreatorSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.CreatorSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.PriorityId = advancedSearchVM.CreatorSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.CreatorSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.SignedById = advancedSearchVM.CreatorSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.StatusId = advancedSearchVM.CreatorSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByCreatorDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByCreatorDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByCreatorDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CreatorSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.CreatorSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.CreatorSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.DirectedToId = advancedSearchVM.CreatorSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.PriorityId = advancedSearchVM.CreatorSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.StatusId = advancedSearchVM.CreatorSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByCreatorDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }

                            searchCriteriaByCreatorDTO.CreatorUserId = advancedSearchVM.CreatorSearch.UserId;
                            searchCriteriaByCreatorDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCreatorDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<CreatorSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<CreatorSearchResultDTO>>>.PostRequest("api/Search/CreatorSearch", searchCriteriaByCreatorDTO).Result;
                            List<CreatorSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CreatorSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByOutboundInternalNumber:
                        {

                            SearchCriteriaByOutboundInternalDTO searchCriteriaByOutboundInternalDTO = new SearchCriteriaByOutboundInternalDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByOutboundInternalDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            if (advancedSearchVM.OutboundInternalSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.ToDate = advancedSearchVM.OutboundInternalSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundInternalSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.FromDate = advancedSearchVM.OutboundInternalSearch.DateFrom.Value.ToString();

                            }

                            searchCriteriaByOutboundInternalDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundInternalDTO.OrderBy = "";
                            searchCriteriaByOutboundInternalDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByOutboundInternalDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Year != null)
                            {
                                searchCriteriaByOutboundInternalDTO.Year = advancedSearchVM.OutboundInternalSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundInternalDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDate =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.FromDateTime =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundInternalDTO.Global = true;

                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.Number = advancedSearchVM.OutboundInternalSearch.Number.Value;

                            }
                            searchCriteriaByOutboundInternalDTO.TypeId = advancedSearchVM.OutboundInternalSearch.TransactionTypeId;
                            searchCriteriaByOutboundInternalDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.SignedById = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                //searchCriteria.Filters.Add(
                                //    AddFilter(SearchFields.SubjectClassifications, subjectClassifications, FilterType.Equals));
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }
                            searchCriteriaByOutboundInternalDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundInternalDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundInternalSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundInternalSearchResultDTO>>>.PostRequest("api/Search/OutboundInternalSearch", searchCriteriaByOutboundInternalDTO).Result;

                            List<OutboundInternalSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundInternalSearchGridPartial", grid), Type = (TransactionCategory.InternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByOutboundNumber:
                        {

                            SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO = new SearchCriteriaByOutboundDTO();

                            searchCriteriaByOutboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDTO.OrderBy = "";
                            searchCriteriaByOutboundDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByOutboundDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchCriteriaByOutboundDTO.DeliveryMethodId = advancedSearchVM.OutboundSearch.DeliveryMethodId;
                            if (!advancedSearchVM.OutboundSearch.Year.HasValue)
                            {
                                advancedSearchVM.OutboundSearch.DateFrom = null;
                                advancedSearchVM.OutboundSearch.DateTo = null;
                            }

                            if (advancedSearchVM.OutboundSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundDTO.ToDate = advancedSearchVM.OutboundSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundDTO.FromDate = advancedSearchVM.OutboundSearch.DateFrom.Value.ToString();

                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundSearch.Year != null)
                            {
                                searchCriteriaByOutboundDTO.Year = advancedSearchVM.OutboundSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.FromDate =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDTO.FromDateTime =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDTO.Global = true;

                            }

                            if (advancedSearchVM.OutboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDTO.Number = advancedSearchVM.OutboundSearch.Number;
                            }



                            searchCriteriaByOutboundDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.DirectedToId = advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.OutboundSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            searchCriteriaByOutboundDTO.TypeId = advancedSearchVM.OutboundSearch.TransactionTypeId;


                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByOutboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundSearchResultDTO>>>.PostRequest("api/Search/OutboundSearch", searchCriteriaByOutboundDTO).Result;

                            List<OutboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundSearchGridPartial", grid), Type = (TransactionCategory.ExternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundDraftNumber:
                        {
                            SearchCriteriaByOutboundDraftDTO searchCriteriaByOutboundDraftDTO = new SearchCriteriaByOutboundDraftDTO();

                            searchCriteriaByOutboundDraftDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDraftDTO.OrderBy = "";
                            searchCriteriaByOutboundDraftDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByOutboundDraftDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;


                            if (!advancedSearchVM.OutboundDraftSearch.Year.HasValue)
                            {
                                advancedSearchVM.OutboundDraftSearch.DateFrom = null;
                                advancedSearchVM.OutboundDraftSearch.DateTo = null;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.ToDate = advancedSearchVM.OutboundDraftSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundDraftSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.FromDate = advancedSearchVM.OutboundDraftSearch.DateFrom.Value.ToString();
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Year != null)
                            {
                                searchCriteriaByOutboundDraftDTO.Year = advancedSearchVM.OutboundDraftSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDraftDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDate =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.FromDateTime =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDraftDTO.Global = true;

                            }
                            if (advancedSearchVM.OutboundDraftSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.Number = advancedSearchVM.OutboundDraftSearch.Number;
                            }

                            searchCriteriaByOutboundDraftDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.DirectedToId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.TypeId = advancedSearchVM.OutboundDraftSearch.TransactionTypeId;


                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByOutboundDraftDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDraftDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundDraftSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundDraftSearchResultDTO>>>.PostRequest("api/Search/OutboundDraftSearch", searchCriteriaByOutboundDraftDTO).Result;

                            List<OutboundDraftSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundDraftSearchGridPartial", grid), Type = (TransactionCategory.DraftOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchBySubject:
                        {
                            SearchCriteriaBySubjectDTO searchSubjectCriteriaDTO = new SearchCriteriaBySubjectDTO();

                            searchSubjectCriteriaDTO.Subject = advancedSearchVM.SubjectSearch.Subject.Trim();

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchSubjectCriteriaDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectSearch.Year != null)
                            {
                                searchSubjectCriteriaDTO.Year = advancedSearchVM.SubjectSearch.Year;
                            }

                            searchSubjectCriteriaDTO.CultureName = SessionInfo.CultureShortName;
                            searchSubjectCriteriaDTO.OrderBy = "";
                            searchSubjectCriteriaDTO.PageIndex = page ?? 0; ;
                            searchSubjectCriteriaDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchSubjectCriteriaDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectSearch.InboundAdvanced.ConfidentialityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.FromPartyId = advancedSearchVM.SubjectSearch.InboundAdvanced.FromPartyId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.SubjectSearch.InboundAdvanced.LetterTypeId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectSearch.InboundAdvanced.PriorityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.SubjectSearch.InboundAdvanced.SignedByDepartmentId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.SignedById = advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById;
                                        searchSubjectCriteriaDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectSearch.InboundAdvanced.StatusId;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchSubjectCriteriaDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.All)
                                            searchSubjectCriteriaDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchSubjectCriteriaDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectSearch.OutboundAdvanced.ConfidentialityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.SubjectSearch.OutboundAdvanced.CreatedDepartmentId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.SubjectSearch.OutboundAdvanced.DestinationPartyId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.DirectedToId = advancedSearchVM.SubjectSearch.OutboundAdvanced.DirectedToId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectSearch.OutboundAdvanced.PriorityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectSearch.OutboundAdvanced.StatusId;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchSubjectCriteriaDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {


                                searchSubjectCriteriaDTO.UserId = -1;
                                searchSubjectCriteriaDTO.Global = true;
                            }

                            GetResult<List<SubjectSearchResultDTO>> result =
                                                                      HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                            List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByAssignTransaction:
                        {

                            SearchCriteriaByAssignTransactionDTO searchCriteriaByAssignTransactionDTO = new SearchCriteriaByAssignTransactionDTO();
                            if (advancedSearchVM.AssignTransactionSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateTo = advancedSearchVM.AssignTransactionSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignTransactionSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateFrom = advancedSearchVM.AssignTransactionSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignTransactionDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignTransactionDTO.OrderBy = "";
                            searchCriteriaByAssignTransactionDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByAssignTransactionDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.AssignTransactionSearch.Number.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.Number = advancedSearchVM.AssignTransactionSearch.Number.Value;

                            }
                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.AssignTransactionSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignTransactionDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateFrom =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignTransactionDTO.FromDateTime =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignTransactionSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourTo.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            searchCriteriaByAssignTransactionDTO.FromEntity = advancedSearchVM.AssignTransactionSearch.FromEntity;
                            if (advancedSearchVM.AssignTransactionSearch.EntityId.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.EntityId = advancedSearchVM.AssignTransactionSearch.EntityId.Value;
                            }
                            searchCriteriaByAssignTransactionDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignTransactionDTO.HasFullPrivilege = HasPermissionSearch;

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.FromPartyId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.SignedById = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByAssignTransactionDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByAssignTransactionDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.DirectedToId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByAssignTransactionDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByAssignTransactionDTO.Global = true;

                            }
                            GetResult<List<AssignTransactionSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<AssignTransactionSearchResultDTO>>>.PostRequest("api/Search/AssignTransactionSearch", searchCriteriaByAssignTransactionDTO).Result;
                            List<AssignTransactionSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignTransactionSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }

                    case SearchType.SearchByRecordNumber:
                        {
                            SearchCriteriaByRecordNumberDTO searchCriteriaByRecordNumberDTO = new SearchCriteriaByRecordNumberDTO();
                            searchCriteriaByRecordNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByRecordNumberDTO.OrderBy = "";
                            searchCriteriaByRecordNumberDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByRecordNumberDTO.PageSize = GridHelper.PageSize;



                            if (advancedSearchVM.RecordNumberSearch.RecordNumber != null)
                            {
                                searchCriteriaByRecordNumberDTO.RecordNumber = advancedSearchVM.RecordNumberSearch.RecordNumber;
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByRecordNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            searchCriteriaByRecordNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByRecordNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            searchCriteriaByRecordNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;



                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByRecordNumberDTO.Global = true;

                            }
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/RecordNumberSearch", searchCriteriaByRecordNumberDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByNames:
                        {
                            SearchCriteriaByNamesDTO searchCriteriaByNamesDTO = new SearchCriteriaByNamesDTO();
                            if (advancedSearchVM.NamesSearch.DateTo.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateTo = advancedSearchVM.NamesSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.NamesSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateFrom = advancedSearchVM.NamesSearch.DateFrom.Value;

                            }

                            searchCriteriaByNamesDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByNamesDTO.OrderBy = "";
                            searchCriteriaByNamesDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByNamesDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByNamesDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.NamesSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.NamesSearch.HourFrom.Value,
                                    (advancedSearchVM.NamesSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.NamesSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByNamesDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateFrom =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByNamesDTO.FromDateTime =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.NamesSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.NamesSearch.HourTo.Value,
                                    (advancedSearchVM.NamesSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.NamesSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByNamesDTO.ToDateTime =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByNamesDTO.ToDateTime =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.ToDateTime = dateValue;
                                }
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByNamesDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.NamesSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.FromPartyId = advancedSearchVM.NamesSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.NamesSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.PriorityId = advancedSearchVM.NamesSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.NamesSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.SignedById = advancedSearchVM.NamesSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.StatusId = advancedSearchVM.NamesSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByNamesDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByNamesDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByNamesDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.NamesSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.NamesSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.NamesSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.DirectedToId = advancedSearchVM.NamesSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.PriorityId = advancedSearchVM.NamesSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.StatusId = advancedSearchVM.NamesSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByNamesDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }




                            searchCriteriaByNamesDTO.FirstName = advancedSearchVM.NamesSearch.FirstName;
                            searchCriteriaByNamesDTO.SecondName = advancedSearchVM.NamesSearch.SecondName;
                            searchCriteriaByNamesDTO.ThirdName = advancedSearchVM.NamesSearch.ThirdName;
                            searchCriteriaByNamesDTO.SearchTypeForFiltersId = advancedSearchVM.NamesSearch.SearchNamesType;
                            searchCriteriaByNamesDTO.FamilyName = advancedSearchVM.NamesSearch.FamilyName;
                            searchCriteriaByNamesDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByNamesDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<NamesSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<NamesSearchResultDTO>>>.PostRequest("api/Search/NamesSearch", searchCriteriaByNamesDTO).Result;

                            List<SearchCriteriaByNamesResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);



                        }
                    case SearchType.SearchDaily:
                        {
                            SearchCriteriaByDailyDTO searchCriteriaByDailyDTO = new SearchCriteriaByDailyDTO();

                            searchCriteriaByDailyDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDailyDTO.OrderBy = "";
                            searchCriteriaByDailyDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByDailyDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDailyDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByDailyDTO.TodayDate = DateTime.Now;

                            searchCriteriaByDailyDTO.UserId = SessionInfo.CurrentUser.Id;


                            GetResult<List<DailySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<DailySearchResultDTO>>>.PostRequest("api/Search/DailySearch", searchCriteriaByDailyDTO).Result;

                            List<SearchCriteriaByDailyResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_DailySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);


                        }
                    case SearchType.SearchByAssignmentNote:
                        {

                            SearchCriteriaByAssignmentNoteDTO searchCriteriaByAssignmentNoteDTO = new SearchCriteriaByAssignmentNoteDTO();

                            if (advancedSearchVM.AssignmentNoteSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateTo = advancedSearchVM.AssignmentNoteSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignmentNoteSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateFrom = advancedSearchVM.AssignmentNoteSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignmentNoteDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignmentNoteDTO.OrderBy = "";
                            searchCriteriaByAssignmentNoteDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByAssignmentNoteDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.AssignmentNoteSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignmentNoteDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateFrom =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignmentNoteDTO.FromDateTime =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignmentNoteSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourTo.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime = dateValue;
                                }
                            }
                            searchCriteriaByAssignmentNoteDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.FromPartyId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.SignedById = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            TransactionCategory transactionCategory = TransactionCategory.Inbound;


                            searchCriteriaByAssignmentNoteDTO.AssignmentNote = advancedSearchVM.AssignmentNoteSearch.AssignmentNote;

                            searchCriteriaByAssignmentNoteDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignmentNoteDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<AssignmentNoteSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<AssignmentNoteSearchResultDTO>>>.PostRequest("api/Search/AssignmentNoteSearch", searchCriteriaByAssignmentNoteDTO).Result;

                            List<SearchCriteriaByAssignmentNoteResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentNoteSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);






                        }
                    case SearchType.SearchByManifestNumber:
                        {
                            SearchCriteriaByManifestNumberDTO searchCriteriaByManifestNumberDTO = new SearchCriteriaByManifestNumberDTO();
                            if (advancedSearchVM.ManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateTo = advancedSearchVM.ManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateFrom = advancedSearchVM.ManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByManifestNumberDTO.OrderBy = "";
                            searchCriteriaByManifestNumberDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByManifestNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateFrom =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByManifestNumberDTO.FromDateTime =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByManifestNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            searchCriteriaByManifestNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByManifestNumberDTO.ManifestNumber = advancedSearchVM.ManifestNumberSearch.ManifestNumber;
                            searchCriteriaByManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ManifestNumberSearch", searchCriteriaByManifestNumberDTO).Result;

                            List<SearchCriteriaByManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByMilitaryNumberOrIdentity:
                        {
                            SearchCriteriaByMilitaryNumberOrIdentityDTO searchCriteriaByMilitaryNumberOrIdentityDTO = new SearchCriteriaByMilitaryNumberOrIdentityDTO();

                            if (advancedSearchVM.IdentificationNumber.DateTo.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo = advancedSearchVM.IdentificationNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.IdentificationNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom = advancedSearchVM.IdentificationNumber.DateFrom.Value;

                            }

                            searchCriteriaByMilitaryNumberOrIdentityDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.OrderBy = "";
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.IdentificationNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourFrom.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.IdentificationNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourTo.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.IdentificationNumber.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.FromPartyId = advancedSearchVM.IdentificationNumber.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.IdentificationNumber.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.PriorityId = advancedSearchVM.IdentificationNumber.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.SignedById = advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.StatusId = advancedSearchVM.IdentificationNumber.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.DirectedToId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.PriorityId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.StatusId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            searchCriteriaByMilitaryNumberOrIdentityDTO.IdentificationNumber = advancedSearchVM.IdentificationNumber.IdentificationNumber;

                            searchCriteriaByMilitaryNumberOrIdentityDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>>.PostRequest("api/Search/MilitaryNumberOrIdentitySearch", searchCriteriaByMilitaryNumberOrIdentityDTO).Result;

                            List<SearchCriteriaByMilitaryNumberOrIdentityResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MilitaryNumberOrIdentitySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }
                    case SearchType.SearchByTransactionNots:
                        {
                            SearchCriteriaByTransactionNotsDTO searchCriteriaByTransactionNotsDTO = new SearchCriteriaByTransactionNotsDTO();

                            if (advancedSearchVM.TransactionNotsSearch.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateTo = advancedSearchVM.TransactionNotsSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNotsSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateFrom = advancedSearchVM.TransactionNotsSearch.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNotsDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNotsDTO.OrderBy = "";
                            searchCriteriaByTransactionNotsDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByTransactionNotsDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNotsSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourFrom.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNotsDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateFrom =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNotsDTO.FromDateTime =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNotsSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourTo.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.FromPartyId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.SignedById = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByTransactionNotsDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.DirectedToId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }
                            searchCriteriaByTransactionNotsDTO.TransactionNots = advancedSearchVM.TransactionNotsSearch.TransactionNots;
                            searchCriteriaByTransactionNotsDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNotsDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNotsSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNotsSearchResultDTO>>>.PostRequest("api/Search/TransactionNotsSearch", searchCriteriaByTransactionNotsDTO).Result;

                            List<SearchCriteriaByTransactionNotsResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNotsSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByELcEmployee:
                        {

                            SearchCriteriaByElcEmployeeDTO searchCriteriaByElcEmployeeDTO = new SearchCriteriaByElcEmployeeDTO();

                            if (advancedSearchVM.ElcEmployeeSearch.DateTo.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateTo = advancedSearchVM.ElcEmployeeSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ElcEmployeeSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateFrom = advancedSearchVM.ElcEmployeeSearch.DateFrom.Value;

                            }

                            searchCriteriaByElcEmployeeDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByElcEmployeeDTO.OrderBy = "";
                            searchCriteriaByElcEmployeeDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByElcEmployeeDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ElcEmployeeSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourFrom.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByElcEmployeeDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateFrom =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByElcEmployeeDTO.FromDateTime =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ElcEmployeeSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourTo.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.PriorityId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.SignedById = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.StatusId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByElcEmployeeDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByElcEmployeeDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.DirectedToId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.PriorityId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.StatusId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByElcEmployeeDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }

                            searchCriteriaByElcEmployeeDTO.ElcEmployeeId = advancedSearchVM.ElcEmployeeSearch.ElcEmployeeId;

                            searchCriteriaByElcEmployeeDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByElcEmployeeDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ELcEmployeeSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ELcEmployeeSearchResultDTO>>>.PostRequest("api/Search/ELcEmployeeSearch", searchCriteriaByElcEmployeeDTO).Result;

                            List<SearchCriteriaByElcEmployeeResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ElcEmployeeSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);




                        }
                    case SearchType.SearchByExternalOutBoundOrManifestNumber:
                        {


                            SearchCriteriaByExternalOutBoundOrManifestNumberDTO searchCriteriaByExternalOutBoundOrManifestNumberDTO = new SearchCriteriaByExternalOutBoundOrManifestNumberDTO();

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrderBy = "";
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Year != null)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.Year = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Year;
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            transactionCategory = TransactionCategory.ExternalOutbound;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.DirectedToId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.Number = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Number;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ExternalOutBoundOrManifestNumberSearch", searchCriteriaByExternalOutBoundOrManifestNumberDTO).Result;

                            List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalOutBoundOrManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByCopyAssignemnt:
                        {
                            SearchCriteriaByCopyAssignemntDTO searchCriteriaByCopyAssignemntDTO = new SearchCriteriaByCopyAssignemntDTO();
                            if (advancedSearchVM.CopyAssignemntSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateTo = advancedSearchVM.CopyAssignemntSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CopyAssignemntSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateFrom = advancedSearchVM.CopyAssignemntSearch.DateFrom.Value;

                            }

                            searchCriteriaByCopyAssignemntDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCopyAssignemntDTO.OrderBy = "";
                            searchCriteriaByCopyAssignemntDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByCopyAssignemntDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.CopyAssignemntSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourFrom.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCopyAssignemntDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateFrom =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCopyAssignemntDTO.FromDateTime =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.CopyAssignemntSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourTo.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;


                            searchCriteriaByCopyAssignemntDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.FromPartyId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.SignedById = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.StatusId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.PriorityId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByCopyAssignemntDTO.FromEntityId = advancedSearchVM.CopyAssignemntSearch.FromEntityId;
                            searchCriteriaByCopyAssignemntDTO.ToEntityId = advancedSearchVM.CopyAssignemntSearch.ToEntityId;
                            searchCriteriaByCopyAssignemntDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCopyAssignemntDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<CopyAssignemntSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<CopyAssignemntSearchResultDTO>>>.PostRequest("api/Search/CopyAssignemntSearch", searchCriteriaByCopyAssignemntDTO).Result;

                            List<SearchCriteriaByCopyAssignemntResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopyAssignemntSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchBySubjectLetter:
                        {
                            SearchCriteriaBySubjectLetterDTO searchCriteriaBySubjectLetterDTO = new SearchCriteriaBySubjectLetterDTO();
                            if (advancedSearchVM.SubjectLetterSearch.DateTo.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateTo = advancedSearchVM.SubjectLetterSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.SubjectLetterSearch.DateFrom.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateFrom = advancedSearchVM.SubjectLetterSearch.DateFrom.Value;

                            }

                            searchCriteriaBySubjectLetterDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaBySubjectLetterDTO.OrderBy = "";
                            searchCriteriaBySubjectLetterDTO.PageIndex = page ?? 0;
                            searchCriteriaBySubjectLetterDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectLetterSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourFrom.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaBySubjectLetterDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateFrom =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaBySubjectLetterDTO.FromDateTime =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.SubjectLetterSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourTo.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.FromPartyId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.SignedById = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaBySubjectLetterDTO.TransactionCategoryId = (int)TransactionCategory.All;


                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.DirectedToId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }
                            searchCriteriaBySubjectLetterDTO.FirstLetter = advancedSearchVM.SubjectLetterSearch.FirstLetter;
                            searchCriteriaBySubjectLetterDTO.SecondLetter = advancedSearchVM.SubjectLetterSearch.SecondLetter;
                            searchCriteriaBySubjectLetterDTO.ThirdLetter = advancedSearchVM.SubjectLetterSearch.ThirdLetter;
                            searchCriteriaBySubjectLetterDTO.FourthLetter = advancedSearchVM.SubjectLetterSearch.FourthLetter;
                            searchCriteriaBySubjectLetterDTO.SearchTypeForFiltersId = advancedSearchVM.SubjectLetterSearch.SearchTypeForFiltersId;

                            searchCriteriaBySubjectLetterDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaBySubjectLetterDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<SubjectLetterSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<SubjectLetterSearchResultDTO>>>.PostRequest("api/Search/SubjectLetterSearch", searchCriteriaBySubjectLetterDTO).Result;

                            List<SearchCriteriaBySubjectLetterResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectLetterSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);



                        }
                    case SearchType.SearchByTransactionNumber:
                        {
                            SearchCriteriaByTransactionNumberDTO searchCriteriaByTransactionNumberDTO = new SearchCriteriaByTransactionNumberDTO();

                            if (advancedSearchVM.TransactionNumber.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateTo = advancedSearchVM.TransactionNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateFrom = advancedSearchVM.TransactionNumber.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNumberDTO.OrderBy = "";
                            searchCriteriaByTransactionNumberDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByTransactionNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourFrom.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateFrom =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNumberDTO.FromDateTime =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourTo.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNumber.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.TransactionNumber.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.TransactionNumber.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNumber.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.TransactionNumber.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.TransactionNumber.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNumber.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByTransactionNumberDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNumber.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.TransactionNumber.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.TransactionNumber.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.DirectedToId = advancedSearchVM.TransactionNumber.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNumber.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNumber.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            searchCriteriaByTransactionNumberDTO.TransactionNumber = advancedSearchVM.TransactionNumber.TransactionNumber;

                            searchCriteriaByTransactionNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNumberSearchResultDTO>>>.PostRequest("api/Search/TransactionNumberSearch", searchCriteriaByTransactionNumberDTO).Result;

                            List<SearchCriteriaByTransactionNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }
                    case SearchType.SearchByExternalPartyCopies:
                        {
                            SearchCriteriaByExternalPartyCopiesDTO searchCriteriaByExternalPartyCopiesDTO = new SearchCriteriaByExternalPartyCopiesDTO();


                            searchCriteriaByExternalPartyCopiesDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByExternalPartyCopiesDTO.OrderBy = "";
                            searchCriteriaByExternalPartyCopiesDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByExternalPartyCopiesDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            searchCriteriaByExternalPartyCopiesDTO.ExternalPartyId = advancedSearchVM.ExternalPartyCopies.ExternalPartyId;


                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByExternalPartyCopiesDTO.UserId = SessionInfo.CurrentUser.Id;

                            GetResult<List<ExternalPartyCopiesSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ExternalPartyCopiesSearchResultDTO>>>.PostRequest("api/Search/ExternalPartyCopiesSearch", searchCriteriaByExternalPartyCopiesDTO).Result;

                            List<SearchCriteriaByExternalPartyCopiesResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalPartyCopiesSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SearchPaging(AdvancedSearchVM advancedSearchVM, int? page)
        {
            try
            {
                string message = string.Empty;
                SearchCriteria searchCriteria = new SearchCriteria();

                searchCriteria.Filters = new List<MCS.Framework.Persistence.Filter>();

                if (advancedSearchVM.OrgUnitId.HasValue)
                {
                    searchCriteria.Filters.Add(
                        AddFilter(SearchFields.OrgUnitId, advancedSearchVM.OrgUnitId.Value.ToString(), FilterType.Equals));
                }

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);


                switch ((SearchType)advancedSearchVM.SearchTypeId)
                {
                    case SearchType.SearchByInboundNumber:
                        {

                            SearchCriteriaByInboundDTO searchCriteriaByInboundDTO = new SearchCriteriaByInboundDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByInboundDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            if (!advancedSearchVM.InboundSearch.Year.HasValue)
                            {
                                advancedSearchVM.InboundSearch.DateFrom = null;
                                advancedSearchVM.InboundSearch.DateTo = null;
                            }

                            if (advancedSearchVM.InboundSearch.DateTo.HasValue)
                            {
                                searchCriteriaByInboundDTO.ToDate = advancedSearchVM.InboundSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.InboundSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByInboundDTO.FromDate = advancedSearchVM.InboundSearch.DateFrom.Value;

                            }
                            searchCriteriaByInboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByInboundDTO.OrderBy = "";
                            searchCriteriaByInboundDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByInboundDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchCriteriaByInboundDTO.DeliveryMethodId = advancedSearchVM.InboundSearch.DeliveryMethodId;
                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByInboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.InboundSearch.Year != null)
                            {
                                searchCriteriaByInboundDTO.Year = advancedSearchVM.InboundSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByInboundDTO.FromDate.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.FromDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByInboundDTO.ToDate.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.ToDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.InboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.InboundSearch.HourFrom.Value,
                                    (advancedSearchVM.InboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByInboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.FromDate =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByInboundDTO.FromDateTime =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByInboundDTO.Global = true;

                            }

                            if (advancedSearchVM.InboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.InboundSearch.HourTo.Value,
                                    (advancedSearchVM.InboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByInboundDTO.ToDateTime =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByInboundDTO.ToDateTime =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.InboundSearch.Number.HasValue)
                            {
                                searchCriteriaByInboundDTO.Number = advancedSearchVM.InboundSearch.Number.Value;

                            }

                            searchCriteriaByInboundDTO.TransactionTypeId = advancedSearchVM.InboundSearch.TransactionTypeId;
                            searchCriteriaByInboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.InboundSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.SignedById = advancedSearchVM.InboundSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.StatusId = advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByInboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            if (advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByInboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByInboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByInboundDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<InboundSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/InboundSearch", searchCriteriaByInboundDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);


                        }

                    case SearchType.SearchByDocumentNumber:
                        {
                            SearchCriteriaByDocumentNumberDTO searchCriteriaByDocumentNumberDTO = new SearchCriteriaByDocumentNumberDTO();
                            searchCriteriaByDocumentNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDocumentNumberDTO.OrderBy = "";
                            searchCriteriaByDocumentNumberDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByDocumentNumberDTO.PageSize = GridHelper.PageSize;

                            if (advancedSearchVM.DocumentNumberSearch.Year != null)
                            {
                                searchCriteriaByDocumentNumberDTO.Year = advancedSearchVM.DocumentNumberSearch.Year;
                            }

                            if (advancedSearchVM.DocumentNumberSearch.DocumentNumber != string.Empty)
                            {
                                searchCriteriaByDocumentNumberDTO.DocumentNumber = advancedSearchVM.DocumentNumberSearch.DocumentNumber;
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDocumentNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            searchCriteriaByDocumentNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByDocumentNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.DocumentNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.DocumentNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByDocumentNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByDocumentNumberDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/DocumentNumberSearch", searchCriteriaByDocumentNumberDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }


                    case SearchType.SearchByEntity:
                        {

                            SearchCriteriaByEntityNameDTO searchCriteriaByEntityNameDTO = new SearchCriteriaByEntityNameDTO();
                            if (advancedSearchVM.EntitySearch.DateTo.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateTo = advancedSearchVM.EntitySearch.DateTo.Value;

                            }
                            if (advancedSearchVM.EntitySearch.DateFrom.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateFrom = advancedSearchVM.EntitySearch.DateFrom.Value;

                            }

                            if (advancedSearchVM.EntitySearch.Number.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.Number = advancedSearchVM.EntitySearch.Number.Value;

                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByEntityNameDTO.Global = true;
                            }


                            searchCriteriaByEntityNameDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByEntityNameDTO.OrderBy = "";
                            searchCriteriaByEntityNameDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByEntityNameDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.EntitySearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.EntitySearch.HourFrom.Value,
                                    (advancedSearchVM.EntitySearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByEntityNameDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateFrom =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByEntityNameDTO.FromDateTime =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByEntityNameDTO.Global = true;
                            }
                            if (advancedSearchVM.EntitySearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.EntitySearch.HourTo.Value,
                                    (advancedSearchVM.EntitySearch.MinuteTo.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            searchCriteriaByEntityNameDTO.ExternalPartyId = advancedSearchVM.EntitySearch.ExternalPartyId;



                            searchCriteriaByEntityNameDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByEntityNameDTO.AdvancedSearch.FromPartyId = advancedSearchVM.EntitySearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.DirectedToId = advancedSearchVM.EntitySearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.DirectedToId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.EntitySearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.StatusId = advancedSearchVM.EntitySearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.StatusId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.PriorityId = advancedSearchVM.EntitySearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.PriorityId.Value : -1;
                            searchCriteriaByEntityNameDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.EntitySearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.EntitySearch.OutboundAdvanced.ConfidentialityId.Value : -1;


                            searchCriteriaByEntityNameDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByEntityNameDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<EntitySearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<EntitySearchResultDTO>>>.PostRequest("api/Search/EntitySearch", searchCriteriaByEntityNameDTO).Result;
                            List<EntitySearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            if (searchResultVMs[0].TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EntitySearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByCreator:
                        {

                            SearchCriteriaByCreatorDTO searchCriteriaByCreatorDTO = new SearchCriteriaByCreatorDTO();
                            if (advancedSearchVM.CreatorSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateTo = advancedSearchVM.CreatorSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CreatorSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateFrom = advancedSearchVM.CreatorSearch.DateFrom.Value;

                            }

                            searchCriteriaByCreatorDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCreatorDTO.OrderBy = "";
                            searchCriteriaByCreatorDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByCreatorDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCreatorDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.CreatorSearch.Number.HasValue)
                            {
                                searchCriteriaByCreatorDTO.Number = advancedSearchVM.CreatorSearch.Number.Value;

                            }
                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.CreatorSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourFrom.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCreatorDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateFrom =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCreatorDTO.FromDateTime =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByCreatorDTO.Global = true;
                            }

                            if (advancedSearchVM.CreatorSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourTo.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByCreatorDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CreatorSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.FromPartyId = advancedSearchVM.CreatorSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.CreatorSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.PriorityId = advancedSearchVM.CreatorSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.CreatorSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.SignedById = advancedSearchVM.CreatorSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.StatusId = advancedSearchVM.CreatorSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.CreatorSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByCreatorDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByCreatorDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByCreatorDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CreatorSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.CreatorSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.CreatorSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.DirectedToId = advancedSearchVM.CreatorSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.PriorityId = advancedSearchVM.CreatorSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByCreatorDTO.AdvancedSearch.StatusId = advancedSearchVM.CreatorSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.CreatorSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.CreatorSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByCreatorDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }

                            searchCriteriaByCreatorDTO.CreatorUserId = advancedSearchVM.CreatorSearch.UserId;
                            searchCriteriaByCreatorDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCreatorDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<CreatorSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<CreatorSearchResultDTO>>>.PostRequest("api/Search/CreatorSearch", searchCriteriaByCreatorDTO).Result;
                            List<CreatorSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CreatorSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByOutboundInternalNumber:
                        {

                            SearchCriteriaByOutboundInternalDTO searchCriteriaByOutboundInternalDTO = new SearchCriteriaByOutboundInternalDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByOutboundInternalDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            if (advancedSearchVM.OutboundInternalSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.ToDate = advancedSearchVM.OutboundInternalSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundInternalSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.FromDate = advancedSearchVM.OutboundInternalSearch.DateFrom.Value.ToString();

                            }

                            searchCriteriaByOutboundInternalDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundInternalDTO.OrderBy = "";
                            searchCriteriaByOutboundInternalDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByOutboundInternalDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Year != null)
                            {
                                searchCriteriaByOutboundInternalDTO.Year = advancedSearchVM.OutboundInternalSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundInternalDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDate =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.FromDateTime =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundInternalDTO.Global = true;

                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.Number = advancedSearchVM.OutboundInternalSearch.Number.Value;

                            }
                            searchCriteriaByOutboundInternalDTO.TypeId = advancedSearchVM.OutboundInternalSearch.TransactionTypeId;
                            searchCriteriaByOutboundInternalDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.SignedById = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundInternalDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                //searchCriteria.Filters.Add(
                                //    AddFilter(SearchFields.SubjectClassifications, subjectClassifications, FilterType.Equals));
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }
                            searchCriteriaByOutboundInternalDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundInternalDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundInternalSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundInternalSearchResultDTO>>>.PostRequest("api/Search/OutboundInternalSearch", searchCriteriaByOutboundInternalDTO).Result;

                            List<OutboundInternalSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundInternalSearchGridPartial", grid), Type = (TransactionCategory.InternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByOutboundNumber:
                        {

                            SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO = new SearchCriteriaByOutboundDTO();

                            searchCriteriaByOutboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDTO.OrderBy = "";
                            searchCriteriaByOutboundDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByOutboundDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchCriteriaByOutboundDTO.DeliveryMethodId = advancedSearchVM.OutboundSearch.DeliveryMethodId;
                            if (!advancedSearchVM.OutboundSearch.Year.HasValue)
                            {
                                advancedSearchVM.OutboundSearch.DateFrom = null;
                                advancedSearchVM.OutboundSearch.DateTo = null;
                            }

                            if (advancedSearchVM.OutboundSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundDTO.ToDate = advancedSearchVM.OutboundSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundDTO.FromDate = advancedSearchVM.OutboundSearch.DateFrom.Value.ToString();

                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundSearch.Year != null)
                            {
                                searchCriteriaByOutboundDTO.Year = advancedSearchVM.OutboundSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.FromDate =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDTO.FromDateTime =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDTO.Global = true;

                            }

                            if (advancedSearchVM.OutboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDTO.Number = advancedSearchVM.OutboundSearch.Number;
                            }



                            searchCriteriaByOutboundDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.DirectedToId = advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.OutboundSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            searchCriteriaByOutboundDTO.TypeId = advancedSearchVM.OutboundSearch.TransactionTypeId;


                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByOutboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundSearchResultDTO>>>.PostRequest("api/Search/OutboundSearch", searchCriteriaByOutboundDTO).Result;

                            List<OutboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundSearchGridPartial", grid), Type = (TransactionCategory.ExternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundDraftNumber:
                        {
                            SearchCriteriaByOutboundDraftDTO searchCriteriaByOutboundDraftDTO = new SearchCriteriaByOutboundDraftDTO();

                            searchCriteriaByOutboundDraftDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDraftDTO.OrderBy = "";
                            searchCriteriaByOutboundDraftDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByOutboundDraftDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;


                            if (!advancedSearchVM.OutboundDraftSearch.Year.HasValue)
                            {
                                advancedSearchVM.OutboundDraftSearch.DateFrom = null;
                                advancedSearchVM.OutboundDraftSearch.DateTo = null;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.DateTo.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.ToDate = advancedSearchVM.OutboundDraftSearch.DateTo.Value.ToString();

                            }
                            if (advancedSearchVM.OutboundDraftSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.FromDate = advancedSearchVM.OutboundDraftSearch.DateFrom.Value.ToString();
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Year != null)
                            {
                                searchCriteriaByOutboundDraftDTO.Year = advancedSearchVM.OutboundDraftSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDraftDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDate =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.FromDateTime =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDraftDTO.Global = true;

                            }
                            if (advancedSearchVM.OutboundDraftSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.Number = advancedSearchVM.OutboundDraftSearch.Number;
                            }

                            searchCriteriaByOutboundDraftDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.DirectedToId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            searchCriteriaByOutboundDraftDTO.TypeId = advancedSearchVM.OutboundDraftSearch.TransactionTypeId;


                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByOutboundDraftDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDraftDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundDraftSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundDraftSearchResultDTO>>>.PostRequest("api/Search/OutboundDraftSearch", searchCriteriaByOutboundDraftDTO).Result;

                            List<OutboundDraftSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundDraftSearchGridPartial", grid), Type = (TransactionCategory.DraftOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchBySubject:
                        {
                            SearchCriteriaBySubjectDTO searchSubjectCriteriaDTO = new SearchCriteriaBySubjectDTO();

                            searchSubjectCriteriaDTO.Subject = advancedSearchVM.SubjectSearch.Subject.Trim();

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchSubjectCriteriaDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectSearch.Year != null)
                            {
                                searchSubjectCriteriaDTO.Year = advancedSearchVM.SubjectSearch.Year;
                            }

                            searchSubjectCriteriaDTO.CultureName = SessionInfo.CultureShortName;
                            searchSubjectCriteriaDTO.OrderBy = "";
                            searchSubjectCriteriaDTO.PageIndex = page - 1 ?? 0; ;
                            searchSubjectCriteriaDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                            searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchSubjectCriteriaDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectSearch.InboundAdvanced.ConfidentialityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.FromPartyId = advancedSearchVM.SubjectSearch.InboundAdvanced.FromPartyId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.SubjectSearch.InboundAdvanced.LetterTypeId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectSearch.InboundAdvanced.PriorityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.SubjectSearch.InboundAdvanced.SignedByDepartmentId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.SignedById = advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById;
                                        searchSubjectCriteriaDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectSearch.InboundAdvanced.StatusId;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchSubjectCriteriaDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.All)
                                            searchSubjectCriteriaDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchSubjectCriteriaDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectSearch.OutboundAdvanced.ConfidentialityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.SubjectSearch.OutboundAdvanced.CreatedDepartmentId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.SubjectSearch.OutboundAdvanced.DestinationPartyId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.DirectedToId = advancedSearchVM.SubjectSearch.OutboundAdvanced.DirectedToId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectSearch.OutboundAdvanced.PriorityId;
                                        searchSubjectCriteriaDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectSearch.OutboundAdvanced.StatusId;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchSubjectCriteriaDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {


                                searchSubjectCriteriaDTO.UserId = -1;
                                searchSubjectCriteriaDTO.Global = true;
                            }

                            GetResult<List<SubjectSearchResultDTO>> result =
                                                                      HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                            List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByAssignTransaction:
                        {

                            SearchCriteriaByAssignTransactionDTO searchCriteriaByAssignTransactionDTO = new SearchCriteriaByAssignTransactionDTO();
                            if (advancedSearchVM.AssignTransactionSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateTo = advancedSearchVM.AssignTransactionSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignTransactionSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateFrom = advancedSearchVM.AssignTransactionSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignTransactionDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignTransactionDTO.OrderBy = "";
                            searchCriteriaByAssignTransactionDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByAssignTransactionDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.AssignTransactionSearch.Number.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.Number = advancedSearchVM.AssignTransactionSearch.Number.Value;

                            }
                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.AssignTransactionSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignTransactionDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateFrom =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignTransactionDTO.FromDateTime =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignTransactionSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourTo.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            searchCriteriaByAssignTransactionDTO.FromEntity = advancedSearchVM.AssignTransactionSearch.FromEntity;
                            if (advancedSearchVM.AssignTransactionSearch.EntityId.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.EntityId = advancedSearchVM.AssignTransactionSearch.EntityId.Value;
                            }
                            searchCriteriaByAssignTransactionDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignTransactionDTO.HasFullPrivilege = HasPermissionSearch;

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.FromPartyId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.SignedById = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignTransactionSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.AssignTransactionSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByAssignTransactionDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByAssignTransactionDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.DirectedToId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByAssignTransactionDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.AssignTransactionSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.AssignTransactionSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByAssignTransactionDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByAssignTransactionDTO.Global = true;

                            }
                            GetResult<List<AssignTransactionSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<AssignTransactionSearchResultDTO>>>.PostRequest("api/Search/AssignTransactionSearch", searchCriteriaByAssignTransactionDTO).Result;
                            List<AssignTransactionSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignTransactionSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }

                    case SearchType.SearchByRecordNumber:
                        {
                            SearchCriteriaByRecordNumberDTO searchCriteriaByRecordNumberDTO = new SearchCriteriaByRecordNumberDTO();
                            searchCriteriaByRecordNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByRecordNumberDTO.OrderBy = "";
                            searchCriteriaByRecordNumberDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByRecordNumberDTO.PageSize = GridHelper.PageSize;



                            if (advancedSearchVM.RecordNumberSearch.RecordNumber != null)
                            {
                                searchCriteriaByRecordNumberDTO.RecordNumber = advancedSearchVM.RecordNumberSearch.RecordNumber;
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByRecordNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            searchCriteriaByRecordNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByRecordNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            searchCriteriaByRecordNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.RecordNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.SubjectSearch.InboundAdvanced.SignedById.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByRecordNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.RecordNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.RecordNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;



                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByRecordNumberDTO.Global = true;

                            }
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/RecordNumberSearch", searchCriteriaByRecordNumberDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByNames:
                        {
                            SearchCriteriaByNamesDTO searchCriteriaByNamesDTO = new SearchCriteriaByNamesDTO();
                            if (advancedSearchVM.NamesSearch.DateTo.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateTo = advancedSearchVM.NamesSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.NamesSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateFrom = advancedSearchVM.NamesSearch.DateFrom.Value;

                            }

                            searchCriteriaByNamesDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByNamesDTO.OrderBy = "";
                            searchCriteriaByNamesDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByNamesDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByNamesDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.NamesSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.NamesSearch.HourFrom.Value,
                                    (advancedSearchVM.NamesSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.NamesSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByNamesDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateFrom =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByNamesDTO.FromDateTime =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.NamesSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.NamesSearch.HourTo.Value,
                                    (advancedSearchVM.NamesSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.NamesSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByNamesDTO.ToDateTime =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByNamesDTO.ToDateTime =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.ToDateTime = dateValue;
                                }
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByNamesDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.NamesSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.FromPartyId = advancedSearchVM.NamesSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.NamesSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.PriorityId = advancedSearchVM.NamesSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.NamesSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.SignedById = advancedSearchVM.NamesSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.StatusId = advancedSearchVM.NamesSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.NamesSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByNamesDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByNamesDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByNamesDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.NamesSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.NamesSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.NamesSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.DirectedToId = advancedSearchVM.NamesSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.PriorityId = advancedSearchVM.NamesSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByNamesDTO.AdvancedSearch.StatusId = advancedSearchVM.NamesSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.NamesSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.NamesSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByNamesDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }




                            searchCriteriaByNamesDTO.FirstName = advancedSearchVM.NamesSearch.FirstName;
                            searchCriteriaByNamesDTO.SecondName = advancedSearchVM.NamesSearch.SecondName;
                            searchCriteriaByNamesDTO.ThirdName = advancedSearchVM.NamesSearch.ThirdName;
                            searchCriteriaByNamesDTO.SearchTypeForFiltersId = advancedSearchVM.NamesSearch.SearchNamesType;
                            searchCriteriaByNamesDTO.FamilyName = advancedSearchVM.NamesSearch.FamilyName;
                            searchCriteriaByNamesDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByNamesDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<NamesSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<NamesSearchResultDTO>>>.PostRequest("api/Search/NamesSearch", searchCriteriaByNamesDTO).Result;

                            List<SearchCriteriaByNamesResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);



                        }
                    case SearchType.SearchDaily:
                        {
                            SearchCriteriaByDailyDTO searchCriteriaByDailyDTO = new SearchCriteriaByDailyDTO();

                            searchCriteriaByDailyDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDailyDTO.OrderBy = "";
                            searchCriteriaByDailyDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByDailyDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDailyDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByDailyDTO.TodayDate = DateTime.Now;

                            searchCriteriaByDailyDTO.UserId = SessionInfo.CurrentUser.Id;


                            GetResult<List<DailySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<DailySearchResultDTO>>>.PostRequest("api/Search/DailySearch", searchCriteriaByDailyDTO).Result;

                            List<SearchCriteriaByDailyResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_DailySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);


                        }
                    case SearchType.SearchByAssignmentNote:
                        {

                            SearchCriteriaByAssignmentNoteDTO searchCriteriaByAssignmentNoteDTO = new SearchCriteriaByAssignmentNoteDTO();

                            if (advancedSearchVM.AssignmentNoteSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateTo = advancedSearchVM.AssignmentNoteSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignmentNoteSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateFrom = advancedSearchVM.AssignmentNoteSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignmentNoteDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignmentNoteDTO.OrderBy = "";
                            searchCriteriaByAssignmentNoteDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByAssignmentNoteDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.AssignmentNoteSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignmentNoteDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateFrom =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignmentNoteDTO.FromDateTime =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignmentNoteSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourTo.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime = dateValue;
                                }
                            }
                            searchCriteriaByAssignmentNoteDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.FromPartyId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.SignedById = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.StatusId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.PriorityId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByAssignmentNoteDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.AssignmentNoteSearch.AdvancedSearch.ConfidentialityId.Value : -1;
                            TransactionCategory transactionCategory = TransactionCategory.Inbound;


                            searchCriteriaByAssignmentNoteDTO.AssignmentNote = advancedSearchVM.AssignmentNoteSearch.AssignmentNote;

                            searchCriteriaByAssignmentNoteDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignmentNoteDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<AssignmentNoteSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<AssignmentNoteSearchResultDTO>>>.PostRequest("api/Search/AssignmentNoteSearch", searchCriteriaByAssignmentNoteDTO).Result;

                            List<SearchCriteriaByAssignmentNoteResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentNoteSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);






                        }
                    case SearchType.SearchByManifestNumber:
                        {
                            SearchCriteriaByManifestNumberDTO searchCriteriaByManifestNumberDTO = new SearchCriteriaByManifestNumberDTO();
                            if (advancedSearchVM.ManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateTo = advancedSearchVM.ManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateFrom = advancedSearchVM.ManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByManifestNumberDTO.OrderBy = "";
                            searchCriteriaByManifestNumberDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByManifestNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateFrom =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByManifestNumberDTO.FromDateTime =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByManifestNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                            searchCriteriaByManifestNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByManifestNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ManifestNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.ManifestNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByManifestNumberDTO.ManifestNumber = advancedSearchVM.ManifestNumberSearch.ManifestNumber;
                            searchCriteriaByManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ManifestNumberSearch", searchCriteriaByManifestNumberDTO).Result;

                            List<SearchCriteriaByManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByMilitaryNumberOrIdentity:
                        {
                            SearchCriteriaByMilitaryNumberOrIdentityDTO searchCriteriaByMilitaryNumberOrIdentityDTO = new SearchCriteriaByMilitaryNumberOrIdentityDTO();

                            if (advancedSearchVM.IdentificationNumber.DateTo.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo = advancedSearchVM.IdentificationNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.IdentificationNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom = advancedSearchVM.IdentificationNumber.DateFrom.Value;

                            }

                            searchCriteriaByMilitaryNumberOrIdentityDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.OrderBy = "";
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.IdentificationNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourFrom.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.IdentificationNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourTo.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.IdentificationNumber.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.FromPartyId = advancedSearchVM.IdentificationNumber.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.IdentificationNumber.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.PriorityId = advancedSearchVM.IdentificationNumber.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.SignedById = advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.StatusId = advancedSearchVM.IdentificationNumber.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.IdentificationNumber.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.DirectedToId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.PriorityId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.AdvancedSearch.StatusId = advancedSearchVM.IdentificationNumber.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.IdentificationNumber.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.IdentificationNumber.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            searchCriteriaByMilitaryNumberOrIdentityDTO.IdentificationNumber = advancedSearchVM.IdentificationNumber.IdentificationNumber;

                            searchCriteriaByMilitaryNumberOrIdentityDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>>.PostRequest("api/Search/MilitaryNumberOrIdentitySearch", searchCriteriaByMilitaryNumberOrIdentityDTO).Result;

                            List<SearchCriteriaByMilitaryNumberOrIdentityResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MilitaryNumberOrIdentitySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }
                    case SearchType.SearchByTransactionNots:
                        {
                            SearchCriteriaByTransactionNotsDTO searchCriteriaByTransactionNotsDTO = new SearchCriteriaByTransactionNotsDTO();

                            if (advancedSearchVM.TransactionNotsSearch.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateTo = advancedSearchVM.TransactionNotsSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNotsSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateFrom = advancedSearchVM.TransactionNotsSearch.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNotsDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNotsDTO.OrderBy = "";
                            searchCriteriaByTransactionNotsDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByTransactionNotsDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNotsSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourFrom.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNotsDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateFrom =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNotsDTO.FromDateTime =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNotsSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourTo.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.FromPartyId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.SignedById = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNotsSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNotsSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByTransactionNotsDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.DirectedToId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNotsDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNotsSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNotsSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }
                            searchCriteriaByTransactionNotsDTO.TransactionNots = advancedSearchVM.TransactionNotsSearch.TransactionNots;
                            searchCriteriaByTransactionNotsDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNotsDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNotsSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNotsSearchResultDTO>>>.PostRequest("api/Search/TransactionNotsSearch", searchCriteriaByTransactionNotsDTO).Result;

                            List<SearchCriteriaByTransactionNotsResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNotsSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByELcEmployee:
                        {

                            SearchCriteriaByElcEmployeeDTO searchCriteriaByElcEmployeeDTO = new SearchCriteriaByElcEmployeeDTO();

                            if (advancedSearchVM.ElcEmployeeSearch.DateTo.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateTo = advancedSearchVM.ElcEmployeeSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ElcEmployeeSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateFrom = advancedSearchVM.ElcEmployeeSearch.DateFrom.Value;

                            }

                            searchCriteriaByElcEmployeeDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByElcEmployeeDTO.OrderBy = "";
                            searchCriteriaByElcEmployeeDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByElcEmployeeDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ElcEmployeeSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourFrom.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByElcEmployeeDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateFrom =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByElcEmployeeDTO.FromDateTime =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ElcEmployeeSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourTo.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.PriorityId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.SignedById = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.StatusId = advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.ElcEmployeeSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByElcEmployeeDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByElcEmployeeDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.DirectedToId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.PriorityId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByElcEmployeeDTO.AdvancedSearch.StatusId = advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.ElcEmployeeSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.ElcEmployeeSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByElcEmployeeDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }

                            searchCriteriaByElcEmployeeDTO.ElcEmployeeId = advancedSearchVM.ElcEmployeeSearch.ElcEmployeeId;

                            searchCriteriaByElcEmployeeDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByElcEmployeeDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ELcEmployeeSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ELcEmployeeSearchResultDTO>>>.PostRequest("api/Search/ELcEmployeeSearch", searchCriteriaByElcEmployeeDTO).Result;

                            List<SearchCriteriaByElcEmployeeResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ElcEmployeeSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);




                        }
                    case SearchType.SearchByExternalOutBoundOrManifestNumber:
                        {


                            SearchCriteriaByExternalOutBoundOrManifestNumberDTO searchCriteriaByExternalOutBoundOrManifestNumberDTO = new SearchCriteriaByExternalOutBoundOrManifestNumberDTO();

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrderBy = "";
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Year != null)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.Year = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Year;
                            }


                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            transactionCategory = TransactionCategory.ExternalOutbound;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DestinationPartyId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DestinationPartyId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.DirectedToId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DirectedToId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.DirectedToId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.CreatedDepartmentId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.CreatedDepartmentId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.AdvancedSearch.ConfidentialityId.Value : -1;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.Number = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Number;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ExternalOutBoundOrManifestNumberSearch", searchCriteriaByExternalOutBoundOrManifestNumberDTO).Result;

                            List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalOutBoundOrManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByCopyAssignemnt:
                        {
                            SearchCriteriaByCopyAssignemntDTO searchCriteriaByCopyAssignemntDTO = new SearchCriteriaByCopyAssignemntDTO();
                            if (advancedSearchVM.CopyAssignemntSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateTo = advancedSearchVM.CopyAssignemntSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CopyAssignemntSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateFrom = advancedSearchVM.CopyAssignemntSearch.DateFrom.Value;

                            }

                            searchCriteriaByCopyAssignemntDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCopyAssignemntDTO.OrderBy = "";
                            searchCriteriaByCopyAssignemntDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByCopyAssignemntDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.CopyAssignemntSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourFrom.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCopyAssignemntDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateFrom =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCopyAssignemntDTO.FromDateTime =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.CopyAssignemntSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourTo.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;


                            searchCriteriaByCopyAssignemntDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.FromPartyId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.FromPartyId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.FromPartyId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.LetterTypeId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.LetterTypeId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedByDepartmentId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedByDepartmentId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.SignedById = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedById.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.SignedById.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.StatusId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.StatusId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.StatusId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.PriorityId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.PriorityId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.PriorityId.Value : -1;
                            searchCriteriaByCopyAssignemntDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.ConfidentialityId.HasValue ? advancedSearchVM.CopyAssignemntSearch.AdvancedSearch.ConfidentialityId.Value : -1;


                            searchCriteriaByCopyAssignemntDTO.FromEntityId = advancedSearchVM.CopyAssignemntSearch.FromEntityId;
                            searchCriteriaByCopyAssignemntDTO.ToEntityId = advancedSearchVM.CopyAssignemntSearch.ToEntityId;
                            searchCriteriaByCopyAssignemntDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCopyAssignemntDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<CopyAssignemntSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<CopyAssignemntSearchResultDTO>>>.PostRequest("api/Search/CopyAssignemntSearch", searchCriteriaByCopyAssignemntDTO).Result;

                            List<SearchCriteriaByCopyAssignemntResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopyAssignemntSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchBySubjectLetter:
                        {
                            SearchCriteriaBySubjectLetterDTO searchCriteriaBySubjectLetterDTO = new SearchCriteriaBySubjectLetterDTO();
                            if (advancedSearchVM.SubjectLetterSearch.DateTo.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateTo = advancedSearchVM.SubjectLetterSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.SubjectLetterSearch.DateFrom.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateFrom = advancedSearchVM.SubjectLetterSearch.DateFrom.Value;

                            }

                            searchCriteriaBySubjectLetterDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaBySubjectLetterDTO.OrderBy = "";
                            searchCriteriaBySubjectLetterDTO.PageIndex = page -1 ?? 0;
                            searchCriteriaBySubjectLetterDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectLetterSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourFrom.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaBySubjectLetterDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateFrom =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaBySubjectLetterDTO.FromDateTime =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.SubjectLetterSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourTo.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.ToDateTime = dateValue;
                                }
                            }

                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.FromPartyId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.SignedById = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectLetterSearch.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.SubjectLetterSearch.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaBySubjectLetterDTO.TransactionCategoryId = (int)TransactionCategory.All;


                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.DirectedToId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.PriorityId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaBySubjectLetterDTO.AdvancedSearch.StatusId = advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.SubjectLetterSearch.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.SubjectLetterSearch.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }
                            searchCriteriaBySubjectLetterDTO.FirstLetter = advancedSearchVM.SubjectLetterSearch.FirstLetter;
                            searchCriteriaBySubjectLetterDTO.SecondLetter = advancedSearchVM.SubjectLetterSearch.SecondLetter;
                            searchCriteriaBySubjectLetterDTO.ThirdLetter = advancedSearchVM.SubjectLetterSearch.ThirdLetter;
                            searchCriteriaBySubjectLetterDTO.FourthLetter = advancedSearchVM.SubjectLetterSearch.FourthLetter;
                            searchCriteriaBySubjectLetterDTO.SearchTypeForFiltersId = advancedSearchVM.SubjectLetterSearch.SearchTypeForFiltersId;

                            searchCriteriaBySubjectLetterDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaBySubjectLetterDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<SubjectLetterSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<SubjectLetterSearchResultDTO>>>.PostRequest("api/Search/SubjectLetterSearch", searchCriteriaBySubjectLetterDTO).Result;

                            List<SearchCriteriaBySubjectLetterResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectLetterSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);



                        }
                    case SearchType.SearchByTransactionNumber:
                        {
                            SearchCriteriaByTransactionNumberDTO searchCriteriaByTransactionNumberDTO = new SearchCriteriaByTransactionNumberDTO();

                            if (advancedSearchVM.TransactionNumber.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateTo = advancedSearchVM.TransactionNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateFrom = advancedSearchVM.TransactionNumber.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNumberDTO.OrderBy = "";
                            searchCriteriaByTransactionNumberDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByTransactionNumberDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourFrom.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateFrom =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNumberDTO.FromDateTime =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourTo.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;
                            switch ((TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                case TransactionCategory.InternalOutbound:
                                case TransactionCategory.All:
                                    {
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNumber.InboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.FromPartyId = advancedSearchVM.TransactionNumber.InboundAdvanced.FromPartyId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.FromPartyId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.TransactionNumber.InboundAdvanced.LetterTypeId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.LetterTypeId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNumber.InboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.TransactionNumber.InboundAdvanced.SignedByDepartmentId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.SignedByDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.SignedById = advancedSearchVM.TransactionNumber.InboundAdvanced.SignedById.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.SignedById.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNumber.InboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNumber.InboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.Inbound ? TransactionCategory.Inbound : TransactionCategory.InternalOutbound;
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        if ((TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.All)
                                            searchCriteriaByTransactionNumberDTO.TransactionCategoryId = (int)TransactionCategory.All;

                                        break;
                                    }
                                case TransactionCategory.DraftOutbound:
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.TransactionNumber.OutboundAdvanced.ConfidentialityId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.ConfidentialityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.CreatedDepartmentId = advancedSearchVM.TransactionNumber.OutboundAdvanced.CreatedDepartmentId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.CreatedDepartmentId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.DestinationPartyId = advancedSearchVM.TransactionNumber.OutboundAdvanced.DestinationPartyId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.DestinationPartyId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.DirectedToId = advancedSearchVM.TransactionNumber.OutboundAdvanced.DirectedToId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.DirectedToId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.PriorityId = advancedSearchVM.TransactionNumber.OutboundAdvanced.PriorityId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.PriorityId.Value : -1;
                                        searchCriteriaByTransactionNumberDTO.AdvancedSearch.StatusId = advancedSearchVM.TransactionNumber.OutboundAdvanced.StatusId.HasValue ? advancedSearchVM.TransactionNumber.OutboundAdvanced.StatusId.Value : -1;
                                        transactionCategory = (TransactionCategory)advancedSearchVM.TransactionNumber.TransactionCategory == TransactionCategory.DraftOutbound ? TransactionCategory.DraftOutbound : TransactionCategory.ExternalOutbound;
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = transactionCategory.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        break;
                                    }
                            }


                            searchCriteriaByTransactionNumberDTO.TransactionNumber = advancedSearchVM.TransactionNumber.TransactionNumber;

                            searchCriteriaByTransactionNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNumberSearchResultDTO>>>.PostRequest("api/Search/TransactionNumberSearch", searchCriteriaByTransactionNumberDTO).Result;

                            List<SearchCriteriaByTransactionNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }
                    case SearchType.SearchByExternalPartyCopies:
                        {
                            SearchCriteriaByExternalPartyCopiesDTO searchCriteriaByExternalPartyCopiesDTO = new SearchCriteriaByExternalPartyCopiesDTO();


                            searchCriteriaByExternalPartyCopiesDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByExternalPartyCopiesDTO.OrderBy = "";
                            searchCriteriaByExternalPartyCopiesDTO.PageIndex = page - 1?? 0; ;
                            searchCriteriaByExternalPartyCopiesDTO.PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;

                            searchCriteriaByExternalPartyCopiesDTO.ExternalPartyId = advancedSearchVM.ExternalPartyCopies.ExternalPartyId;


                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByExternalPartyCopiesDTO.UserId = SessionInfo.CurrentUser.Id;

                            GetResult<List<ExternalPartyCopiesSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ExternalPartyCopiesSearchResultDTO>>>.PostRequest("api/Search/ExternalPartyCopiesSearch", searchCriteriaByExternalPartyCopiesDTO).Result;

                            List<SearchCriteriaByExternalPartyCopiesResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            SessionInfo.SetObjectInSession(advancedSearchVM, "advancedSearchVM");
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, true, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalPartyCopiesSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetSearchViewBySearchTypeId(int id)
        {
            try
            {
                string dateFormat = UIHelper.SystemDateFormat;
                DateTime date = DateTime.Now;
                int currentYear = date.Year;

                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                DateTime startDate = new DateTime(currentYear, 1, 1);

                GetResult<List<ExternalPartyDTO>> externalPartyDTOs;
                GetResult<List<OrgUnitDTO>> orgUnitDTOs;
                GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs;

                switch ((SearchType)id)
                {
                    case SearchType.SearchByInboundNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);
                            if (ViewData["TransactionTypes"].ToString().Length > 2)
                            {
                                //ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("]", " ,{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}]");
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}, ");
                            }
                            else
                            {
                                //ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("]", " {\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}]");
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}");
                            }
                            ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);

                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                            //subjectClassificationDTOs =
                            //    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                            //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                            SearchCriteriaByInboundVM searchCriteriaByInboundVM = new SearchCriteriaByInboundVM();

                            searchCriteriaByInboundVM.Year = currentYear;

                            ViewData.TemplateInfo.HtmlFieldPrefix = "InboundSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchPartial", searchCriteriaByInboundVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByEntity:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.None);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.None);

                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.None);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "EntitySearch";

                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            var externalPartyVMs = ExternalPartyMapper.Map(externalPartyDTOs.Result);
                            ViewData["ExternalPartiesData"] = externalPartyDTOs.Result != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;

                            SearchCriteriaByEntityNameVM searchCriteriaByEntityNameVM = new SearchCriteriaByEntityNameVM();

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EntitySearchPartial", searchCriteriaByEntityNameVM) }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByCreator:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.None);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.None);

                            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.None);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "CreatorSearch";

                            SearchCriteriaByCreatorVM searchCriteriaByCreatorVM = new SearchCriteriaByCreatorVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CreatorSearchPartial", searchCriteriaByCreatorVM) }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByOutboundInternalNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.InternalOutbound);
                            if (ViewData["TransactionTypes"].ToString().Length > 2)
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}, ");
                            }
                            else
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}");
                            }

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.InternalOutbound);

                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.InternalOutbound);

                            //subjectClassificationDTOs =
                            //    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                            //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                            SearchCriteriaByOutboundInternalVM searchCriteriaByOutboundInternalVM = new SearchCriteriaByOutboundInternalVM();

                            searchCriteriaByOutboundInternalVM.Year = currentYear;

                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundInternalSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundInternalSearchPartial", searchCriteriaByOutboundInternalVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);
                            if (ViewData["TransactionTypes"].ToString().Length > 2)
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}, ");
                            }
                            else
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}");
                            }
                            ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                            ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                            //subjectClassificationDTOs =
                            //    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                            //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));


                            SearchCriteriaByOutboundVM searchCriteriaByOutboundVM = new SearchCriteriaByOutboundVM();

                            searchCriteriaByOutboundVM.Year = currentYear;
                            searchCriteriaByOutboundVM.DateTo = date;
                            searchCriteriaByOutboundVM.DateFrom = startDate;

                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundSearchPartial", searchCriteriaByOutboundVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundDraftNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);
                            if (ViewData["TransactionTypes"].ToString().Length > 2)
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}, ");
                            }
                            else
                            {
                                ViewData["TransactionTypes"] = ViewData["TransactionTypes"].ToString().Replace("[", "[{\"label\":\"الكل\",\"value\":\"-1\",\"parameters\":null}");
                            }

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.DraftOutbound);

                            //subjectClassificationDTOs =
                            //    HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                            //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));


                            SearchCriteriaByOutboundDraftVM searchCriteriaByOutboundDraftVM = new SearchCriteriaByOutboundDraftVM();

                            searchCriteriaByOutboundDraftVM.Year = currentYear;
                            searchCriteriaByOutboundDraftVM.DateTo = date;
                            searchCriteriaByOutboundDraftVM.DateFrom = startDate;

                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundDraftSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundDraftSearchPartial", searchCriteriaByOutboundDraftVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchBySubject:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.None);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.None);

                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.None);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectSearch";

                            SearchCriteriaBySubjectVM searchCriteriaBySubjectVM = new SearchCriteriaBySubjectVM();

                            searchCriteriaBySubjectVM.TransactionTypeId = SearchType.SearchBySubject.LookupIdentity(LookupCategory.SearchType, SessionInfo.CultureShortName);

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchPartial", searchCriteriaBySubjectVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByAssignTransaction:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.None);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.None);

                            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.None);

                            orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);

                            ViewData["OrgUnitUsers"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "AssignTransactionSearch";

                            SearchCriteriaByAssignTransactionVM searchCriteriaByAssignTransactionVM = new SearchCriteriaByAssignTransactionVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignTransactionSearchPartial", searchCriteriaByAssignTransactionVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByDocumentNumber:
                        {

                            ViewData.TemplateInfo.HtmlFieldPrefix = "DocumentNumberSearch";

                            SearchCriteriaByDocumentNumberVM searchCriteriaByDocumentNumberVM = new SearchCriteriaByDocumentNumberVM();

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_DocumentNumberSearchPartial", searchCriteriaByDocumentNumberVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByRecordNumber:
                        {

                            ViewData.TemplateInfo.HtmlFieldPrefix = "RecordNumberSearch";

                            SearchCriteriaByRecordNumberVM searchCriteriaByRecordNumberVM = new SearchCriteriaByRecordNumberVM();

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_RecordNumberSearchPartial", searchCriteriaByRecordNumberVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByNames:
                        {

                            ViewData["SearchTypeForFilters"] = GeSearchTypeForFilters();
                            ViewData.TemplateInfo.HtmlFieldPrefix = "NamesSearch";

                            SearchCriteriaByNamesVM searchCriteriaByNamesVM = new SearchCriteriaByNamesVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesSearchPartial", searchCriteriaByNamesVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchDaily:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "DailySearch";

                            SearchCriteriaByDailyVM searchCriteriaByDailyVM = new SearchCriteriaByDailyVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_DailySearchPartial", searchCriteriaByDailyVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByAssignmentNote:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "AssignmentNoteSearch";

                            SearchCriteriaByAssignmentNoteVM searchCriteriaByAssignmentNoteVM = new SearchCriteriaByAssignmentNoteVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentNoteSearchPartial", searchCriteriaByAssignmentNoteVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByManifestNumber:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "ManifestNumberSearch";

                            SearchCriteriaByManifestNumberVM searchCriteriaByManifestNumberVM = new SearchCriteriaByManifestNumberVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManifestNumberSearchPartial", searchCriteriaByManifestNumberVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByMilitaryNumberOrIdentity:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "IdentificationNumber";

                            SearchCriteriaByMilitaryNumberOrIdentityVM searchCriteriaByMilitaryNumberOrIdentityVM = new SearchCriteriaByMilitaryNumberOrIdentityVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MilitaryNumberOrIdentitySearchPartial", searchCriteriaByMilitaryNumberOrIdentityVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByTransactionNots:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNotsSearch";

                            SearchCriteriaByTransactionNotsVM searchCriteriaByTransactionNotsVM = new SearchCriteriaByTransactionNotsVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNotsSearchPartial", searchCriteriaByTransactionNotsVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByELcEmployee:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "ELcEmployeeSearch";

                            SearchCriteriaByElcEmployeeVM searchCriteriaByELcEmployeeVM = new SearchCriteriaByElcEmployeeVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ELcEmployeeSearchPartial", searchCriteriaByELcEmployeeVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByExternalOutBoundOrManifestNumber:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "ExternalOutBoundOrManifestNumberSearch";
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                            SearchCriteriaByExternalOutBoundOrManifestNumberVM searchCriteriaByExternalOutBoundOrManifestNumberVM = new SearchCriteriaByExternalOutBoundOrManifestNumberVM();
                            searchCriteriaByExternalOutBoundOrManifestNumberVM.Year = currentYear;

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalOutBoundOrManifestNumberSearchPartial", searchCriteriaByExternalOutBoundOrManifestNumberVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByCopyAssignemnt:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "CopyAssignemntSearch";

                            SearchCriteriaByCopyAssignemntVM searchCriteriaByCopyAssignemntVM = new SearchCriteriaByCopyAssignemntVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopyAssignemntSearchPartial", searchCriteriaByCopyAssignemntVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchBySubjectLetter:
                        {
                            ViewData["SearchTypeForFilters"] = GeSearchTypeForFilters();
                            ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectLetterSearch";

                            SearchCriteriaBySubjectLetterVM searchCriteriaBySubjectLetterVM = new SearchCriteriaBySubjectLetterVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectLetterSearchPartial", searchCriteriaBySubjectLetterVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByTransactionNumber:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNumber";

                            SearchCriteriaByTransactionNumberVM searchCriteriaByTransactionNoNumberVM = new SearchCriteriaByTransactionNumberVM();


                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNumberSearchPartial", searchCriteriaByTransactionNoNumberVM) }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByExternalPartyCopies:
                        {
                            ViewData.TemplateInfo.HtmlFieldPrefix = "ExternalPartyCopies";

                            SearchCriteriaByExternalPartyCopiesVM searchCriteriaByExternalPartyCopiesVM = new SearchCriteriaByExternalPartyCopiesVM();
                            List<ExternalPartyDTO> parties =
                               HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(String.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result.Result;
                            IList<AutoCompleteDataSource> partiesdataSource = new List<AutoCompleteDataSource>();
                            if (parties != null)
                            {
                                partiesdataSource.Add(UIHelper.GetDefaultSelect());

                                foreach (ExternalPartyDTO item in parties)
                                {
                                    partiesdataSource.Add(new AutoCompleteDataSource()
                                    {
                                        Value = item.Id.ToString(),
                                        Label = item.LocalName
                                    });
                                }

                            }
                            var externalPartyVMs = ExternalPartyMapper.Map(parties);
                            ViewData["ExternalPartiesData"] = parties != null ? UIHelper.BulidExternalPartiesTree(externalPartyVMs) : null;
                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalPartyCopiesSearchPartial", searchCriteriaByExternalPartyCopiesVM) }, JsonRequestBehavior.AllowGet);
                        }
                        //case SearchType.SearchByBarcode:
                        //    {
                        //        SearchCriteriaByBarcodeVM searchCriteriaByBarcodeVM = new SearchCriteriaByBarcodeVM();
                        //        searchCriteriaByBarcodeVM.TypeId = (int)SearchType.SearchByInboundNumber;
                        //        ViewData.TemplateInfo.HtmlFieldPrefix = "BarcodeSearch";

                        //        return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_BarcodeSearchPartial", searchCriteriaByBarcodeVM) }, JsonRequestBehavior.AllowGet);
                        //    }
                        //case SearchType.GeneralSearch:
                        //    {
                        //        SearchCriteriaGeneralVM searchCriteriaGeneralVM = new SearchCriteriaGeneralVM();
                        //        ViewData.TemplateInfo.HtmlFieldPrefix = "GeneralSearch";
                        //        return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GeneralSearchPartial", searchCriteriaGeneralVM) }, JsonRequestBehavior.AllowGet);
                        //    }
                }

                return Json(new { html = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetAdvancedSearchView(int SearchTypeId, int TransactionCategoryId)
        {
            try
            {
                InboundAdvancedVM inboundAdvancedVM = new InboundAdvancedVM();
                OutboundAdvancedVM OutboundAdvancedVM = new OutboundAdvancedVM();
                GetResult<List<ExternalPartyDTO>> externalPartyDTOs;

                //   GetResult<List<SubjectClassificationDTO>> subjectClassificationDTOs =
                //HttpClientWrapper<GetResult<List<SubjectClassificationDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSubjectClassificationsByOrgUnitId?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                //ViewData["SubjectClassificationsData"] = BulidSubjectClassificationsTree(SubjectClassificationMapper.Map(subjectClassificationDTOs.Result));

                ViewData["ConfidentialityData"] = GetConfidentialityLevel();

                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllModules))
                {
                    GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                    ViewData["InboundParties"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllChildsModules))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = true;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["InboundParties"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchParentDepartment))
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["InboundParties"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }
                else
                {
                    GetResult<OrgUnitDTO> orgUnitDTOs =
                        HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                    List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                    orgUnitDTOs.Result.ParentId = -1;
                    orgUnitDTOs.Result.HasChilds = false;
                    newList.Add(orgUnitDTOs.Result);
                    ViewData["InboundParties"] = UIHelper.BulidTree(OrgUnitMapper.Map(newList), SessionInfo.OrgUnitId);
                }



                switch ((SearchType)SearchTypeId)
                {
                    case SearchType.SearchByInboundNumber:
                        {

                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "InboundSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);

                            ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));

                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchBySubject:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                                ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));

                                ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByOutboundInternalNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundInternalSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundDraftNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                            ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);


                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));


                            ViewData.TemplateInfo.HtmlFieldPrefix = "OutboundDraftSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByEntity:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "EntitySearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "EntitySearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByCreator:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "CreatorSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "CreatorSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByAssignTransaction:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                                ViewData.TemplateInfo.HtmlFieldPrefix = "AssignTransactionSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "AssignTransactionSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByDocumentNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                            ViewData.TemplateInfo.HtmlFieldPrefix = "DocumentNumberSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByRecordNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "RecordNumberSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByNames:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "NamesSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "NamesSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByAssignmentNote:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                            ViewData.TemplateInfo.HtmlFieldPrefix = "AssignmentNoteSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByManifestNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));

                            ViewData.TemplateInfo.HtmlFieldPrefix = "ManifestNumberSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByMilitaryNumberOrIdentity:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "IdentificationNumber.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "IdentificationNumber.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByTransactionNots:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNotsSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNotsSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByELcEmployee:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "ElcEmployeeSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "ElcEmployeeSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByExternalOutBoundOrManifestNumber:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                            ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                            externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                            ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));

                            ViewData.TemplateInfo.HtmlFieldPrefix = "ExternalOutBoundOrManifestNumberSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByCopyAssignemnt:
                        {
                            ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                            ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                            ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);


                            ViewData.TemplateInfo.HtmlFieldPrefix = "CopyAssignemntSearch.AdvancedSearch";

                            return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchBySubjectLetter:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectLetterSearch.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);

                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);
                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "SubjectLetterSearch.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                    case SearchType.SearchByTransactionNumber:
                        {
                            if (TransactionCategoryId == (int)TransactionCategory.Inbound || TransactionCategoryId == (int)TransactionCategory.InternalOutbound || TransactionCategoryId == (int)TransactionCategory.All)
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.Inbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.Inbound);


                                ViewData["LetterTypes"] = GetLetterTypes(TransactionCategory.Inbound);

                                ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNumber.InboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundAdvancedSearchPartial", inboundAdvancedVM) }, JsonRequestBehavior.AllowGet);



                            }
                            else
                            {
                                ViewData["TransactionTypes"] = GetTransactionTypes(TransactionCategory.ExternalOutbound);

                                ViewData["Priorities"] = GetPriorities(TransactionCategory.ExternalOutbound);
                                ViewData["DeliveryMethod"] = GetDeliveryMethodForSearch(true);

                                externalPartyDTOs = HttpClientWrapper<GetResult<List<ExternalPartyDTO>>>.GetItemRequest(string.Format("api/Common/GetExternalParties?cultureName={0}&parentId={1}", SessionInfo.CultureShortName, null)).Result;

                                ViewData["ExternalParties"] = UIHelper.BulidExternalPartiesTree(ExternalPartyMapper.Map(externalPartyDTOs.Result));
                                ViewData.TemplateInfo.HtmlFieldPrefix = "TransactionNumber.OutboundAdvanced";

                                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundAdvancedSearchPartial", OutboundAdvancedVM) }, JsonRequestBehavior.AllowGet);


                            }
                        }
                }

                return Json(new { html = "" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateInboundSearchGrid(int? page, string param)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                SearchCriteria searchCriteria = javaScriptSerializer.Deserialize<SearchCriteria>(param);

                string orderBy = System.Web.HttpContext.Current.Request.QueryString["gridColumn"];

                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["pageSize"]))
                {
                    GridHelper.PageSize = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["pageSize"]);
                }

                if (orderBy == SearchFields.DateH)
                {
                    orderBy = SearchFields.Date;
                }

                searchCriteria.OrderBy = orderBy;
                searchCriteria.Ascending = Convert.ToBoolean(Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["dir"]));
                searchCriteria.PageIndex = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["page"]);

                int searchCount = 0;

                GetResult<List<InboundSearchResultDTO>> result =
                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.GetItemRequest("api/Search/InboundSearch").Result;

                //IList<ISearchResult> searchResults = new List<ISearchResult>();searcher.Search(searchCriteria, out searchCount);

                List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page.HasValue ? page.Value : 1, searchCount, page.HasValue);

                return Json(new { Html = grid.ToJson("_InboundSearchGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateOutboundSearchGrid(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                SearchCriteria searchCriteria = javaScriptSerializer.Deserialize<SearchCriteria>(param);

                string orderBy = System.Web.HttpContext.Current.Request.QueryString["gridColumn"];

                if (orderBy == SearchFields.DateH)
                {
                    orderBy = SearchFields.Date;
                }


                if (!string.IsNullOrEmpty(System.Web.HttpContext.Current.Request.QueryString["pageSize"]))
                {
                    GridHelper.PageSize = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["pageSize"]);
                }

                searchCriteria.OrderBy = orderBy;
                searchCriteria.Ascending = Convert.ToBoolean(Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["dir"]));
                searchCriteria.PageIndex = Convert.ToInt32(System.Web.HttpContext.Current.Request.QueryString["page"]);

                int searchCount = 0;

                //IList<ISearchResult> searchResults = new List<ISearchResult>(); //searcher.Search(searchCriteria, out searchCount);

                GetResult<List<OutboundSearchResultDTO>> result =
         HttpClientWrapper<GetResult<List<OutboundSearchResultDTO>>>.GetItemRequest("api/Search/OutboundSearchResult").Result;


                List<OutboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page.HasValue ? page.Value : 1, searchCount, page.HasValue);

                return Json(new { Html = grid.ToJson("_OutboundSearchGridPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
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


                if (userProfileDTOs.Result != null)
                {
                    List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);
                    foreach (UserProfileVM userProfileVM in userProfileVMs)
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

        [HttpGet]
        public string GetManagersByPartyId(int? id)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!id.HasValue || id == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<ManagerDTO>> managerDTOs =
                HttpClientWrapper<GetResult<List<ManagerDTO>>>.GetItemRequest(string.Format("api/Common/GetManagersByPartyId?cultureName={0}&partyId={1}", SessionInfo.CultureShortName, id)).Result;


                if (managerDTOs.Result != null)
                {
                    List<ManagerVM> managerVMs = ManagerMapper.Map(managerDTOs.Result);
                    foreach (ManagerVM manager in managerVMs)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = manager.Id.ToString(),
                            Label = manager.LocalName
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

        [HttpGet]
        public ActionResult GetYearStartEndDateTime(int year)
        {
            try
            {
                string dateFormat = UIHelper.SystemDateFormat;
                DateTime date = DateTime.Now;

                string startDate = (new DateTime(year, 1, 1)).ToString(dateFormat);
                string endDate;

                if (year == date.Year)
                {
                    endDate = date.ToString(dateFormat);
                }
                else
                {
                    endDate = (new DateTime(year, 12, 31)).ToString(dateFormat);
                }

                return Json(new { StartDate = startDate, EndDate = endDate }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private JsonResult ReturnJsonResult(SearchCriteria searchCriteria, string gridName, TransactionCategory transactionType)
        {
            int searchCount = 0;

            IList<ISearchResult> searchResults = new List<ISearchResult>(); //searcher.Search(searchCriteria, out searchCount);

            List<BaseSearchResultVM> searchResultVMs = Map(searchResults.ToList());

            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, searchCount, false);

            return Json(new { Html = grid.ToJson(gridName, this), grid.HasItems, Type = (transactionType).ToString(), Param = JsonConvert.SerializeObject(searchCriteria) }, JsonRequestBehavior.AllowGet);
        }

        private string GetPriorities(TransactionCategory transactionType)
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(transactionType);

            if (priorityVMs.Result != null)
            {
                foreach (PriorityVM priorityVM in priorityVMs.Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = priorityVM.Id.ToString(),
                        Label = priorityVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        private string GetLetterTypes(TransactionCategory transactionType)
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            GetResult<List<LetterTypeVM>> letterTypeVMs = LookupsHelper.GetLetterTypes(transactionType);

            if (letterTypeVMs.Result != null)
            {
                foreach (LetterTypeVM letterTypeVM in letterTypeVMs.Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = letterTypeVM.Id.ToString(),
                        Label = letterTypeVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }

        private string GetTransactionTypes(TransactionCategory transactionType)
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<TransactionTypeVM>> transactionTypeVMs = LookupsHelper.GetTransactionTypes(transactionType);

            if (transactionTypeVMs.Result != null)
            {
                foreach (TransactionTypeVM transactionTypeVM in transactionTypeVMs.Result)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = transactionTypeVM.Id.ToString(),
                        Label = transactionTypeVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }
        public string GetDeliveryMethod(bool isYesseRregistered)
        {
            try
            {
                int[] yesserRegistered = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] notYesserRegistered = { DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isYesseRregistered)
                    {
                        lookups.Result = lookups.Result.Where(a => yesserRegistered.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => notYesserRegistered.Contains(a.Id)).ToList();
                    }
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
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

        public string GetDeliveryMethodForSearch(bool isYesseRregistered)
        {
            try
            {
                int[] yesserRegistered = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] notYesserRegistered = { DeliveryMethodType.Paper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {

                    dataSource.Add(UIHelper.GetDefaultSelect());
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isYesseRregistered)
                    {
                        lookups.Result = lookups.Result.Where(a => yesserRegistered.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => notYesserRegistered.Contains(a.Id)).ToList();
                    }
                    foreach (var item in lookups.Result)
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = item.Id.ToString(),
                            Label = item.Text
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

        private List<BaseSearchResultVM> Map(List<ISearchResult> searchResults)
        {
            List<BaseSearchResultVM> searchResultVMs = new List<BaseSearchResultVM>();

            foreach (ISearchResult searchResult in searchResults)
            {
                searchResultVMs.Add(Map(searchResult));
            }

            return searchResultVMs;
        }

        private BaseSearchResultVM Map(ISearchResult searchResult)
        {
            BaseSearchResultVM searchResultVM = new BaseSearchResultVM()
            {
                Id = searchResult.DocId,
                EncryptedId = searchResult.DocId.ToString(),
                TransactionType = searchResult.Type,
                Number = searchResult.Number,
                Subject = searchResult.Subject,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(searchResult.Date),
                Date = searchResult.Date,
                ConfidentialityName = searchResult.ConfidentialityName,
                PriorityName = searchResult.PriorityName,
                PartyName = searchResult.PartyName,
                OrgUnitName = searchResult.OrgUnitName,
                StatusName = searchResult.StatusName,
                WithArchiving = searchResult.WithArchiving,
                ColorCode = searchResult.ColorCode,
                TransactionCategoryName = searchResult.TransactionTypeName,
                TransactionCategoryId = searchResult.TransactionCategoryId,
                EncryptedTransactionCategoryId = searchResult.TransactionCategoryId.ToString()
            };

            return searchResultVM;
        }

        private string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission =
                    string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}",
                    PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionDTO>> permissionDTOs =
                    HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (permissionDTOs.Result != null)
                {
                    List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
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
            catch (Exception)
            {
                throw;
            }
        }

        private MCS.Framework.Persistence.Filter AddFilter(string name, string value, FilterType type)
        {
            return new Framework.Persistence.Filter()
            {
                ColumnName = name,
                Value = value,
                Type = type
            };
        }

        public TreeViewModel BulidSubjectClassificationsTree(List<SubjectClassificationVM> subjectClassificationVMs)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            if (subjectClassificationVMs == null)
            {
                return tree;
            }

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };


            subjectClassificationVMs.Where(s => subjectClassificationVMs.All(sc => sc.Id != s.ParentId)).ToList().ForEach(s =>
            {
                tree.RootNode.Childs.Add(AddSubjectClassificationsChilds(subjectClassificationVMs, s));
            });

            return tree;
        }

        private TreeNode AddSubjectClassificationsChilds(List<SubjectClassificationVM> subjectClassificationVMs, SubjectClassificationVM subjectClassificationVM)
        {
            TreeNode treeNode = new TreeNode()
            {
                DepartmentNumber = subjectClassificationVM.Id.ToString(),
                IsSelected = subjectClassificationVM.IsSelected,
                Selectable = !subjectClassificationVM.IsGroup,
                Name = subjectClassificationVM.LocalName,
                Id = subjectClassificationVM.Id
            };

            subjectClassificationVMs.Where(s => s.ParentId == subjectClassificationVM.Id).ToList().ForEach(s =>
            {
                treeNode.Childs.Add(AddSubjectClassificationsChilds(subjectClassificationVMs, s));
            });

            return treeNode;
        }
        public ActionResult SearchEventHandler(int? page)
        {
            try
            {
                AdvancedSearchVM advancedSearchVM = SessionInfo.GetObjectFromSession("advancedSearchVM") as AdvancedSearchVM;
                if (advancedSearchVM == null)
                {
                    advancedSearchVM = new AdvancedSearchVM();
                }

                string message = string.Empty;
                SearchCriteria searchCriteria = new SearchCriteria();
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.SearchSettings.MaximumNumber)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                int PageSize = settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize;
                searchCriteria.Filters = new List<MCS.Framework.Persistence.Filter>();

                if (advancedSearchVM.OrgUnitId.HasValue)
                {
                    searchCriteria.Filters.Add(
                        AddFilter(SearchFields.OrgUnitId, advancedSearchVM.OrgUnitId.Value.ToString(), FilterType.Equals));
                }

                switch ((SearchType)advancedSearchVM.SearchTypeId)
                {
                    case SearchType.SearchByInboundNumber:
                        {

                            SearchCriteriaByInboundDTO searchCriteriaByInboundDTO = new SearchCriteriaByInboundDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByInboundDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            if (advancedSearchVM.InboundSearch.DateTo.HasValue)
                            {
                                searchCriteriaByInboundDTO.ToDate = advancedSearchVM.InboundSearch.DateTo.Value;
                            }
                            if (advancedSearchVM.InboundSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByInboundDTO.FromDate = advancedSearchVM.InboundSearch.DateFrom.Value;
                            }
                            searchCriteriaByInboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByInboundDTO.OrderBy = "";
                            searchCriteriaByInboundDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByInboundDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByInboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.InboundSearch.Year != null)
                            {
                                searchCriteriaByInboundDTO.Year = advancedSearchVM.InboundSearch.Year;
                            }

                            if (searchCriteriaByInboundDTO.FromDate.HasValue)
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.FromDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (searchCriteriaByInboundDTO.ToDate.HasValue)
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByInboundDTO.ToDate.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByInboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.InboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.InboundSearch.HourFrom.Value,
                                    (advancedSearchVM.InboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByInboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.FromDate =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByInboundDTO.FromDateTime =
                                        searchCriteriaByInboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.InboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.InboundSearch.HourTo.Value,
                                    (advancedSearchVM.InboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.InboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByInboundDTO.ToDateTime =
                                        searchCriteriaByInboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByInboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByInboundDTO.ToDate =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByInboundDTO.ToDateTime =
                                    searchCriteriaByInboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.InboundSearch.Number.HasValue)
                            {
                                searchCriteriaByInboundDTO.Number = advancedSearchVM.InboundSearch.Number.Value;

                            }

                            searchCriteriaByInboundDTO.TransactionTypeId = advancedSearchVM.InboundSearch.TransactionTypeId;

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.HasValue)
                            {
                                searchCriteriaByInboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.InboundSearch.AdvancedSearch.FromPartyId.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.HasValue)
                            {
                                searchCriteriaByInboundDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.InboundSearch.AdvancedSearch.LetterTypeId.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.SignedById.HasValue)
                            {

                                searchCriteriaByInboundDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.InboundSearch.AdvancedSearch.SignedById.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.HasValue)
                            {
                                searchCriteriaByInboundDTO.AdvancedSearch.StatusId = advancedSearchVM.InboundSearch.AdvancedSearch.StatusId.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.HasValue)
                            {
                                searchCriteriaByInboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.InboundSearch.AdvancedSearch.PriorityId.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.HasValue)
                            {
                                searchCriteriaByInboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.InboundSearch.AdvancedSearch.ConfidentialityId.Value;
                            }

                            if (advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.InboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                //searchCriteria.Filters.Add(
                                //    AddFilter(SearchFields.SubjectClassifications, subjectClassifications, FilterType.Equals));
                                searchCriteriaByInboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }
                            searchCriteriaByInboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByInboundDTO.HasFullPrivilege = HasPermissionSearch;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByInboundDTO.Global = true;

                            }
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/InboundSearch", searchCriteriaByInboundDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByDocumentNumber:
                        {
                            SearchCriteriaByDocumentNumberDTO searchCriteriaByDocumentNumberDTO = new SearchCriteriaByDocumentNumberDTO();
                            searchCriteriaByDocumentNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDocumentNumberDTO.OrderBy = "";
                            searchCriteriaByDocumentNumberDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByDocumentNumberDTO.PageSize = GridHelper.PageSize;

                            if (advancedSearchVM.DocumentNumberSearch.Year != null)
                            {
                                searchCriteriaByDocumentNumberDTO.Year = advancedSearchVM.DocumentNumberSearch.Year;
                            }

                            if (advancedSearchVM.DocumentNumberSearch.DocumentNumber != string.Empty)
                            {
                                searchCriteriaByDocumentNumberDTO.DocumentNumber = advancedSearchVM.DocumentNumberSearch.DocumentNumber;
                            }

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDocumentNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            searchCriteriaByDocumentNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByDocumentNumberDTO.HasFullPrivilege = HasPermissionSearch;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByDocumentNumberDTO.Global = true;

                            }
                            GetResult<List<InboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<InboundSearchResultDTO>>>.PostRequest("api/Search/DocumentNumberSearch", searchCriteriaByDocumentNumberDTO).Result;

                            List<InboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByOutboundInternalNumber:
                        {

                            SearchCriteriaByOutboundInternalDTO searchCriteriaByOutboundInternalDTO = new SearchCriteriaByOutboundInternalDTO();
                            //earchCriteria.Filters.Add(
                            //   AddFilter(SearchFields.TransactionTypeId, ((int)TransactionCategory.Inbound).ToString(), FilterType.Equals));

                            searchCriteriaByOutboundInternalDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            searchCriteriaByOutboundInternalDTO.FromDate = advancedSearchVM.OutboundInternalSearch.DateFrom.HasValue ? advancedSearchVM.OutboundInternalSearch.DateFrom.Value.ToString() : null;
                            searchCriteriaByOutboundInternalDTO.ToDate = advancedSearchVM.OutboundInternalSearch.DateTo.HasValue ? advancedSearchVM.OutboundInternalSearch.DateTo.Value.ToString() : null;

                            searchCriteriaByOutboundInternalDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundInternalDTO.OrderBy = "";
                            searchCriteriaByOutboundInternalDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByOutboundInternalDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Year != null)
                            {
                                searchCriteriaByOutboundInternalDTO.Year = advancedSearchVM.OutboundInternalSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundInternalDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundInternalDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundInternalDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.FromDate =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.FromDateTime =
                                        searchCriteriaByOutboundInternalDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundInternalSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundInternalSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundInternalSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                        searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundInternalDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundInternalDTO.ToDate =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundInternalDTO.ToDateTime =
                                    searchCriteriaByOutboundInternalDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundInternalSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.Number = advancedSearchVM.OutboundInternalSearch.Number.Value;

                            }

                            searchCriteriaByOutboundInternalDTO.TypeId = advancedSearchVM.OutboundInternalSearch.TransactionTypeId;

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.FromPartyId.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.LetterTypeId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.LetterTypeId.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedById.HasValue)
                            {

                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.SignedByDepartmentId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SignedById.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.StatusId.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.PriorityId.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.HasValue)
                            {
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundInternalSearch.AdvancedSearch.ConfidentialityId.Value;
                            }

                            if (advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications != null
                                && advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundInternalSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                //searchCriteria.Filters.Add(
                                //    AddFilter(SearchFields.SubjectClassifications, subjectClassifications, FilterType.Equals));
                                searchCriteriaByOutboundInternalDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }
                            searchCriteriaByOutboundInternalDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundInternalDTO.HasFullPrivilege = HasPermissionSearch;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundInternalDTO.Global = true;

                            }
                            GetResult<List<OutboundInternalSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundInternalSearchResultDTO>>>.PostRequest("api/Search/OutboundInternalSearch", searchCriteriaByOutboundInternalDTO).Result;

                            List<OutboundInternalSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundInternalSearchGridPartial", grid), Type = (TransactionCategory.InternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByOutboundNumber:
                        {
                            SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO = new SearchCriteriaByOutboundDTO();

                            searchCriteriaByOutboundDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDTO.OrderBy = "";
                            searchCriteriaByOutboundDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByOutboundDTO.PageSize = PageSize;
                            searchCriteriaByOutboundDTO.TransactionCategoryId = (TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));

                            searchCriteriaByOutboundDTO.FromDate = advancedSearchVM.OutboundSearch.DateFrom.HasValue ? advancedSearchVM.OutboundSearch.DateFrom.Value.ToString() : null;
                            searchCriteriaByOutboundDTO.ToDate = advancedSearchVM.OutboundSearch.DateTo.HasValue ? advancedSearchVM.OutboundSearch.DateTo.Value.ToString() : null;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDTO.Global = true;

                            }
                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundSearch.Year != null)
                            {
                                searchCriteriaByOutboundDTO.Year = advancedSearchVM.OutboundSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.FromDate =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDTO.FromDateTime =
                                        searchCriteriaByOutboundDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                        searchCriteriaByOutboundDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDTO.ToDate =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDTO.ToDateTime =
                                    searchCriteriaByOutboundDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDTO.Number = advancedSearchVM.OutboundSearch.Number;
                            }

                            searchCriteriaByOutboundDTO.TypeId = advancedSearchVM.OutboundSearch.TransactionTypeId;

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundSearch.AdvancedSearch.DestinationPartyId.Value;
                            }

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.AdvancedSearch.DirectedToUserId = advancedSearchVM.OutboundSearch.AdvancedSearch.DirectedToId.Value.ToString();
                            }

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundSearch.AdvancedSearch.StatusId.Value;
                            }

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundSearch.AdvancedSearch.PriorityId.Value;
                            }

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.HasValue)
                            {
                                searchCriteriaByOutboundDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundSearch.AdvancedSearch.ConfidentialityId.Value;
                            }

                            if (advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }

                            searchCriteriaByOutboundDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<OutboundSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundSearchResultDTO>>>.PostRequest("api/Search/OutboundSearch", searchCriteriaByOutboundDTO).Result;

                            List<OutboundSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundSearchGridPartial", grid), Type = (TransactionCategory.ExternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByOutboundDraftNumber:
                        {
                            SearchCriteriaByOutboundDraftDTO searchCriteriaByOutboundDraftDTO = new SearchCriteriaByOutboundDraftDTO();

                            searchCriteriaByOutboundDraftDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByOutboundDraftDTO.OrderBy = "";
                            searchCriteriaByOutboundDraftDTO.PageIndex = page - 1 ?? 0;
                            searchCriteriaByOutboundDraftDTO.PageSize = PageSize;
                            searchCriteriaByOutboundDraftDTO.TransactionCategoryId = (TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));

                            searchCriteriaByOutboundDraftDTO.FromDate = advancedSearchVM.OutboundDraftSearch.DateFrom.HasValue ? advancedSearchVM.OutboundDraftSearch.DateFrom.Value.ToString() : null;
                            searchCriteriaByOutboundDraftDTO.ToDate = advancedSearchVM.OutboundDraftSearch.DateTo.HasValue ? advancedSearchVM.OutboundDraftSearch.DateTo.Value.ToString() : null;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Year != null)
                            {
                                searchCriteriaByOutboundDraftDTO.Year = advancedSearchVM.OutboundDraftSearch.Year;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.FromDate))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.FromDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByOutboundDraftDTO.ToDate))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByOutboundDraftDTO.ToDate, dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDateTime = dateValue;
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourFrom.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByOutboundDraftDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.FromDate =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.FromDateTime =
                                        searchCriteriaByOutboundDraftDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.OutboundDraftSearch.HourTo.Value,
                                    (advancedSearchVM.OutboundDraftSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.OutboundDraftSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                        searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByOutboundDraftDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByOutboundDraftDTO.ToDate =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59)).ToString();

                                    searchCriteriaByOutboundDraftDTO.ToDateTime =
                                    searchCriteriaByOutboundDraftDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (advancedSearchVM.OutboundDraftSearch.Number.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.Number = advancedSearchVM.OutboundDraftSearch.Number;
                            }

                            searchCriteriaByOutboundDraftDTO.TypeId = advancedSearchVM.OutboundDraftSearch.TransactionTypeId;

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.FromPartyId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DestinationPartyId.Value;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.DirectedToUserId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.DirectedToId.Value.ToString();
                            }

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.StatusId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.StatusId.Value;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.PriorityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.PriorityId.Value;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.HasValue)
                            {
                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.ConfidentialityId = advancedSearchVM.OutboundDraftSearch.AdvancedSearch.ConfidentialityId.Value;
                            }

                            if (advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications != null
                          && advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Count > 0)
                            {
                                string subjectClassifications =
                                    string.Join(",", advancedSearchVM.OutboundDraftSearch.AdvancedSearch.SubjectClassifications.Select(n => n.ToString()).ToArray());

                                searchCriteriaByOutboundDraftDTO.AdvancedSearch.SubjectClassifications = subjectClassifications;
                            }
                            searchCriteriaByOutboundDraftDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByOutboundDraftDTO.HasFullPrivilege = HasPermissionSearch;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByOutboundDraftDTO.Global = true;

                            }
                            GetResult<List<OutboundDraftSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<OutboundDraftSearchResultDTO>>>.PostRequest("api/Search/OutboundDraftSearch", searchCriteriaByOutboundDraftDTO).Result;

                            List<OutboundDraftSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");
                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundDraftSearchGridPartial", grid), Type = (TransactionCategory.DraftOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchBySubject:
                        {
                            SearchCriteriaBySubjectDTO searchSubjectCriteriaDTO = new SearchCriteriaBySubjectDTO();

                            searchSubjectCriteriaDTO.Subject = advancedSearchVM.SubjectSearch.Subject;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchSubjectCriteriaDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectSearch.Year != null)
                            {
                                searchSubjectCriteriaDTO.Year = advancedSearchVM.SubjectSearch.Year;
                            }

                            searchSubjectCriteriaDTO.CultureName = SessionInfo.CultureShortName;
                            searchSubjectCriteriaDTO.OrderBy = "";
                            searchSubjectCriteriaDTO.PageIndex = page - 1 ?? 0;
                            searchSubjectCriteriaDTO.PageSize = PageSize;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {


                                searchSubjectCriteriaDTO.UserId = -1;
                                searchSubjectCriteriaDTO.Global = true;

                            }

                            switch ((TransactionCategory)advancedSearchVM.SubjectSearch.TransactionCategory)
                            {
                                case TransactionCategory.Inbound:
                                    {
                                        searchSubjectCriteriaDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

                                        searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                                        searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                                        GetResult<List<SubjectSearchResultDTO>> result =
                                                                      HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                                        List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                                        if (searchResultVMs.Count == 0)
                                        {
                                            message = DbRes.TValidation("User.Search.NoResult");

                                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                                        }

                                        IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (TransactionCategory.Inbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                                    }

                                case TransactionCategory.InternalOutbound:
                                    {
                                        searchSubjectCriteriaDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                                        searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                                        GetResult<List<SubjectSearchResultDTO>> result =
                                                                      HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                                        List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                                        if (searchResultVMs.Count == 0)
                                        {
                                            message = DbRes.TValidation("User.Search.NoResult");

                                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                                        }
                                        //int searchCount = 0;

                                        //IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, searchCount, false);
                                        //return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", searchResultVMs), Type = (TransactionCategory.Inbound).ToString() }, JsonRequestBehavior.AllowGet);
                                        IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (TransactionCategory.InternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                                    }

                                case TransactionCategory.DraftOutbound:
                                    {
                                        searchSubjectCriteriaDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                                        searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                                        GetResult<List<SubjectSearchResultDTO>> result =
                                                                      HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                                        List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                                        if (searchResultVMs.Count == 0)
                                        {
                                            message = DbRes.TValidation("User.Search.NoResult");
                                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                                        }
                                        IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (TransactionCategory.DraftOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                                    }
                                case TransactionCategory.ExternalOutbound:
                                    {
                                        searchSubjectCriteriaDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                                        searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                                        GetResult<List<SubjectSearchResultDTO>> result =
                                                                       HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                                        List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                                        if (searchResultVMs.Count == 0)
                                        {
                                            message = DbRes.TValidation("User.Search.NoResult");
                                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                                        }
                                        IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (TransactionCategory.ExternalOutbound).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                                    }
                                case TransactionCategory.All:
                                    {
                                        searchSubjectCriteriaDTO.TransactionCategoryId = (int)TransactionCategory.All;
                                        searchSubjectCriteriaDTO.UserId = SessionInfo.CurrentUser.Id;
                                        searchSubjectCriteriaDTO.HasFullPrivilege = HasPermissionSearch;
                                        GetResult<List<SubjectSearchResultDTO>> result =
                                                                       HttpClientWrapper<GetResult<List<SubjectSearchResultDTO>>>.PostRequest("api/Search/SubjectSearch", searchSubjectCriteriaDTO).Result;

                                        List<SubjectSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);
                                        if (searchResultVMs.Count == 0)
                                        {
                                            message = DbRes.TValidation("User.Search.NoResult");
                                            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                                        }
                                        IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                                        return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectSearchGridPartial", grid), Type = (TransactionCategory.All).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                                    }

                            }
                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByAssignTransaction:
                        {

                            SearchCriteriaByAssignTransactionDTO searchCriteriaByAssignTransactionDTO = new SearchCriteriaByAssignTransactionDTO();
                            if (advancedSearchVM.AssignTransactionSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateTo = advancedSearchVM.AssignTransactionSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignTransactionSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.DateFrom = advancedSearchVM.AssignTransactionSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignTransactionDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignTransactionDTO.OrderBy = "";
                            searchCriteriaByAssignTransactionDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByAssignTransactionDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignTransactionDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.AssignTransactionSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignTransactionDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateFrom =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignTransactionDTO.FromDateTime =
                                        searchCriteriaByAssignTransactionDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignTransactionSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignTransactionSearch.HourTo.Value,
                                    (advancedSearchVM.AssignTransactionSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignTransactionSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                        searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignTransactionDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignTransactionDTO.DateTo =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignTransactionDTO.ToDateTime =
                                    searchCriteriaByAssignTransactionDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            //if (advancedSearchVM.byCreatorSearch.Number.HasValue)
                            //{
                            //    searchCriteriaByEntityNameDTO.Number = advancedSearchVM.byCreatorSearch.Number.Value;

                            //}
                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignTransactionDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignTransactionDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignTransactionDTO.ToDateTime = dateValue;
                                }
                            }

                            searchCriteriaByAssignTransactionDTO.TransactionCategoryId = advancedSearchVM.AssignTransactionSearch.TransactionCategory;
                            searchCriteriaByAssignTransactionDTO.FromEntity = advancedSearchVM.AssignTransactionSearch.FromEntity;
                            searchCriteriaByAssignTransactionDTO.EntityId = advancedSearchVM.AssignTransactionSearch.EntityId.Value;
                            searchCriteriaByAssignTransactionDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignTransactionDTO.HasFullPrivilege = HasPermissionSearch;
                            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAll))
                            {
                                searchCriteriaByAssignTransactionDTO.Global = true;

                            }
                            GetResult<List<AssignTransactionSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<AssignTransactionSearchResultDTO>>>.PostRequest("api/Search/AssignTransactionSearch", searchCriteriaByAssignTransactionDTO).Result;

                            List<AssignTransactionSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignTransactionSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }


                    case SearchType.SearchByEntity:
                        {


                            SearchCriteriaByEntityNameDTO searchCriteriaByEntityNameDTO = new SearchCriteriaByEntityNameDTO();
                            if (advancedSearchVM.EntitySearch.DateTo.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateTo = advancedSearchVM.EntitySearch.DateTo.Value;

                            }
                            if (advancedSearchVM.EntitySearch.DateFrom.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.DateFrom = advancedSearchVM.EntitySearch.DateFrom.Value;

                            }
                            if (advancedSearchVM.EntitySearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.EntitySearch.HourFrom.Value,
                                    (advancedSearchVM.EntitySearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByEntityNameDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateFrom =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByEntityNameDTO.FromDateTime =
                                        searchCriteriaByEntityNameDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.EntitySearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.EntitySearch.HourTo.Value,
                                    (advancedSearchVM.EntitySearch.MinuteTo.HasValue ?
                                    advancedSearchVM.EntitySearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                        searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByEntityNameDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByEntityNameDTO.DateTo =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByEntityNameDTO.ToDateTime =
                                    searchCriteriaByEntityNameDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            //if (advancedSearchVM.EntitySearch.Number.HasValue)
                            //{
                            //    searchCriteriaByEntityNameDTO.Number = advancedSearchVM.EntitySearch.Number.Value;

                            //}
                            searchCriteriaByEntityNameDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByEntityNameDTO.OrderBy = "";
                            searchCriteriaByEntityNameDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByEntityNameDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByEntityNameDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByEntityNameDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByEntityNameDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByEntityNameDTO.ToDateTime = dateValue;
                                }
                            }

                            searchCriteriaByEntityNameDTO.TransactionCategoryId = advancedSearchVM.EntitySearch.TransactionCategoryId;
                            searchCriteriaByEntityNameDTO.ExternalPartyId = advancedSearchVM.EntitySearch.ExternalPartyId;
                            searchCriteriaByEntityNameDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByEntityNameDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<EntitySearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<EntitySearchResultDTO>>>.PostRequest("api/Search/EntitySearch", searchCriteriaByEntityNameDTO).Result;

                            List<EntitySearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            if (searchResultVMs[0].TransactionCategoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EntitySearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }

                    case SearchType.SearchByCreator:
                        {

                            SearchCriteriaByCreatorDTO searchCriteriaByCreatorDTO = new SearchCriteriaByCreatorDTO();
                            if (advancedSearchVM.CreatorSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateTo = advancedSearchVM.CreatorSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CreatorSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCreatorDTO.DateFrom = advancedSearchVM.CreatorSearch.DateFrom.Value;

                            }

                            searchCriteriaByCreatorDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCreatorDTO.OrderBy = "";
                            searchCriteriaByCreatorDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByCreatorDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCreatorDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.CreatorSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourFrom.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCreatorDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateFrom =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCreatorDTO.FromDateTime =
                                        searchCriteriaByCreatorDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.CreatorSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourTo.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                        searchCriteriaByCreatorDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCreatorDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCreatorDTO.DateTo =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCreatorDTO.ToDateTime =
                                    searchCriteriaByCreatorDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }
                            //if (advancedSearchVM.byCreatorSearch.Number.HasValue)
                            //{
                            //    searchCriteriaByEntityNameDTO.Number = advancedSearchVM.byCreatorSearch.Number.Value;

                            //}
                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCreatorDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCreatorDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCreatorDTO.ToDateTime = dateValue;
                                }
                            }

                            searchCriteriaByCreatorDTO.TransactionCategoryId = advancedSearchVM.CreatorSearch.TransactionCategory;
                            searchCriteriaByCreatorDTO.CreatorUserId = advancedSearchVM.CreatorSearch.UserId;
                            searchCriteriaByCreatorDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCreatorDTO.HasFullPrivilege = HasPermissionSearch;
                            GetResult<List<CreatorSearchResultDTO>> result =
                                HttpClientWrapper<GetResult<List<CreatorSearchResultDTO>>>.PostRequest("api/Search/CreatorSearch", searchCriteriaByCreatorDTO).Result;

                            List<CreatorSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CreatorSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByNames:
                        {
                            SearchCriteriaByNamesDTO searchCriteriaByNamesDTO = new SearchCriteriaByNamesDTO();
                            if (advancedSearchVM.CreatorSearch.DateTo.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateTo = advancedSearchVM.CreatorSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CreatorSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByNamesDTO.DateFrom = advancedSearchVM.CreatorSearch.DateFrom.Value;

                            }

                            searchCriteriaByNamesDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByNamesDTO.OrderBy = "";
                            searchCriteriaByNamesDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByNamesDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByNamesDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.CreatorSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourFrom.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByNamesDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateFrom =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByNamesDTO.FromDateTime =
                                        searchCriteriaByNamesDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.CreatorSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CreatorSearch.HourTo.Value,
                                    (advancedSearchVM.CreatorSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CreatorSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByNamesDTO.ToDateTime =
                                        searchCriteriaByNamesDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByNamesDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByNamesDTO.DateTo =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByNamesDTO.ToDateTime =
                                    searchCriteriaByNamesDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByNamesDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByNamesDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByNamesDTO.ToDateTime = dateValue;
                                }
                            }

                            searchCriteriaByNamesDTO.TransactionCategoryId = advancedSearchVM.CreatorSearch.TransactionCategory;
                            searchCriteriaByNamesDTO.FirstName = advancedSearchVM.NamesSearch.FirstName;
                            searchCriteriaByNamesDTO.SecondName = advancedSearchVM.NamesSearch.SecondName;
                            searchCriteriaByNamesDTO.ThirdName = advancedSearchVM.NamesSearch.ThirdName;
                            searchCriteriaByNamesDTO.FamilyName = advancedSearchVM.NamesSearch.FamilyName;
                            searchCriteriaByNamesDTO.SearchNamesType = advancedSearchVM.NamesSearch.SearchNamesType;
                            searchCriteriaByNamesDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByNamesDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<CreatorSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<CreatorSearchResultDTO>>>.PostRequest("api/Search/NamesSearch", searchCriteriaByNamesDTO).Result;

                            List<CreatorSearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }


                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_NamesSearchGridPartial", grid), Type = type, MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);


                        }
                    case SearchType.SearchDaily:
                        {
                            SearchCriteriaByDailyDTO searchCriteriaByDailyDTO = new SearchCriteriaByDailyDTO();

                            searchCriteriaByDailyDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByDailyDTO.OrderBy = "";
                            searchCriteriaByDailyDTO.PageIndex = page ?? 0; ;
                            searchCriteriaByDailyDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByDailyDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByDailyDTO.TodayDate = DateTime.Now;

                            searchCriteriaByDailyDTO.UserId = SessionInfo.CurrentUser.Id;


                            GetResult<List<DailySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<DailySearchResultDTO>>>.PostRequest("api/Search/DailySearch", searchCriteriaByDailyDTO).Result;

                            List<SearchCriteriaByDailyResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_DailySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);


                        }
                    case SearchType.SearchByAssignmentNote:
                        {

                            SearchCriteriaByAssignmentNoteDTO searchCriteriaByAssignmentNoteDTO = new SearchCriteriaByAssignmentNoteDTO();

                            if (advancedSearchVM.AssignmentNoteSearch.DateTo.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateTo = advancedSearchVM.AssignmentNoteSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.AssignmentNoteSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.DateFrom = advancedSearchVM.AssignmentNoteSearch.DateFrom.Value;

                            }

                            searchCriteriaByAssignmentNoteDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByAssignmentNoteDTO.OrderBy = "";
                            searchCriteriaByAssignmentNoteDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByAssignmentNoteDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByAssignmentNoteDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.AssignmentNoteSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourFrom.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByAssignmentNoteDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateFrom =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByAssignmentNoteDTO.FromDateTime =
                                        searchCriteriaByAssignmentNoteDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.AssignmentNoteSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.AssignmentNoteSearch.HourTo.Value,
                                    (advancedSearchVM.AssignmentNoteSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.AssignmentNoteSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                        searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByAssignmentNoteDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByAssignmentNoteDTO.DateTo =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByAssignmentNoteDTO.ToDateTime =
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByAssignmentNoteDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByAssignmentNoteDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByAssignmentNoteDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;


                            searchCriteriaByAssignmentNoteDTO.AssignmentNote = advancedSearchVM.AssignmentNoteSearch.AssignmentNote;

                            searchCriteriaByAssignmentNoteDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByAssignmentNoteDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<AssignmentNoteSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<AssignmentNoteSearchResultDTO>>>.PostRequest("api/Search/AssignmentNoteSearch", searchCriteriaByAssignmentNoteDTO).Result;

                            List<SearchCriteriaByAssignmentNoteResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_AssignmentNoteSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);






                        }
                    case SearchType.SearchByManifestNumber:
                        {
                            SearchCriteriaByManifestNumberDTO searchCriteriaByManifestNumberDTO = new SearchCriteriaByManifestNumberDTO();
                            if (advancedSearchVM.ManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateTo = advancedSearchVM.ManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.DateFrom = advancedSearchVM.ManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByManifestNumberDTO.OrderBy = "";
                            searchCriteriaByManifestNumberDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByManifestNumberDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateFrom =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByManifestNumberDTO.FromDateTime =
                                        searchCriteriaByManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                        searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByManifestNumberDTO.DateTo =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByManifestNumberDTO.ToDateTime =
                                    searchCriteriaByManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;



                            searchCriteriaByManifestNumberDTO.ManifestNumber = advancedSearchVM.ManifestNumberSearch.ManifestNumber;
                            searchCriteriaByManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ManifestNumberSearch", searchCriteriaByManifestNumberDTO).Result;

                            List<SearchCriteriaByManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchByMilitaryNumberOrIdentity:
                        {
                            SearchCriteriaByMilitaryNumberOrIdentityDTO searchCriteriaByMilitaryNumberOrIdentityDTO = new SearchCriteriaByMilitaryNumberOrIdentityDTO();

                            if (advancedSearchVM.IdentificationNumber.DateTo.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo = advancedSearchVM.IdentificationNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.IdentificationNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom = advancedSearchVM.IdentificationNumber.DateFrom.Value;

                            }

                            searchCriteriaByMilitaryNumberOrIdentityDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.OrderBy = "";
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByMilitaryNumberOrIdentityDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.IdentificationNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourFrom.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.IdentificationNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.IdentificationNumber.HourTo.Value,
                                    (advancedSearchVM.IdentificationNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.IdentificationNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime =
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByMilitaryNumberOrIdentityDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByMilitaryNumberOrIdentityDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((SearchType)advancedSearchVM.IdentificationNumber.TransactionTypeId)
                            {
                                case SearchType.SearchByInboundNumber:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.Inbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundInternalNumber:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.InternalOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundDraftNumber:
                                    {
                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.DraftOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundNumber:
                                    {

                                        searchCriteriaByMilitaryNumberOrIdentityDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.ExternalOutbound;
                                        break;
                                    }
                            }


                            searchCriteriaByMilitaryNumberOrIdentityDTO.IdentificationNumber = advancedSearchVM.IdentificationNumber.IdentificationNumber;

                            searchCriteriaByMilitaryNumberOrIdentityDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByMilitaryNumberOrIdentityDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>>.PostRequest("api/Search/MilitaryNumberOrIdentitySearch", searchCriteriaByMilitaryNumberOrIdentityDTO).Result;

                            List<SearchCriteriaByMilitaryNumberOrIdentityResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_MilitaryNumberOrIdentitySearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }
                    case SearchType.SearchByTransactionNots:
                        {
                            SearchCriteriaByTransactionNotsDTO searchCriteriaByTransactionNotsDTO = new SearchCriteriaByTransactionNotsDTO();

                            if (advancedSearchVM.TransactionNotsSearch.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateTo = advancedSearchVM.TransactionNotsSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNotsSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.DateFrom = advancedSearchVM.TransactionNotsSearch.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNotsDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNotsDTO.OrderBy = "";
                            searchCriteriaByTransactionNotsDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByTransactionNotsDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNotsDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNotsSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourFrom.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNotsDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateFrom =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNotsDTO.FromDateTime =
                                        searchCriteriaByTransactionNotsDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNotsSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNotsSearch.HourTo.Value,
                                    (advancedSearchVM.TransactionNotsSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNotsSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                        searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNotsDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNotsDTO.DateTo =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNotsDTO.ToDateTime =
                                    searchCriteriaByTransactionNotsDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNotsDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNotsDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNotsDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((SearchType)advancedSearchVM.TransactionNotsSearch.TransactionTypeId)
                            {
                                case SearchType.SearchByInboundNumber:
                                    {
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.Inbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundInternalNumber:
                                    {
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.InternalOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundDraftNumber:
                                    {
                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.DraftOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundNumber:
                                    {

                                        searchCriteriaByTransactionNotsDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.ExternalOutbound;
                                        break;
                                    }
                            }
                            searchCriteriaByTransactionNotsDTO.TransactionNots = advancedSearchVM.TransactionNotsSearch.TransactionNots;
                            searchCriteriaByTransactionNotsDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNotsDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNotsSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNotsSearchResultDTO>>>.PostRequest("api/Search/TransactionNotsSearch", searchCriteriaByTransactionNotsDTO).Result;

                            List<SearchCriteriaByTransactionNotsResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNotsSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByELcEmployee:
                        {

                            SearchCriteriaByElcEmployeeDTO searchCriteriaByElcEmployeeDTO = new SearchCriteriaByElcEmployeeDTO();

                            if (advancedSearchVM.ElcEmployeeSearch.DateTo.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateTo = advancedSearchVM.ElcEmployeeSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ElcEmployeeSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.DateFrom = advancedSearchVM.ElcEmployeeSearch.DateFrom.Value;

                            }

                            searchCriteriaByElcEmployeeDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByElcEmployeeDTO.OrderBy = "";
                            searchCriteriaByElcEmployeeDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByElcEmployeeDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByElcEmployeeDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ElcEmployeeSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourFrom.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByElcEmployeeDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateFrom =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByElcEmployeeDTO.FromDateTime =
                                        searchCriteriaByElcEmployeeDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ElcEmployeeSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ElcEmployeeSearch.HourTo.Value,
                                    (advancedSearchVM.ElcEmployeeSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ElcEmployeeSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                        searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByElcEmployeeDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByElcEmployeeDTO.DateTo =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByElcEmployeeDTO.ToDateTime =
                                    searchCriteriaByElcEmployeeDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByElcEmployeeDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByElcEmployeeDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByElcEmployeeDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByElcEmployeeDTO.ElcEmployeeId = advancedSearchVM.ElcEmployeeSearch.ElcEmployeeId;

                            searchCriteriaByElcEmployeeDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByElcEmployeeDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ELcEmployeeSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ELcEmployeeSearchResultDTO>>>.PostRequest("api/Search/ELcEmployeeSearch", searchCriteriaByElcEmployeeDTO).Result;

                            List<SearchCriteriaByElcEmployeeResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ElcEmployeeSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);




                        }
                    case SearchType.SearchByExternalOutBoundOrManifestNumber:
                        {


                            SearchCriteriaByExternalOutBoundOrManifestNumberDTO searchCriteriaByExternalOutBoundOrManifestNumberDTO = new SearchCriteriaByExternalOutBoundOrManifestNumberDTO();

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.DateFrom.Value;

                            }

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrderBy = "";
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByExternalOutBoundOrManifestNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourFrom.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.HourTo.Value,
                                    (advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                        searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime =
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByExternalOutBoundOrManifestNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByExternalOutBoundOrManifestNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                            transactionCategory = TransactionCategory.ExternalOutbound;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.Number = advancedSearchVM.ExternalOutBoundOrManifestNumberSearch.Number;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByExternalOutBoundOrManifestNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>>.PostRequest("api/Search/ExternalOutBoundOrManifestNumberSearch", searchCriteriaByExternalOutBoundOrManifestNumberDTO).Result;

                            List<SearchCriteriaByExternalOutBoundOrManifestNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ExternalOutBoundOrManifestNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);
                        }
                    case SearchType.SearchByCopyAssignemnt:
                        {
                            SearchCriteriaByCopyAssignemntDTO searchCriteriaByCopyAssignemntDTO = new SearchCriteriaByCopyAssignemntDTO();
                            if (advancedSearchVM.CopyAssignemntSearch.DateTo.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateTo = advancedSearchVM.CopyAssignemntSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.CopyAssignemntSearch.DateFrom.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.DateFrom = advancedSearchVM.CopyAssignemntSearch.DateFrom.Value;

                            }

                            searchCriteriaByCopyAssignemntDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByCopyAssignemntDTO.OrderBy = "";
                            searchCriteriaByCopyAssignemntDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByCopyAssignemntDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByCopyAssignemntDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.CopyAssignemntSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourFrom.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByCopyAssignemntDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateFrom =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByCopyAssignemntDTO.FromDateTime =
                                        searchCriteriaByCopyAssignemntDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.CopyAssignemntSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.CopyAssignemntSearch.HourTo.Value,
                                    (advancedSearchVM.CopyAssignemntSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.CopyAssignemntSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                        searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByCopyAssignemntDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByCopyAssignemntDTO.DateTo =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByCopyAssignemntDTO.ToDateTime =
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByCopyAssignemntDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByCopyAssignemntDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByCopyAssignemntDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;



                            searchCriteriaByCopyAssignemntDTO.FromEntityId = advancedSearchVM.CopyAssignemntSearch.FromEntityId;
                            searchCriteriaByCopyAssignemntDTO.ToEntityId = advancedSearchVM.CopyAssignemntSearch.ToEntityId;
                            searchCriteriaByCopyAssignemntDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByCopyAssignemntDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<CopyAssignemntSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<CopyAssignemntSearchResultDTO>>>.PostRequest("api/Search/CopyAssignemntSearch", searchCriteriaByCopyAssignemntDTO).Result;

                            List<SearchCriteriaByCopyAssignemntResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_CopyAssignemntSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);

                        }
                    case SearchType.SearchBySubjectLetter:
                        {
                            SearchCriteriaBySubjectLetterDTO searchCriteriaBySubjectLetterDTO = new SearchCriteriaBySubjectLetterDTO();
                            if (advancedSearchVM.SubjectLetterSearch.DateTo.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateTo = advancedSearchVM.SubjectLetterSearch.DateTo.Value;

                            }
                            if (advancedSearchVM.SubjectLetterSearch.DateFrom.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.DateFrom = advancedSearchVM.SubjectLetterSearch.DateFrom.Value;

                            }

                            searchCriteriaBySubjectLetterDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaBySubjectLetterDTO.OrderBy = "";
                            searchCriteriaBySubjectLetterDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaBySubjectLetterDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaBySubjectLetterDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.SubjectLetterSearch.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourFrom.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteFrom.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaBySubjectLetterDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateFrom =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaBySubjectLetterDTO.FromDateTime =
                                        searchCriteriaBySubjectLetterDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.SubjectLetterSearch.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.SubjectLetterSearch.HourTo.Value,
                                    (advancedSearchVM.SubjectLetterSearch.MinuteTo.HasValue ?
                                    advancedSearchVM.SubjectLetterSearch.MinuteTo.Value : 0), 0);


                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                        searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaBySubjectLetterDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaBySubjectLetterDTO.DateTo =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaBySubjectLetterDTO.ToDateTime =
                                    searchCriteriaBySubjectLetterDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaBySubjectLetterDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaBySubjectLetterDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaBySubjectLetterDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((SearchType)advancedSearchVM.SubjectLetterSearch.TransactionTypeId)
                            {
                                case SearchType.SearchByInboundNumber:
                                    {
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.Inbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundInternalNumber:
                                    {
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.InternalOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundDraftNumber:
                                    {
                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.DraftOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundNumber:
                                    {

                                        searchCriteriaBySubjectLetterDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.ExternalOutbound;
                                        break;
                                    }
                            }
                            searchCriteriaBySubjectLetterDTO.FirstLetter = advancedSearchVM.SubjectLetterSearch.FirstLetter;
                            searchCriteriaBySubjectLetterDTO.SecondLetter = advancedSearchVM.SubjectLetterSearch.SecondLetter;
                            searchCriteriaBySubjectLetterDTO.ThirdLetter = advancedSearchVM.SubjectLetterSearch.ThirdLetter;
                            searchCriteriaBySubjectLetterDTO.FourthLetter = advancedSearchVM.SubjectLetterSearch.FourthLetter;
                            searchCriteriaBySubjectLetterDTO.SearchTypeForFiltersId = advancedSearchVM.SubjectLetterSearch.SearchTypeForFiltersId;
                            searchCriteriaBySubjectLetterDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaBySubjectLetterDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<SubjectLetterSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<SubjectLetterSearchResultDTO>>>.PostRequest("api/Search/SubjectLetterSearch", searchCriteriaBySubjectLetterDTO).Result;

                            List<SearchCriteriaBySubjectLetterResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }
                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, (int)page - 1, (int)searchResultVMs[0].TotalCount, true, PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SubjectLetterSearchGridPartial", grid), Type = type.ToString().ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);



                        }
                    case SearchType.SearchByTransactionNumber:
                        {
                            SearchCriteriaByTransactionNumberDTO searchCriteriaByTransactionNumberDTO = new SearchCriteriaByTransactionNumberDTO();

                            if (advancedSearchVM.TransactionNumber.DateTo.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateTo = advancedSearchVM.TransactionNumber.DateTo.Value;

                            }
                            if (advancedSearchVM.TransactionNumber.DateFrom.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.DateFrom = advancedSearchVM.TransactionNumber.DateFrom.Value;

                            }

                            searchCriteriaByTransactionNumberDTO.CultureName = SessionInfo.CultureShortName;
                            searchCriteriaByTransactionNumberDTO.OrderBy = "";
                            searchCriteriaByTransactionNumberDTO.PageIndex = page - 1 ?? 0; ;
                            searchCriteriaByTransactionNumberDTO.PageSize = PageSize;

                            if (advancedSearchVM.OrgUnitId.HasValue)
                            {
                                searchCriteriaByTransactionNumberDTO.OrgUnitId = advancedSearchVM.OrgUnitId;
                            }
                            if (advancedSearchVM.TransactionNumber.HourFrom.HasValue)
                            {
                                TimeSpan fromTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourFrom.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteFrom.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteFrom.Value : 0), 0);

                                if (searchCriteriaByTransactionNumberDTO.FromDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateFrom =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);

                                    searchCriteriaByTransactionNumberDTO.FromDateTime =
                                        searchCriteriaByTransactionNumberDTO.FromDateTime.Value.Add(fromTime);
                                }
                            }

                            if (advancedSearchVM.TransactionNumber.HourTo.HasValue)
                            {
                                TimeSpan toTime = new TimeSpan(advancedSearchVM.TransactionNumber.HourTo.Value,
                                    (advancedSearchVM.TransactionNumber.MinuteTo.HasValue ?
                                    advancedSearchVM.TransactionNumber.MinuteTo.Value : 0), 0);


                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                        searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(toTime);
                                }
                            }
                            else
                            {
                                if (searchCriteriaByTransactionNumberDTO.ToDateTime.HasValue)
                                {
                                    searchCriteriaByTransactionNumberDTO.DateTo =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));

                                    searchCriteriaByTransactionNumberDTO.ToDateTime =
                                    searchCriteriaByTransactionNumberDTO.ToDateTime.Value.Add(new TimeSpan(23, 59, 59));
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateFrom.ToString()))
                            {
                                string[] dateFormats = ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateFrom.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.FromDateTime = dateValue;
                                }
                            }

                            if (!string.IsNullOrEmpty(searchCriteriaByTransactionNumberDTO.DateTo.ToString()))
                            {
                                string[] dateFormats = System.Configuration.ConfigurationManager.AppSettings["SystemDateFormats"].Split(',');
                                DateTime dateValue;

                                if (DateTime.TryParseExact(searchCriteriaByTransactionNumberDTO.DateTo.ToString(), dateFormats, null, System.Globalization.DateTimeStyles.None, out dateValue))
                                {
                                    searchCriteriaByTransactionNumberDTO.ToDateTime = dateValue;
                                }
                            }
                            TransactionCategory transactionCategory = TransactionCategory.None;

                            switch ((SearchType)advancedSearchVM.TransactionNumber.TransactionTypeId)
                            {
                                case SearchType.SearchByInboundNumber:
                                    {
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.Inbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundInternalNumber:
                                    {
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.InternalOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundDraftNumber:
                                    {
                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.DraftOutbound;
                                        break;
                                    }

                                case SearchType.SearchByOutboundNumber:
                                    {

                                        searchCriteriaByTransactionNumberDTO.TransactionCategoryId = TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
                                        transactionCategory = TransactionCategory.ExternalOutbound;
                                        break;
                                    }
                            }


                            searchCriteriaByTransactionNumberDTO.TransactionNumber = advancedSearchVM.TransactionNumber.TransactionNumber;

                            searchCriteriaByTransactionNumberDTO.UserId = SessionInfo.CurrentUser.Id;
                            searchCriteriaByTransactionNumberDTO.HasFullPrivilege = HasPermissionSearch;

                            GetResult<List<TransactionNumberSearchResultDTO>> result =
                                  HttpClientWrapper<GetResult<List<TransactionNumberSearchResultDTO>>>.PostRequest("api/Search/TransactionNumberSearch", searchCriteriaByTransactionNumberDTO).Result;

                            List<SearchCriteriaByTransactionNumberResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

                            if (searchResultVMs.Count == 0)
                            {
                                message = DbRes.TValidation("User.Search.NoResult");

                                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);

                            }
                            string type = "";
                            int TransactionResultType = searchResultVMs[0].TransactionCategoryId;

                            if (TransactionResultType == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.Inbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.ExternalOutbound.ToString();
                            }
                            else if (TransactionResultType == TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                            {
                                type = TransactionCategory.DraftOutbound.ToString();
                            }
                            else
                            {
                                type = TransactionCategory.InternalOutbound.ToString();
                            }

                            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, page ?? 0, (int)searchResultVMs[0].TotalCount, false, settingVM.Value != null ? Convert.ToInt32(settingVM.Value) : UIHelper.PageSize);
                            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_TransactionNumberSearchGridPartial", grid), Type = (transactionCategory).ToString(), MessageText = message, MessageType = MessageType.Information, Param = searchCriteria }, JsonRequestBehavior.AllowGet);

                            return Json(new { }, JsonRequestBehavior.AllowGet);





                        }

                }
                return Json(new { }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult AdvancedQuery(string inquirySearch, int inquiryType, int hdnYear)
        {
            try
            {
                ViewData["TagsData"] = null;
                string message = string.Empty;
                string number = string.Empty;

                if (!SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.InquirybyTransactionNumber))
                {
                    inquiryType = (int)InquiryType.TransactionNumber;
                }
                GetResult<List<InquirySearchResultDTO>> result =
                    HttpClientWrapper<GetResult<List<InquirySearchResultDTO>>>.PostRequest(string.Format("api/Search/InquirySearch?TransactionNumber={0}&InquiryType={1}&YearH={2}&DestinationId={3}&Subject={4}&entityId={5}", inquirySearch, inquiryType, hdnYear, null, "------", SessionInfo.OrgUnitId), null).Result;
                List<InquirySearchResultVM> searchResultVMs = SearchResultMapper.Map(result.Result);

                if (searchResultVMs.Count == 0)
                {
                    message = DbRes.TValidation("User.Search.NoResult");
                    ViewData["TagsData"] = false;
                    return View("~/Areas/User/Views/Search/AdvancedQuery.cshtml");
                }
                IAjaxGrid grid = (AjaxGrid<InquirySearchResultVM>)new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 0, searchResultVMs.Count, false, searchResultVMs.Count);
                return View("~/Areas/User/Views/Search/AdvancedQuery.cshtml", grid);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string GeSearchTypeForFilters()
        {

            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            dataSource.Add(new AutoCompleteDataSource() { Value = "1", Label = "و" });
            dataSource.Add(new AutoCompleteDataSource() { Value = "2", Label = "او" });
            dataSource.Add(new AutoCompleteDataSource() { Value = "3", Label = "مطابق" });

            return JsonConvert.SerializeObject(dataSource);

        }

    }
}