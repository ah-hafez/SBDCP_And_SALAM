using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class TransactionAttachmentMapper
    {
        public static List<TransactionAttachmentVM> Map(IList<TransactionAttachmentDTO> transactionAttachmentDTOs)
        {
            if (transactionAttachmentDTOs == null || !transactionAttachmentDTOs.Any())
            {
                return new List<TransactionAttachmentVM>();
            }
            int count = 0;
            List<TransactionAttachmentVM> transactionAttachmentVMs = transactionAttachmentDTOs
                .Select(transactionAttachmentDTO => new TransactionAttachmentVM()
                {
                    Archivable = transactionAttachmentDTO.Archivable,
                    AttachmentName = transactionAttachmentDTO.Attachments,
                    DocumentVM = DocumentMapper.Map(transactionAttachmentDTO.DocumentDTO),
                    Id = transactionAttachmentDTO.Id,
                    Number = transactionAttachmentDTO.Number,
                    TypeId = transactionAttachmentDTO.TypeId,
                    TypeName = transactionAttachmentDTO.TypeName,
                    AttachmentSource = transactionAttachmentDTO.AttachmentSource,
                    UserId = transactionAttachmentDTO.UserId,
                    Key = count++


                }).ToList();

            return transactionAttachmentVMs;
        }
        public static List<TransactionAttachmentDTO> Map(IList<TransactionAttachmentVM> transactionAttachmentVMs)
        {
            if (transactionAttachmentVMs == null || !transactionAttachmentVMs.Any())
            {
                return new List<TransactionAttachmentDTO>();
            }
            List<TransactionAttachmentDTO> transactionAttachmentDTOs = transactionAttachmentVMs
                .Select(transactionAttachmentVM => new TransactionAttachmentDTO()
                {
                    Archivable = transactionAttachmentVM.Archivable,
                    Attachments = transactionAttachmentVM.AttachmentName,
                    DocumentDTO = DocumentMapper.Map(transactionAttachmentVM.DocumentVM),
                    Id = transactionAttachmentVM.Id,
                    Number = transactionAttachmentVM.Number,
                    TypeId = transactionAttachmentVM.TypeId,
                    TypeName = transactionAttachmentVM.TypeName,
                    AttachmentSource = transactionAttachmentVM.AttachmentSource
                }).ToList();

            return transactionAttachmentDTOs;
        }
        public static string GetArchivingFileDate(TransactionAttachmentVM transactionAttachmentVM)
        {
            string sNames = string.Empty;
            var list = new { Id = transactionAttachmentVM.Id, AttachmentName = transactionAttachmentVM.AttachmentName, IsDeleted = 0 };
            return JsonConvert.SerializeObject(list);
        }


        public static TransactionAttachmentVM Map(TransactionAttachmentDTO transactionAttachmentDTOs)
        {
            if (transactionAttachmentDTOs != null)
            {
                TransactionAttachmentVM transactionAttachmentVM = new TransactionAttachmentVM()
                {
                    Archivable = transactionAttachmentDTOs.Archivable,
                    AttachmentName = transactionAttachmentDTOs.Attachments,
                    DocumentVM = DocumentMapper.Map(transactionAttachmentDTOs.DocumentDTO),
                    Id = transactionAttachmentDTOs.Id,
                    Number = transactionAttachmentDTOs.Number,
                    TypeId = transactionAttachmentDTOs.TypeId,
                    TypeName = transactionAttachmentDTOs.TypeName,
                    AttachmentSource = transactionAttachmentDTOs.AttachmentSource,
                    UserId = transactionAttachmentDTOs.UserId
                };

                return transactionAttachmentVM;
            }
            return new TransactionAttachmentVM();
        }
    }
}