using System;
using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IExplanationRepository : IRepository<Explanation>
    {
        int AddExplanation(Transaction transaction, Explanation explanation);
        void DeleteExplanation(int explanationId);
        void UpdateExplanation(Explanation explanation);
        IList<Explanation> GetExplanationsByTransactionId(int transactionId, int userId, string cultureName);
        Explanation GetExplanationById(int explanationId, string cultureName);
        Explanation GetExplanationByDocumentId(int DocumentId, string cultureName);
        IList<Explanation> GetExplanations(Func<Explanation, bool> where);
        IList<Explanation> GetExplanationsByTransactionIdWithoutContent(int transactionId, int userId, string cultureName);
    }
}
