using MCS.Business;
using MCS.DTO;
using MCS.Framework;
using MCS.IntegrationServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.IntegrationServices.Mappers
{
    public static class OrgUnitMapper
    {
        public static IList<OrgUnitModel> Map(IList<OrgUnitDTO> orgUnitDTOs)
        {
            if (orgUnitDTOs == null || !orgUnitDTOs.Any())
            {
                return null;
            }
            List<OrgUnitModel> orgUnits = orgUnitDTOs.Select(orgUnitDTO => new OrgUnitModel()
            {
                Id = orgUnitDTO.Id,
                Number = orgUnitDTO.Number

            }).ToList();



            return orgUnits;
        }



    }
}