using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.Service.Mappers;
using MCS.Domain;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class ICController : ApiBaseController
    {
        [HttpPost]
        public HttpResponseMessage AddIcSubject(IC_SUBJECTDTO icSubjectDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IC_SUBJECT icSubject = IC_SUBJECTMapper.Map(icSubjectDTO);
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    int result = icSubjectBL.AddIC_SUBJECT(icSubject);
                    postResult = PostResult.Create(statusCode, result);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpGet]
        public HttpResponseMessage GetClassificationTypes()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ClassificationDto>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();

                    var classifications = icSubjectBL.GetClassificationTypes();

                    List<ClassificationDto> classificationDtos = IC_SUBJECTMapper.Map(classifications);

                    getResult = GetResult<List<ClassificationDto>>.Create(statusCode, classificationDtos, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ClassificationDto>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ClassificationDto>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage UpdateIC_SUBJECT(IC_SUBJECTDTO icSubjectDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            int Result = 0;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IC_SUBJECT icSubject = IC_SUBJECTMapper.Map(icSubjectDTO);
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    Result = icSubjectBL.UpdateIC_SUBJECT(icSubject);
                    postResult = PostResult.Create(statusCode, Result);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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

        [HttpDelete]
        public HttpResponseMessage DeleteIC_SUBJECT(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    icSubjectBL.DeleteIC_SUBJECT(id);
                    deleteResult = DeleteResult.Create(statusCode);
                    return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetIC_SUBJECTById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IC_SUBJECTDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();

                    IC_SUBJECT icSubject = icSubjectBL.GetIC_SUBJECTById(id);

                    IC_SUBJECTDTO icSubjectDTO = IC_SUBJECTMapper.Map(icSubject);

                    getResult = GetResult<IC_SUBJECTDTO>.Create(statusCode, icSubjectDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IC_SUBJECTDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IC_SUBJECTDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetIC_SUBJECTByParentId(string query, int? id = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<IC_SUBJECTDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();

                    IList<IC_SUBJECT> icSubjects = icSubjectBL.GetIC_SUBJECTByParentId(id, query);

                    List<IC_SUBJECTDTO> icSubjectDTOs = IC_SUBJECTMapper.Map(icSubjects);

                    getResult = GetResult<List<IC_SUBJECTDTO>>.Create(statusCode, icSubjectDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<IC_SUBJECTDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<IC_SUBJECTDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }





        [HttpPost]
        public HttpResponseMessage ICSearch(SearchCriteriaByICDTO searchCriteriaByICDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ICSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<ICSearchResultDTO> socialNumberSearchResultDTO = SearchMapper.Map(SearchBL.ICSearch(searchCriteriaByICDTO.year, searchCriteriaByICDTO.transNumber, searchCriteriaByICDTO.orgId, searchCriteriaByICDTO.type, searchCriteriaByICDTO.userId, searchCriteriaByICDTO.culutre));

                    getResult = GetResult<List<ICSearchResultDTO>>.Create(statusCode, socialNumberSearchResultDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ICSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ICSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }






        [HttpPost]
        public HttpResponseMessage AddIC_SUBJECT_TRANSACTION(IC_SUBJECTTransactionDTO icSubjectDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    int result = icSubjectBL.AddIC_SUBJECT_TRANSACTION(icSubjectDTO);
                    postResult = PostResult.Create(statusCode, result);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
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


        [HttpDelete]
        public HttpResponseMessage DeleteIC_SUBJECT_Transaction(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    icSubjectBL.RemoveIC_SUBJECT_TRANSACTION(id, 0);
                    deleteResult = DeleteResult.Create(statusCode);
                    return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }
        [HttpDelete]
        public HttpResponseMessage IC_GetTransaction(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            DeleteResult deleteResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    var subjectTransaction = icSubjectBL.IC_GetTransaction(id);
                    deleteResult = DeleteResult.Create(statusCode);
                    return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                deleteResult = DeleteResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, deleteResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSubject_TransactionById(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<IC_SUBJECTTransactionDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    IIC_SUBJECTBL icSubjectBL = new IC_SUBJECTBL();
                    var subjectTransaction = icSubjectBL.IC_GetTransaction(id);

                    IC_SUBJECTTransactionDTO iC_SUBJECTTransactionDTO = IC_SUBJECTMapper.Map(subjectTransaction);

                    getResult = GetResult<IC_SUBJECTTransactionDTO>.Create(statusCode, iC_SUBJECTTransactionDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<IC_SUBJECTTransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<IC_SUBJECTTransactionDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}
