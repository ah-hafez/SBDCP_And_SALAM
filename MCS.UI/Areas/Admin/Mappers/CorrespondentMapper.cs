using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models;

namespace MCS.UI.Areas.Admin.Mappers
{
    public class ReporterMapper
    {
        public static ReporterDTO Map(ReporterVM reporterVM)
        {
            if (reporterVM == null)
            {
                return null;
            }
            return new ReporterDTO
            {
                Id = reporterVM.Id,
                ToEntityId = reporterVM.OrgUnitId,
                ToEntityName = reporterVM.OrgUnitName,
                Names = LocalizationMapper.Map(reporterVM.Names)
            };
        }
        public static List<ReporterDTO> Map(List<ReporterVM> reporterVMs)
        {
            if (reporterVMs == null)
            {
                return null;
            }
            return reporterVMs.Select(c => Map(c)).ToList();
        }
        public static ReporterVM Map(ReporterDTO reporterDTO)
        {
            if (reporterDTO == null)
            {
                return null;
            }
            return new ReporterVM
            {
                Id = reporterDTO.Id,
                IsActive = reporterDTO.IsActive,
                IsLocked = reporterDTO.IsLocked,
                OrgUnitId = reporterDTO.ToEntityId,
                LockedBy = reporterDTO.LockedBy,
                Names = LocalizationMapper.Map(reporterDTO.Names),
                OrgUnitName = reporterDTO.ToEntityName
            };
        }
        public static List<ReporterVM> Map(List<ReporterDTO> reporterDTOs)
        {
            if (reporterDTOs == null)
            {
                return null;
            }
            return reporterDTOs.Select(c => Map(c)).ToList();
        }
    }
}