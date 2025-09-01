using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Linq;
using MCS.DTO;

namespace MCS.Business
{
    public class DocumentBL : BaseBL, IDocumentBL
    {
        public int AddDocument(DocumentInfo documentInfo)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                return documentRepository.AddDocument(documentInfo);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public DocumentInfo GetDocumentById(int documentId)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();

                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                DocumentInfo documentInfo = documentRepository.GetDocumentById(documentId, userWeight);
                if (documentInfo?.Document?.Content != null)
                {
                    documentInfo.Document.Content = Decrypt(documentInfo.Document.Content, "Q9s^4E%@", "Q9s^4E%@");
                }

                return documentInfo;
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void DeleteDocument(int documentId)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.DeleteDocument(documentId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateMainDocumentContent(int documentId, int TransactionId, byte[] content, string memType)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.UpdateMainDocumentContent(documentId, TransactionId, content, memType);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateDocumentByECMId(string ECMId, int documentId)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.UpdateDocumentByECMId(ECMId, documentId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public string GetECMIdByDocumentId(int documentId)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                return documentRepository.GetECMIdByDocumentId(documentId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public byte[] GetMainDocument(int documentId)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                return documentRepository.GetMainDocument(documentId, userWeight);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public List<DocumentInfo> GetAllDocuments(int pageSize)
        {
            try
            {
                IPermissionBL permissionBL = new PermissionBL();
                IList<Permission> permissions = permissionBL.GetUserPermissionsByGroupId(PermissionGroupName.TransactiosConfidentiality);

                int? userWeight = null;

                if (permissions != null)
                {
                    userWeight = permissions.Max(s => s.Weight);
                }
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                return documentRepository.GetAllDocuments(pageSize, userWeight);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void ClearMigratedDocumentBinary(int documentId)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.ClearMigratedDocumentBinary(documentId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public void UpdateMainDocumentContentWithDigitalSign(int documentId, byte[] content, bool IsDigitallySigned, string MimeContent)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.UpdateMainDocumentContentWithDigitalSign(documentId, content, IsDigitallySigned, MimeContent);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void UpdateDocumentContentByTransaction(int transactionId, byte[] content)
        {
            try
            {
                IDocumentRepository documentRepository = IoC.Resolve<DocumentRepository>();
                documentRepository.UpdateDocumentContentByTransaction(transactionId, content);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        private byte[] Decrypt(byte[] encrypted, string sKey, string sIV)
        {
            try
            {
                byte[] plain;
                byte[] Key = Encoding.Unicode.GetBytes(sKey);
                byte[] IV = Encoding.Unicode.GetBytes(sIV);
                using (MemoryStream mStream = new MemoryStream(encrypted))
                {
                    using (var decryptedStream = new MemoryStream())
                    {
                        using (AesCryptoServiceProvider aesProvider = new AesCryptoServiceProvider())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream(mStream,
                                aesProvider.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                            {
                                using (StreamReader stream = new StreamReader(cryptoStream))
                                {
                                    int data;
                                    while ((data = cryptoStream.ReadByte()) != -1)
                                        decryptedStream.WriteByte((byte)data);
                                }
                            }
                        }
                        decryptedStream.Position = 0;
                        plain = decryptedStream.ToArray();
                    }
                }
                return plain;
            }
            catch (Exception ex)
            {
                return encrypted;
            }


        }

      
    }
}
