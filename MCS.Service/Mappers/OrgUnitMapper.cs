using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public class OrgUnitMapper
    {
        public static List<OrgUnitDTO> Map(IList<OrgUnit> organizationUnits, string cultureName)
        {
            if (organizationUnits == null || !organizationUnits.Any())
            {
                return new List<OrgUnitDTO>();
            }
            List<OrgUnitDTO> organizationUnitDTOs = new List<OrgUnitDTO>();

            foreach (OrgUnit organizationUnit in organizationUnits)
            {
                OrgUnitDTO organizationUnitDTO = Map(organizationUnit, cultureName);

                organizationUnitDTO.ParentId = -1;

                if (organizationUnit.Parent != null)
                    organizationUnitDTO.ParentId = organizationUnit.Parent.Id;

                organizationUnitDTO.LinkUnitsKeys = new List<int>();

                if (organizationUnit.Links != null)
                {
                    foreach (OrgUnitLink organizationUnitLink in organizationUnit.Links)
                    {
                        int linkUnitKey = organizationUnitLink.ToEntity.Id;

                        organizationUnitDTO.LinkUnitsKeys.Add(linkUnitKey);
                    }
                }
                organizationUnitDTO.FollowupDepartment = organizationUnit.FollowUpDepartment;
                organizationUnitDTOs.Add(organizationUnitDTO);
            }

            return organizationUnitDTOs;
        }
        public static OrgUnitDTO Map(OrgUnit organizationUnit, string cultureName)
        {
            if (organizationUnit == null)
            {
                return null;
            }

            OrgUnitDTO organizationUnitDTO = new OrgUnitDTO()
            {
                Id = organizationUnit.Id,
                Name = organizationUnit.LocalName,
                Number = organizationUnit.Number,
                IsVirtualUnit = organizationUnit.IsVirtualUnit,
                HasChilds = organizationUnit.HasChilds,
                Lineage = organizationUnit.Lineage,
                IsCurrentTreeRoot = organizationUnit.IsCurrentTreeRoot
            };

            if (organizationUnit.LocalizationIdentifier != null)
            {
                Localization name = organizationUnit.LocalizationIdentifier.Localizations.Where(l => l.Culture.ShortName == cultureName).FirstOrDefault();

                if (name != null)
                {
                    organizationUnitDTO.Name = name.Text;
                }
            }

            return organizationUnitDTO;
        }
        public static OrgUnit Map(OrgUnitDTO organizationUnitDTO, string cultureName)
        {
            if (organizationUnitDTO == null)
            {
                return null;
            }

            OrgUnit organizationUnit = new OrgUnit()
            {
                Id = organizationUnitDTO.Id,
                LocalName = organizationUnitDTO.Name,
                Number = organizationUnitDTO.Number,
                IsVirtualUnit = organizationUnitDTO.IsVirtualUnit,
                BarCode = organizationUnitDTO.BarCode,
                Lineage = organizationUnitDTO.Lineage,
                Counter = CounterMapper.Map(organizationUnitDTO.Counter, cultureName)
            };

            return organizationUnit;
        }
        public static OrgUnitDTO Map(OrgUnit organizationUnit)
        {
            if (organizationUnit == null)
            {
                return null;
            }

            OrgUnitDTO organizationUnitDTO = new OrgUnitDTO()
            {
                Id = organizationUnit.Id,
                Name = organizationUnit.LocalName,
                Number = organizationUnit.Number,
                IsVirtualUnit = organizationUnit.IsVirtualUnit,
                HasChilds = organizationUnit.HasChilds,
                Lineage = organizationUnit.Lineage
            };

            return organizationUnitDTO;
        }
        public static OrgUnit MapWithUsers(OrgUnitDTO organizationUnitDTO)
        {
            if (organizationUnitDTO == null)
            {
                return null;
            }

            OrgUnit organizationUnit = new OrgUnit()
            {
                Id = organizationUnitDTO.Id,
                LocalName = organizationUnitDTO.Name,
                Number = organizationUnitDTO.Number,
                IsVirtualUnit = organizationUnitDTO.IsVirtualUnit,
            };

            IUserManagementBL userManagementBL = IoC.Resolve<IUserManagementBL>();
            organizationUnit.Users = new List<UserProfile>();

            if (organizationUnitDTO.Users != null)
            {
                foreach (var item in organizationUnitDTO.Users)
                {
                    UserProfile userProfile = userManagementBL.GetUserById(item.Id);
                    organizationUnit.Users.Add(userProfile);
                }
            }

            return organizationUnit;
        }
        public static OrgUnit MapWithLinks(OrgUnitDTO organizationUnitDTO)
        {
            if (organizationUnitDTO == null)
            {
                return null;
            }

            OrgUnit organizationUnit = new OrgUnit()
            {
                Id = organizationUnitDTO.Id,
                LocalName = organizationUnitDTO.Name,
                Number = organizationUnitDTO.Number
            };
            IOrgUnitBL orgUnitBL = IoC.Resolve<IOrgUnitBL>();
            var fromEntity = orgUnitBL.GetOrgUnitById(organizationUnitDTO.Key);
            organizationUnit.Links = new List<OrgUnitLink>();
            foreach (var ToEntity in organizationUnitDTO.LinkUnitsKeys)
            {
                var toEntity = orgUnitBL.GetOrgUnitById(ToEntity);
                OrgUnitLink orgUnitLink = new OrgUnitLink
                {
                    ToEntity = toEntity,
                    FromEntity = fromEntity
                };
                organizationUnit.Links.Add(orgUnitLink);
            }

            return organizationUnit;
        }
        public static OrgUnit MapWithBarcode(OrgUnitDTO organizationUnitDTO)
        {
            if (organizationUnitDTO == null)
            {
                return null;
            }

            OrgUnit organizationUnit = new OrgUnit()
            {
                Id = organizationUnitDTO.Id,
                BarcodeDesigns = BarcodeMapper.Map(organizationUnitDTO.BarcodeDesigns)
            };

            return organizationUnit;
        }
    }
}