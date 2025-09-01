using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.Business
{
    public interface IHubAttachmentBL
    {
        void Delete(int hubAttachmentId);
        DocumentInfo GetHubDocumentById(int documentId);
    }
}
