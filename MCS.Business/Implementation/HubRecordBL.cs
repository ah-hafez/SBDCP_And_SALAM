using System;
using System.Collections.Generic;
using MCS.Framework;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class HubRecordBL : IHubRecordBL
    {
        public int Add(HubRecord hubRecord)
        {
            try
            {
                IHubRecordRepository hubTransactionRepository = IoC.Resolve<IHubRecordRepository>();
                var hubTransactionId = hubTransactionRepository.Add(hubRecord);
                return hubTransactionId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
