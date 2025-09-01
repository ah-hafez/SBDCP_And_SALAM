using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Helpers;

namespace MCS.UI.Areas.User.Mappers
{
    public class AuditMapper
    {
        public static List<AuditVM> Map(IList<AuditDTO> audits)
        {
            
            if (audits != null)
            {
                List<AuditVM> auditVMs = audits.Select(au =>
                {
                    AuditVM auditVM = new AuditVM
                    {
                        Id = au.Id,
                        EntityName = au.EntityName,
                        IPAddress = au.IPAddress,
                        Date = au.Date,
                        UserId = au.UserId,
                        OperationType = au.OperationType,
                        AuditDetails = Map(au.AuditDetails),
                        UserName = au.UserName,
                        CreatedBy = au.CreatedBy,
                        CreatedOn = au.CreatedOn,
                        ModefiedBy = au.ModefiedBy,
                        ModefiedOn = au.ModefiedOn
                    };
                    return auditVM;
                }).ToList();
            }

            return new List<AuditVM>();  
        }
        public static List<AuditDetailVM> Map(IList<AuditDetailDTO> auditDetails)
        {
            List<AuditDetailVM> auditDetailVMs = auditDetails.Select(ad =>
            {
                AuditDetailVM auditDetailVM = new AuditDetailVM
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
                return auditDetailVM;
            }).ToList();
            return auditDetailVMs;
        }
    }
}