using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework;
using MCS.Business;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class HubAttachmentBL : IHubAttachmentBL
    {
        public void Delete(int hubAttachmentId)
        {
            try
            {
                IHubAttachmentRepository hubAttachmentRepository = IoC.Resolve<IHubAttachmentRepository>();
                hubAttachmentRepository.Delete(hubAttachmentId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public DocumentInfo GetHubDocumentById(int documentId)
        {
            try
            {
                IHubAttachmentRepository documentRepository = IoC.Resolve<IHubAttachmentRepository>();
                return documentRepository.GetHubDocumentById(documentId);
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
    }
}
