using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionVisitTicketMapper
    {
        public static List<TransactionVisitTicketVM> Map(IList<TransactionVisitTicketDTO> transactionVisitTicketDTOs)
        {
            if (transactionVisitTicketDTOs == null || !transactionVisitTicketDTOs.Any())
            {
                return new List<TransactionVisitTicketVM>();
            }
            List<TransactionVisitTicketVM> transactionVisitTicketVMs = transactionVisitTicketDTOs
                .Select(transactionVisitTicketDTO => new TransactionVisitTicketVM()
                {
                    barcodeVM = BarcodeMapper.Map(transactionVisitTicketDTO.barcodeDTO),
                    CompanyName = transactionVisitTicketDTO.CompanyName,
                    Date = transactionVisitTicketDTO.Date,
                    DateH = transactionVisitTicketDTO.DateH,
                    Entity = transactionVisitTicketDTO.Entity,
                    TicketDesignHeight = transactionVisitTicketDTO.TicketDesignHeight,
                    TicketDesignWidth = transactionVisitTicketDTO.TicketDesignWidth,
                    TransactionDate = transactionVisitTicketDTO.TransactionDate,
                    TransactionDateH = transactionVisitTicketDTO.TransactionDateH,
                    TransactionNumber = transactionVisitTicketDTO.TransactionNumber,
                    VisitTicketHtmlDesign = transactionVisitTicketDTO.VisitTicketHtmlDesign,
                    InboundDestination = transactionVisitTicketDTO.InboundDestination,
                    InboundNumber = transactionVisitTicketDTO.InboundNumber,
                    ToEntityName = transactionVisitTicketDTO.ToEntityName
                }).ToList();

            return transactionVisitTicketVMs;
        }
        public static List<TransactionVisitTicketDTO> Map(IList<TransactionVisitTicketVM> transactionVisitTicketVMs)
        {
            if (transactionVisitTicketVMs == null || !transactionVisitTicketVMs.Any())
            {
                return new List<TransactionVisitTicketDTO>();
            }
            List<TransactionVisitTicketDTO> transactionVisitTicketDTOs = transactionVisitTicketVMs
                .Select(transactionVisitTicketVM => new TransactionVisitTicketDTO()
                {
                    barcodeDTO = BarcodeMapper.Map(transactionVisitTicketVM.barcodeVM),
                    CompanyName = transactionVisitTicketVM.CompanyName,
                    Date = transactionVisitTicketVM.Date,
                    DateH = transactionVisitTicketVM.DateH,
                    Entity = transactionVisitTicketVM.Entity,
                    TicketDesignHeight = transactionVisitTicketVM.TicketDesignHeight,
                    TicketDesignWidth = transactionVisitTicketVM.TicketDesignWidth,
                    TransactionDate = transactionVisitTicketVM.TransactionDate,
                    TransactionDateH = transactionVisitTicketVM.TransactionDateH,
                    TransactionNumber = transactionVisitTicketVM.TransactionNumber,
                    VisitTicketHtmlDesign = transactionVisitTicketVM.VisitTicketHtmlDesign,
                    InboundDestination = transactionVisitTicketVM.InboundDestination,
                    InboundNumber = transactionVisitTicketVM.InboundNumber,
                    ToEntityName = transactionVisitTicketVM.ToEntityName
                }).ToList();

            return transactionVisitTicketDTOs;
        }
        public static TransactionVisitTicketVM Map(TransactionVisitTicketDTO transactionVisitTicketDTO)
        {
            if (transactionVisitTicketDTO != null)
            {
                return new TransactionVisitTicketVM()
                {
                    barcodeVM = BarcodeMapper.Map(transactionVisitTicketDTO.barcodeDTO),
                    CompanyName = transactionVisitTicketDTO.CompanyName,
                    Date = transactionVisitTicketDTO.Date,
                    DateH = transactionVisitTicketDTO.DateH,
                    Entity = transactionVisitTicketDTO.Entity,
                    TicketDesignHeight = transactionVisitTicketDTO.TicketDesignHeight,
                    TicketDesignWidth = transactionVisitTicketDTO.TicketDesignWidth,
                    TransactionDate = transactionVisitTicketDTO.TransactionDate,
                    TransactionDateH = transactionVisitTicketDTO.TransactionDateH,
                    TransactionNumber = transactionVisitTicketDTO.TransactionNumber,
                    VisitTicketHtmlDesign = transactionVisitTicketDTO.VisitTicketHtmlDesign,
                    InboundDestination = transactionVisitTicketDTO.InboundDestination,
                    InboundNumber = transactionVisitTicketDTO.InboundNumber,
                    ToEntityName = transactionVisitTicketDTO.ToEntityName,
                    Subject = transactionVisitTicketDTO.Subject
                };
            }
            return new TransactionVisitTicketVM();
        }
    }
}