using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;


namespace MCS.UI.Wrappers
{
    public interface IDashboardApi
    {
        [Get("/api/Dashboard/GetDashboardData")]
        Task<GetResult<List<DashboardDTO>>> GetDashboardData(string cultureName);
    }
}
