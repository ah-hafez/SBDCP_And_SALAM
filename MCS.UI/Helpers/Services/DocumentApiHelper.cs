using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class DocumentApiHelper
    {
        public IDocumentApi DocumentApiClient { get; }
        public DocumentApiHelper()
        {
            DocumentApiClient = ClientFactory.GetClient<IDocumentApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<GetResult<DocumentDTO>> GetDocumentById(string cultureName, int documentId)
        { 
            var client = new DocumentApiHelper();
            var result = await client.DocumentApiClient.GetDocumentById(cultureName, documentId);
            return result;
        }
        public static async Task<DeleteResult> DeleteDocument(int documentId)
        {
            var client = new DocumentApiHelper();
            var result = await client.DocumentApiClient.DeleteDocument(documentId);
            return result;
        }
    }
}