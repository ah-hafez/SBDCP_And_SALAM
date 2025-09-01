using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Framework.Controls;
using MCS.Framework.Controls.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Transaction;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models;
using MCS.UI.Areas.Admin.Models.ReleaseNotes;
using MCS.UI.Common;

namespace MCS.UI.Areas.Admin.Controllers
{
    public class ReleaseNotesController : AdminControllerBase
    {
        public ActionResult Index()
        {
            try
            {
                GetResult<List<ReleaseNotesDTO>> notesDTOList =
                HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.GetItemRequest(string.Format("api/Admin/ReleaseNotesSelect?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;
                IAjaxGrid grid = (AjaxGrid<ReleaseNotesVM>)new AjaxGridFactory().CreateAjaxGrid(ReleaseNotesMapper.Map(notesDTOList.Result).AsQueryable(), 1, false, notesDTOList.RowsCount.Value);
                ViewData["GridData"] = grid;

                return View(new ReleaseNotesVM());
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddReleaseNotes(ReleaseNotesVM addVM)
        {
            try
            {
                string message = string.Empty;

                //post process
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/ReleaseNotesAdd", ReleaseNotesMapper.Map(addVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ReleaseNotesDTO>> notesDTOList =
                       HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.
                       GetItemRequest(string.Format("api/Admin/ReleaseNotesSelect?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (notesDTOList.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, notesDTOList.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<ReleaseNotesVM>)new AjaxGridFactory().CreateAjaxGrid(ReleaseNotesMapper.Map(notesDTOList.Result).AsQueryable(), 1, false, notesDTOList.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.AddSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridReleaseNotePartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult EditReleaseNotes(ReleaseNotesVM editVM)
        {
            try
            {
                string message = string.Empty;


                PutResult putResult = HttpClientWrapper<PutResult>.PutRequest("api/Admin/ReleaseNotesUpdate", editVM).Result;

                if (putResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ReleaseNotesDTO>> notesResult =
                    HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.GetItemRequest(string.Format("api/Admin/ReleaseNotesSelect?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (notesResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, notesResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                IAjaxGrid grid = (AjaxGrid<ReleaseNotesVM>)new AjaxGridFactory().CreateAjaxGrid(ReleaseNotesMapper.Map(notesResult.Result).AsQueryable(), 1, false, notesResult.RowsCount.Value);

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridReleaseNotePartial", grid), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult ReleaseNotesDelete(string ids)
        {
            try
            {
                string message = string.Empty;


                RemoveObjectResult<List<int>> deleteResult = HttpClientWrapper<RemoveObjectResult<List<int>>>.PostRequest(String.Format("api/Admin/ReleaseNotesDelete?ids={0}", ids), null).Result;

                if (deleteResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, deleteResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ActionDTO>> processDTOs =
                    HttpClientWrapper<GetResult<List<ActionDTO>>>.GetItemRequest(string.Format("api/Admin/ReleaseNotesSelect?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                if (processDTOs.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, processDTOs.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                GetResult<List<ReleaseNotesDTO>> notesResult =
                   HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.GetItemRequest(string.Format("api/Admin/ReleaseNotesSelect?PageIndex=1&PageSize={0}&CultureName={1}", GridHelper.PageSize, SessionInfo.CultureShortName)).Result;

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.DeleteSucceeded");
                IAjaxGrid grid = (AjaxGrid<ReleaseNotesVM>)new AjaxGridFactory().CreateAjaxGrid(ReleaseNotesMapper.Map(notesResult.Result).AsQueryable(), 1, false, notesResult.RowsCount.Value);
                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_GridReleaseNotePartial", grid), MessageText = message, MessageType = MessageType.Information, ActionsUsedList = deleteResult.Result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult ReleaseNotesSelect(string id)
        {
            try
            {
                string message = string.Empty;

                GetResult<ReleaseNotesDTO> selectRequest =
                   HttpClientWrapper<GetResult<ReleaseNotesDTO>>.GetItemRequest(String.Format("api/Admin/ReleaseNotesSelectById?id={0}", id)).Result;

                if (selectRequest.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, selectRequest.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.Actions.UpdateSucceeded");

                return Json(new { Html = UIHelper.RenderRazorViewToHtml(ControllerContext, "_EditReleaseNotesPartial", ReleaseNotesMapper.Map(selectRequest.Result)), MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public ActionResult UpdateReleaseNotesGrid(int? page)
        {
            try
            {

                string parameters = GridHelper.GetGridParameters(); GetResult<List<ReleaseNotesDTO>> selectRequest =
                    HttpClientWrapper<GetResult<List<ReleaseNotesDTO>>>.GetItemRequest(String.Format("api/Admin/ReleaseNotesSelect?{0}&CultureName={1}", parameters, SessionInfo.CultureShortName)).Result;

                IAjaxGrid grid = new AjaxGridFactory().CreateAjaxGrid(ReleaseNotesMapper.Map(selectRequest.Result).AsQueryable(), page.HasValue ? page.Value : 1, page.HasValue, selectRequest.RowsCount.Value);

                return Json(new { Html = grid.ToJson("_GridReleaseNotePartial", this), grid.HasItems }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}