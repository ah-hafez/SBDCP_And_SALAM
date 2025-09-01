using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.AdminAudit;

namespace MCS.Service.Mappers
{
    public class AuditLogMapper
    {
        public static List<AuditLogDTO> Map(IList<AuditLog> AuditLog)
        {
            if (AuditLog.Any())
            {
                List<AuditLogDTO> AuditLogDTOs = AuditLog.Select(log =>
                {
                    AuditLogDTO AuditLogDTO = new AuditLogDTO
                    {
                        AuditData = log.AuditData,
                        AuditDate = log.AuditDate,
                        AuditAction = log.AuditAction,
                        //User = UserProfileMapper.MapUserProfile(log.User),
                        AuditUser = log.AuditUser,
                        EntityType = log.EntityType,
                        GuidId = log.GuidId,
                    };
                    return AuditLogDTO;
                }).ToList();
                return AuditLogDTOs;
            }
            return new List<AuditLogDTO>();
        }


        public static ApiAuditLog Map(ApiAuditLogDto AuditLog)
        {
            if (AuditLog != null)
            {

                ApiAuditLog AuditLogDTO = new ApiAuditLog
                {
                    Machine = AuditLog.Machine,
                    RequestContentBody = AuditLog.RequestContentBody,
                    RequestContentType = AuditLog.RequestContentType,
                    RequestHeaders = AuditLog.RequestHeaders,
                    RequestIpAddress = AuditLog.RequestIpAddress,
                    RequestTimestamp = AuditLog.RequestTimestamp,
                    ResponseContentBody = AuditLog.ResponseContentBody,
                    ResponseContentType = AuditLog.ResponseContentType,
                    RequestUri = AuditLog.RequestUri,
                    ResponseHeaders = AuditLog.ResponseHeaders,
                    ResponseStatusCode = AuditLog.ResponseStatusCode,
                    ResponseTimestamp = AuditLog.ResponseTimestamp,
                    UserId = AuditLog.UserId,
                    CreatedOn = DateTime.Now,
                    RequestMethod = AuditLog.RequestMethod,
                    Signature = AuditLog.Signature,
                };
                return AuditLogDTO;


            }
            return null;
        }

    }
}