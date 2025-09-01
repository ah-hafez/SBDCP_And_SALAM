using Refit;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.UI.Wrappers
{
    public interface IDocumentApi
    {
        [Get("/api/Document/GetDocumentById")]
        Task<GetResult<DocumentDTO>> GetDocumentById(string cultureName, int documentId);

        [Delete("/api/Document/DeleteDocument")]
        Task<DeleteResult> DeleteDocument(int documentId);
    }
}
