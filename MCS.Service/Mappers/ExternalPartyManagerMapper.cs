using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class ExternalPartyManagerMapper
    {
        public static ExternalPartyManager Map(ManagerAddDTO managerAddDTO)
        {
            if (managerAddDTO == null)
                return null;
            IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

            ExternalPartyManager externalPartyManager = new ExternalPartyManager()
            {
                Name = LocalizationIdentifierMapper.Map(managerAddDTO.Name),
                ExternalParty = externalPartyBL.GetExternalPartyById(managerAddDTO.PartyId),
                EmailAddress = managerAddDTO.EmailAddress
            };

            return externalPartyManager;
        }

        public static ExternalPartyManager Map(ManagerEditDTO managerEditDTO)
        {
            if (managerEditDTO == null)
                return null;
            IExternalPartyBL externalPartyBL = IoC.Resolve<IExternalPartyBL>();

            ExternalPartyManager externalPartyManager = new ExternalPartyManager()
            {
                Id = managerEditDTO.Id,
                Name = LocalizationIdentifierMapper.Map(managerEditDTO.Name),
                ExternalParty = externalPartyBL.GetExternalPartyById(managerEditDTO.PartyId),
                EmailAddress = managerEditDTO.EmailAddress
            };

            return externalPartyManager;
        }

        public static ManagerEditDTO Map(ExternalPartyManager externalPartyManager)
        {
            if (externalPartyManager == null)
                return null;
            ManagerEditDTO managerEditDTO = new ManagerEditDTO()
            {
                Id = externalPartyManager.Id,
                Name = LocalizationIdentifierMapper.Map(externalPartyManager.Name.Localizations),
                PartyId = externalPartyManager.ExternalParty.Id,
                EmailAddress = externalPartyManager.EmailAddress
            };

            return managerEditDTO;
        }

        public static List<ManagerDTO> Map(IList<ExternalPartyManager> externalPartyManagers)
        {
            if (externalPartyManagers == null || !externalPartyManagers.Any())
            {
                return null;
            }
            List<ManagerDTO> managerDTOs = externalPartyManagers
                .Select(managerDTO => new ManagerDTO()
                {
                    Id = managerDTO.Id,
                    AddedDate = managerDTO.CreatedOn,
                    LocalName = managerDTO.LocalName,
                    PartyId = managerDTO.ExternalParty != null ? managerDTO.ExternalParty.Id : 0,
                    Name = managerDTO.Name != null ? LocalizationIdentifierMapper.Map(managerDTO.Name.Localizations) : null,
                    EmailAddress = managerDTO.EmailAddress
                }).ToList();

            return managerDTOs;
        }
    }
}