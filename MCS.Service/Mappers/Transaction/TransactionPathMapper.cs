using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class TransactionPathMapper
    {
        public static List<TransactionPathDTO> Map(IList<TransactionPath> transactionPaths)
        {
            if (transactionPaths == null || !transactionPaths.Any())
            {
                return new List<TransactionPathDTO>();
            }

            List<TransactionPathDTO> transactionPathsDTOs = transactionPaths
                .Select(pdomain => new TransactionPathDTO()
                {
                    Id = pdomain.Id,
                    OrgUnitId = pdomain.OrgUnitId,
                    TransactionTypeId = pdomain.TransactionTypeId,
                    UserId = pdomain.UserId,
                    Name = pdomain.Name,
                    OrgUnitName = pdomain.OrgUnit != null ? pdomain.OrgUnit.LocalName : string.Empty,
                    TransactionTypeName = pdomain.TransactionType != null ? pdomain.TransactionType.Text : string.Empty,
                    UserName = pdomain.User != null ? pdomain.User.LocalName : string.Empty,
                    TransactionPathDetails = Map(pdomain.TransactionPathDetails)
                }).ToList();

            return transactionPathsDTOs;
        }

        public static IList<TransactionPath> Map(IList<TransactionPathDTO> transactionPathDTOs)
        {
            if (transactionPathDTOs == null || !transactionPathDTOs.Any())
            {
                return new List<TransactionPath>();
            }

            List<TransactionPath> list = transactionPathDTOs
                .Select(pdto => new TransactionPath()
                {
                    Id = pdto.Id,
                    Name = pdto.Name,
                    OrgUnitId = pdto.OrgUnitId,
                    TransactionTypeId = pdto.TransactionTypeId,
                    UserId = pdto.UserId,
                    TransactionPathDetails = Map(pdto.TransactionPathDetails)
                }).ToList();

            return list;
        }

        public static TransactionPathDTO Map(TransactionPath transactionPath)
        {
            if (transactionPath == null)
            {
                return new TransactionPathDTO();
            }

            TransactionPathDTO transactionPathsDTO = new TransactionPathDTO()
            {
                Id = transactionPath.Id,
                OrgUnitId = transactionPath.OrgUnitId,
                TransactionTypeId = transactionPath.TransactionTypeId,
                UserId = transactionPath.UserId,
                Name = transactionPath.Name,
                OrgUnitName = transactionPath.OrgUnit.LocalName,
                TransactionTypeName = transactionPath.TransactionType.Text,
                UserName = transactionPath.User != null ? transactionPath.User.LocalName : null,
                IsReadOnly = transactionPath.IsReadOnly,
                TransactionPathDetails = Map(transactionPath.TransactionPathDetails, transactionPath.IsReadOnly)
            };

            return transactionPathsDTO;
        }

        public static TransactionPath Map(TransactionPathDTO transactionPathDTO)
        {
            if (transactionPathDTO == null)
            {
                return new TransactionPath();
            }

            TransactionPath transactionPath = new TransactionPath()
            {
                Id = transactionPathDTO.Id,
                Name = transactionPathDTO.Name,
                OrgUnitId = transactionPathDTO.OrgUnitId,
                TransactionTypeId = transactionPathDTO.TransactionTypeId,
                UserId = transactionPathDTO.UserId,
                TransactionPathDetails = Map(transactionPathDTO.TransactionPathDetails)
            };

            return transactionPath;
        }

        public static List<TransactionPathDetailsDTO> Map(IList<TransactionPathDetails> transactionPathDetails, bool isReadOnly = false)
        {
            if (transactionPathDetails == null || !transactionPathDetails.Any())
            {
                return new List<TransactionPathDetailsDTO>();
            }

            List<TransactionPathDetailsDTO> transactionPathDetailsDTOs = transactionPathDetails
                .Select(pdomain => new TransactionPathDetailsDTO()
                {
                    Id = pdomain.Id,
                    ActionId = pdomain.ActionId,
                    OrgUnitId = pdomain.OrgUnitId,
                    TransactionPathId = pdomain.TransactionPathId,
                    UserId = pdomain.UserId,
                    ActionName = pdomain.Action.LocalName,
                    OrgUnitName = pdomain.OrgUnit.LocalName,
                    UserName = pdomain.User != null ? pdomain.User.LocalName : null,
                    Sort = pdomain.Sort,
                    IsReadOnly = isReadOnly
                }).ToList();

            return transactionPathDetailsDTOs;
        }

        public static List<TransactionPathDetails> Map(IList<TransactionPathDetailsDTO> transactionPathDetailsDTOs)
        {
            if (transactionPathDetailsDTOs == null || !transactionPathDetailsDTOs.Any())
            {
                return new List<TransactionPathDetails>();
            }

            List<TransactionPathDetails> transactionPathDetails = transactionPathDetailsDTOs
                .Select(pdomain => new TransactionPathDetails()
                {
                    Id = pdomain.Id,
                    ActionId = pdomain.ActionId,
                    OrgUnitId = pdomain.OrgUnitId,
                    TransactionPathId = pdomain.TransactionPathId,
                    UserId = pdomain.UserId,
                    Sort = pdomain.Sort,
                }).ToList();

            return transactionPathDetails;
        }

        public static TransactionPathDetailsDTO Map(TransactionPathDetails transactionPathDetails)
        {
            if (transactionPathDetails == null)
            {
                return new TransactionPathDetailsDTO();
            }

            TransactionPathDetailsDTO transactionPathDetailsDTOs = new TransactionPathDetailsDTO()
            {
                Id = transactionPathDetails.Id,
                ActionId = transactionPathDetails.ActionId,
                OrgUnitId = transactionPathDetails.OrgUnitId,
                TransactionPathId = transactionPathDetails.TransactionPathId,
                UserId = transactionPathDetails.UserId,
                ActionName = transactionPathDetails.Action.LocalName,
                OrgUnitName = transactionPathDetails.OrgUnit.LocalName,
                UserName = transactionPathDetails.User != null ? transactionPathDetails.User.LocalName : null,
                Sort = transactionPathDetails.Sort,
            };

            return transactionPathDetailsDTOs;
        }
    }
}