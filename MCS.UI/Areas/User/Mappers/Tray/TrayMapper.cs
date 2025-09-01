using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Mappers.Lookups;
using MCS.UI.Areas.User.Models.Tray;

namespace MCS.UI.Areas.User.Mappers.Tray
{
    public static class TrayMapper
    {
        public static List<TrayVM> Map(IList<TrayDTO> trayDTOs)
        {
            if (trayDTOs == null || !trayDTOs.Any())
            {
                return new List<TrayVM>();
            }
            List<TrayVM> trayVMs = trayDTOs
                .Select(trayDTO => new TrayVM()
                {
                    Id = trayDTO.Id,
                    IsSelected = trayDTO.IsSelected,
                    LocalName = trayDTO.LocalName,
                    Names = LookupLocalizationMapper.Map(trayDTO.Names),
                    Permission = trayDTO.Permission,
                    sort = trayDTO.Sort
                }).ToList();
            return trayVMs;
        }
        public static List<TrayDTO> Map(IList<TrayVM> trayVMs)
        {
            if (trayVMs == null || !trayVMs.Any())
            {
                return new List<TrayDTO>();
            }
            List<TrayDTO> trayDTOs = trayVMs
                .Select(trayVM => new TrayDTO()
                {
                    Id = trayVM.Id,
                    IsSelected = trayVM.IsSelected,
                    LocalName = trayVM.LocalName,
                    Names = LookupLocalizationMapper.Map(trayVM.Names),
                    Permission = trayVM.Permission,
                    Sort = trayVM.sort
                }).ToList();
            return trayDTOs;
        }
        public static List<EditTrayDTO> Map(IList<EditTrayVM> editTrayVMs)
        {
            if (editTrayVMs == null || !editTrayVMs.Any())
            {
                return new List<EditTrayDTO>();
            }
            List<EditTrayDTO> editTrayDTOs = editTrayVMs
                .Select(editTrayVM => new EditTrayDTO()
                {
                    Id = editTrayVM.Id,
                    Names = LookupLocalizationMapper.Map(editTrayVM.Names),
                    PermissionId = editTrayVM.PermissionId
                }).ToList();
            return editTrayDTOs;
        }
        public static List<EditTrayVM> Map(IList<EditTrayDTO> editTrayDTOs)
        {
            if (editTrayDTOs == null || !editTrayDTOs.Any())
            {
                return new List<EditTrayVM>();
            }
            List<EditTrayVM> editTrayVMs = editTrayDTOs
                .Select(editTrayDTO => new EditTrayVM()
                {
                    Id = editTrayDTO.Id,
                    Names = LookupLocalizationMapper.Map(editTrayDTO.Names),
                    PermissionId = editTrayDTO.PermissionId
                }).ToList();
            return editTrayVMs;
        }
    }
}