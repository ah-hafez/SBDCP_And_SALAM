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
    public class LookupsApiHelper
    {
        public ILookupsApi ILookupsClient { get; }
        public LookupsApiHelper()
        {
            ILookupsClient = ClientFactory.GetClient<ILookupsApi, ServiceHttpClientHandler>("http://localhost/MCS.Service", () => new ServiceHttpClientHandler());
        }

        public static async Task<PostResult> PostLookupItem(LookupDTO lookupDTO)
        {
            var client = new LookupsApiHelper();
            var result = await client.ILookupsClient.PostLookupItem(lookupDTO);
            return result;
        }

        public static async Task<GetResult<LookupDTO>> GetLookupItem(int lookupId, string cultureName)
        {
            var client = new LookupsApiHelper();
            var result = await client.ILookupsClient.GetLookupItem(lookupId, cultureName);
            return result;
        }
        public static async Task<GetResult<List<LookupDTO>>> GetLookupItems(string cultureName)
        {
            var client = new LookupsApiHelper();
            var result = await client.ILookupsClient.GetLookupItems(cultureName);
            return result;
        }
        public static async Task<GetResult<List<FormDTO>>> GetOrgUnitForms(int orgUnitId, string cultureName)
        {
            var client = new LookupsApiHelper();
            var result = await client.ILookupsClient.GetOrgUnitForms(orgUnitId, cultureName);
            return result;
        }
        
    }
}