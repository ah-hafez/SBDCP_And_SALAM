using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ExplanationMapper
    {
        public static Explanation Map(ExplanationDTO explanationDTO)
        {
            if (explanationDTO == null)
                return null;
            Explanation explanation = new Explanation()
            {
                Id = explanationDTO.Id,
                ExplanationEditorType = (int)explanationDTO.EditorType,
                PermissionId = explanationDTO.ConfidentialityId,
                Document = DocumentMapper.Map(explanationDTO.DocumentDTO),
                FromUserId = explanationDTO.FromUserId,
                isCopies = explanationDTO.isCopies,
                CanBeSigned = explanationDTO.CanBeSigned,

            };

            return explanation;
        }

        public static ExplanationDTO Map(Explanation explanation)
        {
            if (explanation == null)
                return null;

            ExplanationDTO explanationDTO = new ExplanationDTO()
            {
                Id = explanation.Id,
                EditorType = (EditorType)explanation.ExplanationEditorType,
                FromUser = explanation.FromUser.LocalName,
                ConfidentialityName = explanation.Permission.LocalName,
                ConfidentialityId = explanation.Permission.Id,
                CanBeDeleted = explanation.CanBeDeleted,
                DocumentDTO = DocumentMapper.MapWithContent(explanation.Document),
                Date = explanation.Date,
                CanBeSigned = explanation.CanBeSigned,

            };

            return explanationDTO;
        }

        public static List<ExplanationDTO> Map(IList<Explanation> explanations)
        {
            if (explanations == null || !explanations.Any())
            {
                return null;
            }
            List<ExplanationDTO> explanationDTOs = explanations
                .Select(explanationDTO => new ExplanationDTO()
                {
                    Id = explanationDTO.Id,
                    EditorType = (EditorType)explanationDTO.ExplanationEditorType,
                    FromUserId = explanationDTO.FromUser.Id,
                    FromUser = explanationDTO.FromUser.LocalName,
                    ConfidentialityName = explanationDTO.Permission.LocalName,
                    ConfidentialityId = explanationDTO.Permission.Id,
                    CanBeDeleted = explanationDTO.CanBeDeleted,
                    DocumentDTO = DocumentMapper.MapWithContent(explanationDTO.Document),
                    TransactionNumber=explanationDTO.Transaction.Number,
                    DocumentId = explanationDTO.Document?.Document?.Id,
                    Date = explanationDTO.Date,
                    DateH = explanationDTO.DateH,
                    RowNumber = explanationDTO.RowNumber,
                    CanBeSigned = explanationDTO.CanBeSigned,
                    
                }).ToList();
            return explanationDTOs;
        }
    }
}