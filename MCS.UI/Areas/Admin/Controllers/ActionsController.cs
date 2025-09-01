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
using MCS.UI.Areas.Admin.Models.Actions;
using MCS.UI.Areas.Admin.Models.Lookups;
using CustomAjaxGrid = MCS.GridMvc.Ajax.GridExtensions;
using UserLookups = MCS.UI.Areas.User.Mappers.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class ActionsController : AdminControllerBase
    {
        public ActionResult Actions()
        {
            try
            {
                string message = string.Empty;
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;
                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.ActionType, SessionInfo.CultureShortName);
                ViewData["ActionTypes"] = lookups.Result;

                GetResult<List<ActionDTO>> processDTOs = HttpClientWrapper<GetResult<List<ActionDTO>>>
                    .GetItemRequest(string.Format("api/Admin/GetActions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (processDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, processDTOs.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<ActionVM>)new AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result).AsQueryable(), 1, false, processDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result), 1, processDTOs.RowsCount.Value, false, UIHelper.PageSize);

                ViewData["GridData"] = grid;

                return View("Index", new ActionViewModel());
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddAction(AddActionVM addActionVM)
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

                //get Types
                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.ActionType, SessionInfo.CultureShortName);

                ViewData["ActionTypes"] = lookups.Result;

                //post process
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostAction", ActionMapper.Map(addActionVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ActionDTO>> processDTOs =
                       HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Admin/GetActions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (processDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, processDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<ActionVM>)new AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result).AsQueryable(), 1, false, processDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result), 1, processDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridActionPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditAction(EditActionVM editActionVM)
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


                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.ActionType, SessionInfo.CultureShortName);

                ViewData["ActionTypes"] = lookups.Result;

                //if (editActionVM.IsAsCopy)
                //{
                    PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/PutAction", ActionMapper.Map(editActionVM)).Result;

                    if (putResult.StatusCode != StatusCode.Ok)
                    {
                        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                    }
                //}
                //else
                //{
                //    AddActionVM actionAddVM = new AddActionVM()
                //    {
                //        Description = editActionVM.Description,
                //        TypeId = editActionVM.TypeId,
                //        IsAsCopy = false
                //    };

                //    PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostAction", ActionMapper.Map(actionAddVM)).Result;

                //    if (postResult.StatusCode != StatusCode.Ok)
                //    {
                //        message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                //        return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //    }
                //}

                GetResult<List<ActionDTO>> actionDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Admin/GetActions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (actionDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, actionDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<ActionVM>)new AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(actionDTOs.Result).AsQueryable(), 1, false, actionDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(actionDTOs.Result), 1, actionDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridActionPartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult DeleteAction(string ids)
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


                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.ActionType, SessionInfo.CultureShortName);

                ViewData["ActionTypes"] = lookups.Result;

                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/DeleteActions?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ActionDTO>> processDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Admin/GetActions?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (processDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, processDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                //IAjaxGrid grid = (AjaxGrid<ActionVM>)new AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result).AsQueryable(), 1, false, processDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result), 1, processDTOs.RowsCount.Value, false);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.DeleteSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridActionPartial", grid), ActionsUsedList = deleteResult.Result, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult GetAction(string id)
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

                GetResult<IList<LookupVM>> lookups = LookupsHelper.GetAdminLookupItems(LookupCategory.ActionType, SessionInfo.CultureShortName);

                ViewData["ActionTypes"] = lookups.Result;

                GetResult<EditActionDTO> ActionEditDTO =
                   HttpClientWrapper<GetResult<EditActionDTO>>.GetItemRequest(String.Format("api/Admin/GetActionById?id={0}", id)).Result;

                if (ActionEditDTO.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, ActionEditDTO.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EditActionPartial", ActionMapper.Map(ActionEditDTO.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult UpdateActionGrid(int? page)
        {
            try
            {
                GetResult<List<CultureDTO>> cultureDTOs = HttpClientWrapper<GetResult<List<CultureDTO>>>.GetItemRequest("api/Common/GetCultures").Result;

                ViewData["Culture"] = UserLookups.CultureMapper.Map(cultureDTOs.Result);

                string parameters = GridHelper.GetGridParameters(); GetResult<List<ActionDTO>> processDTOs = HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(String.Format("api/Admin/GetActions?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                //IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, processDTOs.RowsCount.Value);
                CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<ActionVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(ActionMapper.Map(processDTOs.Result), page ?? 1, processDTOs.RowsCount.Value, page.HasValue, UIHelper.PageSize);

                return Json(new { Html = grid.ToJson("_GridActionPartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult MoveUser()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>
                //    .GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MoveUserPost(MoveTransactionVM moveTransactionVM)
        {
            if (ModelState.IsValid)
            {
                //GetResult<List<UserProfileDTO>> userProfiles = HttpClientWrapper<GetResult<List<UserProfileDTO>>>
                //  .GetItemRequest(string.Format("api/Admin/GetUsersByOrgId?orgUnitId={0}&cultureName={1}", moveTransactionVM.EntityToId, SessionInfo.CultureShortName)).Result;

                //List<string> usersNames = new List<string>();
                //string[] values = moveTransactionVM.UsersFromIds.Split(',');
                //foreach (var item in userProfiles.Result)
                //{
                //    foreach (var value in values)
                //    {
                //        if (value == item.Id.ToString())
                //        {
                //            usersNames.Add(item.LocalName);
                //        }
                //    }   
                //}

                //if (usersNames.Count > 0)
                //{
                //    var error = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                //    return Json(new { errors = error, Users = string.Join(",", usersNames.ToArray()), MessageText = DbRes.TValidation("Admin.Actions.MoveUser.UsersIsInBothDepartmentsValidation"), MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                //}

                var logInUserId = SessionInfo.CurrentUser.Id;
                GetResult<int> getResult = HttpClientWrapper<GetResult<int>>.PutRequest(string.Format("api/Admin/MoveUser?usersIDs={0}&orgunitID={1}&newOrgunitID={2}&loggedinUserID={3}",
                    moveTransactionVM.UsersFromIds,
                    moveTransactionVM.EntityFromId,
                    moveTransactionVM.EntityToId,
                    logInUserId), null).Result;

                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}",
                //    null, SessionInfo.CultureShortName)).Result;

                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult MoveEntity()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MoveEntityPost(MoveTransactionVM moveTransactionVM)
        {
            if (ModelState.IsValid)
            {
                var logInUserId = SessionInfo.CurrentUser.Id;
                GetResult<int> getResult = HttpClientWrapper<GetResult<int>>
                    .GetItemRequest(string.Format("api/Admin/MoveEntity?entityFrom={0}&entityTo={1}&loginUser={2}",
                    moveTransactionVM.EntityFromId, moveTransactionVM.EntityToId, logInUserId)).Result;

                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();

                if (getResult.StatusCode != StatusCode.Ok && getResult.StatusCode != StatusCode.OrgUnitsHaveSameName)
                {
                    string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    return Json(new { errors = errors, MessageText = errorMessage, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                if (getResult.StatusCode == StatusCode.OrgUnitsHaveSameName)
                {
                    int conflictedEntityId = getResult.Result;
                    int entityToMoveId = moveTransactionVM.EntityFromId;
                    string orgUnitIds = conflictedEntityId + "," + entityToMoveId;

                    ChangeEntityNameVM changeEntityNameVM = new ChangeEntityNameVM
                    {
                        EntityFromId = entityToMoveId,
                        EntityToId = conflictedEntityId,
                    };
                    string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                    ViewData["PopUpTitle"] = DbRes.TResource("Admin.Actions.MoveEntity.HaveSameName");
                    ViewData["EntityFromTitle"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityToMove");
                    ViewData["EntityToTitle"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityMoveTo");
                    ViewData["EntityFromAr"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityToMove.ar");
                    ViewData["EntityFromEn"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityToMove.en");
                    ViewData["EntityToAr"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityMoveTo.ar");
                    ViewData["EntityToEn"] = DbRes.TResource("Admin.Actions.MoveEntity.EntityMoveTo.en");

                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ChangeEntitiesNamesPartial", changeEntityNameVM), MessageText = errorMessage, MessageType = MessageType.Warning }, JsonRequestBehavior.AllowGet);
                }

                return Json(new { MessageText = DbRes.TResource("Admin.MoveEntity.Success"), MessageType = getResult.StatusCode }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        //[HttpPost]
        //public ActionResult MoveEntityPost(MoveTransactionVM moveTransactionVM)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        var logInUserId = SessionInfo.CurrentUser.Id;
        //        GetResult<int> getResult = HttpClientWrapper<GetResult<int>>
        //            .GetItemRequest(string.Format("api/Admin/MoveEntity?entityFrom={0}&entityTo={1}&loginUser={2}",
        //            moveTransactionVM.EntityFromId, moveTransactionVM.EntityToId, logInUserId)).Result;

        //        var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();

        //        if (getResult.StatusCode != StatusCode.Ok && getResult.StatusCode != StatusCode.OrgUnitsHaveSameName)
        //        {
        //            string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

        //            return Json(new { errors = errors, MessageText = errorMessage, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
        //        }

        //}

        [HttpPost]
        public ActionResult ChangeEntitiesNamesBeforeMove(ChangeEntityNameVM changeEntityNameVM)
        {
            ChangeEntityNameDTO changeEntityNameDTO = new ChangeEntityNameDTO
            {
                EntityFromId = changeEntityNameVM.EntityFromId,
                EntityToId = changeEntityNameVM.EntityToId,
                EntityFromLocalizations = LocalizationMapper.Map(changeEntityNameVM.EntityFromLocalizations),
                EntityToLocalizations = LocalizationMapper.Map(changeEntityNameVM.EntityToLocalizations)
            };
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/ChangeEntitiesNameBeforeMove", changeEntityNameDTO).Result;

            if (postResult.StatusCode != StatusCode.Ok)
            {
                string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = errorMessage, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

            }
            return Json(new { MessageText = DbRes.TResource("Admin.Actions.MoveEntities.ChangeNamesSuccess"), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult CheckUserClearance(string Ids)
        {
            GetResult<List<UsersClearanceDTO>> getResult = HttpClientWrapper<GetResult<List<UsersClearanceDTO>>>.GetItemRequest(string.Format("api/Admin/CheckUserClearance?usersIds={0}&cultureName={1}", Ids, SessionInfo.CultureShortName)).Result;

            if (getResult.StatusCode != StatusCode.Ok)
            {
                string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, getResult.StatusCode.ToString());

                return Json(new { MessageText = errorMessage, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            List<UsersClearanceVM> usersClearanceVMs = UserClearanceMapper.Map(getResult.Result);
            foreach (var item in usersClearanceVMs)
            {
                if (item.OutboundTransactionsCount > 0 || item.SavedTransactionsCount > 0)
                {
                    return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_UserClearanceResultPartial", usersClearanceVMs), MessageType = MessageType.Warning }, JsonRequestBehavior.AllowGet);
                }
            }

            List<string> usersNames = usersClearanceVMs.Where(uc => uc.InboundTransactionsCount > 0).Select(uc => uc.UserName).ToList();
            string names = string.Join(",", usersNames.Select(x => x).ToArray());

            return Json(new { UsersHasTransactions = names, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult MoveTransactions()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MoveTransactionsPost(MoveTransactionVM moveTransactionVM)
        {
            if (ModelState.IsValid)
            {
                var logInUserId = SessionInfo.CurrentUser.Id;
                GetResult<int> getResult = HttpClientWrapper<GetResult<int>>
                        .PutRequest(string.Format("api/Admin/AdminMoveTransactions?entityFromId={0}&entityToId={1}&userFromId={2}&userToId={3}&logInUser={4}",
                        moveTransactionVM.EntityFromId,
                        moveTransactionVM.EntityToId,
                        moveTransactionVM.DirectedFromId,
                        moveTransactionVM.DirectedToId,
                        logInUserId), null).Result;
                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors = errors, MessageType = getResult.StatusCode }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                //return View("MoveTransactions", moveTransactionVM);
                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult MoveSingleTransaction()
        {
            try
            {
                GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);
                ViewData["TransactionCategory"] = GetTransactionCategoryLookups();
                return View();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MoveSingleTransactionPost(MoveTransactionVM moveTransactionVM)
        {
            if (ModelState.IsValid)
            {
                var logInUserId = SessionInfo.CurrentUser.Id;
                GetResult<int> getResult = HttpClientWrapper<GetResult<int>>
                    .PutRequest(string.Format("api/Admin/MoveTransactionById?transId={0}&toUserId={1}&toEntityId={2}&loggedInUser={3}",
                    moveTransactionVM.TransId, moveTransactionVM.DirectedToId, moveTransactionVM.EntityToId, logInUserId), null).Result;
                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors = errors, MessageType = getResult.StatusCode }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
                //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
                //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

                var errors = ModelState.Where(ms => ms.Value.Errors.Any()).Select(x => new { x.Key, x.Value.Errors }).ToArray();
                return Json(new { errors = errors }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public ActionResult CheckIfTransActionExist(int year, int transNum, int transactionType)
        {
            GetResult<TransactionBasicInfoDTO> transactionBasicInfo = HttpClientWrapper<GetResult<TransactionBasicInfoDTO>>.GetItemRequest(string.Format("api/Transaction/GetTransactionBasicInfoByNumber?transactionNumber={0}&year={1}&transactionType={2}&cultureName={3}", transNum, year, transactionType, SessionInfo.CultureShortName)).Result;
            if (transactionBasicInfo.Result != null)
            {
                return Json(new { result = true, transactionBasicInfo = transactionBasicInfo.Result }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { result = false }, JsonRequestBehavior.AllowGet);
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

                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Assignments.AssignToEmployeeInOtherDepartment) || SessionInfo.CurrentUser.UserOrgUnits.Where(i => i.Id == id).Count() > 0)
                {
                    GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetUsersByOrgUnitId?cultureName={0}&orgUnitId={1}", SessionInfo.CultureShortName, id)).Result;

                    if (userProfileDTOs.Result != null)
                    {
                        foreach (UserProfileVM userProfileVM in UserProfileMapper.Map(userProfileDTOs.Result))
                        {
                            dataSource.Add(new AutoCompleteDataSource()
                            {
                                Value = userProfileVM.Id.ToString(),
                                Label = userProfileVM.LocalName
                            });
                        }
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
        public ActionResult GetUsersGridByOrgUnitId(int id, int? page)
        {
            try
            {
                IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();

                if (SessionInfo.CurrentUser.Claims.Contains(UserClaims.Assignments.AssignToEmployeeInOtherDepartment) || SessionInfo.CurrentUser.UserOrgUnits.Where(i => i.Id == id).Count() > 0)
                {
                    GetResult<List<UserProfileDTO>> userProfileDTOs =
                    HttpClientWrapper<GetResult<List<UserProfileDTO>>>.GetItemRequest(string.Format("api/UserProfile/GetOrgUnitUsers?PageIndex={0}&PageSize={1}&cultureName={2}&orgUnitId={3}&noExternal={4}", page ?? 1, UIHelper.PageSize, SessionInfo.CultureShortName, id, true)).Result;

                    List<UserProfileVM> userProfileVMs = UserProfileMapper.Map(userProfileDTOs.Result);

                    CustomAjaxGrid.IAjaxGrid grid = (CustomAjaxGrid.AjaxGrid<UserProfileVM>)new CustomAjaxGrid.AjaxGridFactory().CreateAjaxGrid(userProfileVMs, page ?? 1, userProfileDTOs.RowsCount.Value, page.HasValue, UIHelper.PageSize);

                    return Json(new { Html = grid.ToJson("UsersGridPartial", this), grid.HasItems, ItemsCount = userProfileDTOs.RowsCount ?? 0, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);

                }
                return Json(new { MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult MergeDepartments()
        {
            GetResult<List<OrgUnitDTO>> orgUnitDTOs =
      HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
            ViewData["orgUnitDataInfo"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
            //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Common/GetOrgUnits?parentId={0}&cultureName={1}", null, SessionInfo.CultureShortName)).Result;
            //ViewData["DepartmentsData"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result));
            //ViewData["OrgUnitsUsersData"] = GetUsersByOrgUnitId(SessionInfo.OrgUnitId);

            return View("MergeDepartmentsPartial", new MergeDepartmentsVM() { BaseEntityId = -1, MergedEntityId = -1 });
        }

        [HttpPost]
        public ActionResult MergeDepartments(MergeDepartmentsVM mergeDepartmentsVM)
        {
            string message = string.Empty;
            MergeDepartmentsDTO mergeDepartmentsDTO = new MergeDepartmentsDTO()
            {
                Id = mergeDepartmentsVM.Id,
                BaseEntityId = mergeDepartmentsVM.BaseEntityId,
                MergedEntityId = mergeDepartmentsVM.MergedEntityId,
                ManagerId = mergeDepartmentsVM.ManagerId,
                NewEntityNames = LocalizationMapper.Map(mergeDepartmentsVM.NewEntityNames)
            };
            PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostMergeDepartments", mergeDepartmentsDTO).Result;

            if (postResult.StatusCode != StatusCode.Ok && postResult.StatusCode != StatusCode.OrgUnitsToBeMergedHaveSameName)
            {
                string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = errorMessage, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            if (postResult.StatusCode == StatusCode.OrgUnitsToBeMergedHaveSameName)
            {
                int conflictedEntityId = (int)postResult.Id;
                int entityToMergeId = mergeDepartmentsVM.MergedEntityId;
                string orgUnitIds = conflictedEntityId + "," + entityToMergeId;

                ChangeEntityNameVM changeEntityNameVM = new ChangeEntityNameVM
                {
                    EntityFromId = entityToMergeId,
                    EntityToId = conflictedEntityId,
                };
                string errorMessage = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                ViewData["PopUpTitle"] = DbRes.TResource("Admin.Actions.MergeEntities.ChangeNamesTitle");
                ViewData["EntityFromTitle"] = DbRes.TResource("Admin.Actions.MergeEntities.EntityToBeMergedTitle");
                ViewData["EntityToTitle"] = DbRes.TResource("Admin.Actions.MergeEntities.BaseEntityTitle");
                ViewData["EntityFromAr"] = DbRes.TResource("Admin.Actions.MergeEntities.EntityToBeMerged.ar");
                ViewData["EntityFromEn"] = DbRes.TResource("Admin.Actions.MergeEntities.EntityToBeMerged.en");
                ViewData["EntityToAr"] = DbRes.TResource("Admin.Actions.MergeEntities.BaseEntity.ar");
                ViewData["EntityToEn"] = DbRes.TResource("Admin.Actions.MergeEntities.BaseEntity.en");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_ChangeEntitiesNamesPartial", changeEntityNameVM), MessageText = errorMessage, MessageType = MessageType.Warning }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { MessageText = DbRes.TResource("Admin.MergeEntities.Success"), MessageType = postResult.StatusCode }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetUsersForBothOrgUnits(int MergedEntityId, int BaseEntityId)
        {
            try
            {
                if (MergedEntityId != 0 && BaseEntityId != 0)
                {
                    var array1 = new JavaScriptSerializer().Deserialize<List<AutoCompleteDataSource>>(GetUsersByOrgUnitId(MergedEntityId));

                    if (MergedEntityId != BaseEntityId)
                    {
                        var array2 = new JavaScriptSerializer().Deserialize<List<AutoCompleteDataSource>>(GetUsersByOrgUnitId(BaseEntityId));
                        array1.AddRange(array2);
                    }

                    var users = JsonConvert.SerializeObject(array1);

                    return Json(new { users = users.ToString(), MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
                }
                return Json(new { MessageType = MessageType.Error, MessageText = "Error" }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private string GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.TransactionCategory, SessionInfo.CultureShortName);
            IList<AutoCompleteDataSource> dataSource = new List<AutoCompleteDataSource>();
            if (lookupVMs != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    if (lookupVM.Id != TransactionCategory.ExternalOutbound.LookupIdentity(LookupCategory.TransactionCategory, SessionInfo.CultureShortName))
                    {
                        dataSource.Add(new AutoCompleteDataSource()
                        {
                            Value = lookupVM.Id.ToString(),
                            Label = lookupVM.Text,

                        });
                    }
                }
            }
            return JsonConvert.SerializeObject(dataSource);
        }
    }
}