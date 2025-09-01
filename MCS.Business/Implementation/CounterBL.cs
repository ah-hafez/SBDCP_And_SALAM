using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class CounterBL : BaseBL, ICounterBL
    {
        public void UpdateCounter(Counter counter)
        {
            try
            {

                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();

                counterRepository.UpdateCounter(counter);
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

        public Counter GetCounterById(int counterId)
        {
            try
            {
                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();

                return counterRepository.Get(counterId);
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

        public Counter GetGeneralCounter()
        {
            try
            {
                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();

                Counter counter = counterRepository.GetGeneralCounter();

                return counter;
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

        public CounterDetail GetCounterDetailById(int counterDetailId)
        {
            try
            {
                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();

                return counterRepository.GetCounterDetailById(counterDetailId);
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

        public IList<CounterDetail> GetCounterDetailsByCounterId(int counterId)
        {
            try
            {
                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();

                return counterRepository.GetCounterDetailsByCounterId(counterId);
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

        public void DeleteCounterDetailById(int counterDetailId)
        {
            try
            {
                ICounterRepository counterRepository = IoC.Resolve<CounterRepository>();
                counterRepository.DeleteCounterDetailById(counterDetailId);
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
