using System.Collections.Generic;
using System.Linq;
using System.Web.SessionState;
using MCS.Common;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class TransactionMapper
    {
        public static Transaction Map(TransactionDTO transactionDTO)
        {
            if (transactionDTO != null)
            {
                switch (transactionDTO.TransactionCategory)
                {
                    case TransactionCategory.Inbound:
                        return InboundTransactionMapper.Map(transactionDTO);

                    case TransactionCategory.ExternalOutbound:
                        return OutboundExternalTransactionMapper.Map(transactionDTO);

                    case TransactionCategory.DraftOutbound:
                        return OutboundDraftTransactionMapper.Map(transactionDTO);

                    case TransactionCategory.InternalOutbound:
                        return OutboundInternalTransactionMapper.Map(transactionDTO);
                }
            }

            return null;
        }

        public static TransactionDTO Map(Transaction transaction)
        {
            if (transaction != null)
            {
                switch ((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty))
                {
                    case TransactionCategory.Inbound:
                        return InboundTransactionMapper.Map(transaction);
                    case TransactionCategory.ExternalOutbound:
                        return OutboundExternalTransactionMapper.Map(transaction);
                    case TransactionCategory.DraftOutbound:
                        return OutboundDraftTransactionMapper.Map(transaction);
                    case TransactionCategory.InternalOutbound:
                        return OutboundInternalTransactionMapper.Map(transaction);
                    default: 
                        return null;
                }
            }

            return null;
        }
        public static VipBasicTransactionInfoDto MapBasic_Vip(Transaction transaction)
        {
            VipBasicTransactionInfoDto transactionDTO = new VipBasicTransactionInfoDto();
            if (transaction != null)
            {
                transactionDTO.Id = transaction.Id;
                transactionDTO.TransactionCategory = (TransactionCategory)transaction.TransactionCategoryId;
                return transactionDTO;
            }

            return null;
        }


        public static TransactionDTO Map_VIP(Transaction transaction)
        {
            if (transaction != null)
            {
                switch ((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty))
                {
                    case TransactionCategory.Inbound:
                        return InboundTransactionMapper.Map_VIP(transaction);
                    case TransactionCategory.ExternalOutbound:
                        return OutboundExternalTransactionMapper.Map(transaction);
                    case TransactionCategory.DraftOutbound:
                        return OutboundDraftTransactionMapper.Map_VIP(transaction);
                    case TransactionCategory.InternalOutbound:
                        return OutboundInternalTransactionMapper.Map_VIP(transaction);
                }
            }

            return null;
        }
        public static TransactionDTO MapLight(Transaction transaction)
        {
            if (transaction == null)
            {
                return null;
            }
            return InboundTransactionMapper.MapLight(transaction);
        }
        public static TransactionDTO MapGetPrevious(Transaction transaction)
        {
            if (transaction != null)
            {
                switch ((TransactionCategory)transaction.TransactionCategory.Id.LookupInternalID(LookupCategory.TransactionCategory, string.Empty))
                {
                    case TransactionCategory.Inbound:
                        return InboundTransactionMapper.MapGetPrevious(transaction);
                    case TransactionCategory.ExternalOutbound:
                    case TransactionCategory.DraftOutbound:
                        return OutboundExternalTransactionMapper.MapGetPrevious(transaction);
                    case TransactionCategory.InternalOutbound:
                        return OutboundInternalTransactionMapper.MapGetPrevious(transaction);

                }
            }

            return null;
        }

        public static List<TransactionDTO> Map(IList<Transaction> transactions)
        {
            if (transactions == null || !transactions.Any())
            {
                return null;
            }
            List<TransactionDTO> transactionDTOs = new List<TransactionDTO>();

            foreach (Transaction transaction in transactions)
            {
                transactionDTOs.Add(TransactionMapper.Map(transaction));
            }

            return transactionDTOs;
        }

        public static List<Transaction> Map(List<TransactionDTO> transactionDTOs)
        {
            if (transactionDTOs == null || !transactionDTOs.Any())
            {
                return null;
            }
            List<Transaction> transactions = new List<Transaction>();

            foreach (TransactionDTO transactionDTO in transactionDTOs)
            {
                transactions.Add(TransactionMapper.Map(transactionDTO));
            }

            return transactions;
        }

        public static Transaction Map(TransactionEditDTO transactionEditDTO)
        {
            if (transactionEditDTO != null)
            {
                var transaction = new Transaction()
                {
                    Id = transactionEditDTO.Id,
                    DeliveryNumber = transactionEditDTO.DeliveryNumber,
                    TransactionCategoryId = (int)transactionEditDTO.TransactionCategory
                };
                return transaction;
            }
            return null;
        }

        public static TransactionPrintDTO MapTransactionPrint(DocumentInfo documentInfo, IList<Attachment> attachments, IList<Explanation> explanations)
        {
            var transactionPrintDTO = new TransactionPrintDTO();
            DocumentDTO documentDTO = DocumentMapper.MapWithContent(documentInfo);
            List<TransactionAttachmentDTO> transactionAttachmentDTOs = TransactionAttachmentMapper.Map(attachments);

            List<ExplanationDTO> explanationDTOs = ExplanationMapper.Map(explanations);

            if (explanationDTOs != null)
            {
                foreach (ExplanationDTO explanation in explanationDTOs)
                {
                    if (explanation.EditorType != EditorType.Text)
                    {
                        if (explanation.DocumentDTO != null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(explanation.DocumentDTO.Id.ToString(), new DocumentLocation());
                            explanation.DocumentDTO.Content = docData.Data;
                        }
                    }
                }
            }

            transactionPrintDTO.DocumentDTO = new DocumentDTO();
            transactionPrintDTO.DocumentDTO = documentDTO;
            transactionPrintDTO.Attachments = new List<TransactionAttachmentDTO>();
            transactionPrintDTO.Attachments = transactionAttachmentDTOs;
            transactionPrintDTO.Explanations = new List<ExplanationDTO>();
            transactionPrintDTO.Explanations = explanationDTOs;

            return transactionPrintDTO;
        }
        public static TransactionDetailsDTO MapTransaction(Transaction transaction)
        {
            TransactionDetailsDTO transactionDetailsDTO = new TransactionDetailsDTO()
            {

                Subject = transaction.Subject,
                Number = transaction.Number,
                Confidentiality = transaction.Confidentiality.LocalName,
                Priority = transaction.Priority.Text

            };

            return transactionDetailsDTO;
        }

        public static List<TransactionDetailsDTO> MapTransaction(IList<Transaction> transactionList)
        {
            List<TransactionDetailsDTO> transactionDetailsDTOList = new List<TransactionDetailsDTO>();

            foreach (Transaction transaction in transactionList)
            {
                transactionDetailsDTOList.Add(
                    new TransactionDetailsDTO()
                    {

                        Subject = transaction.Subject,
                        Number = transaction.Number,
                        Confidentiality = transaction.Confidentiality.LocalName,
                        Priority = transaction.Priority.Text

                    });
            }

            return transactionDetailsDTOList;
        }


        public static List<BasicTransactionDto> BasicMap(IList<Transaction> transactions)
        {

            if (transactions != null && transactions.Count > 0)
            {
                return transactions.Select(t => new BasicTransactionDto
                {
                    Confidentiality = t.Confidentiality.LocalName,
                    CreatedDate = t.Assignments.FirstOrDefault().CreatedOn,
                    CreatedDateH = t.Assignments.FirstOrDefault().DateH,
                    Id = t.Id,
                    Subject = t.Subject,
                    TransactionCategoryId = t.TransactionCategoryId.LookupInternalID(LookupCategory.TransactionCategory, "ar"),
                    TransactionNumber = t.Number,
                    TransactionType = t.TransactionTypeId,


                }).ToList();
            }
            return null;
        }
    }
}

