
using DocumentFormat.OpenXml.Wordprocessing;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Framework.Encryption;
using MCS.Framework.Localization;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Mappers;
using MCS.UI.Areas.User.Mappers.Transaction;
using MCS.UI.Areas.User.Models.Hub;
using MCS.UI.Areas.User.Models.Lookups;
using MCS.UI.Areas.User.Models.Survey;
using MCS.UI.Areas.User.Models.Transaction;
using MCS.UI.Common;
using MCS.UI.Controls;
using MCS.UI.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MCS.UI.Areas.User.Controllers
{
    public class SurveyController : BaseController
    {
        // GET: User/Survey
        public ActionResult Index()
        {
            try
            {
                SurveyVM surveyVM = new SurveyVM(); 
                PostResult postResultCheck =
                              HttpClientWrapper<PostResult>.PostRequest(string.Format("api/Survey/CheckUserFilledSurvey?UserId={0}&OrgUnitId={1}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId), null).Result;

                if (!Convert.ToBoolean(postResultCheck.Result))
                    return View("~/Areas/User/Views/Survey/_DeleteUserSurveyPartial.cshtml");
                else
                surveyVM.SurveyQuestion = GetSurvey();



                return View("~/Areas/User/Views/Survey/_SurveyPartial.cshtml", surveyVM);
            }
            catch (Exception ex)
            {
                throw;
            }
             
        }


        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddSurvey(SurveyVM Survey)
        {
            List<SurveyAnswerVM> SurveyAnswerVMs = new List<SurveyAnswerVM>();
            foreach (SurveyQuestionVM surveyQuestion in Survey.SurveyQuestion)
            {
                SurveyAnswerVMs.Add(surveyQuestion.SurveyAnswer);  
            }
            List<SurveyAnswerDTO> SurveyAnswerDTOs = new List<SurveyAnswerDTO>();
            SurveyAnswerDTOs =SurveyMapper.Map(SurveyAnswerVMs);
            string message = string.Empty;
            PostResult postResult =
            HttpClientWrapper<PostResult>.PostRequest("api/Survey/AddSurveyAnswer", SurveyAnswerDTOs).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }

            SurveyNoteDTO surveyNoteDTO = SurveyMapper.Map(Survey.surveyNote);
            surveyNoteDTO.OrgUnitId = SessionInfo.OrgUnitId;
            surveyNoteDTO.UserId = SessionInfo.CurrentUser.Id;

            PostResult postSurveyNotesResult =
              HttpClientWrapper<PostResult>.PostRequest("api/Survey/AddSurveyNotes", surveyNoteDTO).Result;
            if (postResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
            }
            return RedirectToAction("Index", "Survey");
        }

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult DeleteUserSurvey()
        { 
            string message = string.Empty;


            PutResult putResult =
              HttpClientWrapper<PutResult>.PutRequest(string.Format("api/Survey/DeleteUserSurvey?UserId={0}&OrgUnitId={1}", SessionInfo.CurrentUser.Id,SessionInfo.OrgUnitId), null).Result;
            if (putResult.StatusCode != StatusCode.Ok)
            {
                message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, putResult.StatusCode.ToString()); 
            } 
            return RedirectToAction("Index", "Survey");
        }


        private List<SurveyQuestionVM> GetSurvey()
        {

            List<SurveyQuestionVM> SurveyQuestionVMs = new List<SurveyQuestionVM>();
            GetResult<List<SurveyQuestionDTO>> getResult =
                 HttpClientWrapper<GetResult<List<SurveyQuestionDTO>>>.GetItemRequest(string.Format("api/Survey/GetSurveyQuestions?UserId={0}&OrgUnitId={1}", SessionInfo.CurrentUser.Id, SessionInfo.OrgUnitId)).Result;
           SurveyQuestionVMs = SurveyMapper.Map(getResult.Result);
           
               
            return SurveyQuestionVMs;

        }
    }
} 