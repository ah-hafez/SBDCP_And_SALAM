using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
  public  class SurveyBL : BaseBL, ISurveyBL
    {
        public List<SurveyQuestion> GetSurveyQuestions(int UserId, int OrgUnitId)
        {
            try
            {

                ISurveyRepository SurveyRepository = IoC.Resolve<SurveyRepository>();
                return SurveyRepository.GetSurveyQuestions(UserId, OrgUnitId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AddSurveyAnswer(IList<SurveyAnswer> SurveyAnswers)
        {
            try
            {
                ISurveyRepository SurveyRepository = IoC.Resolve<SurveyRepository>();
                 SurveyRepository.AddSurveyAnswer(SurveyAnswers);

            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void AddSurveyNotes(SurveyNote surveyNote)
        {
            try
            {
                ISurveyRepository SurveyRepository = IoC.Resolve<SurveyRepository>();
                SurveyRepository.AddSurveyNotes(surveyNote);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public void DeleteUserSurvey(int UserId, int OrgUnitId)
        {
            try
            {
                ISurveyRepository SurveyRepository = IoC.Resolve<SurveyRepository>();
                SurveyRepository.DeleteUserSurvey(UserId, OrgUnitId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
        public bool CheckUserFilledSurvey(int UserId, int OrgUnitId)
        {
            try
            {
                ISurveyRepository SurveyRepository = IoC.Resolve<SurveyRepository>();
              return  SurveyRepository.CheckUserFilledSurvey(UserId, OrgUnitId);
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

    }
}
