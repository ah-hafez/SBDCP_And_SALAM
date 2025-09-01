using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionBarcodesMapper
    {
        public static TransactionBarcodesDTO Map(TransactionBarcodesVM transactionBarcodeVMs)
        {
            if (transactionBarcodeVMs != null)
            {
                return new TransactionBarcodesDTO()
                {
                    TransactionBarcodeHtmlDesign = transactionBarcodeVMs.TransactionBarcodeHtmlDesign,
                    TransactionDate = transactionBarcodeVMs.Date,
                    TransactionDateH = transactionBarcodeVMs.DateH,
                    TransactionNumber = transactionBarcodeVMs.TransactionNumber,
                    Date = transactionBarcodeVMs.Date,
                    DateH = transactionBarcodeVMs.DateH,
                    CompanyName = transactionBarcodeVMs.CompanyName,
                    VisitTicketHtmlDesign = transactionBarcodeVMs.VisitTicketHtmlDesign,
                    TransactionDesignWidth = transactionBarcodeVMs.TransactionDesignWidth,
                    TransactionDesignHeight = transactionBarcodeVMs.TransactionDesignHeight,
                    TicketDesignWidth = transactionBarcodeVMs.TicketDesignWidth,
                    TicketDesignHeight = transactionBarcodeVMs.TicketDesignHeight,
                    TransactionType = transactionBarcodeVMs.TransactionType,
                    TransactionCategory = transactionBarcodeVMs.TransactionCategory,
                    BarcodeDTOs = BarcodeMapper.Map(transactionBarcodeVMs.BarcodeVMs),
                    AttachmentBarcodes = AttachmentBarcodeMapper.Map(transactionBarcodeVMs.AttachmentBarcodes),
                    TransactionAttachmentHtml = transactionBarcodeVMs.TransactionAttachmentHtml,
                    TicketBarcodeDTO = BarcodeMapper.Map(transactionBarcodeVMs.TicketBarcodeVM),
                    Entity = transactionBarcodeVMs.Entity
                };
            }
            return new TransactionBarcodesDTO();


        }
        public static TransactionBarcodesVM Map(TransactionBarcodesDTO transactionBarcodeDTOs)
        {
            if (transactionBarcodeDTOs != null)
            {
                return new TransactionBarcodesVM()
                {
                    TransactionBarcodeHtmlDesign = transactionBarcodeDTOs.TransactionBarcodeHtmlDesign,
                    TransactionDate = transactionBarcodeDTOs.Date,
                    TransactionDateH = transactionBarcodeDTOs.DateH,
                    TransactionNumber = transactionBarcodeDTOs.TransactionNumber,
                    Date = transactionBarcodeDTOs.Date,
                    DateH = transactionBarcodeDTOs.DateH,
                    CompanyName = transactionBarcodeDTOs.CompanyName,
                    VisitTicketHtmlDesign = transactionBarcodeDTOs.VisitTicketHtmlDesign,
                    TransactionDesignWidth = transactionBarcodeDTOs.TransactionDesignWidth,
                    TransactionDesignHeight = transactionBarcodeDTOs.TransactionDesignHeight,
                    TicketDesignWidth = transactionBarcodeDTOs.TicketDesignWidth,
                    TicketDesignHeight = transactionBarcodeDTOs.TicketDesignHeight,
                    TransactionType = transactionBarcodeDTOs.TransactionType,
                    TransactionCategory = transactionBarcodeDTOs.TransactionCategory,
                    BarcodeVMs = BarcodeMapper.Map(transactionBarcodeDTOs.CustomBarcodeDTOs),
                    AttachmentBarcodes = AttachmentBarcodeMapper.Map(transactionBarcodeDTOs.AttachmentBarcodes),
                    TransactionAttachmentHtml = transactionBarcodeDTOs.TransactionAttachmentHtml,
                    TicketBarcodeVM = BarcodeMapper.Map(transactionBarcodeDTOs.TicketBarcodeDTO),
                    Entity = transactionBarcodeDTOs.Entity,
                    OutboundDestination = transactionBarcodeDTOs.OutboundDestination,
                    OrgUnitSymbol = transactionBarcodeDTOs.OrgUnitSymbol
                };
            }
            return new TransactionBarcodesVM();


        }


    }
}