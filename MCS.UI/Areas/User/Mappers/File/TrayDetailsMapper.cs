using System.Collections.Generic;
using System.Linq;
using MCS.Common;
using MCS.DTO;
using MCS.UI.Areas.User.Models.File;

namespace MCS.UI.Areas.User.Mappers.File
{
    public static class TrayDetailsMapper
    {
        public static List<TrayDetailsVM> Map(IList<TrayDetailsDTO> trayDetailsDTOs)
        {
            if (trayDetailsDTOs == null || !trayDetailsDTOs.Any())
            {
                return new List<TrayDetailsVM>();
            }
            List<TrayDetailsVM> trayDetailsVMs = trayDetailsDTOs
                .Select(trayDetailsDTO => new TrayDetailsVM()
                {
                    Id = trayDetailsDTO.Id,
                    AllTransactionCount = trayDetailsDTO.AllTransactionCount,
                    IsExcluded = trayDetailsDTO.IsExcluded,
                    Name = trayDetailsDTO.Name,
                    TodayTransactionCount = trayDetailsDTO.TodayTransactionCount,
                    TransactionTrayInfoVMs = TransactionTrayInfoMapper.Map(trayDetailsDTO.TransactionTrayInfoDTOs)
                }).ToList();

            return trayDetailsVMs;
        }
        public static List<TrayDetailsDTO> Map(IList<TrayDetailsVM> trayDetailsVMs)
        {
            if (trayDetailsVMs == null || !trayDetailsVMs.Any())
            {
                return new List<TrayDetailsDTO>();
            }
            List<TrayDetailsDTO> trayDetailsDTOs = trayDetailsVMs
                .Select(trayDetailsVM => new TrayDetailsDTO()
                {
                    Id = trayDetailsVM.Id,
                    AllTransactionCount = trayDetailsVM.AllTransactionCount,
                    IsExcluded = trayDetailsVM.IsExcluded,
                    Name = trayDetailsVM.Name,
                    TodayTransactionCount = trayDetailsVM.TodayTransactionCount,
                    TransactionTrayInfoDTOs = TransactionTrayInfoMapper.Map(trayDetailsVM.TransactionTrayInfoVMs)
                }).ToList();

            return trayDetailsDTOs;
        }
        public static TrayDetailsVM Map(TrayDetailsDTO trayDetailsDTO)
        {
            if (trayDetailsDTO != null)
            {
                return new TrayDetailsVM()
                {
                    Id = trayDetailsDTO.Id,
                    AllTransactionCount = trayDetailsDTO.AllTransactionCount,
                    IsExcluded = trayDetailsDTO.IsExcluded,
                    Name = trayDetailsDTO.Name,
                    TodayTransactionCount = trayDetailsDTO.TodayTransactionCount,
                    TransactionTrayInfoVMs = TransactionTrayInfoMapper.Map(trayDetailsDTO.TransactionTrayInfoDTOs),
                    IsVIPUser = (bool)SessionInfo.GetObjectFromSession(Constants.IsVIPUser)
                };
            }
            return new TrayDetailsVM();
        }
        public static TrayDetailsDTO Map(TrayDetailsVM trayDetailsVM)
        {
            if (trayDetailsVM != null)
            {
                return new TrayDetailsDTO()
                {
                    Id = trayDetailsVM.Id,
                    AllTransactionCount = trayDetailsVM.AllTransactionCount,
                    IsExcluded = trayDetailsVM.IsExcluded,
                    Name = trayDetailsVM.Name,
                    TodayTransactionCount = trayDetailsVM.TodayTransactionCount,
                    TransactionTrayInfoDTOs = TransactionTrayInfoMapper.Map(trayDetailsVM.TransactionTrayInfoVMs)
                };
            }
            return new TrayDetailsDTO();
        }
    }
}