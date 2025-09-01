using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.User.Models.BarcodeDesigner;

namespace MCS.UI.Areas.User.Mappers.BarcodeDesigner
{
    public static class BarcodeDesignerMapper
    {
        public static List<BarcodeDesignerVM> Map(IList<BarcodeDesignerDTO> barcodeDesignerDTOs)
        {
            if (barcodeDesignerDTOs == null || !barcodeDesignerDTOs.Any())
            {
                return new List<BarcodeDesignerVM>();
            }
            List<BarcodeDesignerVM> barcodeDesignerVMs = barcodeDesignerDTOs
                .Select(barcodeDesignerDTO => new BarcodeDesignerVM()
                { 
                    Id = barcodeDesignerDTO.Id,
                    Height = barcodeDesignerDTO.Height,
                    Width = barcodeDesignerDTO.Width,
                    Html = barcodeDesignerDTO.Html,
                    HtmlAttachment = barcodeDesignerDTO.HtmlAttachment,
                    IsGeneral = barcodeDesignerDTO.IsGeneral,
                    TypeId = barcodeDesignerDTO.TypeId
                }).ToList();

            return barcodeDesignerVMs;
        }
        public static List<BarcodeDesignerDTO> Map(IList<BarcodeDesignerVM> barcodeDesignerVMs)
        {
            if (barcodeDesignerVMs == null || !barcodeDesignerVMs.Any())
            {
                return new List<BarcodeDesignerDTO>();
            }
            List<BarcodeDesignerDTO> barcodeDesignerDTOs = barcodeDesignerVMs
                .Select(barcodeDesignerVM => new BarcodeDesignerDTO()
                { 
                    Id = barcodeDesignerVM.Id,
                    Height = barcodeDesignerVM.Height,
                    Width = barcodeDesignerVM.Width,
                    Html = barcodeDesignerVM.Html,
                    HtmlAttachment = barcodeDesignerVM.HtmlAttachment,
                    IsGeneral = barcodeDesignerVM.IsGeneral,
                    TypeId = barcodeDesignerVM.TypeId
                }).ToList();

            return barcodeDesignerDTOs;
        }
    }
}