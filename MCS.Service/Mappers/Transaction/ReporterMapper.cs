using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class ReporterMapper
    {
        public static List<ReporterDTO> Map(IList<Reporter> reporters, string cultureName)
        {
            List<ReporterDTO> reporterDTOs = new List<ReporterDTO>();

            foreach (Reporter item in reporters)
            {
                reporterDTOs.Add(MapReporter(item, cultureName));
            }
            return reporterDTOs;
        }
        private static ReporterDTO MapReporter(Reporter reporter, string cultureName)
        {
            if (reporter == null)
                return null;

            ReporterDTO newReporter = new ReporterDTO()
            {
                Id = reporter.Id,
                ToEntityId = reporter.ToEntityId,
                LocalName = reporter.Text
            };
            return newReporter;
        }
        public static Reporter Map(ReporterDTO reporterDTO)
        {
            if (reporterDTO == null)
            {
                return null;
            }

            Reporter reporter = new Reporter()
            {
                ToEntityId = reporterDTO.ToEntityId,
                LocalizationIdentifier = reporterDTO.Names != null ? LocalizationIdentifierMapper.Map(reporterDTO.Names) : null,
            };

            return reporter;
        }
    }
}