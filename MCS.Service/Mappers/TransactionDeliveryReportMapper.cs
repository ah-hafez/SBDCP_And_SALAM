using System.Collections.Generic;
using System.Linq;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;


namespace MCS.Service.Mappers
{
    public class TransactionDeliveryReportMapper
    {
        public static List<TransactionDeliveryReportDTO> Map(IList<TransactionDeliveryReport> transactionDeliveryReports, string culture)
        {
            if (transactionDeliveryReports == null || !transactionDeliveryReports.Any())
            {
                return new List<TransactionDeliveryReportDTO>();
            }

            List<TransactionDeliveryReportDTO> transactionDeliveryReportDTOs = new List<TransactionDeliveryReportDTO>();

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                TransactionDeliveryReportDTO transactionDeliveryReportDTO = new TransactionDeliveryReportDTO();

                
               
                    transactionDeliveryReportDTO.Confidentiality = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.Confidentiality?.LocalName : string.Empty;
                    transactionDeliveryReportDTO.Date = transactionDeliveryReport.Date;
                    transactionDeliveryReportDTO.DateH = transactionDeliveryReport.DateH;
                    transactionDeliveryReportDTO.DeliveryMethod = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.DeliveryMethod?.Text : string.Empty;
                    transactionDeliveryReportDTO.Id = transactionDeliveryReport.Id;
                    transactionDeliveryReportDTO.Priority = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.Priority?.Text : string.Empty;
                    transactionDeliveryReportDTO.TransactionTypeName = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.TransactionType?.Text : string.Empty;
                    transactionDeliveryReportDTO.ToEntity = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToEntity.LocalName : string.Empty : string.Empty;
                    transactionDeliveryReportDTO.TransactionId = transactionDeliveryReport.TransactionId;
                    transactionDeliveryReportDTO.User = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.User?.LocalName : string.Empty;
                    //transactionDeliveryReportDTO.User = transactionDeliveryReport.TransactionAssignmentHistory != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToUser != null ? transactionDeliveryReport.TransactionAssignmentHistory.ToUser.LocalName : string.Empty : string.Empty;
                    transactionDeliveryReportDTO.Number = transactionDeliveryReport.Number;
                    transactionDeliveryReportDTO.Subject = transactionDeliveryReport.TransactionHistory != null ? transactionDeliveryReport.TransactionHistory.Subject : string.Empty;
                    transactionDeliveryReportDTO.TransactionCategoryName = transactionDeliveryReport.TransactionHistory != null ? (transactionDeliveryReport.TransactionHistory.TransactionCategory != null ? transactionDeliveryReport.TransactionHistory.TransactionCategory.Text : string.Empty) : string.Empty;
                    transactionDeliveryReportDTO.TransactionNumber = transactionDeliveryReport.Transaction.Number.ToString();
                    transactionDeliveryReportDTO.TransactionCategoryId = transactionDeliveryReport.TransactionHistory != null ? (transactionDeliveryReport.TransactionHistory.Transaction != null ? transactionDeliveryReport.TransactionHistory.Transaction.TransactionCategoryId : 0) : 0;
                    transactionDeliveryReportDTO.Document = DocumentMapper.Map(transactionDeliveryReport.Document);
                    transactionDeliveryReportDTO.IsForIndividual = transactionDeliveryReport.Transaction.IsForIndividual;
                    transactionDeliveryReportDTO.ExternalPartyName = transactionDeliveryReport.TransactionExternalCopyId.HasValue ? (transactionDeliveryReport.TransactionExternalCopy != null ? transactionDeliveryReport.TransactionExternalCopy.Entity != null ? transactionDeliveryReport.TransactionExternalCopy.Entity.Name.Localizations.Where(l => l.Culture.ShortName == culture).FirstOrDefault().Text : string.Empty : string.Empty) : (transactionDeliveryReport.TransactionHistory.Transaction?.ExternalParty != null) ? transactionDeliveryReport.TransactionHistory.Transaction?.ExternalParty.LocalName : null;
                    transactionDeliveryReportDTO.ExternalPartyId = transactionDeliveryReport.TransactionExternalCopyId.HasValue ? (transactionDeliveryReport.TransactionExternalCopy != null ? transactionDeliveryReport.TransactionExternalCopy.Entity != null ? transactionDeliveryReport.TransactionExternalCopy.Entity.Id : -1 : -1) : (transactionDeliveryReport.TransactionHistory.Transaction?.ExternalParty != null) ? transactionDeliveryReport.TransactionHistory.Transaction.ExternalParty.Id : -1;


                    transactionDeliveryReportDTO.InternalPartyName = transactionDeliveryReport.TransactionCopyId.HasValue  && transactionDeliveryReport.TransactionCopy != null && transactionDeliveryReport.TransactionCopy.Entity != null ? transactionDeliveryReport.TransactionCopy.Entity.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == culture).FirstOrDefault().Text : string.Empty  ;
                     
                      transactionDeliveryReportDTO.InternalPartyId = transactionDeliveryReport.TransactionCopyId.HasValue ? (transactionDeliveryReport.TransactionCopy != null ? transactionDeliveryReport.TransactionCopy.Entity != null ? transactionDeliveryReport.TransactionCopy.Entity.Id : -1 : -1) :  -1;
                     
                    if (transactionDeliveryReport.TransactionExternalCopyId.HasValue || transactionDeliveryReport.TransactionCopyId.HasValue)
                        transactionDeliveryReportDTO.IsCopy = true;
                    else
                        transactionDeliveryReportDTO.IsCopy = false;

                    transactionDeliveryReportDTO.ToEntityId = transactionDeliveryReport.TransactionAssignmentHistory.ToEntityId;
                    transactionDeliveryReportDTOs.Add(transactionDeliveryReportDTO);
                 
            }

            return transactionDeliveryReportDTOs;
        }

        public static List<TransactionDeliveryReportDTO> MapLight(IList<TransactionDeliveryReport> transactionDeliveryReports)
        {
            if (transactionDeliveryReports == null || !transactionDeliveryReports.Any())
            {
                return new List<TransactionDeliveryReportDTO>();
            }
            List<TransactionDeliveryReportDTO> transactionDeliveryReportDTOs = new List<TransactionDeliveryReportDTO>();

            foreach (TransactionDeliveryReport transactionDeliveryReport in transactionDeliveryReports)
            {
                TransactionDeliveryReportDTO transactionDeliveryReportDTO = new TransactionDeliveryReportDTO
                {
                    Id = transactionDeliveryReport.Id,
                    TransactionId = transactionDeliveryReport.TransactionId
                };
                transactionDeliveryReportDTOs.Add(transactionDeliveryReportDTO);
            }

            return transactionDeliveryReportDTOs;
        }
    }
}