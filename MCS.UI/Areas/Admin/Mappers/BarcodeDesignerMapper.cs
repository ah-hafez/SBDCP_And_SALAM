using System.Collections.Generic;
using System.Linq;
using MCS.DTO;
using MCS.UI.Areas.Admin.Models.BarcodeDesigner;

namespace MCS.UI.Areas.Admin.Mappers
{
    public static class BarcodeDesignerMapper
    {
        public static List<BarcodeDesignerDTO> Map(IList<BarcodeDesignerVM> barcodeDesignerVMs)
        {
            if (barcodeDesignerVMs == null || !barcodeDesignerVMs.Any())
            {
                return null;
            }
            List<BarcodeDesignerDTO> barcodeDesignerDTOs = barcodeDesignerVMs
                .Select(b => new BarcodeDesignerDTO
                {
                    Height = b.Height,
                    Html = b.Html,
                    HtmlAttachment = b.HtmlAttachment,
                    Id = b.Id,
                    IsGeneral = b.IsGeneral,
                    TypeId = b.TypeId,
                    Width = b.Width
                }).ToList();
            return barcodeDesignerDTOs;
        }
        public static List<BarcodeDesignerVM> Map(IList<BarcodeDesignerDTO> barcodeDesignerDTOs)
        {
            if (barcodeDesignerDTOs == null || !barcodeDesignerDTOs.Any())
            {
                return null;
            }
            List<BarcodeDesignerVM> barcodeDesignerVMs = barcodeDesignerDTOs
                .Select(b => new BarcodeDesignerVM
                {
                    Height = b.Height,
                    Html = b.Html,
                    HtmlAttachment = b.HtmlAttachment,
                    Id = b.Id,
                    IsGeneral = b.IsGeneral,
                    TypeId = b.TypeId,
                    Width = b.Width
                }).ToList();
            return barcodeDesignerVMs;
        }
        public static BarcodeDesignerVM Map(BarcodeDesignerDTO barcodeDesignerDTO)
        {
            if (barcodeDesignerDTO != null)
            {

                BarcodeDesignerVM barcodeDesignerVM = new BarcodeDesignerVM()
                { 
                    Height = barcodeDesignerDTO.Height,
                    Html = barcodeDesignerDTO.Html,
                    HtmlAttachment = barcodeDesignerDTO.HtmlAttachment,
                    Id = barcodeDesignerDTO.Id,
                    IsGeneral = barcodeDesignerDTO.IsGeneral,
                    TypeId = barcodeDesignerDTO.TypeId,
                    Width = barcodeDesignerDTO.Width
                };
                return barcodeDesignerVM;
            }
            return null;
        }
        public static BarcodeDesignerDTO Map(BarcodeDesignerVM barcodeDesignerVM)
        {
            if (barcodeDesignerVM != null)
            {

                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO()
                {
                    Height = barcodeDesignerVM.Height,
                    Html = barcodeDesignerVM.Html,
                    HtmlAttachment = barcodeDesignerVM.HtmlAttachment,
                    Id = barcodeDesignerVM.Id,
                    IsGeneral = barcodeDesignerVM.IsGeneral,
                    TypeId = barcodeDesignerVM.TypeId,
                    Width = barcodeDesignerVM.Width
                };
                return barcodeDesignerDTO;
            }
            return null;
        }
    }
}