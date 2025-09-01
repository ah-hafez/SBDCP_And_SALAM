using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework;
using MCS.Business;
using MCS.DataAccess;

namespace MCS.Business
{
    public class HubRelatedPersonBL : IHubRelatedPersonBL
    {
        public void Delete(int hubRelatedPersonId)
        {
            try
            {
                IHubRelatedPersonRepository hubRelatedPersonRepository = IoC.Resolve<IHubRelatedPersonRepository>();
                hubRelatedPersonRepository.Delete(hubRelatedPersonId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
