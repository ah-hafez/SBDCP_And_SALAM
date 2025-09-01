using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Framework.Persistence;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.Domain;
using MCS.DTO;
using MCS.Service.Mappers;
using Action = MCS.Domain.Action;
using System.Web;
using MCS.DTO.Shared;

namespace MCS.Service.Controllers
{
    [CustomAuthenticationAttribute]
    public class CommonController : ApiBaseController
    {
        #region Culture

        [HttpGet]
        public HttpResponseMessage GetCultures()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CultureDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();

                    IList<Culture> cultures = commonBL.GetCultures();

                    List<CultureDTO> cultureDTOs = CultureMapper.Map(cultures);

                    getResult = GetResult<List<CultureDTO>>.Create(statusCode, cultureDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CultureDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CultureDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion  Culture

        //#region Org. Units

        //[HttpGet]
        //public HttpResponseMessage GetOrgUnitById(int orgUnitId, string cultureName)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    GetResult<OrgUnitDTO> getResult = null;

        //    try{using (var transactionContextScope = context.Create())
        //    {
        //        IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
        //        OrgUnit orgUnit = orgUnitBL.GetOrgUnitById(orgUnitId, cultureName);
        //        OrgUnitDTO OrgUnitDTO = OrgUnitMapper.Map(orgUnit, cultureName);

        //        getResult = GetResult<OrgUnitDTO>.Create(statusCode, OrgUnitDTO, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    }catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //}

        //[HttpGet]
        //public HttpResponseMessage GetOrgUnits(string cultureName)
        //{
        //    StatusCode statusCode = Common.StatusCode.Ok;
        //    GetResult<List<OrgUnitDTO>> getResult = null;

        //    try{using (var transactionContextScope = context.Create())
        //    {
        //        IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
        //        IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(cultureName);
        //        List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

        //        getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    }catch (BusinessException ex)
        //    {
        //        statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

        //        getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //    catch (Exception ex)
        //    {
        //        ExceptionHelper.HandleException(ex);

        //        statusCode = Common.StatusCode.GeneralError;

        //        getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

        //        return Request.CreateResponse(HttpStatusCode.OK, getResult);
        //    }
        //}

        //#endregion Org. Units

        #region OrgUnits

        [HttpGet]
        public HttpResponseMessage GetOrgUnits(string cultureName, int? orgUnitId = null)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(cultureName, orgUnitId);
                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitsByParentId(string cultureName, int? ParentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(cultureName, ParentId);
                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnits(int? parentId, string cultureName, int? UserId, OrgUnitTreeMode? orgUnitTreeMode = OrgUnitTreeMode.User)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnits(parentId, cultureName, LoggedInOrgUnitId, UserId, orgUnitTreeMode);
                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitsAutoComplete(string searchQuery, string cultureName, int resultSize, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    IList<OrgUnit> orgUnits = orgUnitBL.GetOrgUnitsAutoComplete(searchQuery, cultureName, resultSize, orgUnitId);

                    List<OrgUnitDTO> OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, OrgUnitDTO, OrgUnitDTO.Count);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnit(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    OrgUnit orgUnits = orgUnitBL.GetOrgUnit(orgUnitId, cultureName);

                    OrgUnitDTO OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<OrgUnitDTO>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetParentOrgUnit(int orgUnitId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    OrgUnit orgUnits = orgUnitBL.GetParentOrgUnit(orgUnitId, cultureName);

                    OrgUnitDTO OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<OrgUnitDTO>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetInternalPartyInfoByNumber(string partyNumber, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<OrgUnitDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                    OrgUnit orgUnits = orgUnitBL.GetInternalPartyInfoByNumber(partyNumber, cultureName);

                    OrgUnitDTO OrgUnitDTO = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<OrgUnitDTO>.Create(statusCode, OrgUnitDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<OrgUnitDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOrgUnitsByIds(string orgUnitIds, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OrgUnitDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<OrgUnit> orgUnits = null;
                    if (!string.IsNullOrWhiteSpace(orgUnitIds))
                    {
                        List<int> Ids = orgUnitIds.Split(',').Select(int.Parse).ToList();

                        IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();

                        orgUnits = orgUnitBL.GetOrgUnits(Ids, cultureName);
                    }


                    List<OrgUnitDTO> orgUnitDTOs = OrgUnitMapper.Map(orgUnits, cultureName);

                    getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, orgUnitDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<OrgUnitDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion OrgUnits

        #region External Parties

        [HttpGet]
        public HttpResponseMessage GetExternalParties(int? parentId, string cultureName, bool getVirtual = false)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    CacheHelper.Remove(CachedObjectsKey.ExternalParties, "ar");
                    CacheHelper.Remove(CachedObjectsKey.ExternalParties, "en");
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalParty> parties = partyBL.GetExternalParties(parentId, cultureName, true);
                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetAllExternalParties(int? parentId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalParty> parties = partyBL.GetAllExternalParties(parentId, cultureName);
                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExternalPartiesAutoComplete(string searchQuery, string cultureName, int resultSize)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();

                    IList<ExternalParty> parties = partyBL.GetExternalPartiesAutoComplete(searchQuery, cultureName, resultSize);

                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, partiesDTO.Count);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetExternalPartyNodes(int? nodeId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalParty> parties = partyBL.GetExternalPartyNodes(nodeId, cultureName);
                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage GetExternalPartiesBySearchCriteria([FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalParty> parties = partyBL.GetExternalParties(searchCriteria);
                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.MapWithParentOrganization(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        public HttpResponseMessage GetExternalPartiesByParentId(int? parentId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalParty> parties = partyBL.GetExternalPartiesByParentId(parentId, cultureName);
                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage GetExternalPartiesByLetterId(int letterId, int? parentId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ExternalPartyDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL partyBL = IoC.Resolve<IExternalPartyBL>();

                    IList<ExternalParty> parties = partyBL.GetExternalPartiesByLetterType(letterId, parentId, cultureName);

                    List<ExternalPartyDTO> partiesDTO = ExternalPartyMapper.Map(parties);

                    getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, partiesDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ExternalPartyDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetManagersByPartyId(int partyId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ManagerDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalPartyManager> externalPartyManagers =
                        externalPartyBL.GetManagersByPartyId(partyId, cultureName);

                    List<ManagerDTO> managersDTO = ExternalPartyManagerMapper.Map(externalPartyManagers);

                    getResult = GetResult<List<ManagerDTO>>.Create(statusCode, managersDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ManagerDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ManagerDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostParty(ExternalPartyAddDTO partyAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                        ExternalParty externalParty = ExternalPartyMapper.Map(partyAddDTO);

                        int partyId = externalPartyBL.AddExternalParty(externalParty);

                        postResult = PostResult.Create(statusCode, partyId);

                        transactionContextScope.Commit();

                        if (statusCode == Common.StatusCode.Ok)
                        {
                            CacheHelper.Remove(CachedObjectsKey.ExternalParties, "ar");
                            CacheHelper.Remove(CachedObjectsKey.ExternalParties, "en");
                        }

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

        [HttpPut]
        public HttpResponseMessage PutParty(ExternalPartyEditDTO externalPartyEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                        ExternalParty externalParty = ExternalPartyMapper.Map(externalPartyEditDTO);

                        externalPartyBL.UpdateExternalParty(externalParty);

                        putResult = PutResult.Create(statusCode);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PutResult.Create(statusCode);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PutResult.Create(statusCode);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExternalParty(int id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ExternalPartyEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    ExternalPartyEditDTO externalPartyEditDTO = ExternalPartyMapper.Map(externalPartyBL.GetExternalPartyById(id));

                    getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, externalPartyEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetLastNumber(int parentId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<string> getResult = null;

            string newExternalPartyNumber = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

                    newExternalPartyNumber = externalPartyBL.GetLastNumber(parentId);

                    getResult = GetResult<string>.Create(statusCode, newExternalPartyNumber, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetLastNumberByCustomizeValue(string numberStartWithCustomizeValue)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<string> getResult = null;

            string newExternalPartyNumber = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

                    newExternalPartyNumber = externalPartyBL.GetLastNumberByCustomizeValue(numberStartWithCustomizeValue);

                    getResult = GetResult<string>.Create(statusCode, newExternalPartyNumber, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<string>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExternalPartyInfoByNumber(string partyNumber)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ExternalPartyEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    ExternalPartyEditDTO externalPartyEditDTO = ExternalPartyMapper.Map(externalPartyBL.GetExternalPartyInfoByNumber(partyNumber));

                    getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, externalPartyEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ExternalPartyEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpGet]
        public HttpResponseMessage CheckPartyNumber(string Number, int partyId = -1)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    bool IsAvailable = externalPartyBL.CheckPartyNumber(Number, partyId);

                    getResult = GetResult<bool>.Create(statusCode, IsAvailable, null);

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

        //managers 
        [HttpPost]
        public HttpResponseMessage PostExternalPartyManager(ManagerAddDTO managerAddDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                        ExternalPartyManager externalPartyManager = ExternalPartyManagerMapper.Map(managerAddDTO);

                        int managerId = externalPartyBL.AddExternalPartyManager(externalPartyManager);

                        postResult = PostResult.Create(statusCode, managerId);

                        transactionContextScope.Commit();

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

        [HttpPost]
        public HttpResponseMessage PutExternalPartyManager(ManagerEditDTO managerEditDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult putResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    if (ModelState.IsValid)
                    {
                        IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                        ExternalPartyManager externalPartyManager = ExternalPartyManagerMapper.Map(managerEditDTO);

                        externalPartyBL.UpdateExternalPartyManager(externalPartyManager);

                        putResult = PostResult.Create(statusCode, null);

                        transactionContextScope.Commit();

                        return Request.CreateResponse(HttpStatusCode.OK, putResult);
                    }

                    statusCode = Common.StatusCode.ModelNotValid;

                    putResult = PostResult.Create(statusCode, null);

                    return Request.CreateResponse(HttpStatusCode.OK, putResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                putResult = PostResult.Create(statusCode, null);

                return Request.CreateResponse(HttpStatusCode.OK, putResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExternalPartyManagerById(int externalPartyManagerId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ManagerEditDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    ManagerEditDTO managerEditDTO =
                        ExternalPartyManagerMapper.Map(externalPartyBL.GetExternalPartyManagerById(externalPartyManagerId));

                    getResult = GetResult<ManagerEditDTO>.Create(statusCode, managerEditDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ManagerEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ManagerEditDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetExternalPartyManagers(int partyId, [FromUri] SearchCriteria searchCriteria)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ManagerDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();
                    IList<ExternalPartyManager> externalPartyManagers =
                        externalPartyBL.GetExternalPartyManagers(partyId, searchCriteria, out rowsCount);

                    List<ManagerDTO> managersDTO = ExternalPartyManagerMapper.Map(externalPartyManagers);

                    getResult = GetResult<List<ManagerDTO>>.Create(statusCode, managersDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ManagerDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ManagerDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion External Parties

        #region Actions

        [HttpGet]
        public HttpResponseMessage GetAllActions(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ActionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IActionBL processBL = IoC.Resolve<IActionBL>();
                    IList<Action> actions = processBL.GetAllAction(cultureName).ToList();
                    List<ActionDTO> actionsDTO = ActionMapper.Map(actions);

                    getResult = GetResult<List<ActionDTO>>.Create(statusCode, actionsDTO, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ActionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        #endregion Actions

        #region Permission

        [HttpGet]
        public HttpResponseMessage GetPermissionsByGroupId(PermissionGroupName permissionGroupName, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();
                    IList<MCS.Domain.Permission> permissions = null;

                    if (HttpContext.Current.Session["permissions" + permissionGroupName.ToString()] == null)
                    {
                        permissions =
                        permissionBL.GetUserPermissionsByGroupId(permissionGroupName, cultureName);
                        HttpContext.Current.Session["permissions" + permissionGroupName.ToString()] = permissions;
                    }
                    else
                    {
                        permissions = HttpContext.Current.Session["permissions" + permissionGroupName.ToString()] as IList<MCS.Domain.Permission>;
                    }


                    List<PermissionDTO> permissionsDTOs = PermissionMapper.Map(permissions);

                    getResult = GetResult<List<PermissionDTO>>.Create(statusCode, permissionsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage RequestRoleItem(UserPendingGroupDTO userPendingGroupDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<object> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    UserPendingGroup userPendingGroup = UserPendingGroupMapper.Map(userPendingGroupDTO);
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();

                    userPendingGroupBL.RequestRoleItem(userPendingGroup, Language);

                    getResult = GetResult<object>.Create(Common.StatusCode.Ok, true, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<object>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<object>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpGet]
        public HttpResponseMessage GetuserPendingGroup(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserPendingGroupDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    IList<UserPendingGroup> userPendingGroup = userPendingGroupBL.GetuserPendingGroup(cultureName).ToList();
                    List<UserPendingGroupDTO> userPendingGroupDTOs = UserPendingGroupMapper.Map(userPendingGroup);
                    getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, userPendingGroupDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpGet]
        public HttpResponseMessage GetuserPendingRequest(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<UserPendingGroupDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    IList<UserPendingGroup> userPendingGroup = userPendingGroupBL.GetuserPendingRequest(cultureName).ToList();
                    List<UserPendingGroupDTO> userPendingGroupDTOs = UserPendingGroupMapper.Map(userPendingGroup);
                    getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, userPendingGroupDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<UserPendingGroupDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage ApproveRoleRequest(int Id, string CultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<UserGroupDTO> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    UserGroup userGroup = userPendingGroupBL.ApproveRoleRequest(Id, CultureName);
                    UserGroupDTO userGroupDTOs = UserGroupMapper.Map(userGroup);
                    getResult = GetResult<UserGroupDTO>.Create(statusCode, userGroupDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<UserGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<UserGroupDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }


        [HttpGet]
        public HttpResponseMessage RejectRoleRequest(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    bool resutl = userPendingGroupBL.RejectRoleRequest(Id);
                    getResult = GetResult<bool>.Create(statusCode, resutl, null);
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


        [HttpPost]
        public HttpResponseMessage ApproveManagerRoleRequest(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateWithTransaction(System.Data.IsolationLevel.ReadCommitted))
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    bool userGroup = userPendingGroupBL.ApproveManagerRoleRequest(Id);

                    getResult = GetResult<bool>.Create(statusCode, userGroup, null);
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


        [HttpPost]
        public HttpResponseMessage RejectManagerRoleRequest(int Id)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IUserPendingGroupBL userPendingGroupBL = IoC.Resolve<IUserPendingGroupBL>();
                    bool resutl = userPendingGroupBL.RejectManagerRoleRequest(Id);
                    getResult = GetResult<bool>.Create(statusCode, resutl, null);
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


        #endregion Permission

        #region collaboration

        [HttpGet]
        public HttpResponseMessage GetIntitialChatHistory(int toUserId, int pageSize, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConversationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    ICollaborationBL conversationBL = new CollaborationBL();

                    IList<MCS.Domain.Collaboration> conversations = conversationBL.GetCollaboration(toUserId, pageSize, cultureName);

                    List<ConversationDTO> conversationDTOs = ConversationMapper.Map(conversations);

                    getResult = GetResult<List<ConversationDTO>>.Create(statusCode, conversationDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);


                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConversationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConversationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }


        }

        [HttpGet]
        public HttpResponseMessage GetChatHistory(int toUserId, int pageSize, int startId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConversationDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    ICollaborationBL conversationBL = new CollaborationBL();

                    IList<MCS.Domain.Collaboration> conversations = conversationBL.GetCollaboration(toUserId, pageSize, startId, cultureName);

                    List<ConversationDTO> conversationDTOs = ConversationMapper.Map(conversations);

                    getResult = GetResult<List<ConversationDTO>>.Create(statusCode, conversationDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);


                }

            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConversationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConversationDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }


        }


        [HttpGet]
        public HttpResponseMessage GetChatNotifications()
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<ChatNotificationsInfoDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICollaborationBL collaborationBL = new CollaborationBL();

                    ChatNotificationsInfo chatNotificationsInfo = collaborationBL.GetChatNotifications();

                    ChatNotificationsInfoDTO chatNotificationsInfoDTO = ConversationMapper.Map(chatNotificationsInfo);

                    getResult = GetResult<ChatNotificationsInfoDTO>.Create(statusCode, chatNotificationsInfoDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<ChatNotificationsInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<ChatNotificationsInfoDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }


        [HttpGet]
        public HttpResponseMessage GetCollaborationUsers(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<CollaborationUserInfoDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICollaborationBL collaborationBL = new CollaborationBL();

                    IList<CollaborationUserInfo> collaborationUserInfos = collaborationBL.GetAllCollaborationUsers(cultureName);

                    List<CollaborationUserInfoDTO> collaborationUserInfoDTOs = ConversationMapper.Map(collaborationUserInfos);

                    getResult = GetResult<List<CollaborationUserInfoDTO>>.Create(statusCode, collaborationUserInfoDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<CollaborationUserInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<CollaborationUserInfoDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpGet]
        public HttpResponseMessage GatTransactionChats(int transactionId, int pageIndex, int pageSize, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ConversationChatDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<ChatRoom> conversations = ChatBL.GetConversations(-1, null, transactionId, pageIndex, pageSize, out int itemsCount);
                    var conversationChatDTOs = new List<ConversationChatDTO>();
                    conversations.ForEach(room =>
                    {
                        var userNames = room.AllowedUsers.Select(u => u.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text).ToList();
                        var conversation = new ConversationChatDTO();
                        conversation.Id = room.Id;
                        conversation.Name = String.Join(" , ", userNames);
                        conversation.SendTime = DateTimeUtility.ConvertToUmAlQuraCalendar(room.CreatedOn);

                        conversationChatDTOs.Add(conversation);
                    });

                    getResult = GetResult<List<ConversationChatDTO>>.Create(statusCode, conversationChatDTOs, itemsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ConversationChatDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ConversationChatDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetConversationMessages(int roomId, int timeZone)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<MessageResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<ChatMessage> messages = ChatBL.GetMessagesByRoomId(roomId);
                    var recentMessages = new List<MessageResultDTO>();
                    messages.ForEach(message =>
                    {
                        var result = new MessageResultDTO()
                        {
                            Id = message.Id,
                            Content = message.Content,
                            User = message.User != null ? UserProfileMapper.MapUserProfileChat(message.User) : null,
                            When = message.When,
                            SendTime = message.When.AddMinutes(timeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                            SendDate = message.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        };

                        recentMessages.Add(result);
                    });
                    recentMessages.Reverse();

                    getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, recentMessages, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        [HttpGet]
        public HttpResponseMessage GetPreviousMessages(int messageId, int timeZone)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<MessageResultDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    List<ChatMessage> messages = ChatBL.GetPreviousMessages(messageId);
                    var recentMessages = new List<MessageResultDTO>();
                    messages.ForEach(message =>
                    {
                        var result = new MessageResultDTO()
                        {
                            Id = message.Id,
                            Content = message.Content,
                            User = message.User != null ? UserProfileMapper.MapUserProfileChat(message.User) : null,
                            When = message.When,
                            SendTime = message.When.AddMinutes(timeZone).ToString("hh:mm tt", CultureInfo.InvariantCulture),
                            SendDate = message.When.Date.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
                        };

                        recentMessages.Add(result);
                    });

                    getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, recentMessages, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<MessageResultDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }

        }

        #endregion collaboration

        [HttpGet]
        public HttpResponseMessage GetReporter(string cultureName, int orgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ReporterDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IReporterBL reporterBL = IoC.Resolve<IReporterBL>();
                    IList<Reporter> reporters = reporterBL.GetReporters(cultureName, orgUnitId).ToList();
                    List<ReporterDTO> reporterDTOs = ReporterMapper.Map(reporters, cultureName);
                    getResult = GetResult<List<ReporterDTO>>.Create(statusCode, reporterDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ReporterDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ReporterDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpPost]
        public HttpResponseMessage PostReporter(ReporterDTO reporterDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PostResult postResult = null;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IReporterBL reporterBL = IoC.Resolve<IReporterBL>();
                    Reporter reporter = ReporterMapper.Map(reporterDTO);
                    int reporterId = reporterBL.AddReporter(reporter);

                    postResult = PostResult.Create(statusCode, reporterId);

                    transactionContextScope.Commit();

                    return Request.CreateResponse(HttpStatusCode.Created, postResult);
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


        [HttpPost]
        public HttpResponseMessage SaveNotification(SupportDTO supportDTO)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<object> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ITenantBL tenantBL = IoC.Resolve<ITenantBL>();

                    tenantBL.SendSupportEmail(supportDTO, Language);

                    getResult = GetResult<object>.Create(Common.StatusCode.Ok, true, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<object>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<object>.Create(statusCode, false, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }



        [HttpGet]
        public HttpResponseMessage GetThemes(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<ThemeDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();

                    IList<Theme> theme = commonBL.GetThemes();

                    List<ThemeDTO> themeDTOs = ThemeMapper.Map(theme, cultureName);

                    getResult = GetResult<List<ThemeDTO>>.Create(statusCode, themeDTOs, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<ThemeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<ThemeDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetOutlookPermissionsByGroupId(PermissionGroupName permissionGroupName, int userId, string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<PermissionDTO>> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    int rowsCount = 0;

                    IPermissionBL permissionBL = IoC.Resolve<IPermissionBL>();

                    IList<MCS.Domain.Permission> permissions =
                        permissionBL.GetOutlookUserPermissionsByGroupId(permissionGroupName, userId, cultureName);

                    List<PermissionDTO> permissionsDTOs = PermissionMapper.Map(permissions);

                    getResult = GetResult<List<PermissionDTO>>.Create(statusCode, permissionsDTOs, rowsCount);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<List<PermissionDTO>>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }

        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage AddOnlineUser(int userid, int OrgUnitId, string connectionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();
                    var result = commonBL.AddUserOnline(userid, OrgUnitId, connectionId);
                    getResult = GetResult<bool>.Create(statusCode, result, null);
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
     
        [HttpPost]
        public HttpResponseMessage UpdateUserOnline(int userid, int OrgUnitId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();
                    var result = commonBL.UpdateUserOnline(userid, OrgUnitId);
                    getResult = GetResult<bool>.Create(statusCode, result, null);
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


        [AllowAnonymous]
        [HttpGet]
        public HttpResponseMessage DeleteOnlineUser(string connectionId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<bool> getResult = null;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();
                    var result = commonBL.DeleteOnlineUser(connectionId);
                    getResult = GetResult<bool>.Create(statusCode, result, null);
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



        [HttpGet]
        public HttpResponseMessage GetOnlineUser(string cultureName)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<OnlineUserDTO>> getResult = null;
            var list = new List<OnlineUserDTO>();

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    ICommonBL commonBL = IoC.Resolve<ICommonBL>();
                    var result = commonBL.GetOnlineUser();

                    foreach (var item in result)
                    {
                        if (list.Any(x => x.UserId == item.UserId) == false)
                        {
                            list.Add(new OnlineUserDTO
                            {
                                UserId = item.UserId,
                                UserFullName = item.User.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text,
                                OrgUnitId = item.OrgUnitId,
                                OrgUnitName = item.OrgUnitId.HasValue ? item.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault().Text : ""
                            });
                        }
                    }

                    getResult = GetResult<List<OnlineUserDTO>>.Create(statusCode, list, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<OnlineUserDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<OnlineUserDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}