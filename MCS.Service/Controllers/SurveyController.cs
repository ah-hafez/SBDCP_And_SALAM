using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Script.Serialization;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Localization.SupportClasses;
using MCS.Framework.Persistence;
using MCS.Framework.Security;
using MCS.Framework.Web;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.Domain.Search.SearchCriteria;
using MCS.DTO;
using MCS.DTO.ExternalParties;
using MCS.DTO.Transaction;
using MCS.Service.Mappers;
using System.Text;
using System.Web;
using YESSER.NCS.MCS.Service.Helpers;
using HashMechanism;
using static MCS.Service.Controllers.TransactionController.CertificationClient;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class SurveyController : ApiBaseController
    {

     

        [HttpGet]
        public HttpResponseMessage GetSurveyQuestions(int UserId , int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SurveyQuestionDTO>> getResult = null;
            try
            {
                using (var SurveyContextScope = context.CreateReadOnly())
                {
                    if (ModelState.IsValid)
                    { 
                        ISurveyBL SurveyBL = IoC.Resolve<SurveyBL>();
                        var SurveyQuestionList = SurveyBL.GetSurveyQuestions(UserId, OrgUnitId);
                        var SurveyQuestionListDTO = SurveyMapper.Map(SurveyQuestionList);
                        getResult = GetResult<List<SurveyQuestionDTO>>.Create(statusCode, SurveyQuestionListDTO, 0);
                        return Request.CreateResponse(HttpStatusCode.OK, getResult);
                    }
                    return Request.CreateResponse(HttpStatusCode.NotFound, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SurveyQuestionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SurveyQuestionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage AddSurveyAnswer(List<SurveyAnswerDTO> SurveyAnswerDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var SurveyContextScope = context.Create())
                {
                    ISurveyBL SurveyBL = IoC.Resolve<SurveyBL>();
                    IList<SurveyAnswer> surveyAnswers = SurveyMapper.Map(SurveyAnswerDTOs);
                     SurveyBL.AddSurveyAnswer(surveyAnswers); 
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage AddSurveyNotes(SurveyNoteDTO SurveyNotes)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var SurveyContextScope = context.Create())
                {
                    ISurveyBL SurveyBL = IoC.Resolve<SurveyBL>();
                    SurveyNote surveyNote = SurveyMapper.Map(SurveyNotes);
                    SurveyBL.AddSurveyNotes(surveyNote);
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpPut]
        public HttpResponseMessage DeleteUserSurvey(int UserId, int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var SurveyContextScope = context.Create())
                {
                    ISurveyBL SurveyBL = IoC.Resolve<SurveyBL>(); 
                    SurveyBL.DeleteUserSurvey(UserId, OrgUnitId);
                     
                }

                postResult = PostResult.Create(statusCode, null);


                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage CheckUserFilledSurvey(int UserId, int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ISurveyBL SurveyBL = IoC.Resolve<SurveyBL>();
                    bool IsFilled = SurveyBL.CheckUserFilledSurvey(UserId, OrgUnitId); 

                    getResult = GetResult<bool>.Create(statusCode, IsFilled, null);
                    transactionContextScope.Commit();
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<bool>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


    }

}

