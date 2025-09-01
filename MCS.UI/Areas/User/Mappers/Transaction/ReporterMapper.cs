using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.User.Models;

namespace MCS.UI.Areas.User.Mappers
{
    public static class ReporterMapper
    {
        public static List<ReporterVM> Map(List<ReporterDTO> reporterDTOs)
        {
            List<ReporterVM> reporterVMs = new List<ReporterVM>();

            foreach (var item in reporterDTOs)
            {
                var newReporterVM = new ReporterVM()
                {
                    Id = item.Id,
                    LocalName = item.LocalName
                };
                reporterVMs.Add(newReporterVM);
            }
            return reporterVMs;
        }
        public static ReporterDTO Map(ReporterVM reporterVM)
        {
            if (reporterVM != null)
            {
                return new ReporterDTO
                {
                    ToEntityId = reporterVM.ToEntityId,
                    Names = LocalizationMapper.Map(reporterVM.Names)
                };
            }
            return null;
        }
    }
}