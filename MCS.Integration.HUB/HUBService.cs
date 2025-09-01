using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.Domain;
using MCS.DTO;
using MCS.Integration.HUB.Helpers;
using MCS.Integration.HUB.CSIAgencyOutboundServicesRef;

namespace MCS.Integration.HUB
{
    public class HUBService
    {
        public SendOutboundResult SendOutbound(AddOutboundExternalDTO outboundExternalDTO, OutboundClassification outboundClassification)
        {
            CSIAgencyOutboundServicesInterfaceClient adapterClient = new CSIAgencyOutboundServicesInterfaceClient();
            OutboundClassification_Type CIoutboundClassification = (OutboundClassification_Type)outboundClassification;
            OutboundSendRequest outboundRec = MapAddOutboundExternalDTOToOutboundSendRequest(outboundExternalDTO, CIoutboundClassification);

            OutboundSendResponse response = adapterClient.OutboundSend(outboundRec);

            adapterClient.Close();

            return new SendOutboundResult
            {
                Status = response.Status.ToString()
            };
        }
        public OutboundStatusInquiryResponse SendStatusInquiry(AddOutboundExternalDTO outboundExternalDTO)
        {
            CSIAgencyOutboundServicesInterfaceClient adapterClient = new CSIAgencyOutboundServicesInterfaceClient();
            OutboundStatusInquiryResponse response = adapterClient.OutboundStatusInquiry(
                new OutboundStatusInquiryRequest
                {
                    From = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.OrgUnitId,
                        outboundExternalDTO.OrgUnitId),
                    To = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.DestinationId,
                        outboundExternalDTO.OutboundExternalBasicInfo.DestinationId),
                    OutboundDocumentNumber = outboundExternalDTO.OutboundExternalBasicInfo.OutboundNumber.ToString()
                }
            );

            adapterClient.Close();

            return response;
        }
        public string SendReject(AddOutboundExternalDTO outboundExternalDTO, string rejectionCode, string rejectionReason)
        {
            CSIAgencyOutboundServicesInterfaceClient adapterClient = new CSIAgencyOutboundServicesInterfaceClient();

            DateTime dt = new DateTime();
            dt = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss"));

            OutboundRejectResponse response = adapterClient.OutboundReject(
                new OutboundRejectRequest
                {
                    From = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.OrgUnitId,
                        outboundExternalDTO.OutboundExternalBasicInfo.DestinationId),
                    To = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.DestinationId,
                        outboundExternalDTO.OrgUnitId),
                    OutboundDocumentNumber = outboundExternalDTO.OutboundExternalBasicInfo.OutboundDocumentNumber,
                    RejectionCode = rejectionCode,
                    RejectionReason = rejectionReason,
                    RejectionDate = dt
                }
            );

            adapterClient.Close();

            return response.Status.ToString();
        }
        private OutboundSendRequest MapAddOutboundExternalDTOToOutboundSendRequest(AddOutboundExternalDTO outboundExternalDTO, OutboundClassification_Type outboundClassification)
        {
            Enum.TryParse(HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.MsgPriority,
                        outboundExternalDTO.OutboundExternalBasicInfo.PriorityLevelId), out Msg_Priority priority);

            Enum.TryParse(HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.MsgSecrecy,
                        outboundExternalDTO.OutboundExternalBasicInfo.ConfidentialityLevelId), out Msg_Secrecy secrecy);

            var outboundSendRequest = new OutboundSendRequest
            {
                OutboundRec = new SendOutboundOutboundRec
                {
                    OutboundInfo = new SendOutboundOutboundRecOutboundInfo
                    {
                        OutboundClassification = outboundClassification,
                        OutboundCategory = CSIAgencyOutboundServicesRef.OutboundCategory_Type.Management,
                        OutboundDocNo = outboundExternalDTO.OutboundExternalBasicInfo.OutboundNumber.ToString(),
                        OutboundGDate = outboundExternalDTO.RecordDate,
                        OutboundHDate = HubHelper.FormatHejriDateString(outboundExternalDTO.HijriRecordDate),
                        OutboundRemarks = outboundExternalDTO.OutboundExternalBasicInfo.Remarks,
                        OutboundSubject = outboundExternalDTO.OutboundExternalBasicInfo.Subject,
                        OutboundType = Msg_Subject.Item01,
                        OutboundGDueDate = outboundExternalDTO.OutboundExternalBasicInfo.RemindDate ?? DateTime.Now.AddDays(15),
                        OutboundHDueDate = outboundExternalDTO.OutboundExternalBasicInfo.RemindDateH == null ? null : HubHelper.FormatHejriDateString(outboundExternalDTO.OutboundExternalBasicInfo.RemindDateH)
                    },
                    RoutingInfo = new SendOutboundOutboundRecRoutingInfo
                    {
                        DeliveryType = outboundExternalDTO.OutboundExternalBasicInfo.DeliveryMethodId == (int)OutboundDelivery_Type.E ? OutboundDelivery_Type.E : OutboundDelivery_Type.M,
                        From = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.OrgUnitId,
                        outboundExternalDTO.OrgUnitId),
                        Priority = priority,
                        SecrecyLevel = secrecy,
                        SenderType = CSIAgencyOutboundServicesRef.OutboundSender_Type.GOVT,
                        To = HubHelper.GetYesserMappedValue(
                        Common.YesserTypesMapping.DestinationId,
                        outboundExternalDTO.OutboundExternalBasicInfo.DestinationId)
                    }
                }
            };

            outboundSendRequest.OutboundRec.OutboundInfo.RelatedPersonsInfo = outboundExternalDTO.Names.Select(t =>
            new SendOutboundOutboundRecOutboundInfoRelatedPersonsInfo
            {
                PersonAddress = t.Address,
                PersonEmail = t.Email,
                PersonFullName = t.FirstName,
                PersonID = t.CivilID
            }).ToArray();


            outboundSendRequest.OutboundRec.AttachmentRec = new SendOutboundOutboundRecAttachmentRec[outboundExternalDTO.Attachments.Count + 1];
            outboundSendRequest.OutboundRec.AttachmentRec[0] = new SendOutboundOutboundRecAttachmentRec
            {
                AttachmentId = outboundExternalDTO.DocumentDTO.Id.ToString(),
                AttachementType = CSIAgencyOutboundServicesRef.Attachment_Type.MAIN,
                AttachementURL = "",
                AttachmentBase64 = outboundExternalDTO.DocumentDTO.Content,
                AttachmentBarcode = null,
                AttachmentContentType = CSIAgencyOutboundServicesRef.AttachmentContent_Type.PDF,
                AttachmentFileName = outboundExternalDTO.DocumentDTO.Name,
                IsObject = false
            };

            for (int i = 0; i < outboundExternalDTO.Attachments.Count; i++)
            {
                var archivable = outboundExternalDTO.Attachments[i].Archivable;
                outboundSendRequest.OutboundRec.AttachmentRec[i + 1] = new SendOutboundOutboundRecAttachmentRec
                {
                    AttachementType = CSIAgencyOutboundServicesRef.Attachment_Type.COPY,
                    AttachementURL = "",
                    AttachmentBarcode = null,
                    AttachmentBase64 = archivable ? outboundExternalDTO.Attachments[i].DocumentDTO.Content : null,
                    AttachmentFileName = archivable ? outboundExternalDTO.Attachments[i].DocumentDTO.Name : null,
                    AttachmentContentType = CSIAgencyOutboundServicesRef.AttachmentContent_Type.PDF,
                    AttachmentId = archivable ? outboundExternalDTO.Attachments[i].DocumentDTO.Id.ToString() : "0",
                    //Remarks = outboundExternalDTO.Attachments[i].Description,
                    IsObject = !archivable
                };
            }

            return outboundSendRequest;
        }
    }
}