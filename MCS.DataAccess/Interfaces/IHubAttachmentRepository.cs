using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IHubAttachmentRepository : IRepository<HubAttachment>
    {
        DocumentInfo GetHubDocumentById(int documentId);
    }
}
