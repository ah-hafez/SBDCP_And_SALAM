using Refit;
using System.Collections.Generic;
using System.Threading.Tasks;
using MCS.Common.ApiControllerResults;
using MCS.DTO;

namespace MCS.UI.Wrappers
{
    public interface ILookupsApi
    {
        [Post("/api/Lookups/PostLookupItem")]
        Task<PostResult> PostLookupItem(LookupDTO lookupDTO);

        [Get("/api/Lookups/GetLookupItem")]
        Task<GetResult<LookupDTO>> GetLookupItem(int lookupId, string cultureName);

        [Get("/api/Lookups/GetLookupItems")]
        Task<GetResult<List<LookupDTO>>> GetLookupItems(string cultureName);

        [Get("/api/Lookups/GetOrgUnitForms")]
        Task<GetResult<List<FormDTO>>> GetOrgUnitForms(int orgUnitId, string cultureName);

       
    }
}
