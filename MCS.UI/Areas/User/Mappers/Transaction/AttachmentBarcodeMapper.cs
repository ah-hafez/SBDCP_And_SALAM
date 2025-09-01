using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class AttachmentBarcodeMapper
    {
        public static List<AttachmentBarcodeVM> Map(IList<AttachmentBarcodeDTO> attachmentBarcodeDTOs)
        {
            if (attachmentBarcodeDTOs == null || !attachmentBarcodeDTOs.Any())
            {
                return new List<AttachmentBarcodeVM>();
            }
            List<AttachmentBarcodeVM> attachmentBarcodeVMs = attachmentBarcodeDTOs
                .Select(attachmentBarcodeDTO => new AttachmentBarcodeVM()
                { 
                    Count = attachmentBarcodeDTO.Count,
                    Id = attachmentBarcodeDTO.Id,
                    Name = attachmentBarcodeDTO.Name
                }).ToList();

            return attachmentBarcodeVMs;
        }
        public static List<AttachmentBarcodeDTO> Map(IList<AttachmentBarcodeVM> attachmentBarcodeVMs)
        {
            if (attachmentBarcodeVMs == null || !attachmentBarcodeVMs.Any())
            {
                return new List<AttachmentBarcodeDTO>();
            }
            List<AttachmentBarcodeDTO> attachmentBarcodeDTOs = attachmentBarcodeVMs
                .Select(attachmentBarcodeVM => new AttachmentBarcodeDTO()
                {
                    Count = attachmentBarcodeVM.Count,
                    Id = attachmentBarcodeVM.Id,
                    Name = attachmentBarcodeVM.Name
                }).ToList();

            return attachmentBarcodeDTOs;
        }


    }
}