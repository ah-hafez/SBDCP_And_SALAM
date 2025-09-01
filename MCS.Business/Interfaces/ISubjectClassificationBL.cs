using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ISubjectClassificationBL
    {
        IList<SubjectClassification> GetAllSubjectClassifications();
        void SaveSubjectClassifications(IList<SubjectClassification> subjectClassifications, out IList<int> subjectClassificationsUsed);
        IList<SubjectClassification> GetSubjectClassificationByOrgUnitId(int OrgUnitId, string cultureName);
        SubjectClassification GetSubjectClassificationById(int subjectClassificationId);
        void UpdateSubjectClassification(SubjectClassification subjectClassification);
    }
}
