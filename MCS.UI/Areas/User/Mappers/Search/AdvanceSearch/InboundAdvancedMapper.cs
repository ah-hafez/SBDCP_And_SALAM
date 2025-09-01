using YESSER.NCS.MCS.DTO;
using YESSER.NCS.MCS.UI.Areas.User.Models.Search;

namespace YESSER.NCS.MCS.UI.Areas.User.Mappers.Search
{
    public static class InboundAdvancedMapper
    {
        public static InboundAdvancedVM Map(InboundAdvancedDTO inboundAdvancedDTO)
        {
            if (inboundAdvancedDTO != null)
            {
                InboundAdvancedVM inboundAdvancedVM = new InboundAdvancedVM()
                { 
                    ConfidentialityId = inboundAdvancedDTO.ConfidentialityId,
                    FromPartyId = inboundAdvancedDTO.FromPartyId,
                    LetterTypeId = inboundAdvancedDTO.LetterTypeId,
                    PriorityId = inboundAdvancedDTO.PriorityId,
                    SignedByDepartmentId = inboundAdvancedDTO.SignedByDepartmentId,
                    SignedById = inboundAdvancedDTO.SignedById,
                    StatusId = inboundAdvancedDTO.StatusId,
                    SubjectClassifications = inboundAdvancedDTO.SubjectClassifications
                };
                return inboundAdvancedVM;
            }
            return new InboundAdvancedVM();
        }
        public static InboundAdvancedDTO Map(InboundAdvancedVM inboundAdvancedVM)
        {
            if (inboundAdvancedVM != null)
            {
                InboundAdvancedDTO inboundAdvancedDTO = new InboundAdvancedDTO()
                {
                    ConfidentialityId = inboundAdvancedVM.ConfidentialityId,
                    FromPartyId = inboundAdvancedVM.FromPartyId,
                    LetterTypeId = inboundAdvancedVM.LetterTypeId,
                    PriorityId = inboundAdvancedVM.PriorityId,
                    SignedByDepartmentId = inboundAdvancedVM.SignedByDepartmentId,
                    SignedById = inboundAdvancedVM.SignedById,
                    StatusId = inboundAdvancedVM.StatusId,
                    SubjectClassifications = inboundAdvancedVM.SubjectClassifications
                };
                return inboundAdvancedDTO;
            }
            return new InboundAdvancedDTO();
        }
    }
}
