using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.DTO.AdminAudit;
using MCS.UI.Areas.Admin.Models.AdminAudit;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Assignment;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class AuditLogMapper
    {
        public static List<AuditLogVM> Map(IList<AuditLogDTO> auditLogDTOs)
        {
            if (auditLogDTOs == null || !auditLogDTOs.Any())
            {
                return new List<AuditLogVM>();
            }
            List<AuditLogVM> auditLogVMs = auditLogDTOs
                .Select(auditLogDTO => new AuditLogVM()
                {
                    AuditAction = auditLogDTO.AuditAction,
                    AuditDate = auditLogDTO.AuditDate,
                    AuditUser = auditLogDTO.AuditUser,
                    EntityType = auditLogDTO.EntityType,
                    GuidId = auditLogDTO.GuidId,
                    AuditData = auditLogDTO.AuditData,
                    User = auditLogDTO.User
                    //Action = auditLogDTO.Action,
                    //GroupName = LocalizationMapper.Map(assignmentGroupDTO.GroupName),
                    //GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.GroupDetails),
                    //LocalName = assignmentGroupDTO.LocalName
                }).ToList();

            return auditLogVMs;
        }


        public static List<AdminAuditGridVM> Map(AuditLogVM AuditLogVMs,List<Change> Changes)
        {
            if (Changes == null || !Changes.Any())
            {
                return new List<AdminAuditGridVM>();
            }
            List<AdminAuditGridVM> AdminAuditGridVMs = Changes
                .Select(Change => new AdminAuditGridVM()
                {
                    UserName = AuditLogVMs.AuditUser.ToString(),
                    Action = AuditLogVMs.AuditAction,
                    AuditDate = AuditLogVMs.AuditDate,
                    Table = AuditLogVMs.EntityType,
                    //NewValue = Change.NewValue,
                    //OriginalValue   = Change.OriginalValue

                    //Action = auditLogDTO.Action,
                    //GroupName = LocalizationMapper.Map(assignmentGroupDTO.GroupName),
                    //GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupDTO.GroupDetails),
                    //LocalName = assignmentGroupDTO.LocalName
                }).ToList();

            return AdminAuditGridVMs;
        }



        //public static List<AdminAuditGridVM> MapAuditData(IList<AuditLogVM> AuditLogVMs)
        //{
        //    if (AuditLogVMs == null || !AuditLogVMs.Any())
        //    {
        //        return new List<AdminAuditGridVM>();
        //    } 
        //    List<AdminAuditGridVM> AdminAuditGridVMs = AuditLogVMs
        //        .Select(AdminAuditGridVM => new AuditLogVM()
        //        {
        //            UserName = AdminAuditGridVM.User.LocalName,

        //            Id = assignmentGroupVM.Id,
        //            GroupName = LocalizationMapper.Map(assignmentGroupVM.GroupName),
        //            GroupDetails = AssignmentGroupDetailMapper.Map(assignmentGroupVM.GroupDetails),
        //            LocalName = assignmentGroupVM.LocalName
        //        }).ToList();

        //    return AdminAuditGridVMs;
        //} 
    }
}