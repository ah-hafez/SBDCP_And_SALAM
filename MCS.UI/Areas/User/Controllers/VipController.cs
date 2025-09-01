using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Controls;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using AssignmentPaperMapper = MCS.UI.Areas.User.Mappers.OrgUnit.AssignmentPaperMapper;
using OrgUnitMapper = MCS.UI.Areas.User.Mappers.OrgUnit.OrgUnitMapper;


namespace MCS.UI.Areas.User.Controllers
{
    public class VipController : TransactionController
    {
        // GET: User/Vip
        public ActionResult VipCopy(string transactionId, string transactionCopyId)
        {
            try
            {

                int TransactionID = int.Parse(StringCipher.DecryptStringAES(transactionId.Replace(" ", "+")));
                int trxCopyId = int.Parse(StringCipher.DecryptStringAES(transactionCopyId.Replace(" ", "+")));
                CopyViewModel copyViewModel = new CopyViewModel();
                var transactionBasicInfoDTO = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfo?transactionId={0}&cultureName={1}&transactionCopyId={2}", TransactionID, SessionInfo.CultureShortName, trxCopyId)).Result;

                var transactionCopyDTOs = HttpClientWrapper<GetResult<List<TransactionCopyDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetTransactionCopiesByTransactionId?transactionId={0}&cultureName={1}", TransactionID, SessionInfo.CultureShortName)).Result;
                copyViewModel.TransactionCopyVM.Copies = TransactionCopyMapper.Map(transactionCopyDTOs.Result);


                //copyViewModel.TransactionCopyVM.Copies.RemoveAll(x => x.IsBcc == true);
                InitializerAssignmentPaperData(TransactionID);

                copyViewModel.TransactionBasicInfoVM = TransactionBasicInfoMapper.Map(transactionBasicInfoDTO.Result);
                copyViewModel.DocumentVM = TransactionHelper.GetMainDocument(TransactionID);
                copyViewModel.TransactionBasicInfoVM.TransactionId = TransactionID;
                copyViewModel.TransactionFollowUp.TransactionId = TransactionID;
                copyViewModel.TransactionFollowUp.IsImportant = false;
                copyViewModel.TransactionFollowUp.IsCopy = true;
                copyViewModel.TransactionFollowUp.UserId = SessionInfo.CurrentUser.Id;
                copyViewModel.TransactionFollowUp.ToEntityId = SessionInfo.OrgUnitId;

                var orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                    .GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;


                GetAssignmentPaper();
                ViewData["TransactionId"] = TransactionID;
                ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["ActionData"] = TransactionHelper.GetOrgUnitActions();
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                ViewData["EditorMainDocumentSessionKey"] = "DocoNutDocument";
                ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["ConfidentialityData"] = TransactionHelper.GetTransactionConfidentialityLevel();
                //ViewData["FollowUpDepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["FollowUpProccess"] = GetFollowUpProccess(TransactionCategory.All);
                ViewData["FollowupPeriod"] = FollowupPeriod();
                ViewData["ExplanationConfidentialityData"] = TransactionHelper.GetExplanationConfidentialityLevel();
                VipInitializer(copyViewModel);
                DoconutInitializer(TransactionID);

                return View("~/Areas/User/Views/Vip/Copies/TransactionCopy.cshtml", copyViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void VipInitializer(CopyViewModel copyViewModel)
        {
            Session["DocoNutDocument"] = null;
            TextEditorViewModel textEditorViewModel = new TextEditorViewModel();
            if (copyViewModel.DocumentVM != null && copyViewModel.DocumentVM.Size > 0)
            {
                string documentId = Guid.NewGuid().ToString();
                ViewData["hdnDocumentId"] = copyViewModel.DocumentVM.Id;
                if (string.IsNullOrEmpty(copyViewModel.DocumentVM.MimeType) || copyViewModel.DocumentVM.MimeType.ToLower() == System.Net.Mime.MediaTypeNames.Application.Pdf)
                {
                    copyViewModel.EditorType = copyViewModel.EditorType = EditorType.Scanning;
                    string sessionKey = Guid.NewGuid().ToString();
                    ViewData[sessionKey] = sessionKey;
                    Session["DocoNutDocument"] = copyViewModel.DocumentVM.Content;
                }
                else
                {
                    textEditorViewModel.EditorType = copyViewModel.EditorType = EditorType.TextEditor;
                    textEditorViewModel.IsSigned = false;
                    textEditorViewModel.IsScanning = false;
                    textEditorViewModel.Content = copyViewModel.DocumentVM != null && copyViewModel.DocumentVM.Content != null ? Encoding.UTF8.GetString(copyViewModel.DocumentVM.Content) : null;
                    ViewData["EditorViewModel"] = textEditorViewModel;
                }
            }
        }
        public void DoconutInitializer(int transactionId)
        {
            List<TransactionArchiveVM> transactionArchiveVMs = new List<TransactionArchiveVM>();
            DocumentVM documentVM = TransactionHelper.GetMainDocument(transactionId);
            if (documentVM != null)
            {
                string documentId = Guid.NewGuid().ToString();
                transactionArchiveVMs.Add(new TransactionArchiveVM
                {
                    Id = documentId,
                    EncryptDocumentId = AESEncrytDecry.Base64Encode(documentVM.Id.ToString()),
                    IsMainDocument = true,
                    DocumentId = documentVM.Id,
                    AttachmentTypeId = -1,
                    ArcivingTypeName = LookupsHelper.GetLookupItem(TransactionAttachmentType.Main.LookupIdentity(LookupCategory.TransactionAttachmentType, SessionInfo.CultureShortName),
                    SessionInfo.CultureShortName).Result.Text
                });
                ViewData["hdnArchivingMainDocumentArray"] = JsonConvert.SerializeObject(transactionArchiveVMs);
                ViewData["SessionArchiveMainDocumentKey"] = Guid.NewGuid().ToString();
                ViewData["MainDocumentId"] = documentVM.Id;
            }
        }

        public void GetAssignmentPaper(string savedTransactionAssignment)
        {
            GetResult<AssignmentPaperDTO> AssignmentPaperDTOs = HttpClientWrapper<GetResult<AssignmentPaperDTO>>
           .GetItemRequest(string.Format("api/UserProfile/GetAssignmentPaperByUserId?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;
            List<TransactionAssignmentVM> transactionAssignmentVMs = new List<TransactionAssignmentVM>();
            if (AssignmentPaperDTOs.Result != null && AssignmentPaperDTOs.Result.Beneficiaries != null)
            {
                transactionAssignmentVMs = AssignmentPaperDTOs.Result.Beneficiaries.Select(a =>
                {
                    TransactionAssignmentVM transactionAssignmentVM = new TransactionAssignmentVM();
                    transactionAssignmentVM.ToOrgUnitId = a.BeneficiaryOrgUnitId;
                    transactionAssignmentVM.ToOrgUnitName = a.OrgUnitName;
                    transactionAssignmentVM.ToUserId = a.UserId;
                    transactionAssignmentVM.ToUserName = a.UserName == null ? "استقبال الادارة" : a.UserName;
                    transactionAssignmentVM.GroupName = a.GroupName;
                    transactionAssignmentVM.GroupId = a.GroupId;
                    transactionAssignmentVM.ChkConstant = a.ChkConstant;
                    transactionAssignmentVM.GroupOrderNo = a.GroupOrderNo;

                    return transactionAssignmentVM;
                }).ToList();
            }

            ViewData["AssignmentPaperData"] = SessionInfo.CurrentUser.DefaultAssignmentPaper && !string.IsNullOrWhiteSpace(savedTransactionAssignment) ?
JsonConvert.DeserializeObject<List<TransactionAssignmentVM>>(savedTransactionAssignment) : transactionAssignmentVMs;


            //ViewData["AssignmentPaperData"] = transactionAssignmentVMs;

        }


        [HttpPost]
        public ActionResult AssignmentPaperAddTemporaryEntity(TransactionAssignmentVM transactionAssignmentVM, List<TransactionAssignmentVM> TransactionAssignments)
        {
            string message = string.Empty;
            bool already = false;
            if (TransactionAssignments != null)
            {
                foreach (var item in TransactionAssignments)
                {
                    if (transactionAssignmentVM.ToOrgUnitId == item.ToOrgUnitId && transactionAssignmentVM.ToUserId == item.ToUserId)
                    {
                        already = true;
                    }
                }
            }
            if (!already)
            {
                GetResult<OrgUnitDTO> orgUnitDTO =
              HttpClientWrapper<GetResult<OrgUnitDTO>>.GetItemRequest(string.Format("api/Common/GetOrgUnit?orgUnitId={0}&cultureName={1}", transactionAssignmentVM.ToOrgUnitId, SessionInfo.CultureShortName)).Result;

                if (orgUnitDTO.Result != null)
                {
                    transactionAssignmentVM.ToOrgUnitName = orgUnitDTO.Result.Name;
                }
                if (transactionAssignmentVM.ToUserId != null)
                {
                    GetResult<EditUserProfileDTO> userProfileEditDTO = HttpClientWrapper<GetResult<EditUserProfileDTO>>.GetItemRequest(String.Format("api/Admin/GetUserById?userId={0}", transactionAssignmentVM.ToUserId)).Result;

                    if (userProfileEditDTO.Result != null)
                    {
                        transactionAssignmentVM.UserImageId = userProfileEditDTO.Result.UserImageId;

                    }
                }
                ViewData["AllActionsData"] = TransactionHelper.GetAllActions();
                transactionAssignmentVM.Key = TransactionAssignments != null ? TransactionAssignments.Count + 1 : 1;
                return Json(new { html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Vip/Copies/AssignmentPaper/_AssignmentAddEntitiesCopies.cshtml", transactionAssignmentVM) });

            }
            else
            {
                message = DbRes.TValidation("User.Transaction.Copy.OrgUnitAlreadyAdded");

                return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
            }
        }




        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult SaveCopy(CopyViewModel copyViewModel)
        {
            try
            {
                string message = string.Empty;

                //if (copyViewModel.TransactionFollowUp.ProccessPeriod != 99 )
                //{
                //    copyViewModel.TransactionFollowUp.FollowUpStatusId = (int)FollowupStatus.New;
                //    copyViewModel.TransactionFollowUp.CreationDate = DateTime.Now;
                //    copyViewModel.TransactionFollowUp.Active = true;
                //    copyViewModel.TransactionFollowUp.CreatingUserId = SessionInfo.CurrentUser.Id;
                //    copyViewModel.TransactionFollowUp.CreatingEntityId = SessionInfo.OrgUnitId; 

                //    if (copyViewModel.TransactionFollowUp.FollowUpTypeId == 2)
                //    {
                //        copyViewModel.TransactionFollowUp.FollowUpUserId = null;
                //        PostResult postResultFodept =
                //        HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/getFollowUpDepartment?EntityId={0}", SessionInfo.OrgUnitId), null).Result;

                //        if (postResultFodept.Id.HasValue)
                //        {
                //            copyViewModel.TransactionFollowUp.FollowUpEntityId = (int)postResultFodept.Id;

                //        }
                //        else
                //        {

                //            message = DbRes.TValidation("User.Transaction.FollowUp.FollowUpOrgUnitDoesNotExist");
                //            return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                //        }
                //    }


                //    if (copyViewModel.TransactionFollowUp.ProccessPeriod == -1)
                //        copyViewModel.TransactionFollowUp.FollowUpExpireDate = (DateTime)copyViewModel.TransactionFollowUp.DateTo;
                //    else
                //        copyViewModel.TransactionFollowUp.FollowUpExpireDate = DateTime.Now.AddDays(Convert.ToInt32(copyViewModel.TransactionFollowUp.ProccessPeriod));


                //    if (copyViewModel.TransactionFollowUp != null)
                //    {
                //        PostResult postResult =
                //  HttpClientWrapper<PostResult>.PostRequest("api/Transaction/TransactionFollowUpAdd?cultureName=" + SessionInfo.CultureShortName, TransactionFollowUpMapper.Map(copyViewModel.TransactionFollowUp)).Result;



                //        if (postResult.StatusCode != StatusCode.Ok)
                //        {
                //            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                //            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //        }
                //        if (postResult.Id.HasValue && postResult.Id > 0)
                //        {
                //            FollowUpAuditTrailVM followUpAuditTrail = new FollowUpAuditTrailVM();
                //            followUpAuditTrail.FollowupId = (int)postResult.Id;
                //            followUpAuditTrail.ProccessDate = DateTime.Now;
                //            followUpAuditTrail.ProccessId = copyViewModel.TransactionFollowUp.FollowUpTypeId == 1 ? (int)FollowupAuditProcess.AddPrivetFollowup : (int)FollowupAuditProcess.AddPublicFollowup;
                //            followUpAuditTrail.ProccessDescription = copyViewModel.TransactionFollowUp.FollowUpTypeId == 1 ? ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPrivetFollowUp") : ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.FollowUpProcess.AddPublicFollowUp");
                //            followUpAuditTrail.UserId = SessionInfo.CurrentUser.Id;
                //            followUpAuditTrail.EntityId = SessionInfo.OrgUnitId;
                //            PostResult postResultAudit =
                //            HttpClientWrapper<PostResult>.PostRequest("api/Transaction/AddFollowupUditTrial?cultureName=" + SessionInfo.CultureShortName, FollowUpAuditTrailMapper.Map(followUpAuditTrail)).Result;

                //        }
                //    }

                //}

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.OutboundInternal.UpdateSucceeded");
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
        protected string FollowupPeriod()
        {

            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                dataSource.Add(new AutoCompleteDataSource() { Value = "99", Label = "اختر" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "1", Label = "يوم" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "2", Label = "يومين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "3", Label = "ثلاثة أيام" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "7", Label = "أسبوع" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "14", Label = "أسبوعين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "30", Label = "شهر" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "60", Label = "شهرين" });
                dataSource.Add(new AutoCompleteDataSource() { Value = "-1", Label = "أخرى" });


                return JsonConvert.SerializeObject(dataSource);
            }
            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        public ActionResult SendACopisssignmentPaper(List<TransactionAssignmentVM> TransactionAssignments, string TransactionId, string explanationTxt, string ConfedentialityId, int deliveryMethodId, int? reporterId)
        {
            string message = string.Empty;
            int? TransactionAssignmentExplanationId = 0;



            foreach (var item in TransactionAssignments)
            {
                if (item.IsAssigned || item.IsCopy)
                {
                    if (item.ActionId == 0)
                    {
                        message = DbRes.TValidation("User.Transaction.AssignmentPaper.ActionsValidate");

                        break;
                    }
                }
            }
            if (message != string.Empty)
            {
                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            TransactionAssignments.ForEach(t =>
            {
                t.FromOrgUnitId = SessionInfo.OrgUnitId;
                if (t.ToUserId == -1)
                {
                    t.ToUserId = null;
                }
            });

            TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault().DeliveryMethodId = deliveryMethodId;
            TransactionAssignments.Where(ta => ta.IsAssigned == true).FirstOrDefault().ReporterId = reporterId;
            List<AssignmentPaperBeneficiaryVM> assignmentPaperBeneficiaryVMs = new List<AssignmentPaperBeneficiaryVM>();
            foreach (var item in TransactionAssignments)
            {
                AssignmentPaperBeneficiaryVM assignmentPaperBeneficiaryVM = new AssignmentPaperBeneficiaryVM();
                assignmentPaperBeneficiaryVM.BeneficiaryOrgUnitId = item.ToOrgUnitId;
                assignmentPaperBeneficiaryVM.Id = item.Id;
                //assignmentPaperBeneficiaryVM.Key = item.Key;
                assignmentPaperBeneficiaryVM.OrgUnitName = item.ToOrgUnitName;
                assignmentPaperBeneficiaryVM.UserId = item.ToUserId;
                assignmentPaperBeneficiaryVM.UserName = item.ToUserName;
                assignmentPaperBeneficiaryVM.UserImageId = item.UserImageId;
                assignmentPaperBeneficiaryVM.GroupId = item.GroupId;
                assignmentPaperBeneficiaryVMs.Add(assignmentPaperBeneficiaryVM);
            }

            AssignmentPaperDTO assignmentPaperDTO = new AssignmentPaperDTO();
            assignmentPaperDTO.Beneficiaries = AssignmentPaperBeneficiaryMapper.Map(assignmentPaperBeneficiaryVMs);

            int? OprEntityId = TransactionAssignments.Where(ta => ta.IsOpr == true).FirstOrDefault()?.ToOrgUnitId;
            List<TransactionCopyDTO> transactionCopyDTOs = TransactionAssignments.Where(ta => ta.IsCopy == true || ta.IsOpr == true).Select(tc => new TransactionCopyDTO
            {
                ActionId = tc.ActionId,
                UserId = tc.ToUserId,
                OrgUnitId = tc.ToOrgUnitId,
                IsSent = 1,
                FromUserId = SessionInfo.CurrentUser.Id,
                FromOrgUnitId = SessionInfo.OrgUnitId,
                SpecialExplanation = tc.SpecialExplanation,
                GeneralExplanation = explanationTxt,
                IsBcc = tc.IsBcc,
                IsOpr = tc.IsOpr,
                OprEntityId = OprEntityId
            }).ToList();

            PostResult postCopiesResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddAssignmentCopies?transactionId={0}", TransactionId), transactionCopyDTOs).Result;



            if (postCopiesResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postCopiesResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            if (explanationTxt != string.Empty)
            {
                byte[] data = Encoding.Unicode.GetBytes(explanationTxt.Trim());
                ExplanationVM explanationVM = new ExplanationVM()
                {
                    Description = explanationTxt,
                    ConfidentialityId = int.Parse(ConfedentialityId),
                    FromUserId = SessionInfo.CurrentUser.Id,
                    EditorType = EditorType.Text,
                    DocumentVM = new Areas.User.Models.Shared.DocumentVM()
                    {
                        MimeType = System.Net.Mime.MediaTypeNames.Text.Plain,
                        Content = data,
                        Size = data.Length,
                        FromEntityId = SessionInfo.OrgUnitId,
                        FromUserId = SessionInfo.CurrentUser.Id
                    }
                };

                PostResult postExplanationResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/AddTransactionExplanation?transactionId={0}", TransactionId), ExplanationMapper.Map(explanationVM)).Result;

                if (postExplanationResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postExplanationResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                TransactionAssignmentExplanationId = postExplanationResult.Id;
            }

            PutResult UpdateTransactionAssignmentHistory = HttpClientWrapper<PutResult>
                              .PutRequest(string.Format("api/Transaction/UpdateTransactionAssignmentHistory?transactionId={0}&ExplanationId={1}", TransactionId, TransactionAssignmentExplanationId), null).Result;


            PostResult putResult =
                HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MoveTransactionsList?transactionsIds={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}", TransactionId, SessionInfo.OrgUnitId, (int)TrayActionType.Viewed, null, 7, null, SessionInfo.CurrentUser.Id), null).Result;

            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            string url = UrlHelper.GetBaseUri() + "/User/File/Copies";

            message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Assignment.SendSucceeded");

            return Json(new
            {
                MessageText = message,
                MessageType = MessageType.Information,
                returnUrl = url
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult ViewedCopy(string TransactionsId)
        {
            try
            {
                string message = string.Empty;
                PostResult putResult =
                    HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/MoveTransactionsList?transactionsIds={0}&orgUnitId={1}&trayActionTypeId={2}&assigmentId={3}&trayId={4}&remarks={5}&userId={6}", TransactionsId, SessionInfo.OrgUnitId, (int)TrayActionType.Viewed, null, 7, null, SessionInfo.CurrentUser.Id), null).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception)
            {
                throw;
            }
        }
    }

}