using System.Linq;
using MCS.Framework;
using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class TransactionCounter
    {
        private static readonly object _transaction = new object();
        private static readonly object _nextCounter = new object();
        private static volatile TransactionCounter _instance;

        private TransactionCounter()
        {
        }

        public static TransactionCounter Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_transaction)
                    {
                        if (_instance == null)

                            _instance = new TransactionCounter();
                    }
                }

                return _instance;
            }
        }

        public long Next(int OrgUnitId, TransactionCategories transactionCategories, int? transactionTypeId)
        {
            lock (_nextCounter)
            {
                int result;
                IOrgUnitBL OrgUnitBL = IoC.Resolve<IOrgUnitBL>();
                ICounterRepository counterRepository = IoC.Resolve<ICounterRepository>();

                OrgUnit OrgUnit = OrgUnitBL.GetOrgUnitById(OrgUnitId);

                if (OrgUnit.Counter == null)
                {
                    throw new BusinessException(StatusCode.NoCounter);
                }
                CounterDetail counterDetail = new CounterDetail();

                if (OrgUnit.Counter.IsGeneral)
                {
                    if (transactionTypeId != null)
                    {
                        counterDetail = OrgUnit.Counter.CounterDetails
                                                        .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == transactionTypeId))
                                                        .Select(c => c).FirstOrDefault();
                    }

                    if (counterDetail == null)
                    {
                        counterDetail = OrgUnit.Counter.CounterDetails
                                        .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == null))
                                        .Select(c => c).FirstOrDefault();
                    }

                }
                else
                {

                    counterDetail = OrgUnit.Counter.CounterDetails
                                    .Where(c => (c.TransactionCategories & transactionCategories) != 0)
                                    .Select(c => c).FirstOrDefault();

                    if (counterDetail != null)
                    {
                        if (transactionTypeId != null)
                        {
                            counterDetail = OrgUnit.Counter.CounterDetails
                                   .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == transactionTypeId))
                                   .Select(c => c).FirstOrDefault();
                        }

                        if (counterDetail == null)
                        {
                            counterDetail = OrgUnit.Counter.CounterDetails
                                            .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == null))
                                            .Select(c => c).FirstOrDefault();
                        }
                    }
                    else
                    {
                        Counter counter = counterRepository.GetGeneralCounter();
                        if (transactionTypeId != null)
                        {
                            counterDetail = counter.CounterDetails
                                                            .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == transactionTypeId))
                                                            .Select(c => c).FirstOrDefault();
                        }

                        if (counterDetail == null)
                        {
                            counterDetail = counter.CounterDetails
                                            .Where(c => (c.TransactionCategories & transactionCategories) != 0 && (c.TransactionTypeId == null))
                                            .Select(c => c).FirstOrDefault();
                        }

                        result = counterDetail.Count;

                        counterDetail.Count = result + 1;

                        counterRepository.UpdateCounter(counter);

                        return System.Convert.ToInt64(result);
                    }

                }

                result = counterDetail.Count;

                counterDetail.Count = result + 1;

                counterRepository.UpdateCounter(OrgUnit.Counter);

                return System.Convert.ToInt64(result);
            }
        }
    }
}
