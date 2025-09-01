using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MCS.Domain;

namespace MCS.DataAccess
{
    public interface IReporterRepository : IRepository<Reporter>
    {
        List<Reporter> GetReporters(string cultureName, int orgUnitId);
        Reporter GetReporterById(int id, string cultureName);
        int AddReporter(Reporter reporter);
    }
}
