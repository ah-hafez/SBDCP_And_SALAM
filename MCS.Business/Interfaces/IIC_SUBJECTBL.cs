using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;
using MCS.Domain.IC;
using MCS.DTO;

namespace MCS.Business
{
    public interface IIC_SUBJECTBL
    {
        int AddIC_SUBJECT(Domain.IC_SUBJECT icSubject);
        void DeleteIC_SUBJECT(int id);
        int  UpdateIC_SUBJECT(Domain.IC_SUBJECT icSubject);
        IList<IC_SUBJECT> GetIC_SUBJECS(SearchCriteria searchCriteria, out int rowsCount, string cultureName);
        IList<IC_SUBJECT> GetAllIC_SUBJECS(string cultureName);
        IC_SUBJECT GetIC_SUBJECTById(int Id);
        void RemoveIC_SUBJECT_TRANSACTION(int transId, int ic_id);
        List<IC_CLASSIFICATION> GetClassificationTypes();
        IList<IC_SUBJECT> GetIC_SUBJECTByParentId(int? Id, string query);
        int AddIC_SUBJECT_TRANSACTION(IC_SUBJECTTransactionDTO icSubjectDTO);
        IC_SUBJECTS_TRANSACTION IC_GetTransaction(int transId);
    }
}
