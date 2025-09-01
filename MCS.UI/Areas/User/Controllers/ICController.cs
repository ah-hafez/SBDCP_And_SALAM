using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.Framework.MultiTenants;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO.HubTransaction;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Hub;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using sharedDocumentVM = MCS.UI.Areas.User.Models.Shared;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Archives;
using MCS.UI.Areas.User.Models.Search;
using MCS.UI.Areas.User.Mappers.Search;
using MCS.UI.Areas.Admin.Mappers;
using MobileApi.Domain;
using MCS.Framework.Controls;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.User.Controllers
{
    public class ICController : BaseController
    {
        public bool HasPermissionSearch
        {
            get
            {
                return SessionInfo.CurrentUser?.Claims.Contains("Search.ShowAllTransactions") == true ? true : false;
            }
        }


        [HttpGet]
        public ActionResult Index()
        {
            return View("~/Areas/User/Views/File/_AddIC.cshtml");
        }
        [HttpGet]
        public ActionResult AddIcSubjectView()
        {

            ArchivDirectory archivDirectory = new ArchivDirectory();
            ViewData["IcClassifications"] = TransactionHelper.GetClassifications();
            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            return View("~/Areas/User/Views/IC/_AddIC.cshtml", archivDirectory);
        }
        [HttpGet]
        public ActionResult AddIcSubject(string ITEM_CODE, string ITEM_DISPLAY, int PARENT_ID, string DirectoryNum, int classificationId)
        {
            try
            {

                IC_SUBJECTDTO icSubjectDTO = new IC_SUBJECTDTO();

                icSubjectDTO.ACTIVE = true;
                icSubjectDTO.ITEM_CODE = ITEM_CODE;
                icSubjectDTO.ITEM_DESCRIPTION_AR = ITEM_DISPLAY;
                icSubjectDTO.ITEM_DISPLAY = ITEM_DISPLAY;
                icSubjectDTO.PARENT_ID = PARENT_ID;
                icSubjectDTO.DirectoryNum = DirectoryNum;
                //icSubjectDTO.ClassificationId = classificationId;
                var postResult = HttpClientWrapper<PostResult>.PostRequest("api/IC/AddIcSubject", icSubjectDTO).Result;

                return Json(new { NewIC = postResult.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult UpdateIC_SUBJECT(int id, string ITEM_CODE, string ITEM_DISPLAY, string DirectoryNum)
        {
            try
            {

                IC_SUBJECTDTO icSubjectDTO = new IC_SUBJECTDTO();

                icSubjectDTO.ACTIVE = true;
                icSubjectDTO.ITEM_CODE = ITEM_CODE;
                icSubjectDTO.ITEM_DESCRIPTION_AR = ITEM_DISPLAY;
                icSubjectDTO.ITEM_DISPLAY = ITEM_DISPLAY;

                icSubjectDTO.Id = id;
                icSubjectDTO.DirectoryNum = DirectoryNum;
                var postResult = HttpClientWrapper<PostResult>.PostRequest("api/IC/UpdateIC_SUBJECT", icSubjectDTO).Result;

                return Json(new { Result = postResult.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult UpdateIC_SUBJECTView(int id)
        {
            try
            {
                if (id == 1)
                {
                    return null;
                }
                GetResult<IC_SUBJECTDTO> result =
            HttpClientWrapper<GetResult<IC_SUBJECTDTO>>.GetItemRequest("api/IC/GetIC_SUBJECTById?id=" + id).Result;

                return View("~/Areas/User/Views/IC/_EditeIC.cshtml", result.Result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetIC_SUBJECTById(int id)
        {
            try
            {


                GetResult<IC_SUBJECTDTO> result =
             HttpClientWrapper<GetResult<IC_SUBJECTDTO>>.GetItemRequest("api/IC/GetIC_SUBJECTById?id=" + id).Result;



                return Json(new { Node = result.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetIC_SUBJECTByParentId(string query)
        {
            try
            {


                GetResult<List<IC_SUBJECTDTO>> result =
             HttpClientWrapper<GetResult<List<IC_SUBJECTDTO>>>.GetItemRequest("api/IC/GetIC_SUBJECTByParentId?query=" + query + "&id=").Result;



                return Json(new { Node = result.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetIC_SUBJECTChildByParentId(int id, string query)
        {
            try
            {


                GetResult<List<IC_SUBJECTDTO>> result =
             HttpClientWrapper<GetResult<List<IC_SUBJECTDTO>>>.GetItemRequest("api/IC/GetIC_SUBJECTByParentId?query=" + query + "&id=" + id.ToString()).Result;

                return Json(new { Node = result.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult DeleteIC_SUBJECT(int id)
        {


            if (id == 1)
            {
                return null;
            }
            string message = string.Empty;
            DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/IC/DeleteIC_SUBJECT?id={0}", id)).Result;
            if (deleteResult.StatusCode != StatusCode.Ok)
            {
                message = "لا يمكن حذف الدليل حاليا , تأكد من وجود معاملات مرتبطة بالدليل  ";

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            message = "تمت عملية الحذف بنجاح";


            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Search(int year, string transNumber, int orgId, int type)
        {


            SearchCriteriaByICDTO searchCriteriaByICDTO = new SearchCriteriaByICDTO();

            searchCriteriaByICDTO.culutre = SessionInfo.CultureShortName;
            searchCriteriaByICDTO.orgId = orgId;
            searchCriteriaByICDTO.transNumber = transNumber;
            searchCriteriaByICDTO.year = year;
            searchCriteriaByICDTO.type = type;
            searchCriteriaByICDTO.userId = SessionInfo.CurrentUser.Id;



            GetResult<List<ICSearchResultDTO>> result =
                               HttpClientWrapper<GetResult<List<ICSearchResultDTO>>>.PostRequest("api/IC/ICSearch", searchCriteriaByICDTO).Result;


            List<SearchICTransactionResultVM> searchResultVMs = SearchResultMapper.Map(result.Result, HasPermissionSearch);

            IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(searchResultVMs, 1, 1, false, UIHelper.PageSize);


            //~/Areas/User/Views/File/_ClassificationCard.cshtml"
            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Search/_ICSearchGridPartial.cshtml", grid), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult AddIC_SUBJECT_TRANSACTION(int transId, int ic_id, int? number, string description)
        {
            try
            {

                IC_SUBJECTTransactionDTO icSubjectDTO = new IC_SUBJECTTransactionDTO
                {
                    Description = description,
                    IcId = ic_id,
                    Number = number,
                    TransactionId = transId,


                };

                
                var postResult = HttpClientWrapper<PostResult>.PostRequest("api/IC/AddIC_SUBJECT_TRANSACTION", icSubjectDTO).Result;

                return Json(new { Id = postResult.Id }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpGet]
        public ActionResult DeleteIC_SUBJECT_TRANSACTION(int id)
        {

            string message = string.Empty;
            DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/IC/DeleteIC_SUBJECT_Transaction?id={0}", id)).Result;
            if (deleteResult.StatusCode != StatusCode.Ok)
            {
                message = "يمكن الاسترجاع حاليا  ";

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            message = "تمت عملية الاسترجاع بنجاح";


            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult GetIC_TransactionById(int id)
        {
            try
            {

                GetResult<IC_SUBJECTTransactionDTO> result =
             HttpClientWrapper<GetResult<IC_SUBJECTTransactionDTO>>.GetItemRequest("api/IC/GetSubject_TransactionById?id=" + id.ToString()).Result;

                return Json(new { Result = result.Result }, JsonRequestBehavior.AllowGet); ;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        public ActionResult ICSubjectIndex(int? page)
        {


            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllModules))
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
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
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


            return View("~/Areas/User/Views/IC/ICSubjectIndex.cshtml");

        }




        [CustomAuthorizationAttribute(UserClaims.Files.File)]
        public ActionResult SubjectClassificationIndex(int? page)
        {


            if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Search.SearchAllModules))
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
                    HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;
                List<OrgUnitDTO> newList = new List<OrgUnitDTO>();
                orgUnitDTOs.Result.ParentId = -1;
                orgUnitDTOs.Result.HasChilds = false;
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


            return View("~/Areas/User/Views/IC/ICSubjectClassificationIndex.cshtml");

        }


        [HttpPost]
        public string GetUsersByOrgUnitId(int? id, bool addSelectOption = false)
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
                if (addSelectOption)
                {
                    dataSource = dataSource.Prepend(new AutoCompleteDataSource()
                    {
                        Value = (-1).ToString(),
                        Label = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Trays.Orgunit")
                    }).ToList();
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
