using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using YESSER.NCS.MCM.Domain;
using YESSER.NCS.MCM.DTO;
using YESSER.NCS.MCM.Tenants.UI;

namespace YESSER.NCS.MCM.Tenants.Service.Mappers
{
    public class CultureMapper
    {
        public static List<CultureDTO> MapCulture(IList<TenantCulture> cultures)
        {
            List<CultureDTO> cultureDTOs = new List<CultureDTO>();

            foreach (TenantCulture tenantCulture in cultures)
            {
                cultureDTOs.Add(CultureMapper.MapCulture(tenantCulture));
            }

            return cultureDTOs;
        }


        private static CultureDTO MapCulture(TenantCulture culture)
        {
            TenantLookupLocalization tenantLookupLocalization = culture.Name.Localizations.Where(l => l.Culture.ShortName == SessionInfo.CultureShortName).FirstOrDefault();

            CultureDTO cultureDTO = new CultureDTO()
            {
                Id = culture.ID,
                ShortName = culture.ShortName,
                LocalName = (tenantLookupLocalization != null) ? tenantLookupLocalization.Text : string.Empty
            };

            return cultureDTO;
        }
        //public static List<TenantCultureVM> Map(IList<CultureDTO> cultureDTOs)
        //{
        //    List<TenantCultureVM> tenantCultureVMs = cultureDTOs.Select(
        //     cultureDTO => new TenantCultureVM()
        //     {
        //         Id = cultureDTO.Id,
        //         ShortName = cultureDTO.ShortName,
        //         LocalName = cultureDTO.LocalName
        //     }).ToList();

        //    return tenantCultureVMs;
        //}
    }
}