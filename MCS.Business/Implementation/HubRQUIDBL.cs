using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class HubRQUIDBL : IHubRQUIDBL
    {
        public long GetByTransactionNumberByRQUID(string rQUID)
        {
            try
            {
                IHubRQUIDRepository hubRQUIDRepository = IoC.Resolve<IHubRQUIDRepository>();
                return hubRQUIDRepository.GetByTransactionNumberByRQUID(rQUID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int Add(HubRQUID hubRQUID)
        {
            try
            {
                IHubRQUIDRepository hubTransactionRepository = IoC.Resolve<IHubRQUIDRepository>();
                var hubTransactionId = hubTransactionRepository.Add(hubRQUID);
                return hubTransactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public HubRQUID GetByRQUID(string rQUID)
        {
            try
            {
                IHubRQUIDRepository hubRQUIDRepository = IoC.Resolve<IHubRQUIDRepository>();
                return hubRQUIDRepository.GetByRQUID(rQUID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
