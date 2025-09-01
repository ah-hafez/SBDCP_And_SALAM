using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IExternalPartyRepository : IRepository<ExternalParty>
    {
        int AddExternalParty(ExternalParty externalParty);

        void UpdateExternalParty(ExternalParty externalParty);
        void DeleteParty(int id);
        ExternalParty GetExternalPartyById(int externalPartyId);

        ExternalParty GetExternalPartyInfoByNumber(string partyNumber);
        bool CheckPartyNumber(string Number, int partyId = -1);
        IList<ExternalParty> GetExternalParties(int? parentId, string cultureName, bool getVirtual = false);
        IList<ExternalParty> GetAllExternalParties(int? parentId, string cultureName);
        IList<ExternalParty> GetExternalPartiesAutoComplete(string searchQuery, string cultureName, int resultSize);
        IList<ExternalParty> GetExternalPartyNodes(int? nodeId, string cultureName);
        IList<ExternalParty> GetExternalPartiesByParentId(int parentId);
        IList<ExternalParty> GetExternalPartiesByParentId(int? parentId, string cultureName);
        IList<ExternalParty> GetExternalParties(SearchCriteria searchCriteria);
        //Managers
        int AddManager(ExternalPartyManager manager);
        void UpdateManager(ExternalPartyManager manager);
        void DeleteManager(int id);
        ExternalPartyManager GetExternalPartyManagerById(int externalPartyManagerId);
        IList<ExternalPartyManager> GetExternalPartyManagers(int externalPartyId, SearchCriteria searchCriteria, out int rowsCount);
        IList<ExternalPartyManager> GetAllExternalPartyManagers(int externalPartyId, string cultureName);
        IList<ExternalParty> GetExternalPartiesByLetterId(LetterListType letterType, int? parentId, string cultureName);
        IList<ExternalParty> UserMobileGetExternalParties(int? parentId, string cultureName);
        ExternalParty GetExternalPartiesByNumber(string Number);
        IList<ExternalParty> UserMobileGetExternalPartiesAC(string searchQuery, string cultureName, int resultSize);
        string GetLastNumber(int ParentId);
        string GetLastNumberByCustomizeValue(string numberStartWithCustomizeValue);
    }
}
