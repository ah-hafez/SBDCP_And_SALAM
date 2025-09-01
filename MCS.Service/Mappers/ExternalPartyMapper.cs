using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class ExternalPartyMapper
    {
        public static ExternalParty Map(ExternalPartyAddDTO externalPartyAddDTO)
        {
            if (externalPartyAddDTO == null)
                return null;
            ExternalParty externalParty = new ExternalParty
            {
                Number = externalPartyAddDTO.PartyNumber,
                Email = externalPartyAddDTO.Email,
                PhoneNumber = externalPartyAddDTO.PhoneNumber,
                Fax = externalPartyAddDTO.FaxNumber,
                IsVirtual = externalPartyAddDTO.IsVirtual,
                ParentId = externalPartyAddDTO.ParentId,
                Name = LocalizationIdentifierMapper.Map(externalPartyAddDTO.Name),
                Address = MapAddress(externalPartyAddDTO.Address),
                PartyType = ExternalPartyTypeListMapper.Map(externalPartyAddDTO.Types),
                YasserRegistered = externalPartyAddDTO.YasserRegistered,
            };

            return externalParty;
        }

        public static ExternalParty Map(ExternalPartyEditDTO externalPartyEditDTO)
        {
            if (externalPartyEditDTO == null)
                return null;
            ExternalParty externalParty = new ExternalParty
            {
                Id = externalPartyEditDTO.Id,
                Number = externalPartyEditDTO.PartyNumber,
                Email = externalPartyEditDTO.Email,
                PhoneNumber = externalPartyEditDTO.PhoneNumber,
                Fax = externalPartyEditDTO.FaxNumber,
                IsVirtual = externalPartyEditDTO.IsVirtual,
                ParentId = externalPartyEditDTO.ParentId,
                Name = LocalizationIdentifierMapper.Map(externalPartyEditDTO.Name),
                Address = MapAddress(externalPartyEditDTO.Address),
                PartyType = ExternalPartyTypeListMapper.Map(externalPartyEditDTO.Types)
            };

            return externalParty;
        }

        public static ExternalPartyEditDTO Map(ExternalParty externalParty)
        {
            if (externalParty == null)
                return null;
            ExternalPartyEditDTO externalPartyEditDTO = new ExternalPartyEditDTO
            {
                Id = externalParty.Id,
                PartyNumber = externalParty.Number,
                Email = externalParty.Email,
                PhoneNumber = externalParty.PhoneNumber,
                FaxNumber = externalParty.Fax,
                IsVirtual = externalParty.IsVirtual,
                Name = LocalizationIdentifierMapper.Map(externalParty.Name?.Localizations),
                Address = (externalParty.Address != null ? MapAddress(externalParty.Address.Localizations) : new List<AddressDTO>()),
                Types = ExternalPartyTypeListMapper.Map(externalParty.PartyType),
                ParentId = externalParty.ParentId,
                IsYesserRegistered = externalParty.YasserRegistered
            };

            return externalPartyEditDTO;
        }

        public static List<ExternalPartyDTO> Map(IList<ExternalParty> externalParties)
        {
            if (externalParties == null || !externalParties.Any())
            {
                return new List<ExternalPartyDTO>();
            }
            List<ExternalPartyDTO> externalPartyDTOs = new List<ExternalPartyDTO>();

            foreach (ExternalParty externalParty in externalParties)
            {
                externalPartyDTOs.Add(ExternalPartyMapper.MapParty(externalParty));
            }

            return externalPartyDTOs;
        }


        public static List<ExternalParty> Map(IList<ExternalPartyDTO> externalPartiesDTO)
        {
            if (externalPartiesDTO == null || !externalPartiesDTO.Any())
            {
                return null;
            }
            List<ExternalParty> externalPartys = new List<ExternalParty>();

            foreach (ExternalPartyDTO externalPartyDTO in externalPartiesDTO)
            {
                externalPartys.Add(ExternalPartyMapper.MapParty(externalPartyDTO));
            }

            return externalPartys;
        }

        public static List<ExternalPartyDTO> MapWithParentOrganization(IList<ExternalParty> externalParties)
        {
            if (externalParties == null || !externalParties.Any())
            {
                return null;
            }
            List<ExternalPartyDTO> externalPartyDTOs = new List<ExternalPartyDTO>();

            foreach (ExternalParty externalParty in externalParties)
            {
                if (externalPartyDTOs.Where(e => e.Id == externalParty.Id).FirstOrDefault() == null)
                {
                    externalPartyDTOs.Add(ExternalPartyMapper.MapParty(externalParty));
                }

                if (externalParty.Parent != null
                    && externalPartyDTOs.Where(e => e.Id == externalParty.Parent.Id).FirstOrDefault() == null)
                {
                    externalPartyDTOs.Add(ExternalPartyMapper.MapParty(externalParty));
                }
            }

            return externalPartyDTOs;
        }

        private static ExternalPartyDTO MapParty(ExternalParty externalParty)
        {
            if (externalParty == null)
                return null;
            ExternalPartyDTO externalPartyDTO = new ExternalPartyDTO
            {
                Id = externalParty.Id,
                Number = externalParty.Number,
                LocalName = externalParty.LocalName,
                ParentId = externalParty.ParentId,
                HasChilds = externalParty.HasChilds,
                IsVirtual = externalParty.IsVirtual,
                YasserRegistered = externalParty.YasserRegistered,
                Lineage = externalParty.Lineage,
                Email = externalParty.Email
            };

            if (externalParty.Name != null)
            {
                externalPartyDTO.Name = LocalizationIdentifierMapper.Map(externalParty.Name.Localizations);
            }

            return externalPartyDTO;
        }


        private static ExternalParty MapParty(ExternalPartyDTO externalPartyDTO)
        {
            if (externalPartyDTO == null)
                return null;
            ExternalParty externalParty = new ExternalParty
            {
                Id = externalPartyDTO.Id,
                Number = externalPartyDTO.Number,
                LocalName = externalPartyDTO.LocalName,
                ParentId = externalPartyDTO.ParentId,
                HasChilds = externalPartyDTO.HasChilds,
                IsVirtual = externalPartyDTO.IsVirtual,
                YasserRegistered = externalPartyDTO.YasserRegistered,
                Lineage = externalPartyDTO.Lineage
            };

            if (externalPartyDTO.Name != null)
            {
                externalParty.Name = LocalizationIdentifierMapper.Map(externalPartyDTO.Name);

            }

            return externalParty;
        }

        public static LocalizationIdentifier MapAddress(List<AddressDTO> addressDTOs)
        {
            if (addressDTOs == null || !addressDTOs.Any())
            {
                return null;
            }
            LocalizationIdentifier identifier = new LocalizationIdentifier();
            IList<Localization> localizations = new List<Localization>();

            //ICommonBL commonBL = IoC.Resolve<ICommonBL>();

            foreach (AddressDTO addressDTO in addressDTOs)
            {
                Localization localization = new Localization()
                {
                    Id = addressDTO.Id,
                    LocalizationIdentifier = identifier,
                    Text = addressDTO.Text,
                    CultureId = addressDTO.CultureId //commonBL.GetCultureById(addressDTO.CultureId)
                };

                localizations.Add(localization);
            }

            identifier.Localizations = localizations;

            return identifier;
        }

        public static List<AddressDTO> MapAddress(IList<Localization> localizations)
        {
            if (localizations == null || !localizations.Any())
            {
                return null;
            }
            List<AddressDTO> addressDTOs = new List<AddressDTO>();

            ICommonBL commonBL = IoC.Resolve<ICommonBL>();

            foreach (Localization localization in localizations)
            {
                Culture culture = commonBL.GetCultureById(localization.Culture.Id);

                AddressDTO addressDTO = new AddressDTO()
                {
                    Id = localization.Id,
                    Text = localization.Text,
                    CultureId = culture.Id,
                    CultureName = culture.ShortName
                };

                addressDTOs.Add(addressDTO);
            }

            return addressDTOs;
        }

    }
}