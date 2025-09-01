using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class AuditMapper
    {
        public static List<AuditDTO> Map(IList<Audit> audits)
        {
            if (audits != null && audits.Any())
            {
                List<AuditDTO> auditDTOs = audits.Select(au =>
                {
                    AuditDTO auditDTO = new AuditDTO
                    {
                        Id = au.Id,
                        EntityName = au.EntityName,
                        IPAddress = au.IPAddress,
                        Date = au.Date,
                        UserId = au.UserId,
                        OperationType = au.OperationType,
                        AuditDetails = Map(au.Details),
                        CreatedBy = au.CreatedBy,
                        CreatedOn = au.CreatedOn,
                        ModefiedBy = au.ModefiedBy,
                        ModefiedOn = au.ModefiedOn
                    };
                    return auditDTO;
                }).ToList();
                return auditDTOs;
            }
            else
            {
                return new List<AuditDTO>();
            }
        }
        public static List<AuditDetailDTO> Map(IList<AuditDetail> auditDetails)
        {
            if (auditDetails != null && auditDetails.Any())
            {
                List<AuditDetailDTO> auditDetailDTOs = auditDetails.Select(ad =>
                {
                    AuditDetailDTO auditDetailDTO = new AuditDetailDTO
                    {
                        Id = ad.Id,
                        PropertyName = ad.PropertyName,
                        PropertyNewValue = ad.PropertyNewValue,
                        PropertyOldValue = ad.PropertyOldValue,
                        CreatedBy = ad.CreatedBy,
                        CreatedOn = ad.CreatedOn,
                        ModefiedBy = ad.ModefiedBy,
                        ModefiedOn = ad.ModefiedOn
                    };
                    return auditDetailDTO;
                }).ToList();
                return auditDetailDTOs;
            }
            else
            {
                return new List<AuditDetailDTO>();
            }

        }
        public static List<AuditDetailDTO> Map(IList<AuditDetails> auditDetails)
        {
            if (auditDetails != null && auditDetails.Any())
            {
                List<AuditDetailDTO> auditDetailDTOs = auditDetails.Select(ad =>
                {
                    AuditDetailDTO auditDetailDTO = new AuditDetailDTO
                    {
                        PropertyName = ad.PropertyName,
                        PropertyNewValue = ad.PropertyNewValue,
                        PropertyOldValue = ad.PropertyOldValue,
                        CreatedOn = ad.CreatedOn
                    };
                    return auditDetailDTO;
                }).ToList();
                return auditDetailDTOs;
            }
            else
            {
                return new List<AuditDetailDTO>();
            }

        }
        public static List<AuditDTO> Map(IList<MainAudit> mainAudits)
        {
            if (mainAudits != null && mainAudits.Any())
            {
                List<AuditDTO> auditDTOs = mainAudits.Select(ad =>
                {
                    AuditDTO auditDTO = new AuditDTO
                    {
                      Id = ad.Id,
                      Date = ad.Date,
                      OperationType = ad.OperationType,
                      UserName = ad.CreatedBy,
                      EntityName = "",
                      IPAddress = "",
                      UserId = 0,
                      AuditDetails = new List<AuditDetailDTO>()
                    };
                    return auditDTO;
                }).ToList();
                return auditDTOs;
            }
            else
            {
                return new List<AuditDTO>();
            }

        }
    }
}