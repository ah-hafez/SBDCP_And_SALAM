using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Framework.Localization;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.DTO.Escalation;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models.Escalation;
using MCS.UI.Areas.Admin.Models.Lookups;
using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class EscalationController : AdminControllerBase
    {

        public ActionResult AllEscalations(int? TransactionCategoryId)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                TransactionCategory transactionCategory = TransactionCategory.Inbound;
                if (TransactionCategoryId != null && TransactionCategoryId > 0)
                {
                    transactionCategory = (TransactionCategory)Enum.ToObject(typeof(TransactionCategory), TransactionCategoryId.Value.LookupInternalID(LookupCategory.TransactionCategory, SessionInfo.CultureShortName));
                }

                GetResult<List<PriorityDTO>> priorityDTOs =
                    HttpClientWrapper<GetResult<List<PriorityDTO>>>.GetItemRequest(string.Format("api/Admin/GetPriorities?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                List<PriorityVM> priorityVMs = PriorityMapper.Map(priorityDTOs.Result);
                List<EscalationVM> allEscalationVMs = new List<EscalationVM>();

                foreach (var item in priorityVMs)
                {
                    GetResult<List<EscalationDTO>> EscalationDTOs =
                             HttpClientWrapper<GetResult<List<EscalationDTO>>>.GetItemRequest(string.Format("api/Admin/GetEscalationsByPriorityId?TransactionCategoryId={0}&priorityId={1}&CultureName={2}", (int)transactionCategory, item.Id, SessionInfo.CultureShortName)).Result;
                    EscalationVM escalationVM = new EscalationVM();
                    if (EscalationDTOs.Result.Count > 0)
                    {
                        List<EscalationVM> escalationVMs = EscalationMapper.Map(EscalationDTOs.Result);
                        foreach (var e in escalationVMs)
                        {
                            escalationVM = new EscalationVM()
                            {
                                EscalationsGrid = escalationVMs,
                                TransactionCategory = (int)transactionCategory,
                                Priority = e.Priority,
                                TransactionCategoryName = transactionCategory.ToString(),
                                EscalationAction = e.EscalationAction,
                                EscalationActionId = e.EscalationActionId,
                                EscalationAfterDays = e.EscalationAfterDays,
                                EscalationTo = e.EscalationTo,
                                EscalationToId = e.EscalationToId,
                                Id = e.Id,
                                PriorityId = e.PriorityId,
                            };
                        }
                    }
                    else
                    {
                        escalationVM = new EscalationVM()
                        {
                            TransactionCategory = (int)transactionCategory,
                            Priority = item.LocalName,
                            TransactionCategoryName = transactionCategory.ToString(),
                            PriorityId = item.Id,
                        };
                    }
                    allEscalationVMs.Add(escalationVM);
                }

                ViewData["Priorties"] = priorityVMs;
                ViewData["EscalationTo"] = GetEscalationLookups(LookupCategory.EscalationTo);
                ViewData["EscalationAction"] = GetEscalationLookups(LookupCategory.EscalationAction);

                return View(allEscalationVMs);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //[HttpPost]
        [ValidateAntiForgeryToken()]
        [HttpPost]
        public ActionResult AddEscalation(EscalationVM escalation, List<EscalationVM> EscalationsGrid)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                EscalationsGrid = EscalationsGrid ?? new List<EscalationVM>();
                if (!EscalationsGrid.Any(es => es.TransactionCategory == escalation.TransactionCategory && es.PriorityId == escalation.PriorityId && es.EscalationActionId == escalation.EscalationActionId && es.EscalationToId == escalation.EscalationToId))
                {

                    PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostEscalation", EscalationMapper.Map(escalation)).Result;
                    if (postResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = DbRes.TValidation("Admin.Escalation.AlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }

                var TransactionCategId = escalation.TransactionCategory;
                GetResult<List<EscalationDTO>> escalationDTOs =
                    HttpClientWrapper<GetResult<List<EscalationDTO>>>.GetItemRequest(string.Format("api/Admin/GetEscalationsByPriorityId?TransactionCategoryId={0}&priorityId={1}&CultureName={2}", TransactionCategId, escalation.PriorityId, SessionInfo.CultureShortName)).Result;

                if (escalationDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, escalationDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<EscalationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(EscalationMapper.Map(escalationDTOs.Result), 1, escalationDTOs.RowsCount.Value, false);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.AddSucceeded");
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EscalationsGridPartial", grid), MessageText = message, MessageType = MessageType.Information, priorityId = escalation.PriorityId }, JsonRequestBehavior.AllowGet);
            }

            catch (Exception)
            {
                throw;
            }
        }
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditEscalation(EscalationVM escalation, List<EscalationVM> EscalationsGrid)
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
                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                if (!EscalationsGrid.Any(row => row.EscalationToId == escalation.EscalationToId && row.EscalationActionId == escalation.EscalationActionId && row.Id != escalation.Id))
                {
                    PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutEscalation", EscalationMapper.Map(escalation)).Result;

                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());
                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    message = DbRes.TValidation("Admin.Escalation.AlreadyAdded");
                    return Json(new { MessageType = MessageType.Error, MessageText = message }, JsonRequestBehavior.AllowGet);
                }
                GetResult<List<EscalationDTO>> escalationDTOs =
                                    HttpClientWrapper<GetResult<List<EscalationDTO>>>.GetItemRequest(string.Format("api/Admin/GetEscalationsByPriorityId?TransactionCategoryId={0}&priorityId={1}&CultureName={2}", escalation.TransactionCategory, escalation.PriorityId, SessionInfo.CultureShortName)).Result;

                if (escalationDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, escalationDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }
                List<EscalationVM> escalationVMs = (EscalationMapper.Map(escalationDTOs.Result));

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<EscalationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(escalationVMs, 1, escalationDTOs.RowsCount.Value, false);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Priority.UpdateSucceeded");

                return Json
                   (new
                   {
                       Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EscalationsGridPartial", grid),
                       MessageText = message,
                       MessageType = MessageType.Information,
                       priorityId = escalation.PriorityId,
                       Id = escalation.Id
                   },
                   JsonRequestBehavior.AllowGet);


            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteEscalation(string ids)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                GetResult<int> DeletedItemCategoryId =
                    HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/Admin/GetEscalationCategoryId?EscalationId={0}", ids)).Result;

                GetResult<int> DeletedItemPriorityId =
                    HttpClientWrapper<GetResult<int>>.GetItemRequest(string.Format("api/Admin/GetEscalationPriorityId?EscalationId={0}", ids)).Result;

                DeleteResult deleteResult = HttpClientWrapper<DeleteResult>.DeleteRequest(String.Format("api/Admin/DeleteEscalation?Id={0}", ids)).Result;
                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<EscalationDTO>> EscalationDTOs =
                            HttpClientWrapper<GetResult<List<EscalationDTO>>>.GetItemRequest(string.Format("api/Admin/GetEscalationsByPriorityId?TransactionCategoryId={0}&priorityId={1}&CultureName={2}", DeletedItemCategoryId.Result, DeletedItemPriorityId.Result, SessionInfo.CultureShortName)).Result;
                List<EscalationVM> escalationVMs = EscalationMapper.Map(EscalationDTOs.Result);

                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<EscalationVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(escalationVMs, 1, escalationVMs.Count, false);
                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.LetterType.DeleteSucceeded");
                return Json
                   (new
                   {
                       Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EscalationsGridPartial", grid),
                       MessageText = message,
                       MessageType = MessageType.Information,
                       priorityId = DeletedItemPriorityId.Result
                   },
                   JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private List<TransactionCategoryVM> GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.TransactionCategories, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();

            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    if (lookupVM.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName) & lookupVM.Id != TransactionCategory.DraftOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        transactionCategoryVMs.Add(new TransactionCategoryVM()
                        {
                            Id = (lookupVM.EnumReference != null) ? lookupVM.EnumReference.Value : -1,
                            Text = lookupVM.Text,
                        });
                    }
                }
            }

            return transactionCategoryVMs;
        }

        [HttpGet]
        public ActionResult GetEscalation(string id)
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

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);
                GetResult<EscalationDTO> EscalationDTO =
                   HttpClientWrapper<GetResult<EscalationDTO>>.GetItemRequest(string.Format("api/Admin/GetEscalationById?EscalationId={0}&CultureName={1}", id, SessionInfo.CultureShortName)).Result;


                if (EscalationDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, EscalationDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                EscalationVM escalationVM = EscalationMapper.Map(EscalationDTO.Result);


                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentType.UpdateSucceeded");

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EscalationLevelPartial", escalationVM),
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
        public string GetEscalationLookups(LookupCategory lookupCategory)
        {
            try
            {

                var dataSource = new List<AutoCompleteDataSource>();
                GetResult<IList<User.Models.Lookups.LookupVM>> lookups = LookupsHelper.GetLookupItems(lookupCategory, SessionInfo.CultureShortName);
                if (lookups.Result != null)
                {
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


        //public static string GetPriorities(TransactionCategory transactionCategory)
        //    {
        //        try
        //        {
        //            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

        //            GetResult<List<User.Models.Lookups.PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(transactionCategory);
        //            if (priorityVMs.Result != null)
        //            {

        //                foreach (User.Models.Lookups.PriorityVM priorityVM in priorityVMs.Result)
        //                {
        //                    dataSource.Add(new AutoCompleteDataSource()
        //                    {
        //                        Value = priorityVM.Id.ToString(),
        //                        Label = priorityVM.LocalName,
        //                        Parameters = new object[] { priorityVM.HasDate }
        //                    });
        //                }
        //            }

        //            return JsonConvert.SerializeObject(dataSource);
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }

    }
}