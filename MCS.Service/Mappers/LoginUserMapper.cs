using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Security;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    //NotDone
    public static class LoginUserMapper
    {
        public static UserDTO Map(UserProfile userProfile)
        {
            if (userProfile == null)
                return null;

            UserDTO userDTO = new UserDTO
            {
                Id = userProfile.Id,
                Name = userProfile.LocalName,
                PendingRegestration = userProfile.PendingRegestration,
                UserCategoryName = (userProfile.Category != null) ? userProfile.Category.LocalName : string.Empty,
                LoclizationName = LocalizationIdentifierMapper.Map(userProfile.LocalizationIdentifier.Localizations),
                LoclizationUserCategory = (userProfile.Category != null) ? LocalizationIdentifierMapper.Map(userProfile.Category.CategoryName.Localizations) : null,
                InternalNumber = userProfile.InternalNumber,
            };

            List<string> claims = new List<string>();

            //if (userProfile.Group.Permissions != null)
            //{
            //    userProfile.Group.Permissions.ToList().ForEach(p =>
            //        claims.Add(p.Code)
            //  );
            //}
            if (userProfile.UserGroups != null)
            {
                userProfile.UserGroups.Select(p => p.Group.Permissions).ToList().ForEach(p =>
                    claims.AddRange(p.Select(x => x.Code))
              );
            }

            userDTO.Claims = claims;

            if (userProfile.OrgUnits != null)
            {
                userDTO.UserOrgUnits = new List<UserOrgUnitDTO>();

                userProfile.OrgUnits.ToList().ForEach(o =>
                {
                    userDTO.UserOrgUnits.Add(new UserOrgUnitDTO
                    {
                        Id = o.Id,
                        Name = o.LocalName,
                        LoclizationName = o.LocalizationIdentifier.Localizations != null ? LocalizationIdentifierMapper.Map(o.LocalizationIdentifier.Localizations) : null,
                        ManagerId = o.ManagerId,
                        IsSelected = o.Id == userProfile.MainOrgUnitId
                    });
                });

                if (!userDTO.UserOrgUnits.Where(o => o.IsSelected).Any())
                {
                    userDTO.UserOrgUnits.FirstOrDefault().IsSelected = true;
                }
            }
            return userDTO;
        }

        public static List<UserClaim> MapClaims(IList<UserPermission> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return null;
            }
            List<UserClaim> claims = new List<UserClaim>();

            if (permissions != null)
            {
                permissions.ToList().ForEach(p =>
                    claims.Add(new UserClaim() { Name = p.Permission.Code })
               );
            }

            return claims;
        }

        public static List<UserClaim> MapClaimsGroup(IList<Permission> permissions)
        {
            if (permissions == null || !permissions.Any())
            {
                return null;
            }
            List<UserClaim> claims = new List<UserClaim>();

            if (permissions != null)
            {
                permissions.ToList().ForEach(p =>
                    claims.Add(new UserClaim() { Name = p.Code })
               );
            }

            return claims;
        }
    }
}