using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.Business
{
    public interface IConfidentialityAcknowledgmentsBL
    {
        int AddConfidentialityAcknowledgments(ConfidentialityAcknowledgment ConfidentialityAcknowledgments);
        void UpdateConfidentialityAcknowledgments(ConfidentialityAcknowledgment ConfidentialityAcknowledgments);
        void DeleteConfidentialityAcknowledgments(IList<int> ids, out IList<int> ConfidentialityAcknowledgmentssCannotBeDeleted);
        ConfidentialityAcknowledgment GetConfidentialityAcknowledgmentsById(int ConfidentialityAcknowledgmentsId);
        IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(SearchCriteria searchCriteria, out int rowsCount);
        IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(TransactionCategories transactionCategories, string cultureName); 
    }
}
