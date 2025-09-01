using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class DashboardMapper
    {
        public static DashboardVM Map(DashboardDTO dashboardDTO)
        {
            if (dashboardDTO != null)
            {
                DashboardVM dashboardVM = new DashboardVM()
                { 
                    Date = dashboardDTO.Date,
                    TypeId = dashboardDTO.TypeId,
                    UserCategoryId = dashboardDTO.UserCategoryId

                };
                return dashboardVM;
            }
            return new DashboardVM();
        }
        public static DashboardDTO Map(DashboardVM dashboardVM)
        {
            if (dashboardVM != null)
            {
                DashboardDTO dashboardDTO = new DashboardDTO()
                { 
                    Date = dashboardVM.Date,
                    TypeId = dashboardVM.TypeId,
                    UserCategoryId = dashboardVM.UserCategoryId

                };
                return dashboardDTO;
            }
            return new DashboardDTO();
        }
        public static List<DashboardDTO> Map(IList<DashboardVM> dashboardVMs)
        {
            if (dashboardVMs == null || !dashboardVMs.Any())
            {
                return new List<DashboardDTO>();
            }
            List<DashboardDTO> dashboardDTOs = dashboardVMs
                .Select(dashboardVM => new DashboardDTO()
                {
                 
                    Date = dashboardVM.Date,
                    TypeId = dashboardVM.TypeId,
                    UserCategoryId = dashboardVM.UserCategoryId
                }).ToList();

            return dashboardDTOs;
        }
        public static List<DashboardVM> Map(IList<DashboardDTO > dashboardDTOs)
        {
            if (dashboardDTOs == null || !dashboardDTOs.Any())
            {
                return new List<DashboardVM>();
            }
            List<DashboardVM> dashboardVMs =dashboardDTOs 
                .Select(dashboardDTO => new DashboardVM()
                { 
                     
                    Date = dashboardDTO.Date, 
                    TypeId = dashboardDTO.TypeId,
                    UserCategoryId = dashboardDTO.UserCategoryId
                }).ToList();

            return dashboardVMs;
        }

    }
}