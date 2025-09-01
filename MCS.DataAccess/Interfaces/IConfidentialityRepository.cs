using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Framework.Persistence;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IConfidentialityRepository
    {
        IList<ConfidentialityLevel> GetConfidentialities(SearchCriteria searchCriteria, int groupId, out int rowsCount);
    }
}
