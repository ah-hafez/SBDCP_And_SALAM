using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Shared;

namespace MCS.UI.Areas.User.Mappers.Shared
{
    public static class ExternalLoginInfoMapper
    {
        public static List<ExternalLoginInfoDTO> Map(IList<ExternalLoginInfoVM> externalLoginInfoVMs)
        {
            if (externalLoginInfoVMs == null || !externalLoginInfoVMs.Any())
            {
                return new List<ExternalLoginInfoDTO>();
            }
            List<ExternalLoginInfoDTO> externalLoginInfoDTOs = externalLoginInfoVMs
                .Select(b => new ExternalLoginInfoDTO
                { 
                    Email = b.Email,
                    ExternalIdentity = b.ExternalIdentity,
                    ProviderName = b.ProviderName,
                    ProviderKey = b.ProviderKey,
                    UserName = b.UserName
                }).ToList();
            return externalLoginInfoDTOs;

        }
        public static List<ExternalLoginInfoVM> Map(IList<ExternalLoginInfoDTO> externalLoginInfoDTOs)
        {
            if (externalLoginInfoDTOs == null || !externalLoginInfoDTOs.Any())
            {
                return new List<ExternalLoginInfoVM>();
            }
            List<ExternalLoginInfoVM> externalLoginInfoVMs = externalLoginInfoDTOs
                .Select(b => new ExternalLoginInfoVM
                {
                    Email = b.Email,
                    ExternalIdentity = b.ExternalIdentity,
                    ProviderName = b.ProviderName,
                    ProviderKey = b.ProviderKey,
                    UserName = b.UserName
                }).ToList();
            return externalLoginInfoVMs;

        }
        public static ExternalLoginInfoVM Map(ExternalLoginInfoDTO b)
        {
            if (b != null)
            {
                ExternalLoginInfoVM externalLoginInfoVM = new ExternalLoginInfoVM()
                {
                    Email = b.Email,
                    ExternalIdentity = b.ExternalIdentity,
                    ProviderName = b.ProviderName,
                    ProviderKey = b.ProviderKey,
                    UserName = b.UserName
                };
                return externalLoginInfoVM;
            }
            return new ExternalLoginInfoVM();

        }
        public static ExternalLoginInfoDTO Map(ExternalLoginInfoVM b)
        {
            if (b != null)
            {
                ExternalLoginInfoDTO externalLoginInfoDTO = new ExternalLoginInfoDTO()
                {
                    Email = b.Email,
                    ExternalIdentity = b.ExternalIdentity,
                    ProviderName = b.ProviderName,
                    ProviderKey = b.ProviderKey,
                    UserName = b.UserName
                };
                return externalLoginInfoDTO;
            }
            return new ExternalLoginInfoDTO();

        }
    }
}