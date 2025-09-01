using System;
using System.Collections.Generic;
using System.Linq;
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
    public class SettingController : ApiBaseController
    {
        [HttpGet]
        public HttpResponseMessage GetSettingByModelId(int modelId)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<List<SettingDTO>> getResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISettingBL settingBL = IoC.Resolve<ISettingBL>();
                    List<Setting> settings = settingBL.GetSettingByModelId(modelId);
                    List<SettingDTO> settingDTOs = SettingMapper.Map(settings);
                    getResult = GetResult<List<SettingDTO>>.Create(statusCode, settingDTOs, null);
                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                getResult = GetResult<List<SettingDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                getResult = GetResult<List<SettingDTO>>.Create(statusCode, null, null);
                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
        [HttpPost]
        public HttpResponseMessage UpdateSettings(List<SettingDTO> settingDTOs)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            PutResult postResult = null;
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    ISettingBL settingBL = IoC.Resolve<ISettingBL>();
                    List<Setting> settings = SettingMapper.Map(settingDTOs);
                    settingBL.UpdateSettings(settings);
                    postResult = PutResult.Create(statusCode);
                    return Request.CreateResponse(HttpStatusCode.OK, postResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);
                postResult = PutResult.Create(statusCode);
                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
                statusCode = Common.StatusCode.GeneralError;
                postResult = PutResult.Create(statusCode);
                return Request.CreateResponse(HttpStatusCode.OK, postResult);
            }
        }

        [HttpGet]
        public HttpResponseMessage GetSettingValue(string Key)
        {
            StatusCode statusCode = Common.StatusCode.Ok;
            GetResult<SettingDTO> getResult = null;

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {

                    ISettingBL settingBL = new SettingBL();
                    SettingDTO settingDTO = null;
                    List<Setting> settings = settingBL.GetSettingByKey(Key);
                    Setting setting = settings.Find(a => a.Key == Key);
                    if (setting != null)
                    {
                        settingDTO = SettingMapper.Map(setting);
                    }

                    getResult = GetResult<SettingDTO>.Create(statusCode, settingDTO, null);

                    return Request.CreateResponse(HttpStatusCode.OK, getResult);
                }
            }
            catch (BusinessException ex)
            {
                statusCode = (StatusCode)Enum.Parse(typeof(StatusCode), ex.Message);

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);

                statusCode = Common.StatusCode.GeneralError;

                getResult = GetResult<SettingDTO>.Create(statusCode, null, null);

                return Request.CreateResponse(HttpStatusCode.OK, getResult);
            }
        }
    }
}
