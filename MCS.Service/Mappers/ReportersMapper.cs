using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class ReportersMapper
    {
        public static ReporterDTO Map(Reporter reporter, string CultureName)
        {
            if (reporter == null)
            {
                return null;
            }
            return new ReporterDTO
            {
                Id = reporter.Id,
                IsActive = reporter.IsActive,
                IsLocked = reporter.IsLocked,
                ToEntityId = reporter.ToEntityId,
                ToEntityName = reporter.OrgUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == CultureName).FirstOrDefault().Text,
                LockedBy = reporter.LockedBy,
                LocalName = reporter.Text,
                Names = LocalizationIdentifierMapper.Map(reporter.LocalizationIdentifier.Localizations),
            };
        }
        public static List<ReporterDTO> Map(List<Reporter> reporters, string CultureName)
        {
            if (reporters == null)
            {
                return null;
            }
            return reporters.Select(c => Map(c, CultureName)).ToList();
        }
        public static Reporter Map(ReporterDTO reporterDTO)
        {
            if (reporterDTO == null)
            {
                return null;
            }
            return new Reporter
            {
                Id = reporterDTO.Id,
                ToEntityId = reporterDTO.ToEntityId,
                LocalizationIdentifier = LocalizationIdentifierMapper.Map(reporterDTO.Names)
            };
        }
        public static List<Reporter> Map(List<ReporterDTO> reporterDTOs)
        {
            if (reporterDTOs == null)
            {
                return null;
            }
            return reporterDTOs.Select(c => Map(c)).ToList();
        }
    }
}