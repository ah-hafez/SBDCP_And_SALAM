using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class ExplanationMapper
    {
        public static List<ExplanationVM> Map(IList<ExplanationDTO> explanationDTOs)
        {
            if (explanationDTOs == null || !explanationDTOs.Any())
            {
                return new List<ExplanationVM>();
            }
            List<ExplanationVM> explanationVMs = explanationDTOs
                .Select(explanationDTO => new ExplanationVM()
                {
                    CanBeDeleted = explanationDTO.CanBeDeleted,
                    ConfidentialityId = explanationDTO.ConfidentialityId,
                    ConfidentialityName = explanationDTO.ConfidentialityName,
                    DocumentVM = DocumentMapper.Map(explanationDTO.DocumentDTO),
                    EditorType = explanationDTO.EditorType,
                    FromUser = explanationDTO.FromUser,
                    FromUserId = explanationDTO.FromUserId,
                    Id = explanationDTO.Id,
                    TransactionNumber=explanationDTO.TransactionNumber,
                    DocumentId = explanationDTO.DocumentId,
                    Date = explanationDTO.Date,
                    DateH = explanationDTO.DateH,
                    RowNumber = explanationDTO.RowNumber,
                    CanBeSigned = explanationDTO.CanBeSigned != null ? explanationDTO.CanBeSigned.Value : false,


                }).ToList();

            return explanationVMs;
        }
        public static ExplanationVM Map(ExplanationDTO explanationDTO)
        {
            if (explanationDTO != null)
            {
                ExplanationVM explanationVM = new ExplanationVM()
                {
                    CanBeDeleted = explanationDTO.CanBeDeleted,
                    ConfidentialityId = explanationDTO.ConfidentialityId,
                    ConfidentialityName = explanationDTO.ConfidentialityName,
                    DocumentVM = DocumentMapper.Map(explanationDTO.DocumentDTO),
                    EditorType = explanationDTO.EditorType,
                    FromUser = explanationDTO.FromUser,
                    FromUserId = explanationDTO.FromUserId,
                    Id = explanationDTO.Id,
                    Date = explanationDTO.Date,
                    DocumentId =explanationDTO.DocumentId,
                    CanBeSigned = explanationDTO.CanBeSigned != null ? explanationDTO.CanBeSigned.Value : false,

                };

                return explanationVM;
            }
            return new ExplanationVM();
        }
        public static ExplanationDTO Map(ExplanationVM explanationVM)
        {
            if (explanationVM != null)
            {
                ExplanationDTO explanationDTO = new ExplanationDTO()
                {
                    CanBeDeleted = explanationVM.CanBeDeleted,
                    ConfidentialityId = explanationVM.ConfidentialityId,
                    ConfidentialityName = explanationVM.ConfidentialityName,
                    DocumentDTO = DocumentMapper.Map(explanationVM.DocumentVM),
                    EditorType = explanationVM.EditorType,
                    FromUser = explanationVM.FromUser,
                    FromUserId = explanationVM.FromUserId,
                    Id = explanationVM.Id,
                    isCopies = explanationVM.isCopies,
                    DocumentId = explanationVM.DocumentId,
                    CanBeSigned = explanationVM.CanBeSigned,
                };

                return explanationDTO;
            }
            return new ExplanationDTO();
        }
        public static List<ExplanationDTO> Map(IList<ExplanationVM> explanationVMs)
        {
            if (explanationVMs == null || !explanationVMs.Any())
            {
                return new List<ExplanationDTO>();
            }
            List<ExplanationDTO> explanationDTOs = explanationVMs
                .Select(explanationVM => new ExplanationDTO()
                {
                    CanBeDeleted = explanationVM.CanBeDeleted,
                    ConfidentialityId = explanationVM.ConfidentialityId,
                    ConfidentialityName = explanationVM.ConfidentialityName,
                    DocumentDTO = DocumentMapper.Map(explanationVM.DocumentVM),
                    EditorType = explanationVM.EditorType,
                    FromUser = explanationVM.FromUser,
                    FromUserId = explanationVM.FromUserId,
                    Id = explanationVM.Id,
                    DocumentId=explanationVM.Id,
                    CanBeSigned = explanationVM.CanBeSigned,

                }).ToList();

            return explanationDTOs;
        }


    }
}