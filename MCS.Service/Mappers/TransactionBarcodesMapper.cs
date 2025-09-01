using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Business.Implementation;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class TransactionBarcodesMapper
    {
        public static TransactionBarcodesDTO Map(TransactionBarcodesInfo transactionBarcodes)
        {
            if (transactionBarcodes == null)
            {
                return null;
            }
            TransactionBarcodesDTO transactionBarcodesDTO = new TransactionBarcodesDTO()
            {
                TransactionBarcodeHtmlDesign = transactionBarcodes.TransactionBarcodeHtmlDesign,
                TransactionDate = transactionBarcodes.Date,
                TransactionDateH = transactionBarcodes.DateH,
                TransactionNumber = transactionBarcodes.TransactionNumber,
                Date = transactionBarcodes.Date,
                DateH = transactionBarcodes.DateH,
                CompanyName = transactionBarcodes.CompanyName,
                VisitTicketHtmlDesign = transactionBarcodes.VisitTicketHtmlDesign,
                BarcodeDTOs = TransactionBarcodesMapper.Map(transactionBarcodes.Barcodes),
                CustomBarcodeDTOs = TransactionBarcodesMapper.Map(transactionBarcodes.CustomBarcodes),
                AttachmentBarcodes = TransactionBarcodesMapper.Map(transactionBarcodes.AttachmentBarcodes),
                TicketBarcodeDTO = TransactionBarcodesMapper.Map(transactionBarcodes.TicketBarcode),
                TransactionDesignWidth = transactionBarcodes.TransactionDesignWidth,
                TransactionDesignHeight = transactionBarcodes.TransactionDesignHeight,
                TicketDesignWidth = transactionBarcodes.TicketDesignWidth,
                TicketDesignHeight = transactionBarcodes.TicketDesignHeight,
                TransactionType = transactionBarcodes.TransactionType,
                TransactionCategory = transactionBarcodes.TransactionCategory,
                TransactionAttachmentHtml = transactionBarcodes.TransactionAttachmentHtmlDesign,
                Entity = transactionBarcodes.Entity,
                OutboundDestination = transactionBarcodes.OutboundDestination,
                OrgUnitSymbol = transactionBarcodes.OrgUnitSymbol
            };

            return transactionBarcodesDTO;
        }

        public static BarcodeDTO Map(Barcode barcode)
        {
            if (barcode == null)
            {
                return null;
            }
            BarcodeDTO barcodeDTO = new BarcodeDTO
            {
                Value = barcode.Value,
                Type = ((barcode.ReferenceType != null) ? TransactionBarcodesMapper.Map((BarcodeReferenceType)barcode.ReferenceType.Id.LookupInternalID(LookupCategory.BarcodeReferenceType, string.Empty)) : BarcodePrintType.Attachment),
                ReferenceId = barcode.ReferenceId
            };

            return barcodeDTO;
        }

        public static List<BarcodeDTO> Map(IList<Barcode> barcodes)
        {
            if (barcodes == null || !barcodes.Any())
            {
                return null;
            }
            List<BarcodeDTO> barcodeDTOs = barcodes
                .Select(barcode => new BarcodeDTO
                {
                    Value = barcode.Value,
                    Type = ((barcode.ReferenceType != null) ? TransactionBarcodesMapper.Map((BarcodeReferenceType)barcode.ReferenceType.Id.LookupInternalID(LookupCategory.BarcodeReferenceType, string.Empty)) : BarcodePrintType.Attachment),
                    ReferenceId = barcode.ReferenceId
                }).ToList();
            return barcodeDTOs;
        }
        public static List<BarcodeDTO> Map(IList<BarcodeInfo> barcodes)
        {
            if (barcodes == null || !barcodes.Any())
            {
                return null;
            }
            List<BarcodeDTO> barcodeDTOs = barcodes
                .Select(barcode => new BarcodeDTO
                {
                    Value = barcode.Value,
                    Type = ((barcode.ReferenceType != null) ? TransactionBarcodesMapper.Map((BarcodeReferenceType)barcode.ReferenceType.Id.LookupInternalID(LookupCategory.BarcodeReferenceType, string.Empty)) : BarcodePrintType.Attachment),
                    ReferenceId = barcode.ReferenceId,
                    EntityName = barcode.EntityName
                }).ToList();
            return barcodeDTOs;
        }
        public static BarcodePrintType Map(BarcodeReferenceType barcodeReferenceType)
        {
            switch (barcodeReferenceType)
            {
                case BarcodeReferenceType.MainTransaction:
                    return BarcodePrintType.Transaction;
                case BarcodeReferenceType.Copy:
                    return BarcodePrintType.Copy;
                default:
                    return BarcodePrintType.Attachment;
            }
        }

        public static List<AttachmentBarcodeDTO> Map(IList<AttachmentBarcode> attachmentBarcodes)
        {
            if (attachmentBarcodes == null || !attachmentBarcodes.Any())
            {
                return null;
            }
            List<AttachmentBarcodeDTO> attachmentBarcodeDTOs = attachmentBarcodes
                .Select(attachmentBarcode => new AttachmentBarcodeDTO
                {
                    Count = attachmentBarcode.Count,
                    Name = attachmentBarcode.Name,
                    Id = attachmentBarcode.Id
                }).ToList();
            return attachmentBarcodeDTOs;
        }

        public static TransactionVisitTicketDTO Map(TransactionVisitTicketInfo transactionVisitTicket)
        {
            if (transactionVisitTicket == null)
            {
                return null;
            }
            TransactionVisitTicketDTO transactionVisitTicketDTO = new TransactionVisitTicketDTO()
            {
                TransactionDate = transactionVisitTicket.Date,
                TransactionDateH = transactionVisitTicket.DateH,
                TransactionNumber = transactionVisitTicket.TransactionNumber,
                Entity = transactionVisitTicket.Entity,
                Date = transactionVisitTicket.Date,
                DateH = transactionVisitTicket.DateH,
                CompanyName = transactionVisitTicket.CompanyName,
                VisitTicketHtmlDesign = transactionVisitTicket.VisitTicketHtmlDesign,
                TicketDesignWidth = transactionVisitTicket.TicketDesignWidth,
                TicketDesignHeight = transactionVisitTicket.TicketDesignHeight,
                barcodeDTO = TransactionBarcodesMapper.Map(transactionVisitTicket.barcode),
                InboundDestination = transactionVisitTicket.InboundDestination,
                InboundNumber = transactionVisitTicket.InboundNumber,
                ToEntityName = transactionVisitTicket.ToEntityName,
                Subject = transactionVisitTicket.Subject
            };

            return transactionVisitTicketDTO;
        }

    }
}