using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.Business
{
    public interface IFormBL
    {
        int AddForm(Form form);
        void UpdateForm(Form form);
        void DeleteForms(IList<int> ids);
        Form GetFormById(int formId);
        DocumentInfo GetContentByFormId(int formId);
        IList<Form> GetForms(SearchCriteria searchCriteria, out int rowsCount);
        IList<Form> GetOrgUnitForms(int organizationUnitId, string cultureName);
      
    }
}
