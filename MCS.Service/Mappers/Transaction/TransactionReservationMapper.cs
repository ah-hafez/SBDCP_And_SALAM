using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.Transaction;

namespace MCS.Service.Mappers
{
    public class TransactionReservationMapper
    {
        public static List<TransactionReservationDTO> Map(IList<TransactionReservation> transactionReservations, string cultureName)
        {
            if (transactionReservations == null || !transactionReservations.Any())
            {
                return new List<TransactionReservationDTO>();
            }

            List<TransactionReservationDTO> transactionReservationsDTOs = transactionReservations
                .Select(rdomain => new TransactionReservationDTO()
                {
                    Id = rdomain.Id,
                    EntityId = rdomain.EntityId,
                    UserId = rdomain.UserId,
                    Count = rdomain.Count,
                    Reason = rdomain.Reason,
                    EntityName = rdomain.Entity.LocalName,
                    UserName = rdomain.User.LocalName,
                    TransactionCategoryId = rdomain.TransactionCategoryId,
                    TransactionCategoryName = rdomain.TransactionCategory.Text,
                    DateTime = rdomain.CreatedOn,
                    Transactions = Map(rdomain.Transactions, cultureName)
                }).ToList();

            return transactionReservationsDTOs;
        }

        public static IList<TransactionReservation> Map(IList<TransactionReservationDTO> transactionReservationDTOs)
        {
            if (transactionReservationDTOs == null || !transactionReservationDTOs.Any())
            {
                return new List<TransactionReservation>();
            }

            List<TransactionReservation> list = transactionReservationDTOs
                .Select(rdto => new TransactionReservation()
                {
                    Id = rdto.Id,
                    EntityId = rdto.EntityId,
                    UserId = rdto.UserId,
                    Count = rdto.Count,
                    Reason = rdto.Reason,
                    TransactionCategoryId = rdto.TransactionCategoryId
                }).ToList();

            return list;
        }

        public static TransactionReservation Map(TransactionReservationDTO transactionReservationDTO)
        {
            if (transactionReservationDTO == null)
            {
                return new TransactionReservation();
            }

            TransactionReservation transactionReservation = new TransactionReservation()
            {
                Id = transactionReservationDTO.Id,
                EntityId = transactionReservationDTO.EntityId,
                UserId = transactionReservationDTO.UserId,
                Count = transactionReservationDTO.Count,
                Reason = transactionReservationDTO.Reason,
                TransactionCategoryId = transactionReservationDTO.TransactionCategoryId
            };

            return transactionReservation;
        }

        public static List<TransactionReservedDTO> Map(IList<Transaction> transactionReservations, string cultureName)
        {
            if (transactionReservations == null || !transactionReservations.Any())
            {
                return new List<TransactionReservedDTO>();
            }

            List<TransactionReservedDTO> transactionReservationsDTOs = transactionReservations
                .Select(rdomain => new TransactionReservedDTO()
                {
                    Id = rdomain.Id,
                    Number = rdomain.Number,
                    Year = rdomain.Year,
                    Type = rdomain.TransactionCategory.Localizations.FirstOrDefault(a => a.Culture.ShortName == cultureName).Text
                }).ToList();

            return transactionReservationsDTOs;
        }
    }
}