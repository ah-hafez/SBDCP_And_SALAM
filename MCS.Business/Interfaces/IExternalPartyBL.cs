using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.Business
{
    public interface IExternalPartyBL
    {
        //External Parties
        int AddExternalParty(ExternalParty externalParty);
        void UpdateExternalParty(ExternalParty externalParty);
        void DeleteParties(IList<int> ids, out IList<int> partiesCannotBeDeleted);
        ExternalParty GetExternalPartyById(int externalPartyId);
        string GetLastNumber(int ParentId);
        string GetLastNumberByCustomizeValue(string numberStartWithCustomizeValue);
        ExternalParty GetExternalPartyInfoByNumber(string partyNumber);
        bool CheckPartyNumber(string Number, int partyId = -1);
        IList<ExternalParty> GetExternalPartiesByParentId(int? parentId, string cultureName);
        IList<ExternalParty> GetExternalParties(int? parentId, string cultureName, bool getVirtual = false);
        IList<ExternalParty> GetAllExternalParties(int? parentId, string cultureName);
        IList<ExternalParty> GetExternalPartiesAutoComplete(string searchQuery, string cultureName, int resultSize);
        IList<ExternalParty> GetExternalPartyNodes(int? nodeId,string cultureName);
        IList<ExternalParty> GetExternalPartiesByLetterType(int letterId, int? parentId, string cultureName);
        IList<ExternalParty> GetExternalParties(SearchCriteria searchCriteria);
        //External Party Managers
        int AddExternalPartyManager(ExternalPartyManager externalPartyManager);
        void UpdateExternalPartyManager(ExternalPartyManager externalPartyManager);
        void DeleteExternalPartyManagers(IList<int> ids, out IList<int> managersCannotBeDeleted);
        ExternalPartyManager GetExternalPartyManagerById(int externalPartyManagerId);
        IList<ExternalPartyManager> GetExternalPartyManagers(int externalPartyId, SearchCriteria searchCriteria, out int rowsCount);
        IList<ExternalPartyManager> GetManagersByPartyId(int partyId, string cultureName);
        ExternalParty GetExternalPartiesByNumber(string number);
    }
}
