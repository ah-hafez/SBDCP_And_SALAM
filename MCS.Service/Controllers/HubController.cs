using FileSignatures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Services.Protocols;
using System.Xml;
using MCS.Framework;
using MCS.Framework.Exceptions;
using MCS.Business;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DocRepository.DataDef;
using MCS.Domain;
using MCS.DTO;
using MCS.DTO.HubTransaction;
using MCS.Integration.HUB;
using MCS.Integration.HUB.CSIAgencyOutboundServicesRef;
using MCS.Service.Hubs;
using MCS.Service.Mappers;

namespace MCS.Service.Controllers
{
    //[CustomAuthenticationAttribute]
    public class HubController : ApiBaseController
    {
        public PostObjectResult<List<HubTransactionDTO>> GetOriginalHubTransactions(string culture, int TypeId)
        {
            var hubTransactionDTOListResult = new PostObjectResult<List<HubTransactionDTO>>();
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    var hubTransactionListResult = hubTransactionBL.GetOriginalHubTransactions(TypeId);
                    hubTransactionDTOListResult.Result = HubTransactionMapper.Map(hubTransactionListResult, culture);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
            return hubTransactionDTOListResult;
        }

        public PostObjectResult<HubTransactionDTO> GetHubTransactionById(int transactionId, string cultureName)
        {
            var hubTransactionDTOResult = new PostObjectResult<HubTransactionDTO>();
            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    var hubTransactionResult = hubTransactionBL.GetHubTransactionById(transactionId);
                    hubTransactionDTOResult.Result = HubTransactionMapper.Map(hubTransactionResult, cultureName);
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
            return hubTransactionDTOResult;
        }
        public PostResult SendOutbound(int transactionId, string culture)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            Transaction transaction;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    transaction = TransactionBL.GetTransactionById(transactionId, culture);

                    if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                    {
                        if (transaction.MainDocument.Document.Content == null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(transaction.MainDocument.Id.ToString(), new DocumentLocation());
                            transaction.MainDocument.Document.Content = docData.Data;
                        }
                    }

                    foreach (Attachment attachment in transaction.Attachments)
                    {
                        if (attachment.DocumentInfo.Document != null)
                        {
                            DocData docData = DocRepository.DocRepository.Load(attachment.DocumentInfo.Id.ToString(), new DocumentLocation());
                            attachment.DocumentInfo.Document.Content = docData.Data;
                        }
                    }

                    AddOutboundExternalDTO addOutboundExternalDTO = MapTransactionToAddOutboundExternalDTO(transaction);

                    HUBService hUBService = new HUBService();
                    SendOutboundResult sentOutboundResult = hUBService.SendOutbound(addOutboundExternalDTO, OutboundClassification.Original);
                    postResult.Result = sentOutboundResult.Status;

                    if (postResult.Result.ToString() == "Success")
                    {
                        TransactionBL.UpdateStatus(transaction.Number, (int)TransactionStatus.Pending.LookupIdentity(LookupCategory.TransactionStatus, culture));
                        transactionContextScope.Commit();
                    }

                    //IHubRQUIDBL hubRQUIDBL = IoC.Resolve<IHubRQUIDBL>();
                    //hubRQUIDBL.Add(new HubRQUID
                    //{
                    //    TransactionNumber = transaction.Number,
                    //    RQUID = sentOutboundResult.RQUID
                    //});


                    var externalPartiesIds = transaction.ExternalCopies.Where(exc => exc.Entity.YasserRegistered).Select(ec => ec.EntityId).ToList();

                    foreach (var partyId in externalPartiesIds)
                    {
                        PostResult postResultForCopy = new PostResult
                        {
                            Result = "faild"
                        };

                        AddOutboundExternalDTO addOutboundExternalCopyDTO = MapTransactionToAddOutboundExternalDTO(transaction);
                        addOutboundExternalCopyDTO.OutboundExternalBasicInfo.DestinationId = partyId.Value;
                        SendOutboundResult sentOutboundCopiesResult = hUBService.SendOutbound(addOutboundExternalCopyDTO, OutboundClassification.Copy);
                        postResultForCopy.Result = sentOutboundCopiesResult.Status;

                        //IHubRQUIDBL hubRQUIDForCopyBL = IoC.Resolve<IHubRQUIDBL>();
                        //hubRQUIDForCopyBL.Add(new HubRQUID
                        //{
                        //    TransactionNumber = transaction.Number,
                        //    RQUID = sentOutboundCopiesResult.RQUID
                        //});
                    }
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

                    if (faultXElements.ContainsKey("Code"))
                    {
                        postResult.Result = faultXElements["Code"];
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }
        public PostResult ResendOutbound(int transactionId, int tenantId, string culture)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            Transaction transaction;

            try
            {
                using (var transactionContextScope = context.Create())
                {
                    transaction = TransactionBL.GetTransactionById(transactionId, culture);
                    AddOutboundExternalDTO addOutboundExternalDTO = MapTransactionToAddOutboundExternalDTO(transaction);

                    HUBService hUBService = new HUBService();
                    SendOutboundResult sentOutboundResult = hUBService.SendOutbound(addOutboundExternalDTO, OutboundClassification.Original);
                    postResult.Result = sentOutboundResult.Status;

                    if (postResult.Result.ToString() == "Success")
                    {
                        TransactionBL.UpdateStatus(transaction.Number, (int)TransactionStatus.Pending.LookupIdentity(LookupCategory.TransactionStatus, culture));
                        transactionContextScope.Commit();
                    }

                    //IHubRQUIDBL hubRQUIDBL = IoC.Resolve<IHubRQUIDBL>();
                    //hubRQUIDBL.Add(new HubRQUID
                    //{
                    //    TransactionNumber = transaction.Number,
                    //    RQUID = sentOutboundResult.RQUID
                    //});

                    if (transaction.Status.Id == (int)TransactionStatus.Rejected.LookupIdentity(LookupCategory.TransactionStatus, culture))
                    {
                        var externalPartiesIds = transaction.ExternalCopies.Where(exc => exc.Entity.YasserRegistered).Select(ec => ec.EntityId).ToList();
                        foreach (var partyId in externalPartiesIds)
                        {
                            PostResult postResultForCopy = new PostResult
                            {
                                Result = "faild"
                            };

                            AddOutboundExternalDTO addOutboundExternalCopyDTO = MapTransactionToAddOutboundExternalDTO(transaction);
                            addOutboundExternalCopyDTO.OutboundExternalBasicInfo.DestinationId = partyId.Value;
                            SendOutboundResult sentOutboundCopiesResult = hUBService.SendOutbound(addOutboundExternalCopyDTO, OutboundClassification.Copy);
                            postResultForCopy.Result = sentOutboundCopiesResult.Status;

                            //IHubRQUIDBL hubRQUIDForCopyBL = IoC.Resolve<IHubRQUIDBL>();
                            //hubRQUIDForCopyBL.Add(new HubRQUID
                            //{
                            //    TransactionNumber = transaction.Number,
                            //    RQUID = sentOutboundCopiesResult.RQUID
                            //});
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }
        public PostResult SendStatusInquiry(int transactionId, string culture)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            try
            {
                Transaction transaction;
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    transaction = TransactionBL.GetTransactionById(transactionId, culture);
                    AddOutboundExternalDTO addOutboundExternalDTO = MapTransactionToAddOutboundExternalDTO(transaction);

                    HUBService hUBService = new HUBService();
                    OutboundStatusInquiryResponse statusInquiryMsgResponse = hUBService.SendStatusInquiry(addOutboundExternalDTO);
                    StringBuilder resultStringBuilde = new StringBuilder();
                    resultStringBuilde.Append(statusInquiryMsgResponse.Status);

                    if (!string.IsNullOrWhiteSpace(statusInquiryMsgResponse.InboundDocumentNumber))
                    {
                        resultStringBuilde.Append("<br> Inbound document number: ");
                        resultStringBuilde.Append(statusInquiryMsgResponse.InboundDocumentNumber);
                        resultStringBuilde.Append("<br> Inbound creation timestamp: ");
                        resultStringBuilde.Append(statusInquiryMsgResponse.InboundCreationTimestamp);
                    }


                    postResult.Result = resultStringBuilde.ToString();
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

                    if (faultXElements.ContainsKey("Code"))
                    {
                        postResult.Result = "حدث خطأ: " + faultXElements["Code"];
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }
        public PostResult SendReject(string transactionNumber, int orgUnitId, string rejectionReason)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            try
            {
                HubTransaction hubTransaction;
                using (var transactionContextScope = context.Create())
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    hubTransaction = hubTransactionBL.GetByTransactionNumber(transactionNumber, orgUnitId, OutboundClassification.Original);
                    AddOutboundExternalDTO addOutboundExternalDTO = MapHubTransactionToOutboundExternalDTO(hubTransaction);

                    HUBService hUBService = new HUBService();
                    string result = hUBService.SendReject(addOutboundExternalDTO, "0", rejectionReason);

                    if (result == "Success")
                    {
                        IHubAttachmentBL hubAttachmentBL = IoC.Resolve<IHubAttachmentBL>();
                        IHubRelatedPersonBL hubRelatedPersonBL = IoC.Resolve<IHubRelatedPersonBL>();

                        foreach (var hubAttachment in hubTransaction.HubAttachments.ToList())
                        {
                            hubTransaction.HubAttachments.Remove(hubAttachment);
                            //hubAttachmentBL.Delete(hubAttachment.Id);
                        }

                        foreach (var hubRelatedPerson in hubTransaction.HubRelatedPersons.ToList())
                        {
                            hubTransaction.HubRelatedPersons.Remove(hubRelatedPerson);
                            hubRelatedPersonBL.Delete(hubRelatedPerson.Id);
                        }

                        hubTransactionBL.Reject(hubTransaction);
                        transactionContextScope.Commit();
                        postResult.Result = result;
                    }
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

                    if (faultXElements.ContainsKey("Code"))
                    {
                        postResult.Result = faultXElements["Code"];
                    }
                }

                ExceptionHelper.HandleException(faultException);
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }
        //[AllowAnonymous]
        public PostResult CreateInbound(string transactionNumber, int orgUnitId, int userId, int internalOrgUnitId)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            HubTransaction hubTransaction;
            try
            {
                using (var transactionContextScope = context.Create())
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    hubTransaction = hubTransactionBL.GetByTransactionNumber(transactionNumber, orgUnitId, OutboundClassification.Original);
                    //AddOutboundExternalDTO addOutboundExternalDTO = MapHubTransactionToOutboundExternalDTO(hubTransaction);
                    //HUBService hUBService = new HUBService();
                    //var result = hUBService.SendConfirm(addOutboundExternalDTO, tenantId);

                    //if (result == "Success")
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.Inbound);
                        byte[] mainDocumentContent = null;

                        Transaction transaction = MapHubTransactionToTransaction(hubTransaction, internalOrgUnitId);
                        transaction.DeliveryMethodId = (int)HubConstants.DeliveryMethodId;
                        transaction.SourceTypeId = (int)HubConstants.SourceTypeId;
                        transaction.LetterTypeId = (int)HubConstants.LetterTypeId; ;
                        transaction.DocumentNumber = hubTransaction.TransactionNumber.ToString();
                        transaction.EntityId = internalOrgUnitId;
                        transaction.InboundDateH = transaction.DateH;
                        transaction.Subject = hubTransaction.Subject;
                        transaction.ToUserId = userId;
                        transaction.UserId = userId;
                        transaction.TransactionTypeId = TransactionCategory.Inbound.LookupIdentity(LookupCategory.TransactionCategories,string.Empty);
                        transaction.Copies = new List<TransactionCopy>();

                        INameBL nameBL = IoC.Resolve<INameBL>();

                        transaction.Names = new List<TransactionName>();

                        foreach (HubRelatedPerson hubRelatedPerson in hubTransaction.HubRelatedPersons)
                        {
                            Name name = nameBL.GetNameByCivilId(hubRelatedPerson.NationalId);

                            if (name == null)
                            {
                                transaction.Names.Add(new TransactionName
                                {
                                    Name = new Name
                                    {
                                        Address = hubRelatedPerson.Address,
                                        Email = hubRelatedPerson.Email,
                                        FirstName = hubRelatedPerson.Name,
                                        CivilID = hubRelatedPerson.NationalId
                                    }
                                });
                            }
                            else
                            {
                                transaction.Names.Add(new TransactionName
                                {
                                    Name = name
                                });
                            }
                        }

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;
                        }
                        transaction.Links = new List<TransactionLink>();
                        transaction.Attachments = new List<Attachment>();

                        IHubAttachmentBL hubAttachmentBL = IoC.Resolve<IHubAttachmentBL>();
                        IHubRelatedPersonBL hubRelatedPersonBL = IoC.Resolve<IHubRelatedPersonBL>();


                        foreach (var hubAttachmnt in hubTransaction.HubAttachments)
                        {
                            transaction.Attachments.Add(new Attachment
                            {
                                Count = 1,
                                Description = hubAttachmnt.Description,
                                TypeId = hubAttachmnt.TypeId,
                                DocumentInfo = hubAttachmnt.DocumentInfo,
                            });
                        }

                        TransactionDetails transactionDetails = transactionBL.Save(transaction);
                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.MainDocument.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        foreach (var hubAttachment in hubTransaction.HubAttachments.ToList())
                        {
                            hubTransaction.HubAttachments.Remove(hubAttachment);
                            //hubAttachmentBL.Delete(hubAttachment.Id);
                        }


                        foreach (var hubRelatedPerson in hubTransaction.HubRelatedPersons.ToList())
                        {
                            hubTransaction.HubRelatedPersons.Remove(hubRelatedPerson);
                            hubRelatedPersonBL.Delete(hubRelatedPerson.Id);
                        }

                        hubTransactionBL.Confirm(hubTransaction, transaction.Number, DateTime.Now);
                        transactionContextScope.Commit();
                        postResult.Result = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }
        public PostResult CreateOutboundInternal(string transactionNumber, int tenantId, int orgUnitId, int userId, int internalOrgUnitId)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            HubTransaction hubTransaction;
            try
            {
                using (var transactionContextScope = context.Create(tenantId: tenantId))
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    hubTransaction = hubTransactionBL.GetByTransactionNumber(transactionNumber, orgUnitId, OutboundClassification.Copy);
                    AddOutboundExternalDTO addOutboundExternalDTO = MapHubTransactionToOutboundExternalDTO(hubTransaction);
                    //HUBService hUBService = new HUBService();
                    //var result = hUBService.SendConfirm(addOutboundExternalDTO, tenantId);

                    //if (result == "Success")
                    {
                        ITransactionBL transactionBL = TransactionBL.Create(TransactionCategory.InternalOutbound);
                        byte[] mainDocumentContent = null;

                        Transaction transaction = MapHubTransactionToTransaction(hubTransaction, internalOrgUnitId);
                        transaction.DeliveryMethodId = (int)HubConstants.DeliveryMethodId;
                        transaction.SourceTypeId = (int)HubConstants.SourceTypeId;
                        transaction.LetterTypeId = (int)HubConstants.LetterTypeId;
                        transaction.DocumentNumber = hubTransaction.TransactionNumber.ToString();
                        transaction.EntityId = internalOrgUnitId;
                        transaction.InboundDateH = transaction.DateH;
                        transaction.Subject = hubTransaction.Subject;
                        transaction.ToUserId = userId;
                        transaction.UserId = userId;
                        transaction.TransactionTypeId = TransactionCategory.InternalOutbound.LookupIdentity(LookupCategory.TransactionCategory,string.Empty);
                        transaction.Copies = new List<TransactionCopy>();

                        INameBL nameBL = IoC.Resolve<INameBL>();

                        transaction.Names = new List<TransactionName>();

                        foreach (HubRelatedPerson hubRelatedPerson in hubTransaction.HubRelatedPersons)
                        {
                            Name name = nameBL.GetNameByCivilId(hubRelatedPerson.NationalId);

                            if (name == null)
                            {
                                transaction.Names.Add(new TransactionName
                                {
                                    Name = new Name
                                    {
                                        Address = hubRelatedPerson.Address,
                                        Email = hubRelatedPerson.Email,
                                        FirstName = hubRelatedPerson.Name,
                                        CivilID = hubRelatedPerson.NationalId
                                    }
                                });
                            }
                            else
                            {
                                transaction.Names.Add(new TransactionName
                                {
                                    Name = name
                                });
                            }
                        }

                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null && transaction.MainDocument.Document.Content != null)
                        {
                            mainDocumentContent = transaction.MainDocument.Document.Content;
                            transaction.MainDocument.Document.Content = null;
                        }
                        transaction.Links = new List<TransactionLink>();
                        transaction.Attachments = new List<Attachment>();

                        IHubAttachmentBL hubAttachmentBL = IoC.Resolve<IHubAttachmentBL>();
                        IHubRelatedPersonBL hubRelatedPersonBL = IoC.Resolve<IHubRelatedPersonBL>();


                        foreach (var hubAttachmnt in hubTransaction.HubAttachments)
                        {
                            transaction.Attachments.Add(new Attachment
                            {
                                Count = 1,
                                Description = hubAttachmnt.Description,
                                TypeId = hubAttachmnt.TypeId,
                                DocumentInfo = hubAttachmnt.DocumentInfo,
                            });
                        }

                        TransactionDetails transactionDetails = transactionBL.Save(transaction);
                        if (transaction.MainDocument != null && transaction.MainDocument.Document != null)
                        {
                            DocData docData = new DocData()
                            {
                                Data = mainDocumentContent,
                                DocName = transaction.MainDocument.Name,
                                DocID = transaction.MainDocument.Id.ToString(),
                                PersonId = transaction.MainDocument.CreatedBy,
                                MimeContent = transaction.MainDocument.MimeType,
                                EntityId = transaction.EntityId,
                                DataSize = Convert.ToInt32(transaction.MainDocument.Size),
                                User_ID = transaction.MainDocument.CreatedBy.ToString(),
                                TransactionId = transaction.Id
                            };
                            DocRepository.DocRepository.Save(docData, new DocumentLocation());
                        }

                        foreach (var hubAttachment in hubTransaction.HubAttachments.ToList())
                        {
                            hubTransaction.HubAttachments.Remove(hubAttachment);
                            //hubAttachmentBL.Delete(hubAttachment.Id);
                        }


                        foreach (var hubRelatedPerson in hubTransaction.HubRelatedPersons.ToList())
                        {
                            hubTransaction.HubRelatedPersons.Remove(hubRelatedPerson);
                            hubRelatedPersonBL.Delete(hubRelatedPerson.Id);
                        }

                        hubTransactionBL.Confirm(hubTransaction, transaction.Number, DateTime.Now);
                        transactionContextScope.Commit();
                        postResult.Result = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }

            return postResult;
        }

        public PostResult MarkCopyAsSeen(int transactionId)
        {
            PostResult postResult = new PostResult
            {
                Result = "faild"
            };

            try
            {
                using (var transactionContextScope = context.CreateReadOnly())
                {
                    IHubTransactionBL hubTransactionBL = IoC.Resolve<IHubTransactionBL>();
                    bool isMarkedAsSeen = hubTransactionBL.MarkHubCopyAsSeen(transactionId);
                    if (isMarkedAsSeen)
                    {
                        postResult.Result = "Success";
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionHelper.HandleException(ex);
            }
            return postResult;
        }
        private TransactionName selectName(HubRelatedPerson hubRelatedPerson, List<Name> transactionNames, int transactionId)
        {
            TransactionName transactionName = new TransactionName();
            bool newName = true;
            foreach (var item in transactionNames)
            {
                if (item.CivilID == hubRelatedPerson.NationalId)
                {
                    newName = false;
                    return transactionName = new TransactionName
                    {
                        TransactionId = transactionId,
                        Name = item
                    };
                }
            };

            if (newName)
            {
                transactionName = new TransactionName
                {
                    Name = new Name
                    {
                        Address = hubRelatedPerson.Address,
                        Email = hubRelatedPerson.Email,
                        FirstName = hubRelatedPerson.Name,
                        CivilID = hubRelatedPerson.NationalId
                    }
                };
            }

            return transactionName;
        }

        private Transaction MapHubTransactionToTransaction(HubTransaction hubTransaction, int internalOrgUnitId)
        {
            Transaction transaction = new Transaction
            {
                DocumentNumber = hubTransaction.TransactionNumber,
                OrgUnitId = internalOrgUnitId,
                PriorityId = hubTransaction.PriorityLevelId,
                ConfidentialityId = hubTransaction.ConfidentialityLevelId,
                ExternalPartyId = hubTransaction.OrgUnitId,
                Date = DateTime.Now,
                DateH = DateTimeUtility.ConvertToUmAlQuraCalendar(DateTime.Now),
                InboundDateH = hubTransaction.HijriRecordDate,
                Remarks = hubTransaction.Remarks,
                RemindDate = hubTransaction.ReminderGDate,
                RemindDateH = hubTransaction.ReminderHDate,
                MainDocument = hubTransaction.MainDocument != null ? new DocumentInfo
                {
                    Id = hubTransaction.MainDocument.Id,
                    Name = hubTransaction.MainDocument.Name,
                    MimeType = hubTransaction.MainDocument.MimeType,
                    Size = hubTransaction.MainDocument.Size,
                    Document = new Document
                    {
                        Content = hubTransaction.MainDocument.Document?.Content
                    }
                } : null
            };
            return transaction;
        }

        private static AddOutboundExternalDTO MapTransactionToAddOutboundExternalDTO(Transaction transaction)
        {
            AddOutboundExternalDTO outboundExternalAddDTO = new AddOutboundExternalDTO()
            {
                OrgUnitId = transaction.OrgUnit.Id,
                OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO
                {
                    Remarks = transaction.Remarks,
                    Subject = transaction.Subject,
                    PriorityLevelId = transaction.PriorityId,
                    ConfidentialityLevelId = transaction.ConfidentialityId,
                    DestinationId = transaction.ExternalParty.Id,
                    OutboundNumber = transaction.Number,
                    RemindDate = transaction.RemindDate,
                    RemindDateH = transaction.RemindDateH
                },
                DocumentDTO = new DocumentDTO
                {
                    Content = transaction.MainDocument.Document.Content,
                    MimeType = "application/pdf",
                    Id = transaction.MainDocument.Id,
                    Name = transaction.MainDocument.Name,
                    Size = transaction.MainDocument.Size
                },
                RecordDate = transaction.Date,
                HijriRecordDate = transaction.DateH
            };

            IDocumentBL documentBL = IoC.Resolve<IDocumentBL>();

            outboundExternalAddDTO.Names = transaction.Names.Select(t => new TransactionNameDTO
            {
                Address = t.Name.Address,
                Email = t.Name.Email,
                FirstName = t.Name.FirstName,
                CivilID = t.Name.CivilID,
            }).ToList();

            outboundExternalAddDTO.Attachments = new List<TransactionAttachmentDTO>();
            foreach (var attachment in transaction.Attachments)
            {
                outboundExternalAddDTO.Attachments.Add(new TransactionAttachmentDTO
                {
                    DocumentDTO = attachment.Type.Archivable ? new DocumentDTO
                    {
                        Id = attachment.Id,
                        Content = (attachment.DocumentInfo.Document != null) ? documentBL.GetMainDocument(attachment.DocumentInfo.Id) : null,
                        MimeType = attachment.DocumentInfo.MimeType,
                        Name = attachment.DocumentInfo.Name,
                        Size = attachment.DocumentInfo.Size,
                    } : null,
                    Archivable = attachment.Type.Archivable,
                    Description = attachment.Description
                });
            }

            return outboundExternalAddDTO;
        }
        private AddOutboundExternalDTO MapHubTransactionToOutboundExternalDTO(HubTransaction hubTransaction)
        {
            AddOutboundExternalDTO outboundExternalAddDTO = new AddOutboundExternalDTO()
            {
                OrgUnitId = hubTransaction.OrgUnitId,
                OutboundExternalBasicInfo = new AddOutboundExternalBasicInfoDTO
                {
                    Remarks = hubTransaction.Remarks,
                    //Abu malek: review this method if needed at all as we transfer all the data
                    //after send confirm returns "success"
                    //and check the need for mapping in the next two lines.
                    //plus the HijriRecordDate line
                    //Subject = hubTransaction.Subject
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
}
