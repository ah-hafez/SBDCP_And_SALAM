using System.Collections.Generic;
using MCS.Domain;

namespace MCS.Business
{
    public interface IReporterBL
    {
        List<Reporter> GetReporters(string cultureName, int orgUnitId);
        Reporter GetReporterById(int id, string cultureName);
        int AddReporter(Reporter reporter);
    }
}
