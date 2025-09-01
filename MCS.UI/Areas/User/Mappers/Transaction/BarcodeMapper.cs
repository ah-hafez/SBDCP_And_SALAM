using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Mappers.Transaction
{
    public static class BarcodeMapper
    {
        public static List<BarcodeVM> Map(IList<BarcodeDTO> barcodeDTOs)
        {
            if (barcodeDTOs == null || !barcodeDTOs.Any())
            {
                return new List<BarcodeVM>();
            }
            List<BarcodeVM> barcodeVMs = barcodeDTOs
                .Select(barcodeDTO => new BarcodeVM()
                { 
                    Content = barcodeDTO.Content,
                    ReferenceId = barcodeDTO.ReferenceId,
                    Templete = barcodeDTO.Templete,
                    Type = barcodeDTO.Type,
                    Value = barcodeDTO.Value,
                    EntityName = barcodeDTO.EntityName,
                }).ToList();

            return barcodeVMs;
        }
        public static BarcodeVM Map(BarcodeDTO barcodeDTOs)
        {
            if (barcodeDTOs != null)
            {
                BarcodeVM barcodeVMs = new BarcodeVM()
                { 
                    Content = barcodeDTOs.Content,
                    ReferenceId = barcodeDTOs.ReferenceId,
                    Templete = barcodeDTOs.Templete,
                    Type = barcodeDTOs.Type,
                    Value = barcodeDTOs.Value
                };

                return barcodeVMs;
            }
            return new BarcodeVM();
        }
        public static List<BarcodeDTO> Map(IList<BarcodeVM> barcodeVMs)
        {
            if (barcodeVMs == null || !barcodeVMs.Any())
            {
                return new List<BarcodeDTO>();
            }
            List<BarcodeDTO> barcodeDTOs = barcodeVMs
                .Select(barcodeVM => new BarcodeDTO()
                {
                    Content = barcodeVM.Content,
                    ReferenceId = barcodeVM.ReferenceId,
                    Templete = barcodeVM.Templete,
                    Type = barcodeVM.Type,
                    Value = barcodeVM.Value
                }).ToList();

            return barcodeDTOs;
        }
        public static BarcodeDTO Map(BarcodeVM barcodeVM)
        {
            if (barcodeVM != null)
            {
                BarcodeDTO barcodeDTO = new BarcodeDTO()
                { 
                    Content = barcodeVM.Content,
                    ReferenceId = barcodeVM.ReferenceId,
                    Templete = barcodeVM.Templete,
                    Type = barcodeVM.Type,
                    Value = barcodeVM.Value
                };

                return barcodeDTO;
            }
            return new BarcodeDTO();
        }
    }
}