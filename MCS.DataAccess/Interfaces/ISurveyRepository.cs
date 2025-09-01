using System.Collections.Generic;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface ISurveyRepository : IRepository<SurveyQuestion>
    {
        List<SurveyQuestion> GetSurveyQuestions(int UserId, int OrgUnitId);
        void AddSurveyAnswer(IList<SurveyAnswer> SurveyAnswers);
        void AddSurveyNotes(SurveyNote surveyNote);
        void DeleteUserSurvey(int UserId, int OrgUnitId);
        bool CheckUserFilledSurvey(int UserId, int OrgUnitId);
    }
}
