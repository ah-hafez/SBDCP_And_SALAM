using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISubjectClassificationRepository : IRepository<SubjectClassification>
    {
        IList<SubjectClassification> GetAllSubjectClassifications();
        void UpdateSubjectClassification(SubjectClassification subjectClassification);
        int AddSubjectClassification(SubjectClassification subjectClassification);
        void DeleteSubjectClassification(int id);
        IList<SubjectClassification> GetSubjectClassificationByOrgUnitId(int orgUnitId, string cultureName);
        SubjectClassification GetSubjectClassificationById(int subjectClassificationId);
    }
}
