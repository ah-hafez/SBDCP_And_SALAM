using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.DTO.Transaction;
using MCS.UI.Areas.User.Models;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public class TransactionReservationMapper
    {
        public static List<TransactionReservationVM> Map(IList<TransactionReservationDTO> transactionReservationDTOs)
        {
            if (transactionReservationDTOs == null || !transactionReservationDTOs.Any())
            {
                return new List<TransactionReservationVM>();
            }

            List<TransactionReservationVM> transactionReservationVMs = transactionReservationDTOs
                .Select(rdto => new TransactionReservationVM()
                {
                    Id = rdto.Id,
                    EntityId = rdto.EntityId,
                    UserId = rdto.UserId,
                    UserName = rdto.UserName,
                    EntityName = rdto.EntityName,
                    Reason = rdto.Reason,
                    Count = rdto.Count,
                    TransactionCategoryId = rdto.TransactionCategoryId,
                    TransactionCategoryName = rdto.TransactionCategoryName,
                    DateTimeHJ = DateTimeUtility.ConvertToUmAlQuraCalendar(rdto.DateTime),
                }).ToList();

            return transactionReservationVMs;
        }

        public static List<TransactionReservationDTO> Map(IList<TransactionReservationVM> transactionReservationVMs)
        {
            if (transactionReservationVMs == null || !transactionReservationVMs.Any())
            {
                return new List<TransactionReservationDTO>();
            }

            List<TransactionReservationDTO> transactionReservationDTOs = new List<TransactionReservationDTO>();
            foreach (var item in transactionReservationVMs)
            {
                transactionReservationDTOs.Add(Map(item));
            }

            return transactionReservationDTOs;
        }

        public static TransactionReservationDTO Map(TransactionReservationVM transactionReservationVM)
        {
            if (transactionReservationVM == null)
            {
                return new TransactionReservationDTO();
            }

            TransactionReservationDTO transactionReservationDTO = new TransactionReservationDTO
            {
                Id = transactionReservationVM.Id,
                EntityId = transactionReservationVM.EntityId.Value,
                UserId = transactionReservationVM.UserId,
                UserName = transactionReservationVM.UserName,
                EntityName = transactionReservationVM.EntityName,
                Reason = transactionReservationVM.Reason,
                Count = transactionReservationVM.Count,
                TransactionCategoryId = transactionReservationVM.TransactionCategoryId
            };

            return transactionReservationDTO;
        }

        public static List<TransactionReservedVM> Map(IList<TransactionReservedDTO> transactionReservationDTOs)
        {
            if (transactionReservationDTOs == null || !transactionReservationDTOs.Any())
            {
                return new List<TransactionReservedVM>();
            }

            List<TransactionReservedVM> transactionReservationVMs = transactionReservationDTOs
                .Select(rdto => new TransactionReservedVM()
                {
                    Id = rdto.Id,
                    Number = rdto.Number,
                    Year = rdto.Year,
                    Type = rdto.Type
                }).ToList();

            return transactionReservationVMs;
        }

    }
}