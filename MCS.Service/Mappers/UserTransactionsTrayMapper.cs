using System.Collections.Generic;
using System.Linq;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class UserTransactionsTrayMapper
    {
        public static UserTransactionsTrayDTO Map(Transaction transaction, string cultureName)
        {
            if (transaction == null )
            {
                return null;
            }
            UserTransactionsTrayDTO userTransactionsTrayDTO = new UserTransactionsTrayDTO()
            {
                Id = transaction.Id,
                Number = transaction.Number,
                Date = transaction.Date,
                DateH = transaction.DateH,
                DocumentNumber = transaction.DocumentNumber,
                PriorityLevel = (transaction.Priority != null) ? PriorityMapper.MapPriority(transaction.Priority, cultureName) : null,
                ToUser = (transaction.ToUser != null) ? UserProfileMapper.MapUserProfile(transaction.ToUser) : null,
                ToEntity = (transaction.Entity != null) ? OrgUnitMapper.Map(transaction.Entity) : null,
                RemindDateH = transaction.RemindDateH,
                RemindDate = transaction.RemindDate,
                TransactionCategoryId = transaction.TransactionCategoryId
            };

            if (transaction.Status != null)
            {
                userTransactionsTrayDTO.StatusId = transaction.Status.Id;
            }

            if (transaction.Confidentiality != null)
            {
                userTransactionsTrayDTO.ConfedentialityId = transaction.Confidentiality.Id;
            }

            return userTransactionsTrayDTO;
        }

        public static List<UserTransactionsTrayDTO> Map(IList<Transaction> transactions, string cultureName)
        {
            if (transactions == null || !transactions.Any())
            {
                return null;
            }
            List<UserTransactionsTrayDTO> userTransactionsTrayDTOs = transactions
                .Select(transaction => new UserTransactionsTrayDTO()
                {
                    Id = transaction.Id,
                    Number = transaction.Number,
                    Date = transaction.Date,
                    DateH = transaction.DateH,
                    DocumentNumber = transaction.DocumentNumber,
                    PriorityLevel = (transaction.Priority != null) ? PriorityMapper.MapPriority(transaction.Priority, cultureName) : null,
                    ToUser = (transaction.ToUser != null) ? UserProfileMapper.MapUserProfile(transaction.ToUser) : null,
                    ToEntity = (transaction.Entity != null) ? OrgUnitMapper.Map(transaction.Entity) : null,
                    RemindDateH = transaction.RemindDateH,
                    RemindDate = transaction.RemindDate,
                    TransactionCategoryId = transaction.TransactionCategoryId,
        }).ToList();

            return userTransactionsTrayDTOs;
        }

        public static List<Transaction> Map(IList<UserTransactionsTrayDTO> userTransactionsTrayDTOs)
        {
            if (userTransactionsTrayDTOs == null || !userTransactionsTrayDTOs.Any())
            {
                return null;
            }
            List<Transaction> transactions = userTransactionsTrayDTOs
               .Select(userTransactionsTrayDTO => new Transaction
               {
                   Id = userTransactionsTrayDTO.Id,
                   Number = userTransactionsTrayDTO.Number,
                   Date = userTransactionsTrayDTO.Date,
                   DateH = userTransactionsTrayDTO.DateH,
                   DocumentNumber = userTransactionsTrayDTO.DocumentNumber,
                   RemindDateH = userTransactionsTrayDTO.RemindDateH,
                   RemindDate = userTransactionsTrayDTO.RemindDate,
                   TransactionCategoryId = userTransactionsTrayDTO.TransactionCategoryId,
               }).ToList();
            return transactions;
        }
}
}