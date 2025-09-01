using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Wrappers;

namespace MCS.UI.Helpers.Services
{
    public class DashboardApiHelper
    {
        public IDashboardApi DashboardApiClient { get; }
        public DashboardApiHelper()
        {
            DashboardApiClient = ClientFactory.GetClient<IDashboardApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<GetResult<List<DashboardDTO>>> GetDashboardData(string cultureName)
        {
            var client = new DashboardApiHelper();
            var result = await client.DashboardApiClient.GetDashboardData(cultureName);
            return result;
        }
    }
}