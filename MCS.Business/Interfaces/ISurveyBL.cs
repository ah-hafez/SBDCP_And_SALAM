using System.Collections.Generic;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.Business
{
    public interface ISurveyBL
    {

        List<SurveyQuestion> GetSurveyQuestions(int UserId, int OrgUnitId);
        void AddSurveyAnswer(IList<SurveyAnswer> SurveyAnswers);
        void AddSurveyNotes(SurveyNote surveyNote);
        void DeleteUserSurvey(int UserId, int OrgUnitId);
        bool CheckUserFilledSurvey(int UserId, int OrgUnitId);
    }
}
