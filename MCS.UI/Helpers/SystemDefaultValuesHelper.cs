using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Lookups;
using System.Collections.Generic;
using System.Linq;

namespace MCS.UI
{
    public class SystemDefaultValuesHelper
    {
        private static List<SystemDefaultValuesVM> GetSystemDefaultValues()
        {
            List<SystemDefaultValuesVM> defaultValuesVMs = CacheHelper.Get(CachedObjectsKey.SystemDefaultValues, SessionInfo.CultureShortName) as List<SystemDefaultValuesVM>;

            if (defaultValuesVMs == null)
            {
                GetResult<List<SystemDefaultValuesDTO>> defaultValuesDTOResult = HttpClientWrapper<GetResult<List<SystemDefaultValuesDTO>>>.GetItemRequest(string.Format("api/Transaction/GetSystemDefaultValues")).Result;
                defaultValuesVMs = SystemDefaultValuesMapper.Map(defaultValuesDTOResult.Result);

                CacheHelper.Insert(CachedObjectsKey.SystemDefaultValues, defaultValuesVMs, SessionInfo.CultureShortName);
            }

            return defaultValuesVMs;
        }

        public static int? GetDefaultValue(TransactionCategories transactionCategory, CategoryTypes categoryType)
        {
            List<SystemDefaultValuesVM> systemDefaultValuesVMs = GetSystemDefaultValues();
            SystemDefaultValuesVM SystemDefaultValuesVM = systemDefaultValuesVMs.FirstOrDefault(p => (p.CategoryId & (int)transactionCategory) != 0 && p.TypeId == (int)categoryType);

            return SystemDefaultValuesVM?.DefaultValueId;
        }
    }
}
