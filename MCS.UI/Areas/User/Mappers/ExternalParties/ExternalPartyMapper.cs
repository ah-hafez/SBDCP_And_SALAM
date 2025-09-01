using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.ExternalParties;

namespace MCS.UI.Areas.User.Mappers.ExternalParties
{
    public static class ExternalPartyMapper
    {
        public static List<ExternalPartyAddVM> Map(IList<ExternalPartyAddDTO> externalPartyAddDTOs)
        {
            if (externalPartyAddDTOs == null || !externalPartyAddDTOs.Any())
            {
                return new List<ExternalPartyAddVM>();
            }
            List<ExternalPartyAddVM> externalPartyAddVMs = externalPartyAddDTOs
                .Select(externalPartyAddDTO => new ExternalPartyAddVM()
                {
                    PartyNumber = externalPartyAddDTO.PartyNumber,
                    Address = AddressMapper.Map(externalPartyAddDTO.Address),
                    Email = externalPartyAddDTO.Email,
                    FaxNumber = externalPartyAddDTO.FaxNumber,
                    IsVirtual = externalPartyAddDTO.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyAddDTO.Name),
                    ParentId = externalPartyAddDTO.ParentId,
                    PhoneNumber = externalPartyAddDTO.PhoneNumber,
                    Types = Map(externalPartyAddDTO.Types),
                    YasserRegistered = externalPartyAddDTO.YasserRegistered
                }).ToList();

            return externalPartyAddVMs;
        }
        public static List<ExternalPartyAddDTO> Map(IList<ExternalPartyAddVM> externalPartyAddVMs)
        {
            if (externalPartyAddVMs == null || !externalPartyAddVMs.Any())
            {
                return new List<ExternalPartyAddDTO>();
            }
            List<ExternalPartyAddDTO> externalPartyAddDTOs = externalPartyAddVMs
                .Select(externalPartyAddVM => new ExternalPartyAddDTO()
                {
                    PartyNumber = externalPartyAddVM.PartyNumber,
                    Address = AddressMapper.Map(externalPartyAddVM.Address),
                    Email = externalPartyAddVM.Email,
                    FaxNumber = externalPartyAddVM.FaxNumber,
                    IsVirtual = externalPartyAddVM.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyAddVM.Name),
                    ParentId = externalPartyAddVM.ParentId,
                    PhoneNumber = externalPartyAddVM.PhoneNumber,
                    Types = Map(externalPartyAddVM.Types),
                    YasserRegistered = externalPartyAddVM.YasserRegistered
                }).ToList();

            return externalPartyAddDTOs;
        }
        public static List<ExternalPartyEditDTO> Map(IList<ExternalPartyEditVM> externalPartyEditVMs)
        {
            if (externalPartyEditVMs == null || !externalPartyEditVMs.Any())
            {
                return new List<ExternalPartyEditDTO>();
                ;
            }
            List<ExternalPartyEditDTO> externalPartyEditDTOs = externalPartyEditVMs
                .Select(externalPartyEditVM => new ExternalPartyEditDTO()
                {
                    Id = externalPartyEditVM.Id,
                    PartyNumber = externalPartyEditVM.PartyNumber,
                    Address = AddressMapper.Map(externalPartyEditVM.Address),
                    Email = externalPartyEditVM.Email,
                    FaxNumber = externalPartyEditVM.FaxNumber,
                    IsVirtual = externalPartyEditVM.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyEditVM.Name),
                    ParentId = externalPartyEditVM.ParentId,
                    PhoneNumber = externalPartyEditVM.PhoneNumber,
                    Types = Map(externalPartyEditVM.Types)
                }).ToList();

            return externalPartyEditDTOs;
        }
        public static ExternalPartyEditDTO Map(ExternalPartyEditVM externalPartyEditVM)
        {
            if (externalPartyEditVM != null)
            {
                return new ExternalPartyEditDTO()
                {
                    Id = externalPartyEditVM.Id,
                    PartyNumber = externalPartyEditVM.PartyNumber,
                    Address = AddressMapper.Map(externalPartyEditVM.Address),
                    Email = externalPartyEditVM.Email,
                    FaxNumber = externalPartyEditVM.FaxNumber,
                    IsVirtual = externalPartyEditVM.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyEditVM.Name),
                    ParentId = externalPartyEditVM.ParentId,
                    PhoneNumber = externalPartyEditVM.PhoneNumber,
                    Types = Map(externalPartyEditVM.Types)
                };
            }
            return new ExternalPartyEditDTO();
        }
        public static ExternalPartyEditVM Map(ExternalPartyEditDTO externalPartyEditDTO)
        {
            if (externalPartyEditDTO != null)
            {
                return new ExternalPartyEditVM()
                {
                    Id = externalPartyEditDTO.Id,
                    PartyNumber = externalPartyEditDTO.PartyNumber,
                    Address = AddressMapper.Map(externalPartyEditDTO.Address),
                    Email = externalPartyEditDTO.Email,
                    FaxNumber = externalPartyEditDTO.FaxNumber,
                    IsVirtual = externalPartyEditDTO.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyEditDTO.Name),
                    ParentId = externalPartyEditDTO.ParentId,
                    PhoneNumber = externalPartyEditDTO.PhoneNumber,
                    Types = Map(externalPartyEditDTO.Types),
                    IsYesserRegistered = externalPartyEditDTO.IsYesserRegistered
                };
            }
            return new ExternalPartyEditVM();
        }
        public static List<ExternalPartyEditVM> Map(IList<ExternalPartyEditDTO> externalPartyEditDTOs)
        {
            if (externalPartyEditDTOs == null || !externalPartyEditDTOs.Any())
            {
                return new List<ExternalPartyEditVM>();
            }
            List<ExternalPartyEditVM> externalPartyEditVMs = externalPartyEditDTOs
                .Select(externalPartyEditDTO => new ExternalPartyEditVM()
                {
                    Id = externalPartyEditDTO.Id,
                    PartyNumber = externalPartyEditDTO.PartyNumber,
                    Address = AddressMapper.Map(externalPartyEditDTO.Address),
                    Email = externalPartyEditDTO.Email,
                    FaxNumber = externalPartyEditDTO.FaxNumber,
                    IsVirtual = externalPartyEditDTO.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyEditDTO.Name),
                    ParentId = externalPartyEditDTO.ParentId,
                    PhoneNumber = externalPartyEditDTO.PhoneNumber,
                    Types = Map(externalPartyEditDTO.Types)
                }).ToList();

            return externalPartyEditVMs;
        }
        public static List<ExternalPartyVM> Map(IList<ExternalPartyDTO> externalPartyDTOs)
        {
            if (externalPartyDTOs == null || !externalPartyDTOs.Any())
            {
                return new List<ExternalPartyVM>();
            }
            List<ExternalPartyVM> externalPartyVMs = externalPartyDTOs
                .Select(externalPartyDTO => new ExternalPartyVM()
                {
                    Id = externalPartyDTO.Id,
                    Name = LocalizationMapper.Map(externalPartyDTO.Name),
                    HasChilds = externalPartyDTO.HasChilds,
                    IsSelected = externalPartyDTO.IsSelected,
                    LocalName = externalPartyDTO.LocalName,
                    IsVirtual = externalPartyDTO.IsVirtual,
                    ParentId = externalPartyDTO.ParentId,
                    YasserRegistered = externalPartyDTO.YasserRegistered,
                    Number = externalPartyDTO.Number,
                    Email=externalPartyDTO.Email
                }).ToList();

            return externalPartyVMs;
        }
        public static List<ExternalPartyDTO> Map(IList<ExternalPartyVM> externalPartyVMs)
        {
            if (externalPartyVMs == null || !externalPartyVMs.Any())
            {
                return new List<ExternalPartyDTO>();
            }
            List<ExternalPartyDTO> externalPartyDTOs = externalPartyVMs
                .Select(externalPartyVM => new ExternalPartyDTO()
                {
                    Id = externalPartyVM.Id,
                    Number = externalPartyVM.Number,
                    Name = LocalizationMapper.Map(externalPartyVM.Name),
                    HasChilds = externalPartyVM.HasChilds,
                    IsSelected = externalPartyVM.IsSelected,
                    IsVirtual = externalPartyVM.IsVirtual,
                    LocalName = externalPartyVM.LocalName,
                    ParentId = externalPartyVM.ParentId,
                    YasserRegistered = externalPartyVM.YasserRegistered
                }).ToList();

            return externalPartyDTOs;
        }

        public static List<ExternalPartyListTypeDTO> Map(IList<ExternalPartyListTypeVM> externalPartyListTypeVMs)
        {
            if (externalPartyListTypeVMs == null || !externalPartyListTypeVMs.Any())
            {
                return new List<ExternalPartyListTypeDTO>();
            }
            List<ExternalPartyListTypeDTO> externalPartyListTypeDTOs = externalPartyListTypeVMs
                .Select(externalPartyListTypeVM => new ExternalPartyListTypeDTO()
                {
                    Id = externalPartyListTypeVM.Id,
                    IsSelected = true,
                    Text = externalPartyListTypeVM.Text
                }).ToList();

            return externalPartyListTypeDTOs;
        }
        public static List<ExternalPartyListTypeVM> Map(IList<ExternalPartyListTypeDTO> externalPartyListTypeDTOs)
        {
            if (externalPartyListTypeDTOs == null || !externalPartyListTypeDTOs.Any())
            {
                return new List<ExternalPartyListTypeVM>();
            }
            List<ExternalPartyListTypeVM> externalPartyListTypeVMs = externalPartyListTypeDTOs
                .Select(externalPartyListTypeDTO => new ExternalPartyListTypeVM()
                {
                    Id = externalPartyListTypeDTO.Id,
                    IsSelected = externalPartyListTypeDTO.IsSelected,
                    Text = externalPartyListTypeDTO.Text
                }).ToList();

            return externalPartyListTypeVMs;
        }

        public static ExternalPartyAddDTO Map(ExternalPartyAddVM externalPartyAddVM)
        {
            if (externalPartyAddVM != null)
            {
                return new ExternalPartyAddDTO()
                {
                    PartyNumber = externalPartyAddVM.PartyNumber,
                    Address = AddressMapper.Map(externalPartyAddVM.Address),
                    Email = externalPartyAddVM.Email,
                    FaxNumber = externalPartyAddVM.FaxNumber,
                    IsVirtual = externalPartyAddVM.IsVirtual,
                    Name = LocalizationMapper.Map(externalPartyAddVM.Name),
                    ParentId = externalPartyAddVM.ParentId,
                    PhoneNumber = externalPartyAddVM.PhoneNumber,
                    Types = Map(externalPartyAddVM.Types),
                    YasserRegistered = externalPartyAddVM.YasserRegistered
                };
            }
            return new ExternalPartyAddDTO();
        }
    }
}