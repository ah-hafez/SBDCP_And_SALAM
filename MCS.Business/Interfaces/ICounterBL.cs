using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface ICounterBL
    {
        void UpdateCounter(Counter counter);
        Counter GetCounterById(int counterId);
        Counter GetGeneralCounter();
        CounterDetail GetCounterDetailById(int counterDetailId);
        void DeleteCounterDetailById(int counterDetailId);
        IList<CounterDetail> GetCounterDetailsByCounterId(int counterId);
    }
}
