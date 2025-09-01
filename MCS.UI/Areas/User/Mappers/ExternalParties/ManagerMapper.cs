using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.ExternalParties;

namespace MCS.UI.Areas.User.Mappers.ExternalParties
{
    public static class ManagerMapper
    {
        public static List<ManagerVM> Map(IList<ManagerDTO> managerDTOs)
        {
            if (managerDTOs == null || !managerDTOs.Any())
            {
                return new List<ManagerVM>();
            }
            List<ManagerVM> managerVMs = managerDTOs
                .Select(managerDTO => new ManagerVM()
                {
                    Id = managerDTO.Id,
                    AddedDate = managerDTO.AddedDate,
                    LocalName = managerDTO.LocalName,
                    Name = managerDTO.Name != null ? LocalizationMapper.Map(managerDTO.Name) : null,
                    PartyId = managerDTO.PartyId,
                    EmailAddress = managerDTO.EmailAddress
                }).ToList();

            return managerVMs;
        }
        public static ManagerVM Map(ManagerDTO managerDTOs)
        {
            if (managerDTOs != null)
            {
                ManagerVM managerVMs = new ManagerVM()

                {
                    Id = managerDTOs.Id,
                    AddedDate = managerDTOs.AddedDate,
                    LocalName = managerDTOs.LocalName,
                    Name = LocalizationMapper.Map(managerDTOs.Name),
                    PartyId = managerDTOs.PartyId
                };
                return managerVMs;
            }
            return new ManagerVM();
        }
        public static ManagerDTO Map(ManagerVM managerVM)
        {
            if (managerVM != null)
            {
                ManagerDTO managerDTO = new ManagerDTO()
                {
                    Id = managerVM.Id,
                    AddedDate = managerVM.AddedDate,
                    LocalName = managerVM.LocalName,
                    Name = LocalizationMapper.Map(managerVM.Name),
                    PartyId = managerVM.PartyId
                };
                return managerDTO;
            }
            return new ManagerDTO();
        }
        public static List<ManagerDTO> Map(IList<ManagerVM> managerVMs)
        {
            if (managerVMs == null || !managerVMs.Any())
            {
                return new List<ManagerDTO>();
            }
            List<ManagerDTO> managerDTOs = managerVMs
                .Select(managerVM => new ManagerDTO()
                {
                    Id = managerVM.Id,
                    AddedDate = managerVM.AddedDate,
                    LocalName = managerVM.LocalName,
                    Name = LocalizationMapper.Map(managerVM.Name),
                    PartyId = managerVM.PartyId
                }).ToList();

            return managerDTOs;
        }
        public static List<ManagerAddDTO> Map(IList<ManagerAddVM> managerAddVMs)
        {
            if (managerAddVMs == null || !managerAddVMs.Any())
            {
                return new List<ManagerAddDTO>();
            }
            List<ManagerAddDTO> managerAddDTOs = managerAddVMs
                .Select(managerAddVM => new ManagerAddDTO()
                {
                    Name = LocalizationMapper.Map(managerAddVM.Name),
                    PartyId = managerAddVM.PartyId
                }).ToList();

            return managerAddDTOs;
        }
        public static ManagerAddDTO Map(ManagerAddVM managerAddVMs)
        {
            if (managerAddVMs != null)
            {
                ManagerAddDTO managerAddDTOs = new ManagerAddDTO()
                {
                    Name = LocalizationMapper.Map(managerAddVMs.Name),
                    PartyId = managerAddVMs.PartyId,
                    EmailAddress = managerAddVMs.EmailAddress
                };

                return managerAddDTOs;
            }
            return new ManagerAddDTO();
        }

        public static List<ManagerAddVM> Map(IList<ManagerAddDTO> managerAddDTOs)
        {
            if (managerAddDTOs == null || !managerAddDTOs.Any())
            {
                return new List<ManagerAddVM>();
            }
            List<ManagerAddVM> managerAddVMs = managerAddDTOs
                .Select(managerAddDTO => new ManagerAddVM()
                {
                    Name = LocalizationMapper.Map(managerAddDTO.Name),
                    PartyId = managerAddDTO.PartyId
                }).ToList();

            return managerAddVMs;
        }
        public static List<ManagerEditVM> Map(IList<ManagerEditDTO> managerEditDTOs)
        {
            if (managerEditDTOs == null || !managerEditDTOs.Any())
            {
                return new List<ManagerEditVM>();
            }
            List<ManagerEditVM> managerEditVMs = managerEditDTOs
                .Select(managerEditDTO => new ManagerEditVM()
                {
                    Id = managerEditDTO.Id,
                    Name = LocalizationMapper.Map(managerEditDTO.Name),
                    PartyId = managerEditDTO.PartyId
                }).ToList();

            return managerEditVMs;
        }
        public static List<ManagerEditDTO> Map(IList<ManagerEditVM> managerEditVMs)
        {
            if (managerEditVMs == null || !managerEditVMs.Any())
            {
                return new List<ManagerEditDTO>();
            }
            List<ManagerEditDTO> managerEditDTOs = managerEditVMs
                .Select(managerEditVM => new ManagerEditDTO()
                {
                    Id = managerEditVM.Id,
                    Name = LocalizationMapper.Map(managerEditVM.Name),
                    PartyId = managerEditVM.PartyId
                }).ToList();
            return managerEditDTOs;
        }
        public static ManagerEditDTO Map(ManagerEditVM managerEditVMs)
        {
            if (managerEditVMs != null)
            {
                ManagerEditDTO managerEditDTOs = new ManagerEditDTO()
                {
                    Id = managerEditVMs.Id,
                    Name = LocalizationMapper.Map(managerEditVMs.Name),
                    PartyId = managerEditVMs.PartyId,
                    EmailAddress = managerEditVMs.EmailAddress
                };
                return managerEditDTOs;
            }
            return new ManagerEditDTO();
        }
        public static ManagerEditVM Map(ManagerEditDTO managerEditDTO)
        {
            if (managerEditDTO != null)
            {
                ManagerEditVM managerEditVM = new ManagerEditVM()
                {
                    Id = managerEditDTO.Id,
                    Name = LocalizationMapper.Map(managerEditDTO.Name),
                    PartyId = managerEditDTO.PartyId,
                    EmailAddress = managerEditDTO.EmailAddress
                };
                return managerEditVM;
            }
            return new ManagerEditVM();
        }





    }
}