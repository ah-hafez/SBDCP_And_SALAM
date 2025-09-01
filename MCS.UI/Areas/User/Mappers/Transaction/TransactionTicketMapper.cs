using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionTicketMapper
    {
        public static List<TransactionTicketVM> Map(IList<TransactionTicketDTO> transactionTicketDTOs)
        {
            if (transactionTicketDTOs == null || !transactionTicketDTOs.Any())
            {
                return new List<TransactionTicketVM>();
            }
            List<TransactionTicketVM> transactionTicketVMs = transactionTicketDTOs
                .Select(transactionTicketDTO => new TransactionTicketVM()
                { 
                    BarcodeValue = transactionTicketDTO.BarcodeValue,
                    Date = transactionTicketDTO.Date,
                    Number = transactionTicketDTO.Number,
                    SequenceNumber = transactionTicketDTO.SequenceNumber
                }).ToList();

            return transactionTicketVMs;
        }
        public static List<TransactionTicketDTO> Map(IList<TransactionTicketVM> transactionTicketVMs)
        {
            if (transactionTicketVMs == null || !transactionTicketVMs.Any())
            {
                return new List<TransactionTicketDTO>();
            }
            List<TransactionTicketDTO> transactionTicketDTOs = transactionTicketVMs
                .Select(transactionTicketVM => new TransactionTicketDTO()
                {
                    BarcodeValue = transactionTicketVM.BarcodeValue,
                    Date = transactionTicketVM.Date,
                    Number = transactionTicketVM.Number,
                    SequenceNumber = transactionTicketVM.SequenceNumber
                }).ToList();

            return transactionTicketDTOs;
        }


    }
}