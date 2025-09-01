using MCS.Domain;
using MCS.DTO;
using MCS.IntegrationServices.Models;
using MCS.IntegrationServices.Models.IAM.Role;
using MCS.IntegrationServices.Models.IAM.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public static class AuditLogMapper
    {
        public static ApiAuditLogDto Map(ApiAuditLogVM apiAuditLogVM)
        {
            if (apiAuditLogVM != null)
            {
                ApiAuditLogDto apiAuditLogDto = new ApiAuditLogDto
                {
                    Machine = apiAuditLogVM.Machine,
                    RequestContentBody = apiAuditLogVM.RequestContentBody,
                    RequestContentType = apiAuditLogVM.RequestContentType,
                    RequestHeaders = apiAuditLogVM.RequestHeaders,
                    RequestIpAddress = apiAuditLogVM.RequestIpAddress,
                    RequestTimestamp = apiAuditLogVM.RequestTimestamp,
                    ResponseContentBody = apiAuditLogVM.ResponseContentBody,
                    ResponseContentType = apiAuditLogVM.ResponseContentType,
                    RequestUri = apiAuditLogVM.RequestUri,
                    ResponseHeaders = apiAuditLogVM.ResponseHeaders,
                    ResponseStatusCode = apiAuditLogVM.ResponseStatusCode,
                    ResponseTimestamp = apiAuditLogVM.ResponseTimestamp,
                    UserId = apiAuditLogVM.UserId,
                    RequestMethod = apiAuditLogVM.RequestMethod,
                    Signature = apiAuditLogVM.Signature,
                };
                return apiAuditLogDto;
            }
            return new ApiAuditLogDto();


        }





    }
}