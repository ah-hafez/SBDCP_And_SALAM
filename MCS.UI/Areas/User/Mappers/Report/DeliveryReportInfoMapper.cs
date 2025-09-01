using MCS.DTO;
using MCS.UI.Areas.User.Models.Report;

namespace MCS.UI.Areas.User.Mappers.Report
{
    public static class DeliveryReportInfoMapper
    {
        public static DeliveryReportInfoVM Map(DeliveryReportInfoDTO deliveryReportInfoDTO)
        {
            if (deliveryReportInfoDTO != null)
            {
                DeliveryReportInfoVM deliveryReportInfoVM = new DeliveryReportInfoVM()
                { 
                    DateH = deliveryReportInfoDTO.DateH,
                    DeliveryReportTransactions = DeliveryReportTransactionInfoMapper.Map(deliveryReportInfoDTO.DeliveryReportTransactions),
                    OrgUnitName = deliveryReportInfoDTO.OrgUnitName,
                    ReportNumber = deliveryReportInfoDTO.ReportNumber,
                    RootOrgUnitName = deliveryReportInfoDTO.RootOrgUnitName,
                    UserName = deliveryReportInfoDTO.UserName
                };
                return deliveryReportInfoVM;
            }
            return new DeliveryReportInfoVM();
        }
        public static DeliveryReportInfoDTO Map(DeliveryReportInfoVM deliveryReportInfoVM)
        {
            if (deliveryReportInfoVM != null)
            {
                DeliveryReportInfoDTO deliveryReportInfoDTO = new DeliveryReportInfoDTO()
                { 
                    DateH = deliveryReportInfoVM.DateH,
                    DeliveryReportTransactions = DeliveryReportTransactionInfoMapper.Map(deliveryReportInfoVM.DeliveryReportTransactions),
                    OrgUnitName = deliveryReportInfoVM.OrgUnitName,
                    ReportNumber = deliveryReportInfoVM.ReportNumber,
                    RootOrgUnitName = deliveryReportInfoVM.RootOrgUnitName,
                    UserName = deliveryReportInfoVM.UserName
                };
                return deliveryReportInfoDTO;
            }
            return new DeliveryReportInfoDTO();
        }
    }
}