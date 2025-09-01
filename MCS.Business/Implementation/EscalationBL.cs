using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{

    public class EscalationBL : BaseBL, IEscalationBL
    {
        public int AddEscalation(Escalation escalation)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                int priorityId = escalationRepository.AddEscalation(escalation);
                return priorityId;
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

        public void UpdateEscalation(Escalation escalation)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                escalationRepository.UpdateEscalation(escalation);
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

        public void DeleteEscalation(int id)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                Escalation escalation = escalationRepository.GetEscalationById(id);
                if (escalation == null)
                {
                    throw new BusinessException(StatusCode.GeneralError);
                }
              
                escalationRepository.DeleteEscalation(id);

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
      
        public Escalation GetEscalationById(int EscalationId)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                return escalationRepository.Get(EscalationId);
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

        public IList<Escalation> GetEscalationByPriority(int TransactionCategoryId, int PriorityId, string cultureName)
        {
            try
            {

                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();

                IList<Escalation> escalations = escalationRepository.GetEscalationByPriority(TransactionCategoryId, PriorityId, cultureName);

                return escalations.ToList();
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
        public IList<Escalation> GetEscalations(int TransactionCategoryId, string cultureName)
        {
            try
            {

                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();

                IList<Escalation> escalations = escalationRepository.GetEscalations(TransactionCategoryId, cultureName);

                return escalations.ToList();
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

        public int GetEscalationCategoryId(int EscalationId)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                int CategoryId = escalationRepository.GetEscalationCategoryId(EscalationId);
                return CategoryId;
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
        }  public int GetEscalationPriorityId(int EscalationId)
        {
            try
            {
                IEscalationRepository escalationRepository = IoC.Resolve<EscalationRepository>();
                int PriortyId = escalationRepository.GetEscalationPriorityId(EscalationId);
                return PriortyId;
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
