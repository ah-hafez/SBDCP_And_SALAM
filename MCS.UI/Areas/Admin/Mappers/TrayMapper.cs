using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.Tray;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class TrayMapper
    {
        public static List<TrayDTO> Map(IList<TrayVM> trayVMs)
        {
            if (trayVMs == null || !trayVMs.Any())
            { return null; }
            List<TrayDTO> trayDTOs = trayVMs
                .Select(b => new TrayDTO
                { 
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    Permission = b.Permission,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Sort = b.sort

                }).ToList();
            return trayDTOs;
        }
        public static List<TrayVM> Map(IList<TrayDTO> trayDTOs)
        {
            if (trayDTOs == null || !trayDTOs.Any())
            { return null; }
            List<TrayVM> trayVMs = trayDTOs
                .Select(b => new TrayVM
                { 
                    Id = b.Id,
                    IsSelected = b.IsSelected,
                    LocalName = b.LocalName,
                    Permission = b.Permission,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    sort = b.Sort

                }).ToList();
            return trayVMs;
        }
        public static List<EditTrayDTO> Map(IList<EditTrayVM> trayEditVMs)
        {
            if (trayEditVMs == null || !trayEditVMs.Any())
            { return null; }
            List<EditTrayDTO> trayEdiDTOs = trayEditVMs
                .Select(b => new EditTrayDTO
                { 
                    PermissionId = b.PermissionId,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Id = b.Id

                }).ToList();
            return trayEdiDTOs;
        }
        public static List<EditTrayVM> Map(IList<EditTrayDTO> trayEdiDTOs)
        {
            if (trayEdiDTOs == null || !trayEdiDTOs.Any())
            { return null; }
            List<EditTrayVM> trayEditVMs = trayEdiDTOs
                .Select(b => new EditTrayVM
                { 
                    PermissionId = b.PermissionId,
                    Names = LookupLocalizationMapper.Map(b.Names),
                    Id = b.Id

                }).ToList();
            return trayEditVMs;
        }
        public static EditTrayDTO Map(EditTrayVM trayEditVM)
        {
            if (trayEditVM != null)
            {
                EditTrayDTO trayEdiDTO = new EditTrayDTO()

                {
                    PermissionId = trayEditVM.PermissionId,
                    Names = LookupLocalizationMapper.Map(trayEditVM.Names),
                    Id = trayEditVM.Id

                };
                return trayEdiDTO;
            }
            return null;
        }
        public static EditTrayVM Map(EditTrayDTO trayEdiDTO)
        {
            if (trayEdiDTO != null)
            {
                EditTrayVM trayEditVM = new EditTrayVM()

                {
                    PermissionId = trayEdiDTO.PermissionId,
                    Names = LookupLocalizationMapper.Map(trayEdiDTO.Names),
                    Id = trayEdiDTO.Id

                };
                return trayEditVM;
            }
            return null;
        }
    }
}