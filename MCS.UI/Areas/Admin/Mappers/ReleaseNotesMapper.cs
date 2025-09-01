using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Lookups;
using MCS.UI.Areas.Admin.Models.ReleaseNotes;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class ReleaseNotesMapper
    {
        public static List<ReleaseNotesDTO> Map(IList<ReleaseNotesVM> vmList)
        {
            if (vmList == null || !vmList.Any())
            { return null; }
            List<ReleaseNotesDTO> dtoList = vmList
                .Select(b => new ReleaseNotesDTO
                {
                    Id = b.Id,
                    ReleaseNumber = b.ReleaseNumber,
                    ReleaseDate = b.ReleaseDate,
                    DateHj = b.DateHj,
                    Description = b.Description


                }).ToList();
            return dtoList;
        }
        public static List<ReleaseNotesVM> Map(IList<ReleaseNotesDTO> dtoList)
        {
            if (dtoList == null || !dtoList.Any())
            {
                return new List<ReleaseNotesVM>();
            }

            List<ReleaseNotesVM> vmList = dtoList
                .Select(b => new ReleaseNotesVM
                {
                    Id = b.Id,
                    ReleaseNumber = b.ReleaseNumber,
                    ReleaseDate = b.ReleaseDate,
                    DateHj = b.DateHj,
                    Description = b.Description,
                    IsActive = b.IsActive

                }).ToList();
            return vmList;
        }
        public static ReleaseNotesDTO Map(ReleaseNotesVM vmObj)
        {
            if (vmObj == null)
            {
                return null;
            }
            return new ReleaseNotesDTO
            {
                Id = vmObj.Id,
                ReleaseNumber = vmObj.ReleaseNumber,
                ReleaseDate = vmObj.ReleaseDate,
                DateHj = vmObj.DateHj,
                Description = vmObj.Description,
                IsActive = vmObj.IsActive
            };

        }

        public static ReleaseNotesVM Map(ReleaseNotesDTO dtoObj)
        {
            if (dtoObj == null)
            {
                return null;
            }

            return new ReleaseNotesVM
            {
                Id = dtoObj.Id,
                ReleaseNumber = dtoObj.ReleaseNumber,
                ReleaseDate = dtoObj.ReleaseDate,
                DateHj = dtoObj.DateHj,
                Description = dtoObj.Description,
                IsActive = dtoObj.IsActive
            };

        }

    }
}