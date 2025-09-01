using MCS.Business;
using MCS.Common;
using MCS.Common.TransactionContext;
using MCS.Domain;
using MCS.DTO;
using MCS.Framework;
using MCS.Framework.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Xml;

namespace MCS.YESSER.Proxy
{
    public class AgencyInboundServices : IAgencyInboundServices
    {
        public ReceiveConfirmTransactionResponse ReceiveConfirmTransaction(ReceiveConfirmTransactionRequest request)
        {
            Status_Type status = Status_Type.Failed;
            ReceiveConfirmTransactionResponse receiveConfirmTransactionResponse = new ReceiveConfirmTransactionResponse()
            {
                Status = Status_Type.Failed
            };

            try
            {
                var context = IoC.Resolve<ITransactionContextScopeFactory>();
                using (var transactionContextScope = context.Create())
                {
                    int transactionStatus;
                    if (request.Status == DeliveryStatus_Type.UnableToDeliver)
                    {
                        //IYesserMappingBL yesserMappingBL = IoC.Resolve<IYesserMappingBL>();
                        //HubTransactionBL hubTransactionBL = new HubTransactionBL();

                        //var yesserMapping = yesserMappingBL.GetCloudMappedValue(
                        //    YesserTypesMapping.DestinationId,
                        //    request.From);

                        //var hubTransaction = hubTransactionBL.GetByTransactionNumber(request.OutboundDocumentNo,
                        //    yesserMapping.CloudTypeId, OutboundClassification.Original);

                        transactionStatus = (int)TransactionStatus.UnableToDeliver;

                        long transactionNumber = int.Parse(request.OutboundDocumentNo);
                        TransactionBL.UpdateStatus(transactionNumber, transactionStatus);
                        transactionContextScope.Commit();
                    }
                    else
                    {
                        transactionStatus = (int)TransactionStatus.Sent;
                        TransactionBL.UpdateStatus(int.Parse(request.OutboundDocumentNo), transactionStatus);
                        transactionContextScope.Commit();
                    }

                    status = Status_Type.Success;
                }

                receiveConfirmTransactionResponse = new ReceiveConfirmTransactionResponse()
                {
                    Status = status
                };
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();
                if (messageFault.HasDetail)
                {
                    //string faultString = "EXCEPTION MESSAGE:\n" +
                    //       faultException.Message + "\n";
                    //string faultNameSpace = "CODE NAMESPACE   : " +
                    //       faultException.Code.Namespace;
                    //string faultCode = "CODE NAME        : " +
                    //       faultException.Code.Name;

                    //we will create dictionary of our element
                    //and values that exist in the XML elements.
                    Dictionary<string, string> faultXElements = new Dictionary<string, string>();
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        while (!xmlReader.EOF)
                        {
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                string localName = xmlReader.LocalName;
                                string readElementString = xmlReader.ReadElementString();
                                faultXElements.Add(localName, readElementString);
                            }
                            else
                            {
                                xmlReader.ReadEndElement();
                            }
                        }
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return receiveConfirmTransactionResponse;
        }

        [return: MessageParameter(Name = "Status")]
        public ReceiveOSUpdateResponse ReceiveOSUpdate(ReceiveOSUpdateRequest request)
        {
            ReceiveOSUpdateResponse response = new ReceiveOSUpdateResponse();
            try
            {
                var context = IoC.Resolve<ITransactionContextScopeFactory>();
                using (var transactionContextScope = context.Create())
                {
                    foreach (ReceiveOSUpdateOSRec item in request.OSRec)
                    {
                        string yesserEntityID = item.MainId + item.SubId;

                        IYesserMappingBL yesserMappingBL = IoC.Resolve<IYesserMappingBL>();
                        YesserMapping yesserMapping = yesserMappingBL.GetCloudMappedValue(YesserTypesMapping.DestinationId, yesserEntityID, false);

                        if (yesserMapping == null)
                        {
                            yesserMappingBL.AddNewEntity(yesserEntityID, item.ARName, item.ENName);
                        }
                    }
                }

                response.Status = Status_Type.Success;
            }
            catch (Exception ex)
            {
                response.Status = Status_Type.Failed;
                response.ErrorCode = "A000005001";
                response.ErrorMessage = ex.Message;

                ExceptionHelper.HandleException(ex);
            }

            return response;
        }

        public Status_Type ReceiveOutbound(out string ErrorCode, out string ErrorMessage, ReceiveOutboundOutboundRec OutboundRec)
        {
            ErrorCode = "";
            ErrorMessage = "";

            Status_Type status = Status_Type.Failed;

            try
            {
                var context = IoC.Resolve<ITransactionContextScopeFactory>();
                using (var transactionContextScope = context.Create())
                {
                    HubTransactionBL hubTransactionBL = new HubTransactionBL();

                    var senderId = OutboundRec.RoutingInfo.From;
                    IYesserMappingBL yesserMappingBL = IoC.Resolve<IYesserMappingBL>();

                    HubTransaction hubTransaction = new HubTransaction();
                    hubTransaction.Subject = OutboundRec.OutboundInfo.OutboundSubject;
                    hubTransaction.TransactionNumber = OutboundRec.OutboundInfo.OutboundDocNo;
                    hubTransaction.OrgUnitId = yesserMappingBL.GetCloudMappedValue(YesserTypesMapping.DestinationId, OutboundRec.RoutingInfo.From).CloudTypeId;
                    hubTransaction.PriorityLevelId = yesserMappingBL.GetCloudMappedValue(YesserTypesMapping.MsgPriority, ((int)OutboundRec.RoutingInfo.Priority).ToString()).CloudTypeId;
                    hubTransaction.ConfidentialityLevelId = yesserMappingBL.GetCloudMappedValue(YesserTypesMapping.MsgSecrecy, ((int)OutboundRec.RoutingInfo.SecrecyLevel).ToString()).CloudTypeId;
                    hubTransaction.DestinationId = yesserMappingBL.GetCloudMappedValue(YesserTypesMapping.OrgUnitId, OutboundRec.RoutingInfo.To).CloudTypeId;
                    hubTransaction.RecordDate = OutboundRec.OutboundInfo.OutboundGDate;
                    hubTransaction.HijriRecordDate = OutboundRec.OutboundInfo.OutboundHDate;
                    hubTransaction.Remarks = OutboundRec.OutboundInfo.OutboundRemarks;
                    hubTransaction.DeliveryType = OutboundRec.RoutingInfo.DeliveryType == OutboundDelivery_Type.E ? HubDeliveryType.E : HubDeliveryType.M;

                    //hubTransaction.RQUID = Guid.Parse(rquid);
                    hubTransaction.CreatedOn = DateTime.Now;

                    if (OutboundRec.OutboundInfo.OutboundGDueDate != null)
                    {
                        hubTransaction.ReminderGDate = OutboundRec.OutboundInfo.OutboundGDueDate;
                    }

                    if (OutboundRec.OutboundInfo.OutboundHDueDate != null)
                    {
                        hubTransaction.ReminderHDate = OutboundRec.OutboundInfo.OutboundHDueDate;
                    }

                    hubTransaction.Status = HubTransactionStatus.Pending;
                    hubTransaction.Classification = (OutboundClassification)(int)OutboundRec.OutboundInfo.OutboundClassification;

                    hubTransaction.HubRelatedPersons = OutboundRec.OutboundInfo.RelatedPersonsInfo?
                        .Select(t => new HubRelatedPerson
                        {
                            Address = t.PersonAddress,
                            Email = t.PersonEmail,
                            Name = t.PersonFullName,
                            NationalId = t.PersonID,
                        }).ToList();

                    var attachements = OutboundRec.AttachmentRec.ToList();
                    var mainDocumnet = attachements.FirstOrDefault(t => t.AttachementType == Attachment_Type.MAIN);

                    if (mainDocumnet == null)
                    {
                        throw new Exception("Error No MAIN document found");
                    }

                    hubTransaction.MainDocument = new DocumentInfo
                    {
                        MimeType = "application/pdf",
                        Name = mainDocumnet.AttachmentFileName,
                        Size = mainDocumnet.AttachmentBase64.Length,
                        Document = new Document
                        {
                            Content = mainDocumnet.AttachmentBase64,
                        },
                    };

                    attachements.Remove(mainDocumnet);

                    if (attachements != null && attachements.Count > 0)
                    {
                        hubTransaction.HubAttachments = new List<HubAttachment>();
                        for (int i = 0; i < attachements.Count; i++)
                        {
                            var hubAttachment = new HubAttachment
                            {
                                AttachementId = attachements[i].AttachmentId,
                                Count = 1,
                                Description = attachements[i].Remarks,
                                TypeId = (int)HubDocumentType.ExternalAttachment,
                                DocumentInfo = null,
                                ExternalAttachementId = attachements[i].AttachmentId
                            };

                            if (!attachements[i].IsObject)
                            {
                                hubAttachment.DocumentInfo = new DocumentInfo
                                {
                                    MimeType = "application/pdf",
                                    Name = attachements[i].AttachmentFileName,
                                    Size = attachements[i].AttachmentBase64 != null ? attachements[i].AttachmentBase64.Length : 0,
                                    Document = new Document
                                    {
                                        Content = attachements[i].AttachmentBase64
                                    }
                                };
                            }

                            hubTransaction.HubAttachments.Add(hubAttachment);
                        }
                    }

                    /* You may need to get the list of all preceding transactions to handle the case
                     * when the hub retries more than one time, and the confirmation fails to be sent,
                     * then multiple hubtransaction records will be written.
                     */
                    var precedingCopy = hubTransactionBL.GetByTransactionNumber(hubTransaction.TransactionNumber, hubTransaction.OrgUnitId, OutboundClassification.Copy);
                    if (precedingCopy != null && !precedingCopy.IsDeleted)
                    {
                        hubTransactionBL.MarkHubCopyAsSeen(precedingCopy.Id);
                    }

                    hubTransactionBL.Add(hubTransaction);
                    transactionContextScope.Commit();

                    //HUBService hUBService = new HUBService();
                    //AddOutboundExternalDTO addOutboundExternalDTO = MapHubTransactionToOutboundExternalDTO(hubTransaction);

                    //BackgroundJob.Enqueue(() => RecurringJobFunction(hUBService, addOutboundExternalDTO, tenant.Id, 0));

                    status = Status_Type.Success;
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();
                if (messageFault.HasDetail)
                {
                    //string faultString = "EXCEPTION MESSAGE:\n" +
                    //       faultException.Message + "\n";
                    //string faultNameSpace = "CODE NAMESPACE   : " +
                    //       faultException.Code.Namespace;
                    //string faultCode = "CODE NAME        : " +
                    //       faultException.Code.Name;

                    //we will create dictionary of our element
                    //and values that exist in the XML elements.
                    Dictionary<string, string> faultXElements = new Dictionary<string, string>();
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        while (!xmlReader.EOF)
                        {
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                string localName = xmlReader.LocalName;
                                string readElementString = xmlReader.ReadElementString();
                                faultXElements.Add(localName, readElementString);
                            }
                            else
                            {
                                xmlReader.ReadEndElement();
                            }
                        }
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ErrorCode = "A000005001";
                ErrorMessage = ex.Message;

                ExceptionHelper.HandleException(ex);
            }

            return status;
        }

        public Status_Type ReceiveRejectOutbound(out string ErrorCode, out string ErrorMessage, string From, string To, string OutboundDocumentNumber, System.DateTime RejectionDate, string RejectionCode, string RejectionReason)
        {
            ErrorMessage = "";
            ErrorCode = "";

            Status_Type status = Status_Type.Failed;

            try
            {
                var context = IoC.Resolve<ITransactionContextScopeFactory>();
                using (var transactionContextScope = context.Create())
                {
                    var transaction = TransactionBL.GetByTransactionNumber(int.Parse(OutboundDocumentNumber));
                    if (transaction == null)
                    {
                        throw new Exception("Transaction not found");
                    }
                    TransactionBL.UpdateStatus(int.Parse(OutboundDocumentNumber), (int)TransactionStatus.Rejected, RejectionReason);
                    transactionContextScope.Commit();
                    status = Status_Type.Success;
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();
                if (messageFault.HasDetail)
                {
                    //string faultString = "EXCEPTION MESSAGE:\n" +
                    //       faultException.Message + "\n";
                    //string faultNameSpace = "CODE NAMESPACE   : " +
                    //       faultException.Code.Namespace;
                    //string faultCode = "CODE NAME        : " +
                    //       faultException.Code.Name;

                    //we will create dictionary of our element
                    //and values that exist in the XML elements.
                    Dictionary<string, string> faultXElements = new Dictionary<string, string>();
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        while (!xmlReader.EOF)
                        {
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                string localName = xmlReader.LocalName;
                                string readElementString = xmlReader.ReadElementString();
                                faultXElements.Add(localName, readElementString);
                            }
                            else
                            {
                                xmlReader.ReadEndElement();
                            }
                        }
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ErrorCode = "A000005001";
                ErrorMessage = ex.Message;

                ExceptionHelper.HandleException(ex);
            }

            return status;
        }

        public void ReceiveStatusInquiry(string From, string To, ref string OutboundDocumentNumber, out string InboundDocumentNumber, out string InboundCreationTimestamp, out MsgStatus_Type Status, out System.DateTime Timestamp, out string ErrorCode, out string ErrorMessage)
        {
            //    ///-Accepted.  ---12340
            //    ///-Pending. ----12341
            //    //- Rejected.-----12342
            //    //- NotFound------12343
            //    //- Failed--------12344
            //    //- Status_Undefined --------12345

            ErrorCode = "";
            ErrorMessage = "";
            InboundDocumentNumber = "";
            InboundCreationTimestamp = "";
            Status = MsgStatus_Type.Failed;
            Timestamp = DateTime.Now;

            try
            {
                /*
                 For handling inbound document number:
                 1. create an object here {NewTransactionId: null, NewTransactionTimestamp: null}
                 2. in case HubTransactionStatus.Confirmed fill that object.
                 3. return the data in StatusInquiryMsgResponse = new StatusInquiryMsgResponse()
                 */

                HubInboundDetails hubInboundDetails = new HubInboundDetails();

                var context = IoC.Resolve<ITransactionContextScopeFactory>();
                using (var transactionContextScope = context.Create())
                {
                    IYesserMappingBL yesserMappingBL = IoC.Resolve<IYesserMappingBL>();
                    HubTransactionBL hubTransactionBL = new HubTransactionBL();

                    var yesserMapping = yesserMappingBL.GetCloudMappedValue(
                            YesserTypesMapping.DestinationId,
                            From);

                    var hubTransaction = hubTransactionBL.GetByTransactionNumber(OutboundDocumentNumber,
                        yesserMapping.CloudTypeId, OutboundClassification.Original);

                    if (hubTransaction != null)
                    {
                        switch (hubTransaction.Status)
                        {
                            case HubTransactionStatus.Pending:
                                Status = MsgStatus_Type.Pending;
                                break;
                            case HubTransactionStatus.Confirmed:
                                hubInboundDetails.NewTransactionId = hubTransaction.NewTransactionId;
                                hubInboundDetails.NewTransactionTimestamp = hubTransaction.NewTransactionTimestamp;
                                Status = MsgStatus_Type.Accepted;
                                break;
                            case HubTransactionStatus.Rejected:
                                Status = MsgStatus_Type.Rejected;
                                break;

                        }
                    }
                    else
                    {
                        Status = MsgStatus_Type.NotFound;
                    }
                }

                Timestamp = DateTime.Now;

                if (hubInboundDetails.NewTransactionTimestamp.HasValue)
                {
                    InboundCreationTimestamp = hubInboundDetails.NewTransactionTimestamp.Value.ToString(SystemConfigurations.HubDateFormat) ?? DateTime.Now.ToString(SystemConfigurations.HubDateFormat);
                    InboundDocumentNumber = hubInboundDetails.NewTransactionId.ToString();
                }
            }
            catch (FaultException faultException)
            {
                MessageFault messageFault = faultException.CreateMessageFault();
                if (messageFault.HasDetail)
                {
                    //string faultString = "EXCEPTION MESSAGE:\n" +
                    //       faultException.Message + "\n";
                    //string faultNameSpace = "CODE NAMESPACE   : " +
                    //       faultException.Code.Namespace;
                    //string faultCode = "CODE NAME        : " +
                    //       faultException.Code.Name;

                    //we will create dictionary of our element
                    //and values that exist in the XML elements.
                    Dictionary<string, string> faultXElements = new Dictionary<string, string>();
                    using (var xmlReader = messageFault.GetReaderAtDetailContents())
                    {
                        while (!xmlReader.EOF)
                        {
                            if (xmlReader.NodeType != XmlNodeType.EndElement)
                            {
                                string localName = xmlReader.LocalName;
                                string readElementString = xmlReader.ReadElementString();
                                faultXElements.Add(localName, readElementString);
                            }
                            else
                            {
                                xmlReader.ReadEndElement();
                            }
                        }
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ErrorCode = "A000005001";
                ErrorMessage = ex.Message;

                ExceptionHelper.HandleException(ex);
            }
        }

        private AddOutboundExternalDTO MapHubTransactionToOutboundExternalDTO(HubTransaction hubTransaction)
        {
            AddOutboundExternalDTO outboundExternalAddDTO = new AddOutboundExternalDTO()
            {
                OrgUnitId = hubTransaction.OrgUnitId,
                OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO
                {
                    Remarks = hubTransaction.Remarks,
                    PriorityLevelId = hubTransaction.PriorityLevelId,
                    ConfidentialityLevelId = hubTransaction.ConfidentialityLevelId,
                    DestinationId = hubTransaction.DestinationId,
                    OutboundDocumentNumber = hubTransaction.TransactionNumber,
                    RQUID = hubTransaction.RQUID
                },
                DocumentDTO = DocumentMapper.MapWithContent(hubTransaction.MainDocument),
                RecordDate = hubTransaction.RecordDate,
                HijriRecordDate = ""

            };

            return outboundExternalAddDTO;
        }
    }

    public class HubInboundDetails
    {
        public long? NewTransactionId { get; set; }
        public DateTime? NewTransactionTimestamp { get; set; }
    }
}
