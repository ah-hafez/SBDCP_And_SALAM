using System.Collections.Generic;
using MCS.Domain;


namespace MCS.DataAccess
{
    public interface ICounterRepository : IRepository<Counter>
    {
        void UpdateCounter(Counter counter);
        Counter GetCounterById(int counterId);
        Counter GetGeneralCounter();
        CounterDetail GetCounterDetailById(int counterDetailId);
        void DeleteCounterDetailById(int counterDetailId);
        IList<CounterDetail> GetCounterDetailsByCounterId(int counterId);   
    }
}
