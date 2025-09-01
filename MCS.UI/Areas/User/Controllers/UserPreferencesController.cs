using FileSignatures;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using MCS.Framework.Controls;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.GridMvc.Helpers;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Permission;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Mappers.UserPreferences;
using MCS.UI.Areas.User.Mappers.UserPreferences.UserDelegation;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Areas.User.Models.UserPreferences;
using MCS.UI.Areas.User.Models.UserPreferences.UserDelegation;
using MCS.UI.Common;
using MCS.UI.Areas.User.Models.Groups;
using MCS.UI.Areas.User.Mappers.Groups;
using System.Security.Cryptography.X509Certificates;
using TXTextControl;
using NPOI.Util;
using System.Configuration;
using DotnetDaddy.DocumentConfig;
using DocumentFormat.OpenXml.Drawing.Charts;
using DotnetDaddy.DocumentViewer;
using System.Runtime.ConstrainedExecution;
using DocumentFormat.OpenXml.Spreadsheet;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using MCS.UI.Helpers;
using MvcSiteMapProvider.Linq;
using MCS.UI.Areas.User.Models.Transaction;
using OpenMcdf;
using DocumentFormat.OpenXml.Wordprocessing;
using MCS.UI.Areas.User.Mappers.Action;
using MCS.UI.Areas.User.Models.Actions;
using System.Globalization;
using System.Web;
using MCS.UI.Helpers.Extensions;

namespace MCS.UI.Areas.User.Controllers
{
    public class UserPreferencesController : BaseController
    {
        private static readonly byte[] EXE_DLL = { 77, 90 };
        private static readonly byte[] RAR = { 82, 97, 114, 33, 26, 7, 0 };

        private int count = 0;
        [CustomAuthorize()]
        [HttpGet]
        public ActionResult Index()
        {
            UserPreferenceVM userPreferenceVM;

            GetResult<UserPreferenceDTO> userPreferenceResult =
             HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}&orgUnitId={2}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName, SessionInfo.OrgUnitId)).Result;

            UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);
            if (userPreferenceVMS != null)
            {
                userPreferenceVM = userPreferenceVMS;

                if (userPreferenceVM.SignatureDoc != null)
                {
                    ViewData["SignatureImgBase64"] = Convert.ToBase64String(userPreferenceVM.SignatureDoc);

                }
                if (userPreferenceVM.SignatureBehalfDoc != null)
                {
                    ViewData["SignatureBehalfImgBase64"] = Convert.ToBase64String(userPreferenceVM.SignatureBehalfDoc);

                }
                if (userPreferenceVM.SignatureCommandDoc != null)
                {
                    ViewData["SignatureCommandImgBase64"] = Convert.ToBase64String(userPreferenceVM.SignatureCommandDoc);

                }
                if (userPreferenceVM.MarkingDoc != null)
                {
                    ViewData["MarkingImgBase64"] = Convert.ToBase64String(userPreferenceVM.MarkingDoc);
                }
                if (userPreferenceVM.MessageSignatureDoc != null)
                {
                    ViewData["MessageSignatureImgBase64"] = Convert.ToBase64String(userPreferenceVM.MessageSignatureDoc);
                }
                if (userPreferenceVM.SealSignatureDoc != null)
                {
                    ViewData["SealSignatureImgBase64"] = Convert.ToBase64String(userPreferenceVM.SealSignatureDoc);
                }
                if(userPreferenceVM.NotificationSubscriptions == null || userPreferenceVM.NotificationSubscriptions.Count == 0)
                {
                    GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.NotificationSubscriptions, SessionInfo.CultureShortName);
                    IList<LookupVM> lookupVMs = lookups.Result;
                    List<int> subscriptionins = new List<int> { 1 , 3 , 6 , 7 , 8 , 9};
                    userPreferenceVM.NotificationSubscriptions = lookups.Result.Where(l => !subscriptionins.Any(s => s == l.EnumReference)).Select(l => new NotificationSubscriptionVM { Id = (l.EnumReference != null) ? l.EnumReference.Value : -1, Name = l.Text }).ToList();
                }
                else
                {
                    List<int> subscriptionins = new List<int> { 1 , 4 , 32 , 64 , 128 , 256};
                    userPreferenceVM.NotificationSubscriptions = userPreferenceVM.NotificationSubscriptions.Where(l => !subscriptionins.Any(s => s == l.Id)).ToList();
                }
            }
            else
            {
                userPreferenceVM = new UserPreferenceVM();
                userPreferenceVM.Id = 0;

                GetResult<List<UserTrayPreferencesDTO>> trayDetailsDTOs =
                     HttpClientWrapper<GetResult<List<UserTrayPreferencesDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.NotificationSubscriptions, SessionInfo.CultureShortName);
                IList<LookupVM> lookupVMs = lookups.Result;
                List<int> subscriptionins = new List<int> { 1, 6, 7, 8, 9 };
                userPreferenceVM.NotificationSubscriptions = lookups.Result.Where(l => !subscriptionins.Any(s => s == l.EnumReference)).Select(l => new NotificationSubscriptionVM { Id = (l.EnumReference != null) ? l.EnumReference.Value : -1, Name = l.Text }).ToList();
                userPreferenceVM.UserTrays = UserTrayPreferencesMapper.Map(trayDetailsDTOs.Result);
                userPreferenceVM.UserId = SessionInfo.CurrentUser.Id;
            }

            userPreferenceVM.Signature = SignType.KeepPresent;
            if (userPreferenceVM.SignatureDoc == null)
            {
                userPreferenceVM.Signature = SignType.Delete;
            }
            userPreferenceVM.MessageSignature = SignType.KeepPresent;
            if (userPreferenceVM.MessageSignatureDoc == null)
            {
                userPreferenceVM.MessageSignature = SignType.Delete;
            }
            userPreferenceVM.SealSignature = SignType.KeepPresent;
            if (userPreferenceVM.SealSignatureDoc == null)
            {
                userPreferenceVM.SealSignature = SignType.Delete;
            }
            userPreferenceVM.SignatureBehalf = SignType.KeepPresent;
            if (userPreferenceVM.SignatureBehalfDoc == null)
            {
                userPreferenceVM.SignatureBehalf = SignType.Delete;
            }
            userPreferenceVM.SignatureCommand = SignType.KeepPresent;
            if (userPreferenceVM.SignatureCommandDoc == null)
            {
                userPreferenceVM.SignatureCommand = SignType.Delete;
            }
            userPreferenceVM.Marking = SignType.KeepPresent;
            if (userPreferenceVM.MarkingDoc == null)
            {
                userPreferenceVM.Marking = SignType.Delete;
            }

            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
            ViewData["Cultures"] = GetCultures();
            ViewData["Theme"] = GetThemes();
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            ViewData["ConfidentialityData"] = GetConfidentialityLevel();
            ViewData["PrioritiesData"] = GetPriorities();
            ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups();
            ViewData["GridData"] = (AjaxGrid<UserDelegationClientAddViewModel>)new AjaxGridFactory().CreateAjaxGrid(new List<UserDelegationClientAddViewModel>(), 1, 1, false);
            ViewData["CurrentDelegationsData"] = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userPreferenceVM.CurrentDelegationUsers, 1, userPreferenceVM.DelegationUsers.Count, true);
            ViewData["CurrentDelegations"] = JsonConvert.SerializeObject(userPreferenceVM.CurrentDelegationUsers);
            return View(userPreferenceVM);
        }

        public ActionResult AllowedAssignment()
        {
            GetResult<List<AllowedAssignmentDTO>> orgUnitDTOs =
              HttpClientWrapper<GetResult<List<AllowedAssignmentDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAllowedAssignment?UserId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

            GetResult<List<OrgUnitDTO>> orgUnits =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnits.Result);
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            AllowedAssignmentVM allowedAssignmentVMs = new AllowedAssignmentVM();


            allowedAssignmentVMs.AllowedAssignmentList = AllowedAssignmentMapper.Map(orgUnitDTOs.Result);


            return View(allowedAssignmentVMs);
        }

        [HttpPost]
        public ActionResult AddAllowedAssignment(AllowedAssignmentVM allowedAssignmentVM)
        {
            AllowedAssignmentDTO allowedAssignmentDTOs = new AllowedAssignmentDTO();
            allowedAssignmentDTOs.UserId = SessionInfo.CurrentUser.Id;
            allowedAssignmentDTOs.EntityId = allowedAssignmentVM.EntityId;
            allowedAssignmentDTOs.ToUserId = allowedAssignmentVM.ToUserId;

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/AddAllowedAssignment", allowedAssignmentDTOs).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }


        [HttpGet]
        public ActionResult RemoveAllowedAssignment(int Id)
        {
            try
            {
                GetResult<bool> userResult = HttpClientWrapper<GetResult<bool>>.GetItemRequest(string.Format("api/UserProfile/RemoveAllowedAssignment?Id={0}", Id)).Result;
                return Json(new { result = userResult.Result }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult RequestRole()
        {
            GetResult<List<GroupDTO>> groups = HttpClientWrapper<GetResult<List<GroupDTO>>>
              .GetItemRequest(string.Format("api/Admin/GetAllUserDefinedGroups?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<GroupVM> GroupVMs = GroupMapper.Map(groups.Result).ToList();

            ViewData["Roles"] = GetGroupsAutoCompleteDataSource(GroupVMs);
            return View();
        }



        [HttpPost]
        public ActionResult RequestRole(int GroupId)
        {
            try
            {
                string message = string.Empty;

                GetResult<MCS.DTO.EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<MCS.DTO.EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", SessionInfo.CurrentUser.Id)).Result;
                int groupAlreadyExists = 0;
                groupAlreadyExists = userProfileEditDTO.Result.UserGroups.Where(u => u.GroupId == GroupId).Count();

                if (userProfileEditDTO.StatusCode != StatusCode.Ok)
                {


                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (groupAlreadyExists > 0)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.roleIsAlreadyExists");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                UserPendingGroupDTO userDelegationDTOs = new UserPendingGroupDTO();

                userDelegationDTOs.GroupId = GroupId;
                userDelegationDTOs.UserId = SessionInfo.CurrentUser.Id;



                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Common/RequestRoleItem", userDelegationDTOs).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information });


                //GetResult<List<UserProfileDTO>> userProfileDTOs =
                //HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Common/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, GroupId)).Result;
                //IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                //List<UserProfileVM> userProfileVMS = UserProfileMapper.Map(userProfileDTOs.Result);

                //if (userProfileVMS != null)
                //{
                //    foreach (UserProfileVM userProfileVM in userProfileVMS)
                //    {
                //        dataSource.Add(new AutoCompleteDataSource()
                //        {
                //            Value = userProfileVM.Id.ToString(),
                //            Label = userProfileVM.LocalName
                //        });
                //    }
                //}

                //return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [ValidateAntiForgeryToken()]
        public ActionResult AddUserPereference(UserPreferenceVM userPreferenceVM, string hdnUsersArray, string hdnSignatureImgBase64, string hdnSignatureBehalfImgBase64, string hdnMarkingImgBase64, string hdnSignatureCommandImgBase64, string hdnMessageSignatureImgBase64, string hdnSealSignatureImgBase64)
        {
            try
            {
                string message = string.Empty;
                switch (userPreferenceVM.Signature)
                {

                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files != null && Request.Files["SignatureFile"] != null && Request.Files["SignatureFile"].ContentLength > 0)
                            {
                                bool Valid = true; // IsValid(Request.Files[0].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["SignatureFile"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.SignatureDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnSignatureImgBase64))
                        {
                            hdnSignatureImgBase64 = hdnSignatureImgBase64.Substring(hdnSignatureImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.SignatureDoc = Convert.FromBase64String(hdnSignatureImgBase64);
                        }
                        break;
                }
                switch (userPreferenceVM.SignatureBehalf)
                {

                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files != null && Request.Files["SignatureBehalfFile"] != null && Request.Files["SignatureBehalfFile"].ContentLength > 0)
                            {
                                bool Valid = true; //IsValid(Request.Files[1].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["SignatureBehalfFile"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.SignatureBehalfDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnSignatureBehalfImgBase64))
                        {
                            hdnSignatureBehalfImgBase64 = hdnSignatureBehalfImgBase64.Substring(hdnSignatureBehalfImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.SignatureBehalfDoc = Convert.FromBase64String(hdnSignatureBehalfImgBase64);
                        }
                        break;
                }
                switch (userPreferenceVM.SignatureCommand)
                {

                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files != null && Request.Files["SignatureCommandFile"] != null && Request.Files["SignatureCommandFile"].ContentLength > 0)
                            {
                                bool Valid = true;//IsValid(Request.Files[2].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["SignatureCommandFile"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.SignatureCommandDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnSignatureCommandImgBase64))
                        {
                            hdnSignatureCommandImgBase64 = hdnSignatureCommandImgBase64.Substring(hdnSignatureCommandImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.SignatureCommandDoc = Convert.FromBase64String(hdnSignatureCommandImgBase64);
                        }
                        break;
                }
                switch (userPreferenceVM.MessageSignature)
                {
                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files != null && Request.Files["MessageSignature"] != null && Request.Files["MessageSignature"].ContentLength > 0)
                            {
                                bool Valid = true;//IsValid(Request.Files[1].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["MessageSignature"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.MessageSignatureDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnMessageSignatureImgBase64))
                        {
                            hdnMessageSignatureImgBase64 = hdnMessageSignatureImgBase64.Substring(hdnMessageSignatureImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.MessageSignatureDoc = Convert.FromBase64String(hdnMessageSignatureImgBase64);
                        }
                        break;
                }
                switch (userPreferenceVM.SealSignature)
                {
                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files != null && Request.Files["SealSignature"] != null && Request.Files["SealSignature"].ContentLength > 0)
                            {
                                bool Valid = true;//IsValid(Request.Files[1].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["SealSignature"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.SealSignatureDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnSealSignatureImgBase64))
                        {
                            hdnSealSignatureImgBase64 = hdnSealSignatureImgBase64.Substring(hdnSealSignatureImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.MessageSignatureDoc = Convert.FromBase64String(hdnSealSignatureImgBase64);
                        }
                        break;
                }
                switch (userPreferenceVM.Marking)
                {
                    case SignType.UploadFile:
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            if (Request.Files["MarkingFile"] != null && Request.Files["MarkingFile"].ContentLength > 0)
                            {
                                bool Valid = true;//IsValid(Request.Files[1].InputStream);
                                if (Valid == false)
                                {
                                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.User.InvalidFile");
                                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                                }
                                Request.Files["MarkingFile"].InputStream.CopyTo(memoryStream);
                                userPreferenceVM.MarkingDoc = memoryStream.ToArray();
                            }
                        };
                        break;
                    case SignType.SignOnScreen:
                    case SignType.Wacom:
                        if (!string.IsNullOrEmpty(hdnMarkingImgBase64))
                        {
                            hdnMarkingImgBase64 = hdnMarkingImgBase64.Substring(hdnMarkingImgBase64.IndexOf(",") + 1);
                            userPreferenceVM.MarkingDoc = Convert.FromBase64String(hdnMarkingImgBase64);
                        }
                        break;
                }

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();


                if (!string.IsNullOrEmpty(hdnUsersArray))
                {
                    userPreferenceVM.DelegationUsers.AddRange(javaScriptSerializer.Deserialize(hdnUsersArray, typeof(List<AddUserDelegationVM>)) as List<AddUserDelegationVM>);
                }
                if (userPreferenceVM.Id == 0)
                {
                    if (userPreferenceVM.Signature == SignType.Delete)
                    {
                        userPreferenceVM.SignatureDoc = null;
                    }
                    if (userPreferenceVM.SignatureBehalf == SignType.Delete)
                    {
                        userPreferenceVM.SignatureBehalfDoc = null;
                    }
                    if (userPreferenceVM.MessageSignature == SignType.Delete)
                    {
                        userPreferenceVM.MessageSignatureDoc = null;
                    }
                    if (userPreferenceVM.SealSignature == SignType.Delete)
                    {
                        userPreferenceVM.SealSignatureDoc = null;
                    }
                    if (userPreferenceVM.SignatureCommand == SignType.Delete)
                    {
                        userPreferenceVM.SignatureCommandDoc = null;
                    }

                    if (userPreferenceVM.Marking == SignType.Delete)
                    {
                        userPreferenceVM.MarkingDoc = null;
                    }


                    PostResult postResult = HttpClientWrapper<PostResult>
                        .PostRequest("api/UserProfile/PostUserPreference?cultureName=" 
                        + SessionInfo.CultureShortName 
                        + "&orgUnitId=" 
                        + SessionInfo.OrgUnitId,
                        userPreferenceVM).Result;

                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }


                    SessionInfo.SetObjectInSession(userPreferenceVM.MarkingDoc, "StampImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureDoc, "SignatureImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.MessageSignatureDoc, "MessageSignatureImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureDoc, "BarcodeImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureBehalfDoc, "SignatureBehalfImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureBehalfDoc, "BarcodeBehalfImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureCommandDoc, "SignatureCommandImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureCommandDoc, "BarcodeCommandImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.Email, "UserEmailAddress");

                    userPreferenceVM.Id = postResult.Id.Value;
                }
                else
                {
                    if (userPreferenceVM.SignatureDoc == null && userPreferenceVM.Signature != SignType.Delete)
                    {
                        userPreferenceVM.SignatureDoc = SessionInfo.CurrentUser.Signature;
                    }
                    if (userPreferenceVM.SignatureBehalfDoc == null && userPreferenceVM.SignatureBehalf != SignType.Delete)
                    {
                        userPreferenceVM.SignatureBehalfDoc = SessionInfo.CurrentUser.SignatureBehalf;
                    }
                    if (userPreferenceVM.MessageSignatureDoc == null && userPreferenceVM.MessageSignature != SignType.Delete)
                    {
                        userPreferenceVM.MessageSignatureDoc = SessionInfo.CurrentUser.MessageSignature;
                    }
                    if (userPreferenceVM.SealSignatureDoc == null && userPreferenceVM.SealSignature != SignType.Delete)
                    {
                        userPreferenceVM.SealSignatureDoc = SessionInfo.CurrentUser.SealSignatureDoc;
                    }
                    if (userPreferenceVM.SignatureCommandDoc == null && userPreferenceVM.SignatureCommand != SignType.Delete)
                    {
                        userPreferenceVM.SignatureCommandDoc = SessionInfo.CurrentUser.SignatureCommand;
                    }
                    if (userPreferenceVM.MarkingDoc == null && userPreferenceVM.Marking != SignType.Delete)
                    {
                        userPreferenceVM.MarkingDoc = SessionInfo.CurrentUser.Marking;
                    }
                    if (userPreferenceVM.MessageSignatureDoc == null && userPreferenceVM.MessageSignature != SignType.Delete)
                    {
                        userPreferenceVM.MessageSignatureDoc = SessionInfo.CurrentUser.MessageSignature;
                    }
                    if (userPreferenceVM.SealSignatureDoc == null && userPreferenceVM.SealSignature != SignType.Delete)
                    {
                        userPreferenceVM.SealSignatureDoc = SessionInfo.CurrentUser.SealSignatureDoc;
                    }
                    if (userPreferenceVM.Email == null)
                    {
                        userPreferenceVM.Email = SessionInfo.CurrentUser.Email;
                    }
                    PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/UserProfile/PutUserPreference?cultureName=" + SessionInfo.CultureShortName + "&orgUnitId=" + SessionInfo.OrgUnitId, userPreferenceVM).Result;

                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }

                    SessionInfo.SetObjectInSession(userPreferenceVM.MarkingDoc, "StampImage"); 
                    SessionInfo.SetObjectInSession(userPreferenceVM.MessageSignatureDoc, "MessageSignatureImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureDoc, "SignatureImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureDoc, "BarcodeImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureBehalfDoc, "SignatureBehalfImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureBehalfDoc, "BarcodeBehalfImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureCommandDoc, "SignatureCommandImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.SignatureCommandDoc, "BarcodeCommandImage");
                    SessionInfo.SetObjectInSession(userPreferenceVM.Email, "UserEmailAddress");
                    //SessionInfo.SetObjectInSession(userPreferenceVM.DefaultDisplay, "DefaultDisplay");
                    //SessionInfo.SetObjectInSession(userPreferenceVM.DefaultAssignmentPaper, "DefaultAssignmentPaper");
                }


                string theme = "Blue";
                switch (userPreferenceVM.Theme)
                {
                    case 1005:
                        theme = "Blue";
                        break;
                    case 1003:
                        theme = "Black";
                        break;
                    case 1004:
                        theme = "Green";
                        break;
                }
                SessionInfo.CurrentUser.DefaultDisplay = userPreferenceVM.DefaultDisplay;
                SessionInfo.SetObjectInSession(userPreferenceVM.DefaultDisplay, Constants.DefaultDisplay);
                SessionInfo.SetObjectInSession(userPreferenceVM.DefaultAssignmentPaper, Constants.DefaultAssignmentPaper);
                SessionInfo.CurrentUser.DefaultAssignmentPaper = userPreferenceVM.DefaultAssignmentPaper;
                SessionInfo.SetObjectInSession(theme, "ThemePath");
                SessionInfo.CurrentUser.ThemePath = theme;
                SessionInfo.CurrentUser.ThemeId = userPreferenceVM.Theme;
                SessionInfo.CurrentUser.Signature = userPreferenceVM.SignatureDoc;
                SessionInfo.CurrentUser.SignatureBehalf = userPreferenceVM.SignatureBehalfDoc;
                SessionInfo.CurrentUser.MessageSignature = userPreferenceVM.MessageSignatureDoc;
                SessionInfo.CurrentUser.SealSignatureDoc = userPreferenceVM.SealSignatureDoc;
                SessionInfo.CurrentUser.SignatureCommand = userPreferenceVM.SignatureCommandDoc;
                SessionInfo.CurrentUser.Marking = userPreferenceVM.MarkingDoc;

                SessionInfo.SetLoggedInUserInSession(SessionInfo.CurrentUser);
                /* set culture cookie to the updated one */
                CultureInfo cultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture;
                HttpCookie cookieTemp;
                var arCulture = ConfigurationManager.AppSettings["DefaultArabicCulture"].ToString();
                var enCulture = ConfigurationManager.AppSettings["DefaultEnglishCulture"].ToString();
                if (userPreferenceVM.LanguageId == 1)
                    cookieTemp = cultureInfo.SetCookieCulture(arCulture);
                else
                    cookieTemp = cultureInfo.SetCookieCulture(enCulture);
                Response.Cookies.Add(cookieTemp);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.USerPreferences.SaveCucceeded");

                return Json(new
                {
                    MessageText = message,
                    MessageType = MessageType.Information,
                }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GenerateVerificationCode()
        {
            VerificationInfo verificationInfo = new VerificationInfo();
            verificationInfo.VerificationType = VerificationType.None;

            if (string.IsNullOrEmpty(SessionInfo.CurrentUser.Email))
            {
                verificationInfo.VerificationType = VerificationType.NeedEmail;
                verificationInfo.Title = DbRes.TResource("Admin.User.Email");
            }
            else
            {
                var postResult = HttpClientWrapper<PostResult>.PostRequest($"api/UserProfile/GenerateVerificationCode?userId={SessionInfo.CurrentUser.Id}", null).Result;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    string message = DbRes.TResource("General.GeneralErrorEmail");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                else if ((bool)postResult.Result)
                {
                    verificationInfo.VerificationType = VerificationType.NeedCode;
                    verificationInfo.Title = DbRes.TResource("User.VarificationCode", SessionInfo.CultureShortName);
                }
            }
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_VerificationCodePartial", verificationInfo),
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult GenerateVerificationCode(VerificationInfo verificationInfo)
        {
            VerificationType verificationType = VerificationType.None;
            if (verificationInfo.VerificationType == VerificationType.NeedEmail)
            {
                verificationType = VerificationType.NeedCode;
                var postResultEmail = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/UpdateUserProfile?userId={0}&email={1}",
                    SessionInfo.CurrentUser.Id, verificationInfo.Email), null).Result;
                if (postResultEmail.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResultEmail.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                SessionInfo.CurrentUser.Email = verificationInfo.Email;
            }
            else if (verificationInfo.VerificationType == VerificationType.NeedCode)
            {
                var postResultCode = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/UpdateUserPreference?userId={0}&Code={1}",
                    SessionInfo.CurrentUser.Id, verificationInfo.Code), null).Result;
                if (postResultCode.StatusCode != StatusCode.Ok)
                {
                    string message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResultCode.StatusCode.ToString(), Thread.CurrentThread.CurrentCulture.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { VerificationType = verificationType, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult ChangePassword(string passwordType)
        {
            PasswordType password = (PasswordType)Enum.Parse(typeof(PasswordType), passwordType, true);

            CredentialVM credentialVM = new CredentialVM() { PasswordType = (PasswordType)password };
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ChangePasswordPartial", credentialVM),
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(CredentialVM credentialVM)
        {
            string message = string.Empty;
            GetResult<bool> userPreferenceResult = HttpClientWrapper<GetResult<bool>>
                .PostRequest($"api/UserProfile/VerifySignaturePassword?userId={SessionInfo.CurrentUser.Id}", credentialVM).Result;

            if (userPreferenceResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userPreferenceResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            bool isMatchCurrentPassword = userPreferenceResult.Result;
            if (isMatchCurrentPassword == false)
            {
                return Json(new { MessageText = DbRes.TResource("UserPreferences.CurrentPasswordIncorrect"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            if (credentialVM.PasswordType == PasswordType.Delete)
            {
                var postResultDelete = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateSignaturePassword", credentialVM).Result;
                if (postResultDelete.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResultDelete.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }
            else if (credentialVM.PasswordType == PasswordType.Edit)
            {
                var postResultEdit = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateSignaturePassword", credentialVM).Result;
                if (postResultEdit.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResultEdit.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { MessageText = "", MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult GetCredentialView()
        {
            CredentialVM credentialVM = new CredentialVM();
            return View("~/Areas/User/Views/UserPreferences/_ChangePasswordPartial.cshtml", credentialVM);
        }

        [CustomAuthorize()]
        [HttpGet]
        public ActionResult UserDelegations()
        {
            UserDelegationSettingsVM userDelegationSettingsVM = new UserDelegationSettingsVM();

            GetResult<UserPreferenceDTO> userPreferenceResult =
             HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

            GetResult<UserProfileDTO> managerProfile =
            HttpClientWrapper<GetResult<UserProfileDTO>>.GetItemRequest(string.Format("api/UserProfile/GetOrgUnitManager?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            string parameters = GridHelper.GetGridParameters();

            UserPreferenceVM userPreferenceVMS = UserPreferenceMapper.Map(userPreferenceResult.Result);

            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();
            transactionCategoryVMs.Add(new TransactionCategoryVM
            {
                Id = (int)TransactionCategorieColor.InternalOutbound,
                IsSelected = true,
                Text = "معاملة داخلية"
            });

            transactionCategoryVMs.Add(new TransactionCategoryVM
            {
                Id = (int)TransactionCategorieColor.Inbound,
                IsSelected = false,
                Text = "وارد"
            });


            foreach (var myDelegation in userPreferenceVMS.CurrentDelegationUsers)
            {
                if (!string.IsNullOrWhiteSpace(myDelegation.TransacionCategoryIds))
                {
                    myDelegation.SelectedTransactionCategoriesIdList = myDelegation.TransacionCategoryIds;
                    List<int> TransacionCategoryIds = myDelegation.TransacionCategoryIds.Split(',').Select(u => int.Parse(u)).ToList();
                    myDelegation.SelectedTransactionCategoriesText += string.Join(", ", transactionCategoryVMs.Where(u => TransacionCategoryIds.Contains(u.Id)).Select(u => u.Text).ToList());
                }
            }

            // here

            if (userPreferenceVMS != null)
            {
                userDelegationSettingsVM.IsManager = true;
                userDelegationSettingsVM.userDelegations = userPreferenceVMS.CurrentDelegationUsers;

                var approvedDelegations = userDelegationSettingsVM.userDelegations.Where(d => d.StatusId == DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName)).ToList();
                userDelegationSettingsVM.userApprovedDelegates.ApprovedDelegationListGrid =
                         (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(approvedDelegations, 1, approvedDelegations.Count, false, GridHelper.PageSize);

                userDelegationSettingsVM.userCurrentDelegates = new UserDelegationVM();

                List<UserDelegationVM> currentuserDelegations = approvedDelegations.Where(t => t.DirectedToId != SessionInfo.CurrentUser.Id).ToList();
                userDelegationSettingsVM.userCurrentDelegates.DelegationListGrid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(currentuserDelegations, 1, currentuserDelegations.Count, false, GridHelper.PageSize);

                List<UserDelegationVM> myDelegations = userPreferenceVMS.MyDelegations;
                userDelegationSettingsVM.MyDelegates.MyDelegationListGrid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(myDelegations, 1, myDelegations.Count, false, GridHelper.PageSize);

                int keyCount = 1;
                foreach (var item in userDelegationSettingsVM.userDelegations)
                {
                    item.Key = keyCount++;
                }

                GetResult<List<UserDelegationDTO>> managerDelegations =
                   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserDelegationsByUserId?userId={0}&cultureName={1}&orgUnitId={2}&{3}", null, SessionInfo.CultureShortName, SessionInfo.OrgUnitId, parameters)).Result;

                List<UserDelegationVM> managerDelegationVMs = UserDelegationMapper.Map(managerDelegations.Result);
                List<UserDelegationVM> filtermanagerDelegationVMs = managerDelegationVMs.Where(d => d.StatusId == DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName) && d.DirectedToId == SessionInfo.CurrentUser.Id).ToList();
                userDelegationSettingsVM.userManagerDelegates.ManagerDelegationListGrid =
                        (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(filtermanagerDelegationVMs, 1, managerDelegations.RowsCount.Value, false);
            }
            else
            {
                userDelegationSettingsVM = new UserDelegationSettingsVM();
                userDelegationSettingsVM.IsManager = true;
            }

            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
            ViewData["Cultures"] = GetCultures();
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            ViewData["ConfidentialityData"] = GetConfidentialityLevel();
            ViewData["PrioritiesData"] = GetPriorities();
            ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups();
            // var InProcessDelegations = userDelegationSettingsVM.userDelegations.Where(d => d.StatusId == DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName) || d.StatusId == DelegationStatus.Rejected.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName)).ToList();
            // userDelegationSettingsVM.userCurrentDelegates = new UserDelegationVM();
            // userDelegationSettingsVM.userCurrentDelegates.DelegationListGrid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(InProcessDelegations, 1, InProcessDelegations.Count, false);

            var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);
            GetResult<List<PermissionDTO>> permissionDTOs = HttpClientWrapper<GetResult<List<PermissionDTO>>>.GetItemRequest(urlPermission).Result;

            List<PermissionVM> permissionVMs = PermissionMapper.Map(permissionDTOs.Result);
            permissionVMs.First().IsSelected = true;

            foreach (var myDelegation in userPreferenceVMS.CurrentDelegationUsers)
            {
                if (!string.IsNullOrWhiteSpace(myDelegation.TransacionConfidentialityIds))
                {
                    myDelegation.SelectedConfidentialityLevelsIdList = myDelegation.TransacionConfidentialityIds;
                    List<int> TransacionConfidentialityIds = myDelegation.TransacionConfidentialityIds.Split(',').Select(u => int.Parse(u)).ToList();
                    myDelegation.SelectedConfidentialityLevelsText += string.Join(", ", permissionVMs.Where(u => TransacionConfidentialityIds.Contains(u.Id)).Select(u => u.Text).ToList());
                }
            }

            //List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();
            //transactionCategoryVMs = transactionCategoryVMs.Where(d => d.Id != 8).ToList();
            userDelegationSettingsVM.userCurrentDelegates.TransactionCategories = transactionCategoryVMs;
            userDelegationSettingsVM.userCurrentDelegates.ConfidentialityLevels = permissionVMs;

            return View("~/Areas/User/Views/UserPreferences/Delegation/UserDelegations.cshtml", userDelegationSettingsVM);
        }

        [HttpPost]
        public ActionResult AddUserDelegate(UserDelegationVM userDelegationClientAddVM, List<UserDelegationVM> DelegationListGrid)
        {
            try
            {
                List<UserDelegationVM> userDelegations = new List<UserDelegationVM>();

                if (!userDelegationClientAddVM.DelegationListGrid.Any(d =>
                       //d.OrgUnitId == userDelegationClientAddVM.OrgUnitId &&
                       //d.DirectedToId == userDelegationClientAddVM.DirectedToId &&
                       (d.FromDate < userDelegationClientAddVM.ToDate && userDelegationClientAddVM.FromDate < d.ToDate) && d.StatusId != DelegationStatus.Disabled.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName)
                        ) && userDelegationClientAddVM.DirectedToId != SessionInfo.CurrentUser.Id)
                {
                    userDelegationClientAddVM.StatusId = DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName);
                    GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DelegationStatus, SessionInfo.CultureShortName);
                    IList<LookupVM> lookupVMs = lookups.Result;
                    userDelegationClientAddVM.Status = lookups.Result.Where(l => l.Id == userDelegationClientAddVM.StatusId).FirstOrDefault().Text.ToString();
                    userDelegationClientAddVM.Key = userDelegationClientAddVM.DelegationListGrid.Count + 1;
                    userDelegationClientAddVM.FromDateG = userDelegationClientAddVM.FromDate.ToShortDateString();
                    userDelegationClientAddVM.ToDateG = userDelegationClientAddVM.ToDate.ToShortDateString();
                    if (userDelegationClientAddVM.OrgUnitId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(userDelegationClientAddVM.OrgUnitId, SessionInfo.CultureShortName);
                        userDelegationClientAddVM.OrgUnit = orgUnitDTO.Name;
                    }
                    userDelegationClientAddVM.SelectedTransactionCategoriesText = string.Join(", ", userDelegationClientAddVM.TransactionCategories.Where(x => x.IsSelected).Select(x => x.Text).ToList());
                    userDelegationClientAddVM.SelectedTransactionCategoriesIdList = string.Join(",", userDelegationClientAddVM.TransactionCategories.Where(x => x.IsSelected).Select(x => x.Id).ToList());

                    userDelegationClientAddVM.SelectedConfidentialityLevelsText = string.Join(", ", userDelegationClientAddVM.ConfidentialityLevels.Where(x => x.IsSelected).Select(x => x.Text).ToList());
                    userDelegationClientAddVM.SelectedConfidentialityLevelsIdList = string.Join(",", userDelegationClientAddVM.ConfidentialityLevels.Where(x => x.IsSelected).Select(x => x.Id).ToList());

                    userDelegations.Add(userDelegationClientAddVM);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserDelegation.DuplicateDelegation"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userDelegations, 1, userDelegations.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ClientEditUserDelegate(UserDelegationVM userDelegationEdit)
        {
            try
            {
                string message = string.Empty;

                List<UserDelegationVM> userDelegations = new List<UserDelegationVM>();

                if (!userDelegationEdit.DelegationListGrid.Any(d =>
                       (d.FromDate < userDelegationEdit.ToDate && userDelegationEdit.FromDate < d.ToDate)
                        && d.Key != userDelegationEdit.Key))
                {
                    userDelegationEdit.StatusId = DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName);
                    GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DelegationStatus, SessionInfo.CultureShortName);
                    IList<LookupVM> lookupVMs = lookups.Result;
                    userDelegationEdit.Status = lookups.Result.Where(l => l.Id == userDelegationEdit.StatusId).FirstOrDefault().Text.ToString();
                    userDelegationEdit.FromDateG = userDelegationEdit.FromDate.ToShortDateString();
                    userDelegationEdit.ToDateG = userDelegationEdit.ToDate.ToShortDateString();
                    if (userDelegationEdit.OrgUnitId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(userDelegationEdit.OrgUnitId, SessionInfo.CultureShortName);
                        userDelegationEdit.OrgUnit = orgUnitDTO.Name;
                    }

                    userDelegationEdit.SelectedTransactionCategoriesText = string.Join(", ", userDelegationEdit.TransactionCategories.Where(x => x.IsSelected).Select(x => x.Text).ToList());
                    userDelegationEdit.SelectedTransactionCategoriesIdList = string.Join(",", userDelegationEdit.TransactionCategories.Where(x => x.IsSelected).Select(x => x.Id).ToList());

                    userDelegationEdit.SelectedConfidentialityLevelsText = string.Join(", ", userDelegationEdit.ConfidentialityLevels.Where(x => x.IsSelected).Select(x => x.Text).ToList());
                    userDelegationEdit.SelectedConfidentialityLevelsIdList = string.Join(",", userDelegationEdit.ConfidentialityLevels.Where(x => x.IsSelected).Select(x => x.Id).ToList());

                    userDelegations.Add(userDelegationEdit);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserDelegation.DuplicateDelegation"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = userDelegationEdit.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml",
                    (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userDelegations, 1, userDelegations.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region oldDelegationMethods
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditUserDelegate(UserDelegationVM userDelegationEditVM)
        {
            try
            {
                string message = string.Empty;
                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/UserProfile/PutUserDelegation", userDelegationEditVM).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<UserDelegationDTO>> userDelegationEditDTOs =
                 HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(String.Format("api/UserProfile/GetUserDelegations?preferenceId={0}&PageIndex=1&PageSize={1}&CultureName={2}", userDelegationEditVM.UserPreferenceId, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userDelegationEditDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userDelegationEditDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                IAjaxGrid grid = (AjaxGrid<UserDelegationDTO>)new AjaxGridFactory().CreateAjaxGrid(UserDelegationMapper.Map(userDelegationEditDTOs.Result), 1, userDelegationEditDTOs.RowsCount.Value, false);
                string data = JsonConvert.SerializeObject(userDelegationEditDTOs);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.EditSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteDelegationById(int delegateId)
        {
            try
            {
                string message = string.Empty;
                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/UserProfile/DeleteDelegations?ids={0}", delegateId)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }


                return UpdateGridDelegations(0, "");

                //GetResult<List<UserDelegationDTO>> userDelegationEditDTOs =
                //   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(String.Format("api/UserProfile/GetUserDelegations?preferenceId={0}&PageIndex=1&PageSize={1}&CultureName={2}", SessionInfo.CurrentUser.Id, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                //if (userDelegationEditDTOs.StatusCode != StatusCode.Ok)
                //{
                //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userDelegationEditDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}
                //IAjaxGrid grid = (AjaxGrid<UserDelegationDTO>)new AjaxGridFactory().CreateAjaxGrid(UserDelegationMapper.Map(userDelegationEditDTOs.Result), 1, userDelegationEditDTOs.RowsCount.Value, false);
                //string data = JsonConvert.SerializeObject(userDelegationEditDTOs);
                //message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.DeleteSucceeded");

                //return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml", grid), hdnValue = data, id = delegateId, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ClientDeleteDelegation(int ids, string hdnUsersArray)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<UserDelegationClientAddViewModel> userDelegationVMs = new List<UserDelegationClientAddViewModel>();

                if (!string.IsNullOrEmpty(hdnUsersArray))
                {
                    userDelegationVMs.AddRange(javaScriptSerializer.Deserialize(hdnUsersArray, typeof(List<UserDelegationClientAddViewModel>)) as List<UserDelegationClientAddViewModel>);
                }
                userDelegationVMs.RemoveAt(ids);

                string data = JsonConvert.SerializeObject(userDelegationVMs);

                IAjaxGrid grid = (AjaxGrid<UserDelegationClientAddViewModel>)new AjaxGridFactory().CreateAjaxGrid(userDelegationVMs, 1, userDelegationVMs.Count, true);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.DeleteSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserDelegationGridPartial.cshtml", grid), hdnValue = data, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteDelegation(int ids, string Id)
        {
            try
            {
                string message = string.Empty;
                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/UserProfile/DeleteDelegations?ids={0}", ids)).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<UserDelegationDTO>> userDelegationEditDTOs =
                   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(String.Format("api/UserProfile/GetUserDelegations?preferenceId={0}&PageIndex=1&PageSize={1}&CultureName={2}", Id, GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (userDelegationEditDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userDelegationEditDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                IAjaxGrid grid = (AjaxGrid<UserDelegationDTO>)new AjaxGridFactory().CreateAjaxGrid(UserDelegationMapper.Map(userDelegationEditDTOs.Result), 1, userDelegationEditDTOs.RowsCount.Value, false);
                string data = JsonConvert.SerializeObject(userDelegationEditDTOs);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml", grid), hdnValue = data, id = ids, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult GetDelegationById(int id)
        {
            try
            {
                string message = string.Empty;

                GetResult<EditUserDelegationDTO> userDelegationEditDTO =
                   HttpClientWrapper<GetResult<EditUserDelegationDTO>>.GetItemRequest(String.Format("api/UserProfile/GetUserDelegationById?id={0}&cultureName={1}", id, SessionInfo.CultureShortName)).Result;

                if (userDelegationEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userDelegationEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
            HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                ViewData["Cultures"] = GetCultures();
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups();
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserDelegationEditPartial.cshtml",
                    UserDelegationMapper.Map(userDelegationEditDTO.Result)),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult GetDelegationByIndex(int index, string hdnUsersArray, string hdnCurrentUsersArray)
        {
            try
            {
                string message = string.Empty;
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<UserDelegationClientEditViewModel> userDelegationDTOs = new List<UserDelegationClientEditViewModel>();

                if (!string.IsNullOrEmpty(hdnUsersArray))
                {
                    userDelegationDTOs.AddRange(javaScriptSerializer.Deserialize(hdnUsersArray, typeof(List<UserDelegationClientEditViewModel>)) as List<UserDelegationClientEditViewModel>);
                }

                UserDelegationClientEditViewModel userDelegationEditDTO = userDelegationDTOs[index];
                userDelegationEditDTO.Index = index;

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                    HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);

                ViewData["Cultures"] = GetCultures();
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups();
                ViewData["CurrentDelegations"] = hdnCurrentUsersArray;
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/Delegation/_UserDelegationClientEditPartial.cshtml", userDelegationEditDTO), hdnValue = hdnUsersArray, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        [HttpPost]
        public ActionResult UpdateGridDelegations(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<UserDelegationDTO>> userDelegations =
                   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserDelegationsByUserId?userId={0}&statusId={1}&orgUnitId={2}&{3}", SessionInfo.CurrentUser.Id, null, null, parameters)).Result;

                List<UserDelegationVM> userDelegationVMs = UserDelegationMapper.Map(userDelegations.Result);
                IAjaxGrid grid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userDelegationVMs, page.HasValue ? page.Value : 1, userDelegations.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/UserPreferences/Delegation/_UserCurrentDelegationsGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridDelegationsApproved(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<UserDelegationDTO>> userDelegations =
                   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserDelegationsByUserId?userId={0}&statusId={1}&orgUnitId={2}&{3}", SessionInfo.CurrentUser.Id, DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName), null, parameters)).Result;

                List<UserDelegationVM> userDelegationVMs = UserDelegationMapper.Map(userDelegations.Result);
                IAjaxGrid grid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userDelegationVMs, page.HasValue ? page.Value : 1, userDelegations.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/UserPreferences/Delegation/_UserApprovedDelegationsGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateGridDelegationsManager(int? page, string param)
        {
            try
            {
                string parameters = GridHelper.GetGridParameters();

                GetResult<List<UserDelegationDTO>> userDelegations =
                   HttpClientWrapper<GetResult<List<UserDelegationDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUserDelegationsByUserId?userId={0}&statusId={1}&orgUnitId={2}&{3}", null, DelegationStatus.InProcess.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName), SessionInfo.OrgUnitId, parameters)).Result;

                List<UserDelegationVM> userDelegationVMs = UserDelegationMapper.Map(userDelegations.Result);
                IAjaxGrid grid = (AjaxGrid<UserDelegationVM>)new AjaxGridFactory().CreateAjaxGrid(userDelegationVMs, page.HasValue ? page.Value : 1, userDelegations.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/UserPreferences/Delegation/_UserManagerDelegationsGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveUserDelegations(UserDelegationVM userDelegationVM)
        {
            /*
             * both SelectedTransactionCategoriesIdList and SelectedConfidentialityLevelsIdList can be
             * found in userDelegationVM.DelegationListGrid, not directly in the userDelegationVM object.
             */

            List<UserDelegationDTO> userDelegationDTOs = new List<UserDelegationDTO>();

            userDelegationDTOs = UserDelegationMapper.Map(userDelegationVM.DelegationListGrid);

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/PostUserDelegations?userId=" + SessionInfo.CurrentUser.Id, userDelegationDTOs).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }

        [HttpPost]
        public ActionResult UpdateUserDelegationStatus(int delegateId, int statusType, string rejectionReason)
        {
            string message = string.Empty;
            int statusId = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName);

            switch (statusType)
            {
                case 1:
                    statusId = DelegationStatus.Approved.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName); ;
                    break;
                case 2:
                    statusId = DelegationStatus.Rejected.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName); ;
                    break;
                case 3:
                    statusId = DelegationStatus.Disabled.LookupIdentity(LookupCategory.DelegationStatus, SessionInfo.CultureShortName); ;
                    break;
            }

            PostResult postResult = HttpClientWrapper<PostResult>
                       .PostRequest(string.Format("api/UserProfile/UpdateUserDelegationStatus?delegateId={0}&statusId={1}&rejectionReason={2}&cultureName={3}", delegateId, statusId, rejectionReason, SessionInfo.CultureShortName), null).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }



        [HttpPost]
        public ActionResult AddEntityToDistributionList(DistributionListVM distributionListVM)
        {
            try
            {
                string message = string.Empty;
                List<DistributionListDetailsVM> distributionListDetails = new List<DistributionListDetailsVM>();
                if (!distributionListVM.DistributionListDetailsGrid.Any(d => d.OrgUnitId == distributionListVM.OrgUnitId
                && d.UserId == distributionListVM.UserId))
                {
                    DistributionListDetailsVM distributionListDetailsVM = new DistributionListDetailsVM
                    {
                        OrgUnitId = distributionListVM.OrgUnitId.Value,
                        OrgUnitName = distributionListVM.OrgUnitName,
                        UserId = distributionListVM.UserId.Value,
                        UserName = distributionListVM.UserName,
                        Key = distributionListVM.DistributionListDetailsGrid.Count + 1
                    };
                    if (distributionListDetailsVM.OrgUnitId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(distributionListDetailsVM.OrgUnitId, SessionInfo.CultureShortName);
                        distributionListDetailsVM.OrgUnitName = orgUnitDTO.Name;
                    }
                    distributionListDetails = distributionListVM.DistributionListDetailsGrid.ToList();
                    distributionListDetails.Add(distributionListDetailsVM);
                }
                else
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, DbRes.TValidation("User.DistrubutionList.EntityExist"));
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~\Areas\User\Views\UserPreferences\DistributionList\_DistributionListDetailsGridPartial.cshtml", (AjaxGrid<DistributionListDetailsVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(distributionListDetails, 1, distributionListDetails.Count, false))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult EditDistributionListDetails(DistributionListVM distributionListVM)
        {
            try
            {
                string message = string.Empty;
                try
                {
                    List<DistributionListDetailsVM> distributionListDetailsVMs = new List<DistributionListDetailsVM>();
                    if (!distributionListVM.DistributionListDetailsGrid
                        .Any(copy => copy.OrgUnitId == distributionListVM.OrgUnitId &&
                        copy.UserId == distributionListVM.UserId && copy.Key != distributionListVM.Key))
                    {
                        DistributionListDetailsVM distributionListDetailsVM = new DistributionListDetailsVM
                        {
                            OrgUnitId = distributionListVM.OrgUnitId.Value,
                            OrgUnitName = distributionListVM.OrgUnitName,
                            UserId = distributionListVM.UserId.Value,
                            UserName = distributionListVM.UserName,
                            Key = distributionListVM.Key
                        };
                        if (distributionListDetailsVM.OrgUnitId > 0)
                        {
                            OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(distributionListDetailsVM.OrgUnitId, SessionInfo.CultureShortName);
                            distributionListDetailsVM.OrgUnitName = orgUnitDTO.Name;
                        }

                        distributionListDetailsVMs.Add(distributionListDetailsVM);
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                    return Json(new
                    {
                        MessageType = MessageType.Information,
                        MessageText = message,
                        Key = distributionListVM.Key,
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~\Areas\User\Views\UserPreferences\DistributionList\_DistributionListDetailsGridPartial.cshtml", (AjaxGrid<DistributionListDetailsVM>)new AjaxGridFactory()
                       .CreateAjaxGrid(distributionListDetailsVMs, 1, distributionListDetailsVMs.Count, true))
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddDistributionList(string ArabicDistributionListName, string EnglishDistributionListName, bool IsPublic)
        {

            List<LocalizationDTO> localizationDTOs = new List<LocalizationDTO>();
            localizationDTOs.Add(new LocalizationDTO { Text = ArabicDistributionListName, CultureName = "ar", CultureId = (int)CultureType.Arabic });
            localizationDTOs.Add(new LocalizationDTO { Text = EnglishDistributionListName, CultureName = "en", CultureId = (int)CultureType.English });
            int? userid = SessionInfo.CurrentUser.Id;
            if (IsPublic)
                userid = null;

            DistributionListDTO distributionListDTO = new DistributionListDTO
            {
                UserId = userid,
                OrgUnitId = SessionInfo.OrgUnitId,
                Name = localizationDTOs,
                DistributionListDetails = new List<DistributionListDetailsDTO>()
            };

            GetResult<List<DistributionListDTO>> DistributionListDTO =
              HttpClientWrapper<GetResult<List<DistributionListDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionList?userId={0}&orgUnitId={1}&cultureName={2}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<DistributionListVM> distributionListVMs = DistributionListMapper.Map(DistributionListDTO.Result);

            foreach (var item in distributionListVMs)
            {
                if (item.Name.Where(s => s.CultureName == "ar").FirstOrDefault().Text.Trim() == ArabicDistributionListName.Trim() || item.Name.Where(s => s.CultureName == "en").FirstOrDefault().Text.Trim() == EnglishDistributionListName.Trim())
                {
                    return Json(new
                    {
                        MessageType = MessageType.Error,
                        message = "Duplicate"
                    }, JsonRequestBehavior.AllowGet);
                }
            }

            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/AddDistributionList", distributionListDTO).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                return Json(new
                {
                    MessageType = MessageType.Error,
                    message = "Fail"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                MessageType = MessageType.Information,
                message = "Success"
            }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult SaveDistributionListDetails(DistributionListVM distributionListVM, int EditedDistributionListId)
        {
            try
            {
                List<DistributionListDetailsDTO> distributionListDetailsDTOs = DistributionListMapper.Map(distributionListVM.DistributionListDetailsGrid.ToList(), SessionInfo.CultureShortName);
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/SaveDistributionListDetails?DistributionListId=" + EditedDistributionListId, distributionListDetailsDTOs).Result;

                string message = string.Empty;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.DistributionList.SaveDistributionListSuccess");
                    return Json(new { MessageType = MessageType.Error, }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpGet]
        public ActionResult GetDistributionListById(int id)
        {
            try
            {
                GetResult<DistributionListDTO> distributionListDTO =
                HttpClientWrapper<GetResult<DistributionListDTO>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionListById?userId={0}&orgUnitId={1}&cultureName={2}&id={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName, id)).Result;

                DistributionListVM distributionList = DistributionListMapper.Map(distributionListDTO.Result);

                string distributionListName = distributionList.Name.Where(s => s.CultureName == SessionInfo.CultureShortName).FirstOrDefault().Text;

                List<DistributionListDetailsVM> DistributionListDetailsVM = distributionList.DistributionListDetails.ToList();
                for (int i = 1; i <= DistributionListDetailsVM.Count; i++)
                {
                    DistributionListDetailsVM[i - 1].Key = i;
                }
                return Json(new
                {
                    DistributionListName = distributionListName,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~\Areas\User\Views\UserPreferences\DistributionList\_DistributionListDetailsGridPartial.cshtml", (AjaxGrid<DistributionListDetailsVM>)new AjaxGridFactory()
                   .CreateAjaxGrid(DistributionListDetailsVM, 1, DistributionListDetailsVM.Count, false))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.DistributionList)]
        public ActionResult DistributionList()
        {
            try
            {
                GetResult<List<DistributionListDTO>> DistributionList =
                HttpClientWrapper<GetResult<List<DistributionListDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetDistributionList?userId={0}&orgUnitId={1}&cultureName={2}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                List<DistributionListVM> distributionListVM = DistributionListMapper.Map(DistributionList.Result);

                for (int i = 1; i <= distributionListVM.Count; i++)
                {
                    distributionListVM[i - 1].Key = i;
                }

                AjaxGrid<DistributionListVM> grid = (AjaxGrid<DistributionListVM>)new AjaxGridFactory().CreateAjaxGrid(distributionListVM, 1, distributionListVM.Count, false);

                AjaxGrid<DistributionListDetailsVM> gridDetails = (AjaxGrid<DistributionListDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(new List<DistributionListDetailsVM>(), 1, 0, false);


                GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                                          .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);


                ViewData["OrgUnitItems"] = UIHelper.BulidTree(organizationUnitVMs, -1);
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["DistributionListGrid"] = grid;
                ViewData["DistributionListDetailsGrid"] = gridDetails;
                return View("~/Areas/User/Views/UserPreferences/DistributionList/_DistributionListSettingsPartial.cshtml", new DistributionListVM());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteDistributionList(int distributionListId)
        {
            try
            {
                var distributionListDTO = new DistributionListDTO();
                distributionListDTO.Id = distributionListId;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/DeleteDistributionList", distributionListDTO).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = "Fail" });
                }

                return Json(new { MessageType = MessageType.Information, MessageText = "Success" });

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
        [HttpGet]
        public ActionResult VerifySignaturePassword(string signaturePassword)
        {
            GetResult<bool?> userPreferenceResult =
             HttpClientWrapper<GetResult<bool?>>.GetItemRequest(string.Format("api/UserProfile/VerifySignaturePassword?SignaturePasswordTxt={0}&userId={1}", signaturePassword, SessionInfo.CurrentUser.Id)).Result;

            var message = "";
            if (userPreferenceResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userPreferenceResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.UserPreferences.Signature");
            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                ValidationStatus = userPreferenceResult.Result.Value
            }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult TransactionPath()
        {
            TransactionPathVM transactionPathVM = new TransactionPathVM();
            transactionPathVM.TransactionPathDetailsVM = new TransactionPathDetailsVM();

            GetResult<List<TransactionPathDTO>> transactionPathsResult =
             HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetTransactionPath?userId={0}&orgUnitId={1}&pageIndex={2}&pageSize={3}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, 1, GridHelper.PageSize)).Result;

            List<TransactionPathVM> transactionPathVMs = TransactionPathMapper.Map(transactionPathsResult.Result);

            int keyCount = 1;
            foreach (var item in transactionPathVMs)
            {
                item.Key = keyCount++;
            }

            transactionPathVM.TransactionPathsGrid = (AjaxGrid<TransactionPathVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPathVMs, 1, transactionPathsResult.RowsCount.Value, false, GridHelper.PageSize);

            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                   HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
            ViewData["Cultures"] = GetCultures();
            ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
            ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
            ViewData["ConfidentialityData"] = GetConfidentialityLevel();
            ViewData["PrioritiesData"] = GetPriorities();
            ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups(TransactionCategory.DraftOutbound);
            ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
            ViewData["DeliveryMethod"] = GetDelivery(true);

            return View("~/Areas/User/Views/UserPreferences/TransactionPath/TransactionPathSettings.cshtml", transactionPathVM);
        }

        [HttpPost]
        public ActionResult UpdateGridTransactionPath(int? page, string param)
        {
            try
            {
                GetResult<List<TransactionPathDTO>> transactionPathsResult =
                 HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetTransactionPath?userId={0}&orgUnitId=null&pageIndex={1}&pageSize={2}", SessionInfo.CurrentUser.Id, page.HasValue ? page.Value : 1, GridHelper.PageSize)).Result;

                List<TransactionPathVM> transactionPathVMs = TransactionPathMapper.Map(transactionPathsResult.Result);

                int keyCount = 1;
                foreach (var item in transactionPathVMs)
                {
                    item.Key = keyCount++;
                }

                IAjaxGrid grid = (AjaxGrid<TransactionPathVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPathVMs, page.HasValue ? page.Value : 1, transactionPathsResult.RowsCount.Value, page.HasValue);

                return Json(new { Html = grid.ToJson("~/Areas/User/Views/UserPreferences/TransactionPath/_TransactionPathGridPartial.cshtml", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AddTransactionPathDetails(TransactionPathDetailsVM transactionPathDetailsVM, List<TransactionPathDetailsVM> TransactionPathDetailsGrid)
        {
            try
            {
                List<TransactionPathDetailsVM> transactionPaths = new List<TransactionPathDetailsVM>();
                if (TransactionPathDetailsGrid == null)
                {
                    TransactionPathDetailsGrid = new List<TransactionPathDetailsVM>();
                }

                if (!TransactionPathDetailsGrid.Any(d =>
                       d.EntityId == transactionPathDetailsVM.EntityId && d.UserId == transactionPathDetailsVM.UserId))
                {
                    transactionPathDetailsVM.Key = TransactionPathDetailsGrid.Count + 1;
                    transactionPathDetailsVM.Sort = TransactionPathDetailsGrid.Count + 1;
                    transactionPaths.Add(transactionPathDetailsVM);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DuplicatePathDetails"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                if (transactionPathDetailsVM.EntityId > 0)
                {
                    OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(transactionPathDetailsVM.EntityId, SessionInfo.CultureShortName);
                    transactionPathDetailsVM.EntityName = orgUnitDTO.Name;
                }

                IAjaxGrid grid = (AjaxGrid<TransactionPathDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPaths, 1, transactionPaths.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/TransactionPath/_TransactionPathDetailsGridPartial.cshtml", grid) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SaveTransactionPath(TransactionPathVM transactionPathVM)
        {
            TransactionPathDTO transactionPathDTOs = new TransactionPathDTO();
            transactionPathVM.OrgUnitId = SessionInfo.OrgUnitId;
            transactionPathVM.UserId = SessionInfo.CurrentUser.Id;

            transactionPathDTOs = TransactionPathMapper.Map(transactionPathVM);
            string message = string.Empty;

            if (transactionPathVM.TransactionPathDetailsGrid.Count < 2)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DetailsCount");
                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            GetResult<List<TransactionPathDTO>> PathsNameResult =
         HttpClientWrapper<GetResult<List<TransactionPathDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetPathsName?OrgUnitId={0}", transactionPathVM.OrgUnitId)).Result;


            foreach (var item in PathsNameResult.Result)
            {
                if ((transactionPathVM.Name.Trim() == item.Name.Trim() && transactionPathVM.Id != 0 && transactionPathVM.Id != item.Id) || (transactionPathVM.Name.Trim() == item.Name.Trim() && transactionPathVM.Id == 0))
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.NameDuplicate");
                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }
            }
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/PostTransactionPath", transactionPathDTOs).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }

        [HttpGet]
        public ActionResult GetTransactionPathById(int pathId)
        {
            try
            {
                string message = string.Empty;

                GetResult<TransactionPathDTO> transactionPathDTO =
                   HttpClientWrapper<GetResult<TransactionPathDTO>>.GetItemRequest(String.Format("api/UserProfile/GetTransactionPathById?pathId={0}", pathId)).Result;

                if (transactionPathDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, transactionPathDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                TransactionPathVM transactionPathVM = TransactionPathMapper.Map(transactionPathDTO.Result);

                int keyCount = 1;
                foreach (var item in transactionPathVM.TransactionPathDetails)
                {
                    item.Key = keyCount++;
                }

                transactionPathVM.TransactionPathDetailsGrid = (AjaxGrid<TransactionPathDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPathVM.TransactionPathDetails, 1, transactionPathVM.TransactionPathDetails.Count, false);

                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                            HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
                List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnitDTOs.Result);
                ViewData["Cultures"] = GetCultures();
                ViewData["DepartmentsData"] = UIHelper.BulidTree(organizationUnitVMs, SessionInfo.OrgUnitId);
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();
                ViewData["TransactionCategoryData"] = GetTransactionCategoryLookups(TransactionCategory.DraftOutbound);
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["DeliveryMethod"] = GetDelivery(true);
                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/TransactionPath/_TransactionPathAddPartial.cshtml", transactionPathVM),
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
        public ActionResult DeleteTransactionPath(int pathId)
        {
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/DeleteTransactionPath?pathId=" + pathId, null).Result;

                if (!postResult.Id.HasValue || (postResult.Id.HasValue && postResult.Id < 0))
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.UnableToRemove") });
                }

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    return Json(new { MessageType = MessageType.Error, MessageText = "Fail" });
                }

                return Json(new { MessageType = MessageType.Information, MessageText = "Success" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ClientEditTransactionPathInfo(TransactionPathDetailsVM transactionPathDetailsVM, List<TransactionPathDetailsVM> TransactionPathDetailsGrid)
        {
            try
            {
                string message = string.Empty;

                List<TransactionPathDetailsVM> transactionPaths = new List<TransactionPathDetailsVM>();
                if (TransactionPathDetailsGrid == null)
                {
                    TransactionPathDetailsGrid = new List<TransactionPathDetailsVM>();
                }

                if (!TransactionPathDetailsGrid.Any(d =>
                       d.EntityId == transactionPathDetailsVM.EntityId && d.UserId == transactionPathDetailsVM.UserId && d.Key != transactionPathDetailsVM.Key))
                {
                    if (transactionPathDetailsVM.EntityId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(transactionPathDetailsVM.EntityId, SessionInfo.CultureShortName);
                        transactionPathDetailsVM.EntityName = orgUnitDTO.Name;
                    }
                    transactionPaths.Add(transactionPathDetailsVM);
                }
                else
                {
                    return Json(new { MessageText = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Path.DuplicatePathDetails"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Key = transactionPathDetailsVM.Key,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/TransactionPath/_TransactionPathDetailsGridPartial.cshtml",
                    (AjaxGrid<TransactionPathDetailsVM>)new AjaxGridFactory().CreateAjaxGrid(transactionPaths, 1, transactionPaths.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ClientEditTransactionPathInfoSort(int pathId, int sort, string order)
        {
            try
            {
                string message = string.Empty;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/UpdateTransactionPathDetailsSort?pathId={0}&sort={1}&order={2}", pathId, sort, order), null).Result;

                return Json(new
                {
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public string GetDelivery(bool isPaper)
        {
            try
            {
                int[] ContainPaper = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName), DeliveryMethodType.ElectronicPaper.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };
                int[] elctronic = { DeliveryMethodType.Electronic.LookupIdentity(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName) };

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetLookupItems(LookupCategory.DeliveryMethod, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
                    lookups.Result = lookups.Result.OrderBy(a => a.Sort).ToList();
                    if (isPaper)
                    {
                        lookups.Result = lookups.Result.Where(a => ContainPaper.Contains(a.Id)).ToList();
                    }
                    else
                    {
                        lookups.Result = lookups.Result.Where(a => elctronic.Contains(a.Id)).ToList();
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
        #region AssignmentPaper
        [HttpPost]
        // [ValidateAntiForgeryToken()]
        public ActionResult AddAssignmentPaperGroup(AssignmentPaperGroupVM assignmentPaperGroupVM)
        {
            try
            {
                string message = string.Empty;

                //GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                //if (cultureDTOs.StatusCode != StatusCode.Ok)
                //{
                //    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, cultureDTOs.StatusCode.ToString());

                //    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                //ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                //AssignmentPaperGroupVM.IsNew = true;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/PostAssignmentPaperGroup", AssignmentPaperGroupMapper.Map(assignmentPaperGroupVM, SessionInfo.CurrentUser.Id)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<AssignmentPaperGroupDTO>> assignmentPaperGroupDTOList = HttpClientWrapper<GetResult<List<AssignmentPaperGroupDTO>>>
                .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperGroupsByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

                if (assignmentPaperGroupDTOList.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, assignmentPaperGroupDTOList.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperGroupMapper.Map(AssignmentPaperGroupDTOs.Result).AsQueryable(), 1, false, AssignmentPaperGroupDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTOList.Result.OrderBy(x => x.OrderNo).ToList()), 1, assignmentPaperGroupDTOList.Result.Count, false, GridHelper.PageSize);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم الحفظ");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperGroupsGridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetAssignmentPaperGroup(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<AssignmentPaperGroupDTO> assignmentPaperGroupDTO =
                    HttpClientWrapper<GetResult<AssignmentPaperGroupDTO>>.GetItemRequest(String.Format("api/UserProfile/GetAssignmentPaperGroupById?assignmentPaperGroupId={0}", id)).Result;

                if (assignmentPaperGroupDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, assignmentPaperGroupDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                ViewData["ActionData"] = GetActions();
                //TODO: Change Source Key To Be "Admin.AssignmentPaperGroup.UpdateSucceeded"
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم التعديل");

                AssignmentPaperGroupEditVM assignmentPaperGroupEditVM = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTO.Result, SessionInfo.CultureShortName);

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperGroupsEditPartial.cshtml", assignmentPaperGroupEditVM
                    ),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult EditAssignmentPaperGroup(AssignmentPaperGroupVM assignmentPaperGroupVM)
        {
            try
            {
                string message = string.Empty;

                PutResult PutResult = HttpClientWrapper<PutResult>.PutRequest("api/UserProfile/PutAssignmentPaperGroup", AssignmentPaperGroupMapper.Map(assignmentPaperGroupVM, SessionInfo.CurrentUser.Id)).Result;

                if (PutResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, PutResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<AssignmentPaperGroupDTO>> assignmentPaperGroupDTOs =
                     HttpClientWrapper<GetResult<List<AssignmentPaperGroupDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperGroupsByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

                if (assignmentPaperGroupDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, assignmentPaperGroupDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTOs.Result).AsQueryable(), 1, false, assignmentPaperGroupDTOs.RowsCount.Value);
                IAjaxGrid grid = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTOs.Result.OrderBy(x => x.OrderNo).ToList()), 1, assignmentPaperGroupDTOs.Result.Count, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم التعديل");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperGroupsGridPartial.cshtml", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public ActionResult DeleteAssignmentPaperGroup(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<AssignmentPaperGroupDTO> assignmentPaperGroupDTO =
                    HttpClientWrapper<GetResult<AssignmentPaperGroupDTO>>.GetItemRequest(String.Format("api/UserProfile/DeleteAssignmentPaperGroup?assignmentPaperGroupId={0}", id)).Result;

                if (assignmentPaperGroupDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, assignmentPaperGroupDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //TODO: Change Source Key To Be "Admin.AssignmentPaperGroup.UpdateSucceeded"
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "تم الحذف");

                AssignmentPaperGroupEditVM assignmentPaperGroupEditVM = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTO.Result, SessionInfo.CultureShortName);

                return Json(new
                {
                    //Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperGroupsEditPartial.cshtml", assignmentPaperGroupEditVM
                    //),
                    MessageText = message,
                    MessageType = MessageType.Information
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult ChangeGroupOrder(int id, bool isMoveUp)
        {
            try
            {
                var postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/UserProfile/ChangeGroupOrder?id={0}&isMoveUp={1}", id, isMoveUp), null).Result;
                string message = string.Empty;
                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error });
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information });

            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpPost]
        public ActionResult ChangeBeneficiaryOrder(List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs, int Refkey, bool isMoveUp)
        {
            try
            {
                string message = string.Empty;
                assignmentPaperBeneficiaryVMs = assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue).ToList();
                ViewData["ActionData"] = GetActions();
                var targetValue = assignmentPaperBeneficiaryVMs.Where(x => x.Key == Refkey).FirstOrDefault();
                var exchangeAssignmentPaper = isMoveUp ? assignmentPaperBeneficiaryVMs.OrderBy(x => x.OrderNo).ToList().Where(x => x.OrderNo > targetValue.OrderNo).FirstOrDefault()
                    : assignmentPaperBeneficiaryVMs.OrderByDescending(x => x.OrderNo).ToList().Where(x => x.OrderNo < targetValue.OrderNo).FirstOrDefault();

                if (exchangeAssignmentPaper != null)
                {
                    //assignmentPaperBeneficiaryVMs.Remove(targetValue);
                    //assignmentPaperBeneficiaryVMs.Remove(exchangeAssignmentPaper);

                    int oldOrder = targetValue.OrderNo;
                    int NewOrder = exchangeAssignmentPaper.OrderNo;
                    assignmentPaperBeneficiaryVMs.Where(x => x.Key == targetValue.Key).FirstOrDefault().OrderNo = NewOrder;
                    assignmentPaperBeneficiaryVMs.Where(x => x.Key == exchangeAssignmentPaper.Key).FirstOrDefault().OrderNo = oldOrder;
                    //targetValue.OrderNo = exchangeAssignmentPaper.OrderNo;
                    //exchangeAssignmentPaper.OrderNo = oldOrder;

                    //assignmentPaperBeneficiaryVMs.Add(targetValue);
                    //assignmentPaperBeneficiaryVMs.Add(exchangeAssignmentPaper);
                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~\Areas\User\Views\UserPreferences\AssignmentPaper\_AssignmentPaperSettingsGridPartial.cshtml", (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory()
             .CreateAjaxGrid(assignmentPaperBeneficiaryVMs.OrderBy(x => x.OrderNo).ToList(), 1, assignmentPaperBeneficiaryVMs.Count, false))
                }, JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult AssignmentPaperSettings()
        {
            var AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
                .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;


            if (AssignmentPaperDTOs.StatusCode != StatusCode.Ok)
            {

            }
            ViewData["ActionData"] = GetActions();

            GetResult<List<AssignmentPaperGroupDTO>> assignmentPaperGroupDTOList = HttpClientWrapper<GetResult<List<AssignmentPaperGroupDTO>>>
                .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperGroupsByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
            //GetResult<List<OrgUnitDTO>> orgUnits =
            //     HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;
            //List<OrgUnitVM> organizationUnitVMs = OrgUnitMapper.Map(orgUnits.Result);
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
            //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            AssignmentPaperVM assignmentPaperVM = AssignmentPaperMapper.Map(AssignmentPaperDTOs.Result);
            List<AssignmentPaperGroupVM> AssignmentPaperGroupVMList = AssignmentPaperGroupMapper.Map(assignmentPaperGroupDTOList.Result);

            if (assignmentPaperVM != null && assignmentPaperVM.Beneficiaries != null)
            {
                int i = 0;
                assignmentPaperVM.Beneficiaries.ForEach(beneficiary =>
                {
                    i++;
                    beneficiary.OrderNo = i;
                    beneficiary.Key = i;
                    beneficiary.UserId = beneficiary.UserId <= 0 || beneficiary.UserId.HasValue ? -1 : beneficiary.UserId;

                });
            }



            //var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
            //    .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), SessionInfo.OrgUnitId);
            ViewData["AssignmentPaperGroups"] = GetAssignmentPaperGroups(SessionInfo.CurrentUser.Id);
            var resutl = new AssignmentPaperBeneficiaryVM();
            resutl.AssignmentPaperBeneficiaryVMs = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentPaperVM.Beneficiaries.OrderBy(x => x.OrderNo).ToList(), 1, assignmentPaperVM.Beneficiaries.Count, false);
            resutl.AssignmentPaperGroupVMs = (AjaxGrid<AssignmentPaperGroupVM>)new AjaxGridFactory().CreateAjaxGrid(AssignmentPaperGroupVMList.OrderBy(x => x.OrderNo).ToList(), 1, AssignmentPaperGroupVMList.Count, false);
            // resutl.GroupName = AssignmentPaperDTOs.Result.GroupName;
            resutl.BeneficiaryOrgUnitId = SessionInfo.OrgUnitId;
            resutl.DefaultActionId = 1;
            return View("~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperSettingsPartial.cshtml", resutl);
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult AssignmentPaperSettings(List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs)
        {
            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue).ToList());
            int groupId = assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0).FirstOrDefault().GroupId;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateGroupAssignmentPaper?groupId=" + groupId, assignmentPaperDTO.Beneficiaries).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult SaveAssignmentPaperBeneficiary(List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs)
        {
            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0 && x.BeneficiaryOrgUnitId.HasValue).ToList());
            int groupId = assignmentPaperBeneficiaryVMs.Where(x => x.GroupId > 0).FirstOrDefault().GroupId;
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/UserProfile/UpdateGroupAssignmentPaper?groupId=" + groupId, assignmentPaperDTO.Beneficiaries).Result;
            string message = string.Empty;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error });
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information });

        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult AddBeneficiary(AssignmentPaperBeneficiaryVM assignmentPaperBeneficiaryVM)
        {
            try
            {
                string message = string.Empty;
                ViewData["ActionData"] = GetActions();
                List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiarys = new List<AssignmentPaperBeneficiaryVM>();

                if (assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs == null)
                {
                    assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(new List<AssignmentPaperBeneficiaryVM>(), 1, 0, false);
                }
                if (!assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs.Any(copy => copy.BeneficiaryOrgUnitId == assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId
                && copy.UserId == assignmentPaperBeneficiaryVM.UserId))
                {
                    assignmentPaperBeneficiaryVM.Key = assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs.Count + 1;
                    assignmentPaperBeneficiaryVM.OrderNo = assignmentPaperBeneficiaryVM?.AssignmentPaperBeneficiaryVMs?.Max(x => x.OrderNo) != null ? assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs.Max(x => x.OrderNo) + 1 : 1;
                    assignmentPaperBeneficiarys.Add(assignmentPaperBeneficiaryVM);
                }
                else
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AssignmentPaperBeneficiaries.BeneficiaryExist");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                if (assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId > 0)
                {
                    OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId.Value, SessionInfo.CultureShortName);
                    assignmentPaperBeneficiaryVM.OrgUnitName = orgUnitDTO.Name;
                    assignmentPaperBeneficiaryVM.OrgUnitCode = orgUnitDTO.Number;


                }
                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperSettingsGridPartial.cshtml", (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(assignmentPaperBeneficiarys.OrderBy(x => x.OrderNo).ToList(), 1, assignmentPaperBeneficiarys.Count, true))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult EditBeneficiary(AssignmentPaperBeneficiaryVM assignmentPaperBeneficiaryVM)
        {
            try
            {
                ViewData["ActionData"] = GetActions();
                string message = string.Empty;
                try
                {
                    List<AssignmentPaperBeneficiaryVM> beneficiarys = new List<AssignmentPaperBeneficiaryVM>();
                    if (!assignmentPaperBeneficiaryVM.AssignmentPaperBeneficiaryVMs
                        .Any(copy => copy.BeneficiaryOrgUnitId == assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId &&
                        copy.UserId == assignmentPaperBeneficiaryVM.UserId && copy.Key != assignmentPaperBeneficiaryVM.Key))
                    {
                        beneficiarys.Add(assignmentPaperBeneficiaryVM);
                    }
                    else
                    {
                        message = DbRes.TValidation("User.Transaction.AssignmentAlreadyAdded");
                        return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                    }
                    if (assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId > 0)
                    {
                        OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId.Value, SessionInfo.CultureShortName);
                        assignmentPaperBeneficiaryVM.OrgUnitName = orgUnitDTO.Name;
                        assignmentPaperBeneficiaryVM.OrgUnitCode = orgUnitDTO.Number;
                    }
                    return Json(new
                    {
                        MessageType = MessageType.Information,
                        MessageText = message,
                        Key = assignmentPaperBeneficiaryVM.Key,
                        Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperSettingsGridPartial.cshtml",
                        (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(beneficiarys, 1, beneficiarys.Count, true))
                    }, JsonRequestBehavior.AllowGet);
                }
                catch (Exception)
                {
                    throw;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteBeneficiary(string ids, string hdnBeneficiariesGrid)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = new List<AssignmentPaperBeneficiaryVM>();

                if (!string.IsNullOrEmpty(hdnBeneficiariesGrid))
                {
                    object objects = javaScriptSerializer.Deserialize(hdnBeneficiariesGrid, typeof(object[]));

                    List<object> list = ((object[])objects).ToList();

                    objects = list.ToArray<object>();

                    ((object[])objects).ToList().ForEach(o =>
                    {
                        if (o is AssignmentPaperBeneficiaryVM)
                        {
                            assignmentPaperBeneficiaryVMs.Add(o as AssignmentPaperBeneficiaryVM);
                        }
                        else
                        {
                            AssignmentPaperBeneficiaryVM assignmentPaperBeneficiary =
                                javaScriptSerializer.Deserialize<AssignmentPaperBeneficiaryVM>(javaScriptSerializer.Serialize(o));

                            assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiary);
                        }
                    });
                }

                List<int> BeneficiaryIds = ids.Split(',').Select(int.Parse).ToList();

                BeneficiaryIds.ForEach(id =>
                {
                    AssignmentPaperBeneficiaryVM remove = assignmentPaperBeneficiaryVMs.Where(n => n.Key == id).FirstOrDefault();
                    assignmentPaperBeneficiaryVMs.Remove(remove);
                });

                string data = JsonConvert.SerializeObject(assignmentPaperBeneficiaryVMs);

                IAjaxGrid grid = (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory().CreateAjaxGrid(assignmentPaperBeneficiaryVMs, 1, assignmentPaperBeneficiaryVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/UserPreferences/AssignmentPaper/_AssignmentPaperSettingsGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.UserPreferences.AssignmentPaper)]
        public ActionResult GetBeneficiaryByAssignmentPaperGroupId(int assignmentPaperGroupId)
        {
            try
            {
                string message = string.Empty;
                List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiarys = new List<AssignmentPaperBeneficiaryVM>();


                GetResult<List<AssignmentPaperBeneficiaryDTO>> assignmentPaperGroupDTOList = HttpClientWrapper<GetResult<List<AssignmentPaperBeneficiaryDTO>>>
                    .GetItemRequest(string.Format("api/UserProfile/GetBeneficiaryByAssignmentPaperGroupId?groupId={0}&cultureName={1}", assignmentPaperGroupId, SessionInfo.CultureShortName)).Result;
                assignmentPaperBeneficiarys = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperGroupDTOList.Result).OrderBy(x => x.OrderNo).ToList();

                ViewData["ActionData"] = GetActions();

                if (assignmentPaperBeneficiarys != null)
                {
                    int i = 0;
                    assignmentPaperBeneficiarys.ForEach(beneficiary =>
                    {

                        i++;
                        beneficiary.OrderNo = i;
                        beneficiary.Key = i;
                        beneficiary.UserId = beneficiary.UserId <= 0 || !beneficiary.UserId.HasValue ? -1 : beneficiary.UserId;
                        if (beneficiary.BeneficiaryOrgUnitId > 0)
                        {
                            OrgUnitDTO orgUnitDTO = OrgHelper.GetOrgUnit(beneficiary.BeneficiaryOrgUnitId.Value, SessionInfo.CultureShortName);
                            beneficiary.OrgUnitName = orgUnitDTO.Name;
                            beneficiary.OrgUnitCode = orgUnitDTO.Number;
                        }
                    });
                }



                return Json(new
                {
                    MessageType = MessageType.Information,
                    MessageText = message,
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, @"~\Areas\User\Views\UserPreferences\AssignmentPaper\_AssignmentPaperSettingsGridPartial.cshtml", (AjaxGrid<AssignmentPaperBeneficiaryVM>)new AjaxGridFactory()
                    .CreateAjaxGrid(assignmentPaperBeneficiarys.OrderBy(x => x.OrderNo).ToList(), 1, assignmentPaperBeneficiarys.Count, false))
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }




        #endregion


        protected string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

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
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetPriorities()
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<List<PriorityDTO>> priorityDTOs = HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetPriorities?cultureName={0}", SessionInfo.CultureShortName)).Result;

                List<PriorityVM> priorityVMs = PriorityMapper.Map(priorityDTOs.Result);
                foreach (PriorityVM priorityVM in priorityVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = priorityVM.Id.ToString(),
                        Label = priorityVM.LocalName
                    });
                }
                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected string GetTransactionCategoryLookups(TransactionCategory transactionType = TransactionCategory.None)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);


                if (lookupVMs != null)
                {
                    foreach (LookupVM lookupVM in lookupVMs.Result.Where(t => transactionType == TransactionCategory.None || t.Id == (int)transactionType.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName)))
                    {
                        if (lookupVM.Id != (int)TransactionCategory.None &&
                            lookupVM.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, string.Empty))
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = lookupVM.Id.ToString(),
                                Label = lookupVM.Text
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
        private string GetCultures()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;


            List<CultureVM> cultureVMs = CultureMapper.Map(cultureDTOs.Result);
            if (cultureVMs != null)
            {
                foreach (CultureVM cultureDTOVM in cultureVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = cultureDTOVM.Id.ToString(),
                        Label = cultureDTOVM.LocalName
                    });
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }
        private string GetThemes()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ThemeDTO>> themeDTOs = HttpClientWrapper<GetResult<List<ThemeDTO>>>.GetItemRequest(string.Format("api/Common/GetThemes?CultureName={0}", SessionInfo.CultureShortName)).Result;
            List<ThemeVM> themeVMs = ThemeMapper.Map(themeDTOs.Result);
            if (themeVMs != null)
            {
                foreach (ThemeVM themeVM in themeVMs)
                {
                    dataSource.Add(new AutoCompleteDataSource()
                    {
                        Value = themeVM.Id.ToString(),
                        Label = themeVM.LocalName
                    });
                }
            }



            return JsonConvert.SerializeObject(dataSource);
        }
        private bool IsValid(Stream InputStream)
        {
            bool isValid = false;
            string message = string.Empty;
            GetResult<List<AttachmentExtensionDTO>> attachmentTypeDTOs =
            HttpClientWrapper<GetResult<List<AttachmentExtensionDTO>>>.GetItemRequest(string.Format("api/Admin/GetAttachmentExtentions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

            List<Admin.Models.Lookups.AttachmentExtensionVM> attachmentExtensionVMs = Admin.Mappers.AttachmentExtensionMapper.Map(attachmentTypeDTOs.Result);
            var validFileExtentions = attachmentExtensionVMs;
            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(InputStream);
            var fileType = format.MediaType;
            //bool isValidExtension = validFileExtentions.ToList().Any(y => fileType.Trim().ToLower().EndsWith(y));
            if (validFileExtentions != null && validFileExtentions.Any())
            {
                foreach (Admin.Models.Lookups.AttachmentExtensionVM attachment in validFileExtentions)
                {
                    if (format.ToString() == attachment.ExtensionName || format.Extension == attachment.ExtensionName)
                    {

                        bool isValidExtension = true;

                        if (format == null || !isValidExtension)
                        {

                            isValid = false;
                            return isValid;
                        }
                        else
                        {
                            isValid = true;
                        }
                    }

                }
            }
            return isValid;

        }

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
        public ActionResult SignatureAuthentication(string signtureTypeId)
        {


            SigntureType Signture = (SigntureType)Enum.Parse(typeof(SigntureType), signtureTypeId, true);
            CredentialVM credentialVM = new CredentialVM() { SigntureType = (SigntureType)Signture };
            return Json(new
            {
                Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_SignatureAuthentication", credentialVM),
                MessageType = MessageType.Information
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SignatureAuthentication(CredentialVM credentialVM)
        {
            string message = string.Empty;
            GetResult<bool> userPreferenceResult = HttpClientWrapper<GetResult<bool>>
                .PostRequest($"api/UserProfile/VerifySignaturePassword?userId={SessionInfo.CurrentUser.Id}", credentialVM).Result;

            if (userPreferenceResult.StatusCode != StatusCode.Ok)
            {

                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userPreferenceResult.StatusCode.ToString());
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            bool isMatchCurrentPassword = userPreferenceResult.Result;
            if (isMatchCurrentPassword == false)
            {
                return Json(new { MessageText = DbRes.TResource("UserPreferences.CurrentPasswordIncorrect"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            string SigntureTypeId = credentialVM.SigntureType.ToString();

            return Json(new { SigntureTypeId, MessageText = "", MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public ActionResult PendingRoleRequest()
        {
            try
            {
                if (SessionInfo.CurrentUser.UserOrgUnits.Where(x => x.Id == SessionInfo.OrgUnitId && x.ManagerId == SessionInfo.CurrentUser.Id).Count() > 0)
                {
                    string message = string.Empty;
                    GetResult<List<UserPendingGroupDTO>> userPendingGroupDTOs;
                    userPendingGroupDTOs = HttpClientWrapper<GetResult<List<UserPendingGroupDTO>>>.GetItemRequest(string.Format("api/Common/GetuserPendingRequest?CultureName={0}", SessionInfo.CultureShortName)).Result;
                    List<UserPendingRequest> userPendingGroupVMs = UserPreferenceMapper.Map(userPendingGroupDTOs.Result);
                    return View(userPendingGroupVMs);
                }
                else
                    throw new Exception();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ApproveManagerRoleRequest(int Id)
        {
            string message = "";
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Common/ApproveManagerRoleRequest?id={0}", Id), null).Result;

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.USerPreferences.SaveCucceeded");
                return Json(new { MessageType = MessageType.Information, result = postResult.Result, MessageText = message });

            }
            catch (Exception)
            {
                message = "نأسف , حدث خطأ الرجاء المحاولة لاحقا";
                return Json(new { MessageType = MessageType.Error, MessageText = message });
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RejectManagerRoleRequest(int Id)
        {
            string message = "";
            try
            {
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Common/RejectManagerRoleRequest?id={0}", Id), null).Result;
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.USerPreferences.SaveCucceeded");
                return Json(new { MessageType = MessageType.Information, MessageText = message });

            }
            catch (Exception)
            {
                message = "نأسف , حدث خطأ الرجاء المحاولة لاحقا";
                return Json(new { MessageType = MessageType.Error, MessageText = message });
            }
        }

        protected string GetActions()
        {
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

            GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Common/GetAllActions?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<ActionVM> processVMs = ActionMapper.Map(actionDTOs.Result);
            if (processVMs != null)
            {
                foreach (ActionVM actionVM in processVMs)
                {
                    AutoCompleteDataSource autoCompleteDataSource = new AutoCompleteDataSource()
                    {
                        Value = actionVM.Id.ToString(),
                        Label = actionVM.LocalName,
                        Parameters = new object[1]
                    };

                    autoCompleteDataSource.Parameters[0] = actionVM.TypeId;

                    dataSource.Add(autoCompleteDataSource);
                }
            }

            return JsonConvert.SerializeObject(dataSource);
        }



    }
}