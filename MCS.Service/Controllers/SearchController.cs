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
using MCS.Service.Helpers;
using Microsoft.AspNet.Identity;
using MCS.Framework.Security;
using MCS.Framework.Web;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class SearchController : ApiBaseController
    {
        [HttpPost]
        public HttpResponseMessage InboundSearch(SearchCriteriaByInboundDTO searchCriteriaByInboundDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<InboundSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<InboundSearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchInbound(SearchCriteriaMapper.Map(searchCriteriaByInboundDTO)));

                    getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage DocumentNumberSearch(SearchCriteriaByDocumentNumberDTO searchCriteriaByDocumentNumberDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<InboundSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<InboundSearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchDocumentNumber(SearchCriteriaMapper.Map(searchCriteriaByDocumentNumberDTO)));

                    getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage RecordNumberSearch(SearchCriteriaByRecordNumberDTO searchCriteriaByRecordNumberDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<InboundSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<InboundSearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchRecordNumber(SearchCriteriaMapper.Map(searchCriteriaByRecordNumberDTO)));

                    getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<InboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage EntitySearch(SearchCriteriaByEntityNameDTO searchCriteriaByEntityNameDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<EntitySearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<EntitySearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchEntity(SearchCriteriaMapper.Map(searchCriteriaByEntityNameDTO)));

                    getResult = GetResult<List<EntitySearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<EntitySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<EntitySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreatorSearch(SearchCriteriaByCreatorDTO searchCriteriaByEntityNameDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CreatorSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<CreatorSearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchCreator(SearchCriteriaMapper.Map(searchCriteriaByEntityNameDTO)));

                    getResult = GetResult<List<CreatorSearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CreatorSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CreatorSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage NamesSearch(SearchCriteriaByNamesDTO searchCriteriaByNamesDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<NamesSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<NamesSearchResultDTO> NamesResultDTOs = SearchMapper.Map(SearchBL.SearchNames(SearchCriteriaMapper.Map(searchCriteriaByNamesDTO)));

                    getResult = GetResult<List<NamesSearchResultDTO>>.Create(statusCode, NamesResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<NamesSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<NamesSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage DailySearch(SearchCriteriaByDailyDTO searchCriteriaByDailyDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<DailySearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<DailySearchResultDTO> DailyResultDTOs = SearchMapper.Map(SearchBL.SearchDaily(SearchCriteriaMapper.Map(searchCriteriaByDailyDTO)));

                    getResult = GetResult<List<DailySearchResultDTO>>.Create(statusCode, DailyResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<DailySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<DailySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage AssignmentNoteSearch(SearchCriteriaByAssignmentNoteDTO searchCriteriaByAssignmentNoteDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AssignmentNoteSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<AssignmentNoteSearchResultDTO> AssignmentNoteResultDTOs = SearchMapper.Map(SearchBL.SearchAssignmentNote(SearchCriteriaMapper.Map(searchCriteriaByAssignmentNoteDTO)));

                    getResult = GetResult<List<AssignmentNoteSearchResultDTO>>.Create(statusCode, AssignmentNoteResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AssignmentNoteSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AssignmentNoteSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage ManifestNumberSearch(SearchCriteriaByManifestNumberDTO searchCriteriaByManifestNumberDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ManifestNumberSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<ManifestNumberSearchResultDTO> ManifestNumberResultDTOs = SearchMapper.Map(SearchBL.SearchManifestNumber(SearchCriteriaMapper.Map(searchCriteriaByManifestNumberDTO)));

                    getResult = GetResult<List<ManifestNumberSearchResultDTO>>.Create(statusCode, ManifestNumberResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ManifestNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ManifestNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage MilitaryNumberOrIdentitySearch(SearchCriteriaByMilitaryNumberOrIdentityDTO searchCriteriaByMilitaryNumberOrIdentityDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<MilitaryNumberOrIdentitySearchResultDTO> MilitaryNumberOrIdentityResultDTOs = SearchMapper.Map(SearchBL.SearchMilitaryNumberOrIdentity(SearchCriteriaMapper.Map(searchCriteriaByMilitaryNumberOrIdentityDTO)));

                    getResult = GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>.Create(statusCode, MilitaryNumberOrIdentityResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<MilitaryNumberOrIdentitySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage TransactionNumberSearch(SearchCriteriaByTransactionNumberDTO searchCriteriaByTransactionNumberDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionNumberSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {


                    List<TransactionNumberSearchResultDTO> TransactionNumberResultDTOs = SearchMapper.Map(SearchBL.SearchTransactionNumber(SearchCriteriaMapper.Map(searchCriteriaByTransactionNumberDTO)));

                    getResult = GetResult<List<TransactionNumberSearchResultDTO>>.Create(statusCode, TransactionNumberResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage TransactionNotsSearch(SearchCriteriaByTransactionNotsDTO searchCriteriaByTransactionNotsDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<TransactionNotsSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<TransactionNotsSearchResultDTO> TransactionNotsResultDTOs = SearchMapper.Map(SearchBL.SearchTransactionNots(SearchCriteriaMapper.Map(searchCriteriaByTransactionNotsDTO)));

                    getResult = GetResult<List<TransactionNotsSearchResultDTO>>.Create(statusCode, TransactionNotsResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<TransactionNotsSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<TransactionNotsSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ELcEmployeeSearch(SearchCriteriaByElcEmployeeDTO searchCriteriaByELcEmployeeDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ELcEmployeeSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<ELcEmployeeSearchResultDTO> ELcEmployeeResultDTOs = SearchMapper.Map(SearchBL.SearchElcEmployee(SearchCriteriaMapper.Map(searchCriteriaByELcEmployeeDTO)));

                    getResult = GetResult<List<ELcEmployeeSearchResultDTO>>.Create(statusCode, ELcEmployeeResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ELcEmployeeSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ELcEmployeeSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage ExternalOutBoundOrManifestNumberSearch(SearchCriteriaByExternalOutBoundOrManifestNumberDTO searchCriteriaByExternalOutBoundOrManifestNumberDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<ExternalOutBoundOrManifestNumberSearchResultDTO> ExternalOutBoundOrManifestNumberResultDTOs = SearchMapper.Map(SearchBL.SearchExternalOutBoundOrManifestNumber(SearchCriteriaMapper.Map(searchCriteriaByExternalOutBoundOrManifestNumberDTO)));

                    getResult = GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>.Create(statusCode, ExternalOutBoundOrManifestNumberResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalOutBoundOrManifestNumberSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage CopyAssignemntSearch(SearchCriteriaByCopyAssignemntDTO searchCriteriaByCopyAssignemntDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CopyAssignemntSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<CopyAssignemntSearchResultDTO> CopyAssignemntResultDTOs = SearchMapper.Map(SearchBL.SearchCopyAssignemnt(SearchCriteriaMapper.Map(searchCriteriaByCopyAssignemntDTO)));

                    getResult = GetResult<List<CopyAssignemntSearchResultDTO>>.Create(statusCode, CopyAssignemntResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CopyAssignemntSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CopyAssignemntSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage SubjectLetterSearch(SearchCriteriaBySubjectLetterDTO searchCriteriaBySubjectLetterDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SubjectLetterSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<SubjectLetterSearchResultDTO> SubjectLetterResultDTOs = SearchMapper.Map(SearchBL.SearchSubjectLetter(SearchCriteriaMapper.Map(searchCriteriaBySubjectLetterDTO)));

                    getResult = GetResult<List<SubjectLetterSearchResultDTO>>.Create(statusCode, SubjectLetterResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SubjectLetterSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SubjectLetterSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage AssignTransactionSearch(SearchCriteriaByAssignTransactionDTO searchCriteriaByEntityNameDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<AssignTransactionSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<AssignTransactionSearchResultDTO> inboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchAssignTransaction(SearchCriteriaMapper.Map(searchCriteriaByEntityNameDTO)));

                    getResult = GetResult<List<AssignTransactionSearchResultDTO>>.Create(statusCode, inboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<AssignTransactionSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<AssignTransactionSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage OutboundInternalSearch(SearchCriteriaByOutboundInternalDTO searchCriteriaByOutboundInternalDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OutboundInternalSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<OutboundInternalSearchResultDTO> OutboundInternalSearchResultDTOs = SearchMapper.Map(SearchBL.SearchOutboundInternal(SearchCriteriaMapper.Map(searchCriteriaByOutboundInternalDTO)));

                    getResult = GetResult<List<OutboundInternalSearchResultDTO>>.Create(statusCode, OutboundInternalSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OutboundInternalSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OutboundInternalSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage OutboundSearch(SearchCriteriaByOutboundDTO searchCriteriaByOutboundDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OutboundSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<OutboundSearchResultDTO> outboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchOutbound(SearchCriteriaMapper.Map(searchCriteriaByOutboundDTO)));

                    getResult = GetResult<List<OutboundSearchResultDTO>>.Create(statusCode, outboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OutboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OutboundSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage ExternalPartyCopiesSearch(SearchCriteriaByExternalPartyCopiesDTO searchCriteriaByExternalPartyCopiesDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyCopiesSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<ExternalPartyCopiesSearchResultDTO> externalPartyCopiesResultDTO = SearchMapper.Map(SearchBL.SearchExternalPartyCopies(SearchCriteriaMapper.Map(searchCriteriaByExternalPartyCopiesDTO)));

                    getResult = GetResult<List<ExternalPartyCopiesSearchResultDTO>>.Create(statusCode, externalPartyCopiesResultDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyCopiesSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyCopiesSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage OutboundDraftSearch(SearchCriteriaByOutboundDraftDTO searchCriteriaByOutboundDraftDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OutboundDraftSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    List<OutboundDraftSearchResultDTO> outboundSearchResultDTOs = SearchMapper.Map(SearchBL.SearchOutboundDraft(SearchCriteriaMapper.Map(searchCriteriaByOutboundDraftDTO)));

                    getResult = GetResult<List<OutboundDraftSearchResultDTO>>.Create(statusCode, outboundSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OutboundDraftSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OutboundDraftSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage SubjectSearch(SearchCriteriaBySubjectDTO searchCriteriaBySubjectDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SubjectSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<SubjectSearchResultDTO> subjectSearchResultDTOs = SearchMapper.Map(SearchBL.SearchSubject(SearchCriteriaMapper.Map(searchCriteriaBySubjectDTO)));

                    getResult = GetResult<List<SubjectSearchResultDTO>>.Create(statusCode, subjectSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<SubjectSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<SubjectSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage BarcodeSearch(SearchCriteriaByBarcodeDTO searchCriteriaByBarcodeDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<BaseSearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<BaseSearchResultDTO> barcodeSearchResultDTOs = SearchMapper.Map(SearchBL.SearchBarcode(SearchCriteriaMapper.Map(searchCriteriaByBarcodeDTO)));

                    getResult = GetResult<List<BaseSearchResultDTO>>.Create(statusCode, barcodeSearchResultDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<BaseSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<BaseSearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpPost]
        public HttpResponseMessage InquirySearch(string TransactionNumber, int InquiryType, int YearH, int? DestinationId, string subject, int entityId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<InquirySearchResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int? userWeigth;
                    User user = (User)UserContext.LoggedInUser;
                    IList<Transaction> transaction = TransactionBL.GetTransactionsByNumber(TransactionNumber, InquiryType, out userWeigth, YearH, DestinationId, subject, user.Id, entityId);

                    List<InquirySearchResultDTO> inquirySearchResultDTO = SearchMapper.Map(transaction);

                    foreach (InquirySearchResultDTO SearchResult in inquirySearchResultDTO)
                    {
                        if (SearchResult.Weight <= userWeigth)
                            SearchResult.HasPermission = true;
                        LogAction(AuditingActionCode.AdvanceQuery, SearchResult.Id);
                    }

                    getResult = GetResult<List<InquirySearchResultDTO>>.Create(statusCode, inquirySearchResultDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<InquirySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<InquirySearchResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

    }
}
