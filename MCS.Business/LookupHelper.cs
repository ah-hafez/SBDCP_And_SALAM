using MCS.Common;
using MCS.DataAccess;
using MCS.Domain;
using MCS.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MCS.Business
{
    public class LookupHelper : ILookupHelper
    {
        public int GetLookupIdentity(int lookupInternalID, LookupCategory lookupCategory, string cultureName)
        {
            try
            {
                IList<Lookup> lookups = CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<Lookup>;

                if (lookups == null)
                {
                    ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();

                    lookups = lookupRepository.GetLookupItems((int)lookupCategory, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                }

                return lookups.Where(x=>x.EnumReference == lookupInternalID).Select(x=>x.Id).FirstOrDefault();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }

        public int GetLookupInternalID(int lookupID, LookupCategory lookupCategory, string cultureName)
        {
            try
            {
                IList<Lookup> lookups = CacheHelper.Get(CachedObjectsKey.Lookups + lookupCategory.ToString(), cultureName) as IList<Lookup>;

                if (lookups == null)
                {
                    ILookupRepository lookupRepository = IoC.Resolve<LookupRepository>();

                    lookups = lookupRepository.GetLookupItems((int)lookupCategory, cultureName);

                    CacheHelper.Insert(CachedObjectsKey.Lookups + lookupCategory.ToString(), lookups, cultureName);
                }

                return lookups.Where(x => x.Id == lookupID).Select(x => x.EnumReference.Value).FirstOrDefault();
            }
            catch (BusinessException)
            {
                throw;
            }
            catch (DataAccessException)
            {
                throw new BusinessException(StatusCode.GeneralError);
            }
            catch (Exception ex)
            {
                throw BusinessException.Translate(ex);
            }
        }
    }
}
