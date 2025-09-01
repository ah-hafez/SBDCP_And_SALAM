using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework;
using MCS.Framework.Persistence;
using MCS.DataAccess;
using MCS.Domain;

namespace MCS.Business
{
    public class ConfidentialityBL : BaseBL, IConfidentialityBL
    {
        public IList<ConfidentialityLevel> GetConfidentialities(SearchCriteria searchCriteria, int groupId, out int rowsCount)
        {
            IConfidentialityRepository confidentialityRepository = IoC.Resolve<ConfidentialityRepository>();
            return confidentialityRepository.GetConfidentialities(searchCriteria, groupId, out rowsCount);
        }
    }
}
