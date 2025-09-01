using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class LookupsController : ApiBaseController
    {
        [HttpPost]
        public HttpResponseMessage PostLookupItem(LookupDTO lookupDTO, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                        Lookup lookup = LookupMapper.Map(lookupDTO);

                        int lookupId = lookupBL.AddLookupItem(lookup);

                        postResult = PostResult.Create(statusCode, lookupId);

                        transactionContextScope.Commit();

                        CacheHelper.Remove($"{CachedObjectsKey.Lookups}{LookupCategory.Title.ToString()}", cultureName);

                        return Request.CreateResponse(HttpStatusCode.Created, postResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    postResult = PostResult.Create(statusCode, -1);

                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                postResult = PostResult.Create(statusCode, -1);

                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLookupItem(int lookupId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<LookupDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ILookupBL lookupBL = IoC.Resolve<ILookupBL>();
                    Lookup lookup = lookupBL.GetLookupItem(lookupId, cultureName);
                    LookupDTO lookupDTO = LookupMapper.Map(lookup);

                    getResult = GetResult<LookupDTO>.Create(statusCode, lookupDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<LookupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<LookupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetLookupItems(LookupCategory lookupCategory, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LookupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    IList<Lookup> lookups =
                        CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<Lookup>;

                    if (lookups == null)
                    {
                        ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                        lookups = lookupBL.GetLookupItems(lookupCategory, cultureName);

                        CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                    }

                    List<LookupDTO> lookupDTOs = LookupMapper.Map(lookups);

                    getResult = GetResult<List<LookupDTO>>.Create(statusCode, lookupDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetLookupItemsWithoutCache(LookupCategory lookupCategory, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LookupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    ILookupBL lookupBL = IoC.Resolve<ILookupBL>();
                    IList<Lookup> lookups = lookupBL.GetLookupItemsWithoutCach(lookupCategory, cultureName) as IList<Lookup>;

                    if (lookups == null)
                    {

                        lookups = lookupBL.GetLookupItemsWithoutCach(lookupCategory, cultureName);

                        //CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                    }
                    rowsCount = lookups.Count;
                    List<LookupDTO> lookupDTOs = LookupMapper.Map(lookups);

                    getResult = GetResult<List<LookupDTO>>.Create(statusCode, lookupDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetActiveLookupItemsWithoutCache(LookupCategory lookupCategory, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<LookupDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;
                    ILookupBL lookupBL = IoC.Resolve<ILookupBL>();
                    IList<Lookup> lookups = lookupBL.GetActiveLookupItemsWithoutCach(lookupCategory, cultureName) as IList<Lookup>;

                    if (lookups == null)
                    {

                        lookups = lookupBL.GetActiveLookupItemsWithoutCach(lookupCategory, cultureName);

                        //CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                    }
                    rowsCount = lookups.Count;
                    List<LookupDTO> lookupDTOs = LookupMapper.Map(lookups);

                    getResult = GetResult<List<LookupDTO>>.Create(statusCode, lookupDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<LookupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetOrgUnitForms(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<FormDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IFormBL formBL = IoC.Resolve<IFormBL>();

                    IList<Form> forms = formBL.GetOrgUnitForms(orgUnitId, cultureName);

                    List<FormDTO> formDocumentDTOs = FormMapper.Map(forms, cultureName);

                    getResult = GetResult<List<FormDTO>>.Create(statusCode, formDocumentDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<FormDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<FormDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        //[HttpGet]
        //public HttpResponseMessage GetOrgUnitFormDocuments(int orgUnitId)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    GetResult<List<FormContentDTO>> getResult = null;

        //    try
        //    {
        //        int rowsCount = 0;

        //        IFormBL formBL = IoC.Resolve<IFormBL>();

        //        IList<FormContent> formContents = formBL.GetOrgUnitFormContent(orgUnitId);

        //        List<FormContentDTO> formContentDTOs = FormContentMapper.Map(formContents);

        //        getResult = GetResult<List<FormContentDTO>>.Create(statusCode, formContentDTOs, rowsCount);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        getResult = GetResult<List<FormContentDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        getResult = GetResult<List<FormContentDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //}
    }
}
