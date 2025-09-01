using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction.Inbound;

namespace MCS.UI.Areas.User.Mappers.Transaction.Inbound
{
    public static class OpenInboundMapper
    {
        public static List<OpenInboundVM> Map(IList<OpenInboundDTO> openInboundDTOs)
        {
            if (openInboundDTOs == null || !openInboundDTOs.Any())
            {
                return new List<OpenInboundVM>();
            }
            List<OpenInboundVM> openInboundVMs = openInboundDTOs
                .Select(openInboundDTO => new OpenInboundVM()
                { 
                    InboundNumber = openInboundDTO.InboundNumber,
                    SourceId = openInboundDTO.SourceId,
                    Year = openInboundDTO.Year
                }).ToList();

            return openInboundVMs;
        }
        public static List<OpenInboundDTO> Map(IList<OpenInboundVM> openInboundVMs)
        {
            if (openInboundVMs == null || !openInboundVMs.Any())
            {
                return new List<OpenInboundDTO>();
            }
            List<OpenInboundDTO> openInboundDTOs = openInboundVMs
                .Select(openInboundVM => new OpenInboundDTO()
                {
                    InboundNumber = openInboundVM.InboundNumber,
                    SourceId = openInboundVM.SourceId,
                    Year = openInboundVM.Year
                }).ToList();

            return openInboundDTOs;
        }

    }
}