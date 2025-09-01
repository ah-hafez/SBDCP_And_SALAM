using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class OutboundAdvancedMapper
    {
        public static OutboundAdvancedVM Map(OutboundAdvancedDTO outboundAdvancedDTO)
        {
            if (outboundAdvancedDTO != null)
            {
                OutboundAdvancedVM outboundAdvancedVM = new OutboundAdvancedVM()
                { 
                    ConfidentialityId = outboundAdvancedDTO.ConfidentialityId,
                    CreatedDepartmentId = outboundAdvancedDTO.CreatedDepartmentId,
                    DestinationPartyId = outboundAdvancedDTO.DestinationPartyId,
                    DirectedToId = outboundAdvancedDTO.DirectedToId,
                    PriorityId = outboundAdvancedDTO.PriorityId,
                    StatusId = outboundAdvancedDTO.StatusId,
                    SubjectClassifications = outboundAdvancedDTO.SubjectClassifications

                };
                return outboundAdvancedVM;
            }
            return new OutboundAdvancedVM();
        }
        public static OutboundAdvancedDTO Map(OutboundAdvancedVM outboundAdvancedVM)
        {
            if (outboundAdvancedVM != null)
            {
                OutboundAdvancedDTO outboundAdvancedDTO = new OutboundAdvancedDTO()
                { 
                    ConfidentialityId = outboundAdvancedVM.ConfidentialityId,
                    CreatedDepartmentId = outboundAdvancedVM.CreatedDepartmentId,
                    DestinationPartyId = outboundAdvancedVM.DestinationPartyId,
                    DirectedToId = outboundAdvancedVM.DirectedToId,
                    PriorityId = outboundAdvancedVM.PriorityId,
                    StatusId = outboundAdvancedVM.StatusId,
                    SubjectClassifications = outboundAdvancedVM.SubjectClassifications

                };
                return outboundAdvancedDTO;
            }
            return new OutboundAdvancedDTO();
        }
    }
}