using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common.TransactionContext;
using MCS.Domain;

namespace MCS.DataAccess
{
    public class SurveyRepository : BaseRepository<SurveyQuestion>, ISurveyRepository
    {
        #region Attributes



        #endregion Attributes

        #region Constructors

        public SurveyRepository(IAmbienTTransactionContextLocator ambienTTransactionContextLocator)
            : base(ambienTTransactionContextLocator)
        {

        }

        #endregion Constructors

        #region Methods

        public List<SurveyQuestion> GetSurveyQuestions(int UserId, int OrgUnitId)
        {
            try
            {
                List<SurveyQuestion> surveyQuestions = _oMCSDbContext.SurveyQuestions.ToList();
                return surveyQuestions;
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }


        public void AddSurveyAnswer(IList<SurveyAnswer> SurveyAnswers)
        {
            try
            {
                foreach (SurveyAnswer surveyAnswer in SurveyAnswers)
                {
                    surveyAnswer.CreatedOn = surveyAnswer.AnswerDate = DateTime.Now;
                    surveyAnswer.CreatedBy = surveyAnswer.UserId;

                    _oMCSDbContext.SurveyAnswers.Add(surveyAnswer);

                    _oMCSDbContext.SaveChanges();


                }

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public void AddSurveyNotes(SurveyNote surveyNote)
        {
            try
            {
                surveyNote.CreatedOn = surveyNote.NoteDate = DateTime.Now;
                surveyNote.CreatedBy = surveyNote.UserId;

                _oMCSDbContext.SurveyNotes.Add(surveyNote);

                _oMCSDbContext.SaveChanges();


            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        public void DeleteUserSurvey(int UserId, int OrgUnitId)
        {
            try
            {
                var SurveyAnswers = _oMCSDbContext.SurveyAnswers.Where(s => s.UserId == UserId).ToList();
                var SurveyNote = _oMCSDbContext.SurveyNotes.Where(s => s.UserId == UserId).SingleOrDefault();

                if (SurveyAnswers.Count > 0)
                {
                    foreach (var SurveyAnswer in SurveyAnswers)
                    {

                        _oMCSDbContext.SurveyAnswers.Remove(SurveyAnswer);
                        _oMCSDbContext.SaveChanges();

                    }
                }
                if (SurveyNote != null)
                {
                    _oMCSDbContext.SurveyNotes.Remove(SurveyNote);
                    _oMCSDbContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }
        public bool CheckUserFilledSurvey(int UserId, int OrgUnitId)
        {
            try
            {
                var SurveyNote = _oMCSDbContext.SurveyNotes.Where(s => s.UserId == UserId).FirstOrDefault();


                if (SurveyNote == null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw DataAccessException.Translate(ex);
            }
        }

        #endregion Methods
    }
}
