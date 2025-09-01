using System.Collections.Generic;
using System.Linq;
using MCS.Framework;
using MCS.Business;
using MCS.Domain;
using MCS.DTO;

namespace MCS.Service.Mappers
{
    public static class BarcodeMapper
    {
        public static BarcodeDesign Map(BarcodeDesignerDTO barcodeDesignerDTO)
        {
            if (barcodeDesignerDTO != null)
            {
                ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

                BarcodeDesign barcodeDesign = new BarcodeDesign
                {
                    Id = barcodeDesignerDTO.Id,
                    Html = barcodeDesignerDTO.Html,
                    Type = lookupBL.GetLookupItem(barcodeDesignerDTO.TypeId),
                    IsGeneral = barcodeDesignerDTO.IsGeneral,
                    Width = barcodeDesignerDTO.Width,
                    Height = barcodeDesignerDTO.Height,
                    AttachmentHtml = barcodeDesignerDTO.HtmlAttachment
                };

                return barcodeDesign;
            }
            return null;
        }

        public static BarcodeDesignerDTO Map(BarcodeDesign barcodeDesign)
        {
            if (barcodeDesign != null)
            {
                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO
                {
                    Id = barcodeDesign.Id,
                    Html = barcodeDesign.Html,
                    TypeId = barcodeDesign.Type.Id,
                    IsGeneral = barcodeDesign.IsGeneral,
                    Width = barcodeDesign.Width,
                    Height = barcodeDesign.Height,
                    HtmlAttachment = barcodeDesign.AttachmentHtml
                };

                return barcodeDesignerDTO;
            }
            return null;
        }

        public static List<BarcodeDesign> Map(IList<BarcodeDesignerDTO> barcodeDesignerDTOs)
        {

            if (barcodeDesignerDTOs == null || !barcodeDesignerDTOs.Any())
            {
                return null;
            }
            List<BarcodeDesign> barcodeDesigns = new List<BarcodeDesign>();
            ILookupBL lookupBL = IoC.Resolve<ILookupBL>();

            if (barcodeDesignerDTOs.Any())
            {
                barcodeDesigns = barcodeDesignerDTOs
                    .Select(barcodeDesign => new BarcodeDesign
                    {
                        Id = barcodeDesign.Id,
                        Html = barcodeDesign.Html,
                        Type = lookupBL.GetLookupItem(barcodeDesign.TypeId),
                        IsGeneral = barcodeDesign.IsGeneral,
                        Width = barcodeDesign.Width,
                        Height = barcodeDesign.Height,
                        AttachmentHtml = barcodeDesign.HtmlAttachment
                    }).ToList();

            }
            return barcodeDesigns;
        }

        public static List<BarcodeDesignerDTO> Map(IList<BarcodeDesign> barcodeDesigns)
        {

            if (barcodeDesigns == null || !barcodeDesigns.Any())
            {
                return null;
            }
            List<BarcodeDesignerDTO> barcodeDesignerDTOs = new List<BarcodeDesignerDTO>();


            barcodeDesignerDTOs = barcodeDesigns
                .Select(barcodeDesignerDTO => new BarcodeDesignerDTO
                {
                    Id = barcodeDesignerDTO.Id,
                    Html = barcodeDesignerDTO.Html,
                    TypeId = barcodeDesignerDTO.Type.Id,
                    IsGeneral = barcodeDesignerDTO.IsGeneral,
                    Width = barcodeDesignerDTO.Width,
                    Height = barcodeDesignerDTO.Height,
                    HtmlAttachment = barcodeDesignerDTO.AttachmentHtml
                }).ToList();



            return barcodeDesignerDTOs;
        }
    }
}