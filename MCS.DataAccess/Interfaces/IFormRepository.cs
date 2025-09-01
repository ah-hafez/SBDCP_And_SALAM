using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IFormRepository : IRepository<Form>
    {
        int AddForm(Form form);
        void UpdateForm(Form form);
        void DeleteForm(int id);
        Form GetFormById(int formId);
        DocumentInfo GetContentByFormId(int formId);
        IList<Form> GetForms(SearchCriteria searchCriteria, out int rowsCount);
        IList<Form> GetOrgUnitForms(int orgUnitId, string cultureName);
        void LockUnlockLookup(int FormId, int UserId);
        void ActiveDeactiveLookup(int FormId);
    }
}
