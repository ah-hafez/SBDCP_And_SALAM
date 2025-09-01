using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IConfidentialityAcknowledgmentRepository : IRepository<ConfidentialityAcknowledgment>
    {
        IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(SearchCriteria searchCriteria, out int rowsCount);
        IList<ConfidentialityAcknowledgment> GetConfidentialityAcknowledgments(string cultureName);
        bool CheckIfConfidentialityAcknowledgmentUsed(int attachmnetTypeId);
        void UpdateConfidentialityAcknowledgment(ConfidentialityAcknowledgment confidentialityAcknowledgments); 
        void LockUnlockLookup(int ConfidentialityAcknowledgmented, int UserId);
        void ActiveDeactiveLookup(int ConfidentialityAcknowledgmentId);
    }
}
