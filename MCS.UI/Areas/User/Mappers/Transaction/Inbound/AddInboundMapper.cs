using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Shared;
using MCS.UI.Areas.User.Mappers.Transaction.Outbound;
using MCS.UI.Areas.User.Models.Transaction.Inbound;

namespace MCS.UI.Areas.User.Mappers.Transaction.Inbound
{
    public static class AddInboundMapper
    {
        public static List<AddInboundVM> Map(IList<AddInboundDTO> addInboundDTOs)
        {
            if (addInboundDTOs == null || !addInboundDTOs.Any())
            {
                return new List<AddInboundVM>();
            }
            List<AddInboundVM> addInboundVMs = addInboundDTOs
                .Select(addInboundDTO => new AddInboundVM()
                { 
                    Attachments = TransactionAttachmentMapper.Map(addInboundDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(addInboundDTO.DocumentDTO),
                    HijriRecordDate = addInboundDTO.HijriRecordDate,
                    Id = addInboundDTO.Id,
                    InboundBasicInfo = AddInboundBasicInfoMapper.Map(addInboundDTO.InboundBasicInfo),
                    IsSigned = addInboundDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(addInboundDTO.Links),
                    Names = TransactionNameMapper.Map(addInboundDTO.Names),
                    OrgUnitId = addInboundDTO.OrgUnitId,
                    RecordDate = addInboundDTO.RecordDate,
                    StatusId = addInboundDTO.StatusId,
                    UserId = addInboundDTO.UserId,
                    Copies = TransactionCopyMapper.Map(addInboundDTO.Copies),
                    ExternalCopies= TransactionExternalCopyMapper.Map(addInboundDTO.ExternalCopies),
                    
                }).ToList();

            return addInboundVMs;
        }
        public static List<AddInboundDTO> Map(IList<AddInboundVM> addInboundVMs)
        {
            if (addInboundVMs == null || !addInboundVMs.Any())
            {
                return new List<AddInboundDTO>();
            }
            List<AddInboundDTO> addInboundDTOs = addInboundVMs
                .Select(addInboundDTO => new AddInboundDTO()
                {
                    Attachments = TransactionAttachmentMapper.Map(addInboundDTO.Attachments),
                    DocumentDTO = DocumentMapper.Map(addInboundDTO.DocumentVM),
                    HijriRecordDate = addInboundDTO.HijriRecordDate,
                    Id = addInboundDTO.Id,
                    InboundBasicInfo = AddInboundBasicInfoMapper.Map(addInboundDTO.InboundBasicInfo),
                    IsSigned = addInboundDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(addInboundDTO.Links),
                    Names = TransactionNameMapper.Map(addInboundDTO.Names),
                    OrgUnitId = addInboundDTO.OrgUnitId,
                    RecordDate = addInboundDTO.RecordDate,
                    StatusId = addInboundDTO.StatusId,
                    UserId = addInboundDTO.UserId,
                    Copies = TransactionCopyMapper.Map(addInboundDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(addInboundDTO.ExternalCopies),
                }).ToList();

            return addInboundDTOs;
        }
        public static AddInboundDTO Map(AddInboundVM addInboundVM)
        {
            if (addInboundVM != null)
            {
                AddInboundDTO addInboundDTO = new AddInboundDTO()
                { 
                    Attachments = TransactionAttachmentMapper.Map(addInboundVM.Attachments),
                    DocumentDTO = DocumentMapper.Map(addInboundVM.DocumentVM),
                    HijriRecordDate = addInboundVM.HijriRecordDate,
                    Id = addInboundVM.Id,
                    InboundBasicInfo = AddInboundBasicInfoMapper.Map(addInboundVM.InboundBasicInfo),
                    IsSigned = addInboundVM.IsSigned,
                    Links = TransactionLinkMapper.Map(addInboundVM.Links),
                    Names = TransactionNameMapper.Map(addInboundVM.Names),
                    OrgUnitId = addInboundVM.OrgUnitId,
                    RecordDate = addInboundVM.RecordDate,
                    StatusId = addInboundVM.StatusId,
                    UserId = addInboundVM.UserId,
                    Copies = TransactionCopyMapper.Map(addInboundVM.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(addInboundVM.ExternalCopies),


                };

                return addInboundDTO;
            }
            return new AddInboundDTO();
        }
        public static AddInboundVM Map(AddInboundDTO addInboundDTO)
        {
            if (addInboundDTO != null)
            {
                AddInboundVM addInboundVM = new AddInboundVM()
                {
                    Attachments = TransactionAttachmentMapper.Map(addInboundDTO.Attachments),
                    DocumentVM = DocumentMapper.Map(addInboundDTO.DocumentDTO),
                    HijriRecordDate = addInboundDTO.HijriRecordDate,
                    Id = addInboundDTO.Id,
                    InboundBasicInfo = AddInboundBasicInfoMapper.Map(addInboundDTO.InboundBasicInfo),
                    IsSigned = addInboundDTO.IsSigned,
                    Links = TransactionLinkMapper.Map(addInboundDTO.Links),
                    Names = TransactionNameMapper.Map(addInboundDTO.Names),
                    OrgUnitId = addInboundDTO.OrgUnitId,
                    SideContactExternalEntityID = addInboundDTO.InboundBasicInfo.SideContactExternalEntityID,
                    RecordDate = addInboundDTO.RecordDate,
                    StatusId = addInboundDTO.StatusId,
                    UserId = addInboundDTO.UserId,
                    Copies = TransactionCopyMapper.Map(addInboundDTO.Copies),
                    ExternalCopies = TransactionExternalCopyMapper.Map(addInboundDTO.ExternalCopies),
                };

                return addInboundVM;
            }
            return new AddInboundVM();
        }

    }
}