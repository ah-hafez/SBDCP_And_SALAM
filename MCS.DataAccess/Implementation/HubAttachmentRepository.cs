using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    class HubAttachmentRepository : BaseRepository<HubAttachment>, IHubAttachmentRepository
    {
        public HubAttachmentRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        public DocumentInfo GetHubDocumentById(int documentId)
        {
            try
            {
                HubAttachment hubAttachment = this.FindBy(d => d.Id == documentId);
                
                return hubAttachment.DocumentInfo;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
    }
}
