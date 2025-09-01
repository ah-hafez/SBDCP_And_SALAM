using System;
using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class ExternalPartyTypeListMapper
    {
        //NeedsRevision
        public static ExternalPartyType Map(IList<ExternalPartyListTypeDTO> externalPartyListTypeDTOs)
        {
            if (externalPartyListTypeDTOs == null || !externalPartyListTypeDTOs.Any())
            {
                return ExternalPartyType.None;
            }
            ExternalPartyType externalPartyType = ExternalPartyType.None;

            foreach (ExternalPartyListTypeDTO externalPartyListTypeDTO in externalPartyListTypeDTOs)
            {
                if (externalPartyListTypeDTO.IsSelected)
                {
                    externalPartyType = externalPartyType ^ (ExternalPartyType)externalPartyListTypeDTO.Id;
                }
            }

            return externalPartyType;
        }

        public static List<ExternalPartyListTypeDTO> Map(ExternalPartyType externalPartyType)
        {
            if (externalPartyType == ExternalPartyType.None)
            {
                return null;
            }
            List<ExternalPartyListTypeDTO> externalPartyListTypeDTOs = new List<ExternalPartyListTypeDTO>();

            foreach (ExternalPartyType item in Enum.GetValues(typeof(ExternalPartyType)))
            {
                ExternalPartyListTypeDTO externalPartyListTypeDTO = new ExternalPartyListTypeDTO();

                externalPartyListTypeDTO.Id = (int)item;

                if (Convert.ToBoolean((ExternalPartyType)externalPartyType & item))
                {
                    externalPartyListTypeDTO.IsSelected = true;
                }

                externalPartyListTypeDTOs.Add(externalPartyListTypeDTO);
            }

            return externalPartyListTypeDTOs;
        }
    }
}