using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.UserPreferences;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionPathMapper
    {
        public static List<TransactionPathVM> Map(IList<TransactionPathDTO> transactionPathDTOs)
        {
            if (transactionPathDTOs == null || !transactionPathDTOs.Any())
            {
                return new List<TransactionPathVM>();
            }

            List<TransactionPathVM> transactionPathVMs = transactionPathDTOs
                .Select(pdto => new TransactionPathVM()
                {
                    Id = pdto.Id,
                    Name = pdto.Name,
                    TransactionTypeId = pdto.TransactionTypeId,
                    TransactionTypeName = pdto.TransactionTypeName,
                    OrgUnitId = pdto.OrgUnitId,
                    OrgUnitName = pdto.OrgUnitName,
                    UserId = pdto.UserId,
                    CreatedByName = pdto.UserName,
                    TransactionPathDetails = Map(pdto.TransactionPathDetails)
                }).ToList();

            return transactionPathVMs;
        }

        public static List<TransactionPathDTO> Map(IList<TransactionPathVM> transactionPathVMs)
        {
            if (transactionPathVMs == null || !transactionPathVMs.Any())
            {
                return new List<TransactionPathDTO>();
            }

            List<TransactionPathDTO> transactionPathDTOs = new List<TransactionPathDTO>();
            foreach (var item in transactionPathVMs)
            {
                transactionPathDTOs.Add(Map(item));
            }

            return transactionPathDTOs;
        }

        public static TransactionPathDTO Map(TransactionPathVM transactionPathVM)
        {
            if (transactionPathVM == null)
            {
                return new TransactionPathDTO();
            }

            TransactionPathDTO transactionPathDTO = new TransactionPathDTO
            {
                Id = transactionPathVM.Id,
                Name = transactionPathVM.Name,
                OrgUnitId = transactionPathVM.OrgUnitId,
                TransactionTypeId = transactionPathVM.TransactionTypeId,
                UserId = transactionPathVM.UserId,
                TransactionPathDetails = Map(transactionPathVM.TransactionPathDetailsGrid)
            };

            return transactionPathDTO;
        }

        public static TransactionPathVM Map(TransactionPathDTO transactionPathDTO)
        {
            if (transactionPathDTO == null)
            {
                return new TransactionPathVM();
            }

            TransactionPathVM transactionPathVM = new TransactionPathVM()
            {
                Id = transactionPathDTO.Id,
                Name = transactionPathDTO.Name,
                TransactionTypeId = transactionPathDTO.TransactionTypeId,
                TransactionTypeName = transactionPathDTO.TransactionTypeName,
                OrgUnitId = transactionPathDTO.OrgUnitId,
                OrgUnitName = transactionPathDTO.OrgUnitName,
                UserId = transactionPathDTO.UserId,
                TransactionPathDetails = Map(transactionPathDTO.TransactionPathDetails, transactionPathDTO.IsReadOnly),
                IsReadOnly = transactionPathDTO.IsReadOnly,
                TransactionPathDetailsVM = new TransactionPathDetailsVM()
            };

            return transactionPathVM;
        }

        public static List<TransactionPathDetailsVM> Map(IList<TransactionPathDetailsDTO> transactionPathDetailsDTOs, bool isReadOnly = false)
        {
            if (transactionPathDetailsDTOs == null || !transactionPathDetailsDTOs.Any())
            {
                return new List<TransactionPathDetailsVM>();
            }

            List<TransactionPathDetailsVM> transactionPathDetailsVMs = transactionPathDetailsDTOs
                .Select(pdto => new TransactionPathDetailsVM()
                {
                    Id = pdto.Id,
                    ActionId = pdto.ActionId,
                    ActionName = pdto.ActionName,
                    EntityId = pdto.OrgUnitId,
                    EntityName = pdto.OrgUnitName,
                    UserId = pdto.UserId,
                    UserName = pdto.UserName,
                    Sort = pdto.Sort,
                    IsReadOnly = isReadOnly
                }).ToList();

            return transactionPathDetailsVMs;
        }

        public static List<TransactionPathDetailsDTO> Map(IList<TransactionPathDetailsVM> transactionPathDetailsVMs)
        {
            if (transactionPathDetailsVMs == null || !transactionPathDetailsVMs.Any())
            {
                return new List<TransactionPathDetailsDTO>();
            }

            List<TransactionPathDetailsDTO> transactionPathDetailsDTOs = transactionPathDetailsVMs
                .Select(pdto => new TransactionPathDetailsDTO()
                {
                    Id = pdto.Id,
                    ActionId = pdto.ActionId,
                    OrgUnitId = pdto.EntityId,
                    UserId = pdto.UserId,
                    Sort = pdto.Sort,
                }).ToList();

            return transactionPathDetailsDTOs;
        }

    }
}