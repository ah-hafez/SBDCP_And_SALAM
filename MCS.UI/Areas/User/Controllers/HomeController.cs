using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
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
using MCS.UI.Areas.User.Mappers.Assignment;
using MCS.UI.Areas.User.Mappers.File;
using MCS.UI.Areas.User.Mappers.OrgUnit;
using MCS.UI.Areas.User.Mappers.Report;
using MCS.UI.Areas.User.Mappers.Search.TransactionCertificate;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Mappers.UserManagement;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Assignment;
using MCS.UI.Areas.User.Models.File;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Permission;
using MCS.UI.Areas.User.Models.Report;
using MCS.UI.Areas.User.Models.Search.TransactionCertificate;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Areas.User.Models.UserCategories;
using MCS.UI.Areas.User.Models.UserManagement;
using MCS.UI.Common;
using MCS.UI.TraysUISettings;
using DocumentFormat.OpenXml.Office2010.Excel;

namespace MCS.UI.Areas.User.Controllers
{
    [CustomAuthorize()]
    public class HomeController : BaseController
    {
        [HttpGet]
        public ActionResult Tasks()
        {
            try
            {
                TasksViewModel tasksViewModel = new TasksViewModel();

                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                string pageSize = settingVM.Value;

                int pageIndex = 1;

                GetResult<List<ReceivedTaskDTO>> ReceivedTaskDTOs =
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, pageSize, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (ReceivedTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ReceivedTaskDTOs.StatusCode.ToString());
                }

                GetResult<List<SentTaskDTO>> sentTaskDTOs =
                  HttpClientWrapper<GetResult<List<SentTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSentTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, pageSize, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (sentTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(sentTaskDTOs.StatusCode.ToString());
                }

                tasksViewModel.ReceivedTaskVMs = ReceivedTaskMapper.Map(ReceivedTaskDTOs.Result);
                tasksViewModel.ReceivedTasksCount = ReceivedTaskDTOs.RowsCount.Value;
                tasksViewModel.SentTaskVMs = SentTaskMapper.Map(sentTaskDTOs.Result);
                tasksViewModel.SentTasksCount = sentTaskDTOs.RowsCount.Value;

                ViewData["SelectedPageIndex"] = pageIndex;
                ViewData["PageSize"] = pageSize;

                return View(tasksViewModel);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Edit)]
        public ActionResult ExtendTaskDate(int taskId, string dileveryDate)
        {
            try
            {
                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest(String.Format("api/Transaction/ExtendTaskDate?taskId={0}&dateTime={1}", taskId, dileveryDate), null).Result;

                string message = string.Empty;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = DbRes.TResource("User.Task.ExtendTaskDate.ExtendDateFail");

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = DbRes.TResource("User.Task.ExtendTaskDate.ExtendDateSuccess");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Reminder)]
        public ActionResult ReminderTask(int taskId)
        {
            try
            {
                string message = string.Empty;

                MessageType messageType = MessageType.Information;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostTaskReminder?taskId={0}&cultureName={1}", taskId, SessionInfo.CultureShortName), null).Result;

                message = DbRes.TResource("User.Task.ReminderTask.ReminderTaskFail");

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = DbRes.TResource("User.Task.ReminderTask.ReminderTaskSuccess");

                    messageType = MessageType.Error;
                }

                return Json(new { MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetSentTasks(int pageIndex, int? pageSize)
        {
            try
            {
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                GetResult<List<SentTaskDTO>> sentTaskDTOs =
                   HttpClientWrapper<GetResult<List<SentTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSentTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, settingVM.Value, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (sentTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(sentTaskDTOs.StatusCode.ToString());
                }

                ViewData["SelectedPageIndex"] = pageIndex;
                ViewData["SentTasksCount"] = sentTaskDTOs.RowsCount.Value;
                ViewData["PageSize"] = settingVM.Value;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "SendTasksPartial", SentTaskMapper.Map(sentTaskDTOs.Result)), MessageType = MessageType.Information, PageSize = settingVM.Value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetReceivedTasks(int pageIndex, int? pageSize)
        {
            try
            {
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);

                GetResult<List<ReceivedTaskDTO>> ReceivedTaskDTOs =
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, settingVM.Value, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (ReceivedTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ReceivedTaskDTOs.StatusCode.ToString());
                }

                ViewData["SelectedPageIndex"] = pageIndex;
                ViewData["ReceivedTasksCount"] = ReceivedTaskDTOs.RowsCount.Value;
                ViewData["PageSize"] = settingVM.Value;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "ReceivedTasksPartial", ReceivedTaskMapper.Map(ReceivedTaskDTOs.Result)), MessageType = MessageType.Information, PageSize = settingVM.Value }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult RejectTask(int pageIndex, int taskId, string rejectReason)
        {
            try
            {
                string message = string.Empty;

                TaskActionDTO taskActionDTO = new TaskActionDTO();
                taskActionDTO.TaskId = taskId;
                taskActionDTO.Description = rejectReason;
                taskActionDTO.Subject = rejectReason;

                PutResult putResult =
                    HttpClientWrapper<PutResult>.PutRequest("api/Transaction/PutRejectTransactionTask", taskActionDTO).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = DbRes.TResource("User.Task.RejectTask.RejectTaskSuccess");

                return GetReceivedTaskBypageIndex(pageIndex, message, MessageType.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult AcceptTask(int pageIndex, int taskId)
        {
            try
            {
                TaskActionVM taskActionVM = new TaskActionVM();

                ViewData["SelectedPageIndex"] = pageIndex;

                taskActionVM.TaskId = taskId;

                return View(taskActionVM);
            }

            catch (Exception)
            {

                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Tasks.DisplayLink)]
        public ActionResult ReceivedTaskDetails(int taskId)
        {
            try
            {
                return View();
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.DisplayLink)]
        public ActionResult ShowReceivedTask(int taskId)
        {
            try
            {
                string message = string.Empty;

                GetResult<ReceivedTaskDTO> ReceivedTaskDTO =
                   HttpClientWrapper<GetResult<ReceivedTaskDTO>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTask?taskId={0}&cultureName={2}", taskId, SessionInfo.CultureShortName)).Result;

                if (ReceivedTaskDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ReceivedTaskDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return View();
            }

            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.DisplayLink)]
        public ActionResult ViewReceivedTask(int taskId)
        {
            try
            {
                string message = string.Empty;

                GetResult<ReceivedTaskDTO> ReceivedTaskDTO =
                   HttpClientWrapper<GetResult<ReceivedTaskDTO>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTask?taskId={0}&cultureName={1}", taskId, SessionInfo.CultureShortName)).Result;

                if (ReceivedTaskDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ReceivedTaskDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return View(ReceivedTaskMapper.Map(ReceivedTaskDTO.Result));
            }

            catch (Exception)
            {

                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.DisplayLink)]
        public ActionResult ViewSentTask(int taskId)
        {
            try
            {
                string message = string.Empty;

                GetResult<SentTaskDTO> sentTaskDTO =
                   HttpClientWrapper<GetResult<SentTaskDTO>>.GetItemRequest(string.Format("api/Transaction/GetSentTask?taskId={0}&cultureName={1}", taskId, SessionInfo.CultureShortName)).Result;

                if (sentTaskDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, sentTaskDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                return View(SentTaskMapper.Map(sentTaskDTO.Result));
            }

            catch (Exception)
            {

                throw;
            }
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //public ActionResult GetAcceptTask(int pageIndex, TaskActionVM taskActionVM)
        //{
        //    try
        //    {
        //        string message = string.Empty;

        //        taskActionVM.Document = new DocumentVM();

        //        using (MemoryStream memoryStream = new MemoryStream())
        //        {
        //            if (Request.Files != null && Request.Files.Count > 0)
        //            {
        //                Request.Files[0].InputStream.CopyTo(memoryStream);

        //                taskActionVM.Document.Content = memoryStream.ToArray();
        //                taskActionVM.Document.MimeType = Request.Files[0].ContentType;
        //                taskActionVM.Document.Name = Request.Files[0].FileName;
        //            }
        //        };

        //        PutResult putResult =
        //            HttpClientWrapper<PutResult>.PutRequest("api/Transaction/PutCompleteTransactionTask", TaskActionMapper.Map(taskActionVM)).Result;

        //        if (putResult.StatusCode != StatusCode.Ok)
        //        {
        //            message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

        //            return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
        //        }

        //        message = DbRes.TResource("User.Task.TaskAdd.TaskCompleteSuccess");

        //        return GetReceivedTaskBypageIndex(pageIndex, message, MessageType.Information);
        //    }

        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult AddTask(TaskAddVM taskAddVM, string hdnTaskArray)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TaskAddVM> taskAddVMs = new List<TaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddVMs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<TaskAddVM>)) as List<TaskAddVM>);
                }

                bool checkDetail = true;

                taskAddVMs.ForEach(t =>
                {
                    if (t.SentToOrgUnitId == taskAddVM.SentToOrgUnitId && t.SentToUserId == taskAddVM.SentToUserId)
                    {
                        checkDetail = false;
                    }
                });

                if (checkDetail)
                {
                    taskAddVMs.Add(taskAddVM);
                }

                string data = JsonConvert.SerializeObject(taskAddVMs);

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddVMs, 1, taskAddVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSubTask(SubTaskAddVM subTaskAddVM, string hdnTaskArray)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubTaskAddVM> taskAddSubVMs = new List<SubTaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddSubVMs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<SubTaskAddVM>)) as List<SubTaskAddVM>);
                }

                bool checkDetail = true;

                taskAddSubVMs.ForEach(t =>
                {
                    if (t.ToOrgUnitId == subTaskAddVM.ToOrgUnitId && t.ToUserId == subTaskAddVM.ToUserId)
                    {
                        checkDetail = false;
                    }
                });

                if (checkDetail)
                {
                    taskAddSubVMs.Add(subTaskAddVM);
                }

                string data = JsonConvert.SerializeObject(taskAddSubVMs);

                IAjaxGrid grid = (AjaxGrid<SubTaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddSubVMs, 1, taskAddSubVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Home/_SubTasksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ActionResult DeleteSubTask(string ids, string hdnTaskArray)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubTaskAddVM> taskAddSubVMs = new List<SubTaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddSubVMs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<SubTaskAddVM>)) as List<SubTaskAddVM>);
                }

                int index = Convert.ToInt32(ids);

                taskAddSubVMs.RemoveAt(index);

                string data = JsonConvert.SerializeObject(taskAddSubVMs);

                IAjaxGrid grid = (AjaxGrid<SubTaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddSubVMs, 1, taskAddSubVMs.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Home/_SubTasksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        [ValidateAntiForgeryToken()]
        public ActionResult PostSubTasks(int pageIndex, int sentTasksPageIndex, int taskId, int transactionId, string hdnTaskArray)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<SubTaskAddVM> subTaskAddDTOs = new List<SubTaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    subTaskAddDTOs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<SubTaskAddVM>)) as List<SubTaskAddVM>);
                }

                foreach (SubTaskAddVM subTaskAddVM in subTaskAddDTOs)
                {
                    subTaskAddVM.FromOrgUnitId = SessionInfo.OrgUnitId;
                }

                TransactionSubTaskDTO transactionSubTaskDTO = new TransactionSubTaskDTO();

                transactionSubTaskDTO.ParentId = taskId;
                transactionSubTaskDTO.SubTasks = SubTaskAddMapper.Map(subTaskAddDTOs);
                transactionSubTaskDTO.TransactionId = transactionId;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostSubTransactionTask?cultureName={0}", SessionInfo.CultureShortName), transactionSubTaskDTO).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Task.SubTaskAdd.TaskAddedSuccess");
                GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
                var settingVM = SettingMapper.Map(SettingValue.Result);
                GetResult<List<ReceivedTaskDTO>> ReceivedTaskDTOs =
                   HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", sentTasksPageIndex, settingVM.Value, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (ReceivedTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(ReceivedTaskDTOs.StatusCode.ToString());
                }
                GetResult<List<SentTaskDTO>> sentTaskDTOs =
                  HttpClientWrapper<GetResult<List<SentTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSentTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, settingVM.Value, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                if (sentTaskDTOs.StatusCode != StatusCode.Ok)
                {
                    throw new Exception(sentTaskDTOs.StatusCode.ToString());
                }
                ViewData["SelectedPageIndex"] = pageIndex;
                ViewData["ReceivedTasksCount"] = ReceivedTaskDTOs.RowsCount;
                ViewData["PageSize"] = settingVM.Value;

                string ReceivedTasksPartial = UIHelper.RenderRazorViewToHtml(ControllerContext, "ReceivedTasksPartial", ReceivedTaskMapper.Map(ReceivedTaskDTOs.Result));

                ViewData["SentTasksCount"] = sentTaskDTOs.RowsCount;
                ViewData["SelectedPageIndex"] = sentTasksPageIndex;
                ViewData["PageSize"] = settingVM.Value;

                string SentTasksPartial = UIHelper.RenderRazorViewToHtml(ControllerContext, "SendTasksPartial", SentTaskMapper.Map(sentTaskDTOs.Result));

                return Json(new { Html = ReceivedTasksPartial, SendTasks = SentTasksPartial, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult SubTask(int pageIndex, int sentTasksPageIndex, int taskId, int transId)
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                        HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTaskSequenceOrgUnits?orgUnitId={0}&cultureName={1}&taskId={2} ", SessionInfo.OrgUnitId, SessionInfo.CultureShortName, taskId)).Result;

                ViewData["DepartmentsData"] = BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                ViewData["SelectedOrgUnitName"] = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;
                ViewData["hdnTaskArray"] = JsonConvert.SerializeObject(new List<TaskAddDTO>()); ;
                ViewData["taskId"] = taskId;
                ViewData["pageIndex"] = pageIndex;
                ViewData["sentTasksPageIndex"] = sentTasksPageIndex;

                IAjaxGrid grid = (AjaxGrid<SubTaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(new List<SubTaskAddVM>(), 1, 0, true);

                ViewData["Grid"] = grid;

                SubTaskAddVM subTaskAddVM = new SubTaskAddVM();

                subTaskAddVM.TransactionId = transId;

                return View(subTaskAddVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public string GetUsersByOrgUnitId(int taskId, int? toOrgunitId)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
                if (!toOrgunitId.HasValue || toOrgunitId == 0)
                {
                    return JsonConvert.SerializeObject(dataSource);
                }
                GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/Transaction/GetTaskSequenceUsers?cultureName={0}&toOrgUnitId={1}&taskId={2}&fromOrgUnitId={3}", SessionInfo.CultureShortName, toOrgunitId, taskId, SessionInfo.OrgUnitId)).Result;


                List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);

                if (userProfileVMs != null)
                {
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

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult TaskWorkflow(int taskIndex, string OrgSettings, List<OrgStructureInfoVM> OrgStructure, TaskAddVM taskAddVM)
        {
            try
            {
                GetResult<OrgUnitStructureDesignDTO> orgUnitStructureDesignDTO =
                       HttpClientWrapper<GetResult<OrgUnitStructureDesignDTO>>.GetItemRequest(string.Format("api/Admin/GetOrgUnitStructure?cultureName=" + SessionInfo.CultureShortName)).Result;

                orgUnitStructureDesignDTO.Result.OrgUnits.ForEach(o =>
                {

                    o.Users = new List<OrgUnitUserDTO>();
                    o.AssignmentPaper = null;
                });


                OrgUnitStructureDesignVM organizationUnitStructureDesignVM = OrgUnitStructureDesignMapper.Map(orgUnitStructureDesignDTO.Result);
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
                  HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Transaction/GetOrgUnitLinks?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

                List<OrgStructureInfoVM> OrgStructureInfoVMsss = new List<OrgStructureInfoVM>();

                foreach (OrgStructureInfoVM orgStructureInfoVM in organizationUnitStructureDesignVM.OrgUnits)
                {
                    orgStructureInfoVM.LinkUnitsKeys = new List<int>();

                    List<TaskWorkflowVM> taskWorkflowVMs = taskAddVM.TaskWorkflows.Where(w => w.FromOrgUnitId == orgStructureInfoVM.Key).ToList();

                    foreach (TaskWorkflowVM taskWorkflowVM in taskWorkflowVMs)
                    {
                        orgStructureInfoVM.LinkUnitsKeys.Add(taskWorkflowVM.ToOrgUnitId);
                    }

                    if (orgUnitDTOs.Result.Find(jj => jj.Id == orgStructureInfoVM.Key) != null)
                    {
                        OrgStructureInfoVMsss.Add(orgStructureInfoVM);
                    }
                }

                ViewData["DepartmentsStructure"] = OrgStructureInfoVMsss;

                if (OrgStructure != null)
                {
                    foreach (OrgStructureInfoVM orgStructureInfoVM in OrgStructure)
                    {
                        if (orgStructureInfoVM.LinkUnitsKeys == null)
                        {
                            orgStructureInfoVM.LinkUnitsKeys = new List<int>();
                        }
                    }

                    ViewData["DepartmentsStructure"] = OrgStructure;
                }

                if (orgUnitStructureDesignDTO.Result.Settings != string.Empty)
                {
                    ViewData["SettingsStructure"] = organizationUnitStructureDesignVM.Settings;
                }
                else
                {
                    ViewData["SettingsStructure"] = JsonConvert.SerializeObject(new List<object>());
                }

                if (taskAddVM.OrgSettings != null)
                {
                    ViewData["SettingsStructure"] = OrgSettings;
                }

                Dictionary<string, string> listOfActions = new Dictionary<string, string>()
                {
                    {DbRes.TResource("User.Task.SelectTaskUser.SelectUnitUser"), "SelectUser"}
                };

                ViewData["ListOfActions"] = listOfActions;
                ViewData["TaskIndex"] = taskIndex;

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TaskWorkflowGialogPartial.cshtml", null) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Add)]
        public ActionResult PostTasks(int taskId, string hdnTaskArray)
        {
            try
            {
                string message = string.Empty;

                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();

                List<TaskAddVM> taskAddVMs = new List<TaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddVMs.AddRange(javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<TaskAddVM>)) as List<TaskAddVM>);
                }

                TransactionSubTaskVM transactionSubTaskVM = new TransactionSubTaskVM();

                transactionSubTaskVM.ParentId = taskId;

                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Transaction/PostSubTransactionTask?cultureName={0}", SessionInfo.CultureShortName), null).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Task.TaskAdd.TaskAddedSuccess");

                return Json(new { MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [CustomAuthorizationAttribute(UserClaims.Tasks.Delete)]
        public ActionResult DeleteTasks(string ids, string hdnTaskArray)
        {
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                List<TaskAddVM> taskAddVM = new List<TaskAddVM>();

                if (!string.IsNullOrEmpty(hdnTaskArray))
                {
                    taskAddVM = javaScriptSerializer.Deserialize(hdnTaskArray, typeof(List<TaskAddVM>)) as List<TaskAddVM>;
                }

                int index = Convert.ToInt32(ids);

                taskAddVM.RemoveAt(index);

                string data = JsonConvert.SerializeObject(taskAddVM);

                IAjaxGrid grid = (AjaxGrid<TaskAddVM>)new AjaxGridFactory().CreateAjaxGrid(taskAddVM, 1, taskAddVM.Count, true);

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Editor/TaskManagement/_TasksGridPartial.cshtml", grid), hdnValue = data }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult Index()
        {
            return RedirectToAction("MyTransactions", "File", new { area = "User" });
            //try
            //{
            //    HomeViewModel homeViewModel = new HomeViewModel();

            //    GetResult<List<TrayDetailsDTO>> trayDetailsDTOs =
            //         HttpClientWrapper<GetResult<List<TrayDetailsDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTrays?orgUnitId={0}&cultureName={1}", SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            //    GetResult<UserPreferenceDTO> userPreferenceResult =
            //               HttpClientWrapper<GetResult<UserPreferenceDTO>>.GetItemRequest(string.Format("api/UserProfile/GetUserPreference?userId={0}&cultureName={1}", SessionInfo.CurrentUser.Id, SessionInfo.CultureShortName)).Result;

            //    List<TrayDetailsVM> trayDetailsVMs = TrayDetailsMapper.Map(trayDetailsDTOs.Result);
            //    if (userPreferenceResult != null
            //        && userPreferenceResult.Result != null
            //        && userPreferenceResult.StatusCode == StatusCode.Ok)
            //    {
            //        trayDetailsVMs.ForEach(t =>
            //        {
            //            t.IsExcluded = userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault() != null ?
            //                !userPreferenceResult.Result.UserTrays.Where(u => u.Id == t.Id).FirstOrDefault().IsSelected : false;
            //        });
            //    }

            //    //Nasser
            //    trayDetailsVMs[7].IsExcluded = false;
            //    TempData["TrayDetails"] = trayDetailsVMs;

            //    homeViewModel.TrayDetails = trayDetailsVMs;

            //    List<Tray> trayConfigElements = TraysConfig.Trays;

            //    ViewData["trayStyle"] = trayConfigElements;

            //    GetResult<List<TaskStatusDTO>> taskStatusDTOs =
            //      HttpClientWrapper<GetResult<List<TaskStatusDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTasksStatus?userId={0}&orgUnitId={1}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId)).Result;

            //    List<TaskStatusVM> taskStatusVMs = TaskStatusMapper.Map(taskStatusDTOs.Result);
            //    if (taskStatusDTOs.StatusCode == StatusCode.Ok)
            //    {
            //        homeViewModel.TasksStatus = taskStatusVMs;
            //    }

            //    GetResult<TrayDetailsDTO> trayDetailsDTO =
            //       HttpClientWrapper<GetResult<TrayDetailsDTO>>.GetItemRequest(string.Format("api/Transaction/GetPopulariazations?orgUnitId={0}&PageIndex={1}&PageSize={2}&CultureName={3}", SessionInfo.OrgUnitId, 1, 6, SessionInfo.CultureShortName)).Result;

            //    TrayDetailsVM trayDetailsVM = TrayDetailsMapper.Map(trayDetailsDTO.Result);
            //    if (trayDetailsDTO.StatusCode == StatusCode.Ok)
            //    {
            //        homeViewModel.TransactionTrayInfos = trayDetailsVM.TransactionTrayInfoVMs;
            //    }

            //    BulidDashboards();

            //    return View(homeViewModel);
            //}
            //catch (Exception)
            //{

            //    throw;
            //}
        }

        [HttpPost]
        public void SetOrgUnit(int orgUnitId)
        {
            try
            {
                UserVM userVM = SessionInfo.GetObjectFromSession(Constants.LoggedInUserKey) as UserVM;

                userVM.UserOrgUnits.ForEach(o => o.IsSelected = false);
                userVM.UserOrgUnits.Where(o => o.Id == orgUnitId).FirstOrDefault().IsSelected = true;

                SessionInfo.SetObjectInSession(userVM, Constants.LoggedInUserKey);

                // Flush HttpClient to add selected orgunit to headers
                HttpClientWrapper<object>.FlushClient();

                 //PostResult postResult = HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Common/UpdateUserOnline?userid={0}&OrgUnitId={1}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId, SessionInfo.CultureShortName), null).Result;
                 


            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult SetCultureName(string cultureName)
        {
            try
            {
                if (Constants.Languages.English == cultureName)
                {
                    SessionInfo.SetObjectInSession(Constants.Languages.English, Constants.CultureNameKey);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("en-US");
                }
                else
                {
                    SessionInfo.SetObjectInSession(Constants.Languages.Arabic, Constants.CultureNameKey);
                    System.Threading.Thread.CurrentThread.CurrentCulture = new CultureInfo("ar-SA");
                    System.Threading.Thread.CurrentThread.CurrentUICulture = new CultureInfo("ar-SA");
                    CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("ar-SA");
                }

                return Json(new { ReturnUrl = Request.UrlReferrer.AbsolutePath }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult RenderUserTrayTransactions(int trayId)
        {
            try
            {
                string message = string.Empty;

                GetResult<List<UserTransactionsTrayDTO>> userTransactionsTrayDTOs =
                    HttpClientWrapper<GetResult<List<UserTransactionsTrayDTO>>>.GetItemRequest("api/Transaction/GetUserTransactionsTray?orgUnitId=" + SessionInfo.OrgUnitId + "&transactionDate=2&trayType=" + trayId + "&PageIndex=1&PageSize=" + GridHelper.PageSize + "&CultureName=" + SessionInfo.CultureShortName).Result;

                if (userTransactionsTrayDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userTransactionsTrayDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["TrayType"] = trayId;
                ViewData["DataCount"] = (userTransactionsTrayDTOs.RowsCount.HasValue) ? userTransactionsTrayDTOs.RowsCount.Value : 0;
                ViewData["PageNumber"] = 1;
                ViewData["ConfidentialityData"] = GetConfidentialityLevel();
                ViewData["PrioritiesData"] = GetPriorities();

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/File/_TrayTransactionsPartial.cshtml", UserTransactionsTrayMapper.Map(userTransactionsTrayDTOs.Result)) }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateTrayTransactions(int trayId, int page, string columnName, bool dir, Dictionary<string, string> dictionary)
        {
            try
            {
                string message = string.Empty;
                string parameters = GridHelper.GetGridParameters();
                StringBuilder filterData = new StringBuilder();

                for (int i = 0; i < dictionary.Count; i++)
                {
                    filterData.Append("&Filters[").Append(i).Append("].ColumnName=")
                        .Append(dictionary.ToList()[i].Key).Append("&Filters[").Append(i)
                        .Append("].FilterType=").Append(1).Append("&Filters[")
                        .Append(i).Append("].FilterValue=").Append(dictionary.ToList()[i].Value);
                }

                GetResult<List<UserTransactionsTrayDTO>> userTransactionsTrayDTOs =
                 HttpClientWrapper<GetResult<List<UserTransactionsTrayDTO>>>.GetItemRequest(string.Format("api/Transaction/GetUserTransactionsTray?{0}&orgUnitId={0}&transactionDate={1}&trayType={2}&PageIndex={3}&PageSize={4}&CultureName={5}&OrderBy={6}&Ascending={7}{8}", SessionInfo.OrgUnitId, 2, trayId, page, GridHelper.PageSize, SessionInfo.CultureShortName, columnName, dir, filterData.ToString())).Result;

                if (userTransactionsTrayDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, userTransactionsTrayDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                ViewData["TrayType"] = trayId;
                ViewData["DataCount"] = (userTransactionsTrayDTOs.RowsCount.HasValue) ? userTransactionsTrayDTOs.RowsCount.Value : 0;

                return Json(new
                {
                    Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/File/_TrayTransactionsBodyPartial.cshtml",
                      UserTransactionsTrayMapper.Map(userTransactionsTrayDTOs.Result)),
                    Count = userTransactionsTrayDTOs.RowsCount,
                    FooterHtml = UIHelper.RenderRazorViewToHtml(ControllerContext, "~/Areas/User/Views/Shared/File/_TrayTransactionsFooterPartialcshtml", null)
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult GetTransactionCertificateById(int transctionId)
        {
            try
            {
                GetResultExtraData<Object> trayDetailsDTO =
                          HttpClientWrapper<GetResultExtraData<Object>>.GetItemRequest(string.Format("api/Transaction/GetTransactionCertificateById?transactionId={0}&cultureName={1}", transctionId, SessionInfo.CultureShortName)).Result;

                string message = string.Empty;

                if (trayDetailsDTO.StatusCode == StatusCode.TransactionNotFound)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.Message, "User.Transaction.Open.TransactionNotFound");
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                int categoryId = Convert.ToInt32(trayDetailsDTO.ExtraData.ToString());

                if (categoryId == TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                {
                    InboundCertificateVM inboundCertificateVM = JsonConvert.DeserializeObject<InboundCertificateVM>(trayDetailsDTO.Result.ToString());

                    inboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                    IAjaxGrid inboundNames = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Names, 1, inboundCertificateVM.Names.Count(), true);
                    ViewData["NamesData"] = inboundNames;

                    IAjaxGrid assignments = (AjaxGrid<TransactionAssignmentVM>)new AjaxGridFactory().CreateAjaxGrid(inboundCertificateVM.Assignments, 1, inboundCertificateVM.Assignments.Count(), true);
                    ViewData["AssignmentsData"] = assignments;

                    ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                    return View("~/Areas/User/Views/Shared/TransactionCertificate/_InboundCertificatePartial.cshtml", inboundCertificateVM);
                }

                OutboundCertificateVM outboundCertificateVM = Newtonsoft.Json.JsonConvert.DeserializeObject<OutboundCertificateVM>(OutboundCertificateMapper.Map((OutboundCertificateDTO)trayDetailsDTO.Result).ToString());

                outboundCertificateVM.OrgUnit = SessionInfo.CurrentUser.UserOrgUnits.Where(o => o.Id == SessionInfo.OrgUnitId).FirstOrDefault().Name;

                IAjaxGrid outboundNames = (AjaxGrid<TransactionNameVM>)new AjaxGridFactory().CreateAjaxGrid(outboundCertificateVM.Names, 1, outboundCertificateVM.Names.Count(), true);

                ViewData["NamesData"] = outboundNames;
                ViewData["DocumentSessionKey"] = Guid.NewGuid().ToString();

                return View("~/Areas/User/Views/Shared/TransactionCertificate/_OutboundCertificatePartial.cshtml", outboundCertificateVM);
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected string GetConfidentialityLevel()
        {
            try
            {
                var urlPermission = string.Format("api/Common/GetPermissionsByGroupId?permissionGroupName={0}&cultureName={1}", PermissionGroupName.TransactiosConfidentiality, SessionInfo.CultureShortName);

                GetResult<List<PermissionVM>> permissionVMs = HttpClientWrapper<GetResult<List<PermissionVM>>>.GetItemRequest(urlPermission).Result;

                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (permissionVMs.Result != null)
                {
                    foreach (PermissionVM permissionVM in permissionVMs.Result)
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

                GetResult<List<PriorityVM>> priorityVMs = LookupsHelper.GetPriorities(TransactionCategory.Inbound);
                if (priorityVMs != null)
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
            catch (Exception)
            {
                throw;
            }
        }

        private JsonResult GetReceivedTaskBypageIndex(int pageIndex, string message = "", MessageType messageType = MessageType.Information)
        {

            GetResult<SettingDTO> SettingValue = HttpClientWrapper<GetResult<SettingDTO>>.GetItemRequest(string.Format("api/Transaction/GetSettingValue?Key={0}", Constants.TraysSettings.InTasksTray)).Result;
            var settingVM = SettingMapper.Map(SettingValue.Result);
            GetResult<List<ReceivedTaskDTO>> ReceivedTaskDTOs =
               HttpClientWrapper<GetResult<List<ReceivedTaskDTO>>>.GetItemRequest(string.Format("api/Transaction/GetReceivedTasks?pageIndex={0}&pageSize={1}&orgUnitId={2}&cultureName={3}", pageIndex, settingVM.Value, SessionInfo.OrgUnitId, SessionInfo.CultureShortName)).Result;

            if (ReceivedTaskDTOs.StatusCode != StatusCode.Ok)
            {
                throw new Exception(ReceivedTaskDTOs.StatusCode.ToString());
            }

            ViewData["SelectedPageIndex"] = pageIndex;
            ViewData["ReceivedTasksCount"] = ReceivedTaskDTOs.RowsCount.Value;
            ViewData["PageSize"] = settingVM.Value;

            return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "ReceivedTasksPartial", ReceivedTaskMapper.Map(ReceivedTaskDTOs.Result)), MessageText = message, MessageType = messageType }, JsonRequestBehavior.AllowGet);
        }

        private static TreeViewModel BulidTree(List<OrgUnitVM> orgUnitVMs, int selectedOrgUnitId = -1)
        {
            TreeViewModel tree = new TreeViewModel();
            List<TreeNode> nodes = new List<TreeNode>();

            if (orgUnitVMs == null)
            {
                return tree;
            }

            tree.RootNode = new TreeNode { Id = 0, Mode = tree.Mode };

            foreach (OrgUnitVM orgUnitVM in orgUnitVMs)
            {
                TreeNode treeNode = new TreeNode()
                {
                    DepartmentNumber = orgUnitVM.Number.ToString(),
                    IsSelected = false,
                    Selectable = true,
                    Name = orgUnitVM.Name,
                    Id = orgUnitVM.Id
                };

                if (orgUnitVM.Id == selectedOrgUnitId)
                {
                    treeNode.IsSelected = true;
                }

                tree.RootNode.Childs.Add(treeNode);
            }

            return tree;
        }

        private void BulidDashboards()
        {
            GetResult<List<DashboardDTO>> dashboardDTOs =
                         HttpClientWrapper<GetResult<List<DashboardDTO>>>.GetItemRequest(string.Format("api/Dashboard/GetDashboardData?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<DashboardVM> dashboardVMs = DashboardMapper.Map(dashboardDTOs.Result);

            IEnumerable<IGrouping<int, DashboardVM>> Queue = dashboardVMs.GroupBy(d => d.Date.Year);

            GetResult<IList<LookupVM>> transactionCateogryVMs =
                LookupsHelper.GetLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);

            List<ChartDataSourceForBarLineRadar> allTransactionsBarChart = new List<ChartDataSourceForBarLineRadar>();

            foreach (IGrouping<int, DashboardVM> dashboardGroup in Queue)
            {
                ChartDataSourceForBarLineRadar chartDataSource = new ChartDataSourceForBarLineRadar()
                {
                    ID = dashboardGroup.Key,
                    Value = new List<ChartValue>(),
                    Label = dashboardGroup.Key.ToString()
                };

                transactionCateogryVMs.Result.ToList().ForEach(t =>
                {
                    if (t.Text != null)
                    {
                        int count = 0;

                        if (dashboardVMs.Where(d => d.TypeId == t.Id && d.Date.Year == dashboardGroup.Key).FirstOrDefault() != null)
                        {
                            count = dashboardVMs.Where(d => d.TypeId == t.Id && d.Date.Year == dashboardGroup.Key).ToList().Count;
                        }

                        chartDataSource.Value.Add(new ChartValue() { Label = t.Text, Value = count });
                    }
                });

                allTransactionsBarChart.Add(chartDataSource);
            }

            if (allTransactionsBarChart.Count == 1)
            {
                int nextYear = DateTime.Now.Year - 1;
                ChartDataSourceForBarLineRadar chartDataSource = new ChartDataSourceForBarLineRadar()
                {
                    ID = nextYear,
                    Value = new List<ChartValue>(),
                    Label = nextYear.ToString()
                };

                transactionCateogryVMs.Result.ToList().ForEach(t =>
                {
                    if (t.Text != null)
                    {
                        chartDataSource.Value.Add(new ChartValue() { Label = t.Text, Value = 0 });
                    }
                });

                allTransactionsBarChart.Insert(0, chartDataSource);
            }

            ViewData["AllTransactionsBarChart"] = allTransactionsBarChart;

            GetResult<List<UserCategoryDTO>> userCategoryDTOs =
              HttpClientWrapper<GetResult<List<UserCategoryDTO>>>.GetItemRequest(String.Format("api/Admin/GetAllUsersCategories?cultureName={0}", SessionInfo.CultureShortName)).Result;

            List<ChartDataSourceForBarLineRadar> transactionsByEmployeeTypeBarChart = new List<ChartDataSourceForBarLineRadar>();


            List<UserCategoryVM> userCategoryVMs = UserCategoryMapper.Map(userCategoryDTOs.Result);
            foreach (IGrouping<int, DashboardVM> dashboardGroup in Queue)
            {
                ChartDataSourceForBarLineRadar chartDataSource = new ChartDataSourceForBarLineRadar()
                {
                    ID = dashboardGroup.Key,
                    Value = new List<ChartValue>(),
                    Label = dashboardGroup.Key.ToString()
                };

                userCategoryVMs.ToList().ForEach(c =>
                {
                    if (c.CategoryText != null)
                    {
                        int count = 0;

                        if (dashboardVMs.Where(d => d.UserCategoryId == c.Id && d.Date.Year == dashboardGroup.Key).FirstOrDefault() != null)
                        {
                            count = dashboardVMs.Where(d => d.UserCategoryId == c.Id && d.Date.Year == dashboardGroup.Key).ToList().Count;
                        }

                        chartDataSource.Value.Add(new ChartValue() { Label = c.CategoryText, Value = count });
                    }
                });

                transactionsByEmployeeTypeBarChart.Add(chartDataSource);
            }

            ViewData["TransactionsByEmployeeTypeBarChart"] = JsonConvert.SerializeObject(transactionsByEmployeeTypeBarChart);

            List<ChartDataSourceForDoughnutPiePolar> thisDayTransactionsPieChart = new List<ChartDataSourceForDoughnutPiePolar>();

            transactionCateogryVMs.Result.ToList().ForEach(l =>
            {
                if (l.Text != null)
                {
                    int count = 0;

                    if (dashboardVMs.Where(t => t.Date.ToString(UIHelper.SystemDateFormat) == DateTime.Now.ToString(UIHelper.SystemDateFormat)).FirstOrDefault() != null)
                    {
                        if (dashboardVMs.Where(t => t.Date.ToString(UIHelper.SystemDateFormat) == DateTime.Now.ToString(UIHelper.SystemDateFormat)).Where(t => t.TypeId == l.Id) != null)
                        {
                            count = dashboardVMs.Where(t => t.Date.ToString(UIHelper.SystemDateFormat) == DateTime.Now.ToString(UIHelper.SystemDateFormat)).Where(t => t.TypeId == l.Id).ToList().Count;
                        }
                    }

                    thisDayTransactionsPieChart.Add(new ChartDataSourceForDoughnutPiePolar
                    {
                        ID = l.Id,
                        Value = count,
                        Label = l.Text
                    });
                }
            });

            ViewData["ThisDayTransactionsPieChart"] = JsonConvert.SerializeObject(thisDayTransactionsPieChart);
        }
    }
}