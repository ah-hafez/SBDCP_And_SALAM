using MCS.DTO;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public class OutboundExternalMapper
    {
        public static AddOutboundExternalDTO Map(AddOutboundExternalVM addOutboundExternalVM)
        {
            if (addOutboundExternalVM != null)
            {
                AddOutboundExternalDTO addOutboundExternalDTO = new AddOutboundExternalDTO()
                {
                    EditorTypeId = addOutboundExternalVM.EditorTypeId,
                    OutboundExternalBasicInfo = OutboundExternalBasicInfoMapper.Map(addOutboundExternalVM.OutboundExternalBasicInfo),
                    //Copies = TransactionCopyMapper.Map(addOutboundExternalVM.Copies),
                    //Attachments = TransactionAttachmentMapper.Map(addOutboundExternalVM.Attachments),
                    //Names = TransactionNameMapper.Map(addOutboundExternalVM.Names),
                    //Links = TransactionLinkMapper.Map(addOutboundExternalVM.Links),
                    DocumentDTO = DocumentMapper.Map(addOutboundExternalVM.DocumentVM),
                    HijriRecordDate = addOutboundExternalVM.HijriRecordDate,
                    Id = addOutboundExternalVM.Id,
                    IsSigned = addOutboundExternalVM.IsSigned,
                    OrgUnitId = addOutboundExternalVM.OrgUnitId,
                    RecordDate = addOutboundExternalVM.RecordDate,
                    StatusId = addOutboundExternalVM.StatusId,
                    UserId = addOutboundExternalVM.UserId,
                    //ExternalCopies = TransactionExternalCopyMapper.Map(addOutboundExternalVM.ExternalCopies),

                };
                return addOutboundExternalDTO;
            }
            return null;
        }
    }
}