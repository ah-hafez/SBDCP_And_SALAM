using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using MCS.Common;
using MCS.Common.ApiControllerResults;
using MCS.DTO;
using MCS.UI.Areas.Admin.Mappers;
using MCS.UI.Areas.Admin.Models.BarcodeDesigner;
using MCS.UI.Areas.Admin.Models.Lookups;

namespace MCS.UI.Areas.Admin.Controllers
{

    public class BarcodeDesignerController : AdminControllerBase
    {
        public ActionResult Index()
        {
            try
            {
                List<TransactionCategoryVM> transactionCategoryVMs = GetTransactionCategoryLookups();

                ViewData["TransactionCategory"] = transactionCategoryVMs;
                //GetResult<List<OrgUnitDTO>> orgUnitDTOs = HttpClientWrapper<GetResult<List<OrgUnitDTO>>>.GetItemRequest(string.Format("api/Admin/GetOrgUnits?cultureName={0}", SessionInfo.CultureShortName)).Result;

                //if (orgUnitDTOs.Result == null)
                //{
                //    orgUnitDTOs.Result = new List<OrgUnitDTO>();
                //    orgUnitDTOs.RowsCount = 0;
                //}

                //ViewData["OrgUnits"] = UIHelper.BulidTree(OrgUnitMapper.Map(orgUnitDTOs.Result), -1, true);
                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO();
                barcodeDesignerDTO.TypeId = BarcodeDesignType.Inbound.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
                barcodeDesignerDTO.IsGeneral = true;
                GetResult<BarcodeDesignerDTO> barcodeDesigner = HttpClientWrapper<GetResult<BarcodeDesignerDTO>>.GetItemRequest(String.Format("api/Admin/GetBarcodeDesign?isGeneral={0}&typeId={1}", true, barcodeDesignerDTO.TypeId)).Result;

                if (barcodeDesigner.Result == null)
                {
                    barcodeDesignerDTO.Id = 0;
                    barcodeDesignerDTO.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    barcodeDesignerDTO.HtmlAttachment = "<div id=\"barCodeAttachment\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                }
                else
                {
                    string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesigner.Result.Width * 0.6, barcodeDesigner.Result.Height * 0.2, UrlHelper.GetBaseUri());
                    string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesigner.Result.Width * 0.3, barcodeDesigner.Result.Height * 0.3, UrlHelper.GetBaseUri());

                    barcodeDesignerDTO.Id = barcodeDesigner.Result.Id;
                    barcodeDesignerDTO.Html = string.Format(barcodeDesigner.Result.Html, "", imag2DStyle, "",
                                               imag3DStyle, "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DepartmentPreparedInbound"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "",
                                               ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName"), ""
                                               );

                    if (barcodeDesigner.Result.HtmlAttachment != null)
                    {
                        barcodeDesignerDTO.HtmlAttachment = FillAttachmentDesgin(barcodeDesigner.Result.HtmlAttachment, barcodeDesigner.Result);
                    }
                    else
                    {
                        barcodeDesignerDTO.HtmlAttachment = "<div id=\"barCodeAttachment\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }

                }
                return View(BarcodeDesignerMapper.Map(barcodeDesignerDTO));
            }

            catch (Exception)
            {
                throw;
            }
        }

        public string FillAttachmentDesgin(string htmlDesign, BarcodeDesignerDTO barcodeDesignerDTO)
        {
            string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesignerDTO.Width * 0.6, barcodeDesignerDTO.Height * 0.2, UrlHelper.GetBaseUri());
            string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesignerDTO.Width * 0.3, barcodeDesignerDTO.Height * 0.3, UrlHelper.GetBaseUri());

            htmlDesign = htmlDesign.Replace("{attachmentOrgunit}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Orgunit"));
            htmlDesign = htmlDesign.Replace("{attachmentCount}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.Count"));
            htmlDesign = htmlDesign.Replace("{attachmentName}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentName"));
            htmlDesign = htmlDesign.Replace("{attachmentDate}", ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.AttachmentBarcodeDesigner.AttachmentDate"));
            htmlDesign = htmlDesign.Replace("{attachment2DImage}", "");
            htmlDesign = htmlDesign.Replace("{attachment2DImageValue}", imag2DStyle);
            htmlDesign = htmlDesign.Replace("{attachment3DImage}", "");
            htmlDesign = htmlDesign.Replace("{attachment3DImageValue}", imag3DStyle);
            htmlDesign = htmlDesign.Replace("{attachmentOrgunitValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentCountValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentNameValue}", "");
            htmlDesign = htmlDesign.Replace("{attachmentDateValue}", "");

            return htmlDesign;
        }

        [ValidateInput(false)]

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AddDesign(BarcodeDesignerVM designVM)
        {
            try
            {
                string message = string.Empty;
                string html = string.Empty;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostDesign", BarcodeDesignerMapper.Map(designVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());

                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.AddSucceeded");
                designVM.Id = postResult.Id.Value;

                html = GetViewHtml(designVM);

                return Json(new { html = html, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        [HttpGet]
        public ActionResult GetDesign(bool isGeneral, int typeId)
        {
            try
            {
                string message = string.Empty;
                string html = string.Empty;
                GetResult<BarcodeDesignerDTO> barcodeDesigner = HttpClientWrapper<GetResult<BarcodeDesignerDTO>>.GetItemRequest(String.Format("api/Admin/GetBarcodeDesign?isGeneral={0}&typeId={1}", isGeneral, typeId)).Result;

                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO();
                barcodeDesignerDTO.TypeId = typeId;
                barcodeDesignerDTO.IsGeneral = isGeneral;
                if (barcodeDesigner.Result == null)
                {
                    barcodeDesignerDTO.Id = 0;
                    barcodeDesignerDTO.Html = "";
                    barcodeDesignerDTO.HtmlAttachment = "<div id=\"barCodeAttachment\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>"; ;

                }
                else
                {
                    barcodeDesignerDTO.Id = barcodeDesigner.Result.Id;
                    barcodeDesignerDTO.Html = barcodeDesigner.Result.Html;
                    barcodeDesignerDTO.Width = barcodeDesigner.Result.Width;
                    barcodeDesignerDTO.Height = barcodeDesigner.Result.Height;
                    barcodeDesignerDTO.HtmlAttachment = FillAttachmentDesgin(barcodeDesigner.Result.HtmlAttachment ?? string.Empty, barcodeDesignerDTO);
                }
                html = GetViewHtml(BarcodeDesignerMapper.Map(barcodeDesignerDTO));

                return Json(new { Html = html }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpGet]
        public ActionResult VisitTicket()
        {
            try
            {
                BarcodeDesignerDTO barcodeDesignerDTO = new BarcodeDesignerDTO();
                barcodeDesignerDTO.TypeId = BarcodeDesignType.VisitTicket.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
                barcodeDesignerDTO.IsGeneral = true;
                GetResult<BarcodeDesignerDTO> barcodeDesigner = HttpClientWrapper<GetResult<BarcodeDesignerDTO>>.GetItemRequest(String.Format("api/Admin/GetBarcodeDesign?isGeneral={0}&typeId={1}", true, barcodeDesignerDTO.TypeId)).Result;
                if (barcodeDesigner.Result == null)
                {
                    barcodeDesignerDTO.Id = 0;
                    barcodeDesignerDTO.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_A6\"> </div>";//<div class=\"col-md-6 encryption_code\" id=\"barCode\" style=\"position:relative;height:250px ;width:400px\"></div>";
                }
                else
                {
                    string imag2DStyle = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesigner.Result.Width * 0.4, barcodeDesigner.Result.Height * 0.1, UrlHelper.GetBaseUri());
                    string imag3DStyle = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesigner.Result.Width * 0.15, barcodeDesigner.Result.Height * 0.15, UrlHelper.GetBaseUri());

                    barcodeDesignerDTO.Id = barcodeDesigner.Result.Id;
                    barcodeDesignerDTO.Html = string.Format(barcodeDesigner.Result.Html, "", imag2DStyle, "",
                                               imag3DStyle, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Department"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                        "", "",
                        "", "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DocumentNumber"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InboundDestination"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.ToEntity"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Name"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Signature"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Inquery"), "");
                }
                return View(BarcodeDesignerMapper.Map(barcodeDesignerDTO));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [ValidateInput(false)]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult VisitTicket(BarcodeDesignerVM designVM)
        {
            try
            {
                string message = string.Empty;
                string html = string.Empty;
                PostResult postResult = HttpClientWrapper<PostResult>.PostRequest("api/Admin/PostDesign", BarcodeDesignerMapper.Map(designVM)).Result;

                if (postResult.StatusCode != StatusCode.Ok)
                {
                    message = ResourceHelper.GetResourceValue(ResourceSet.StatusCode, postResult.StatusCode.ToString());
                    return Json(new { MessageText = message, MessageType = MessageType.Error }, JsonRequestBehavior.AllowGet);
                }

                message = ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.ExternalParty.AddSucceeded");
                designVM.Id = postResult.Id.Value;

                html = string.Format(designVM.Html, "",
                    ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.twoDimensionsBarcode"), "",
                    ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.threeDimensionsBarcode"), "",
                    ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Department"), "",
                    ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                    ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                        "", "",
                        "", "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DocumentNumber"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.InboundDestination"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.ToEntity"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Name"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Signature"), "",
                        ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Inquery"), "");

                return Json(new { html = html, ID = designVM.Id, MessageText = message, MessageType = MessageType.Information }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                throw;
            }

        }

        private List<TransactionCategoryVM> GetTransactionCategoryLookups()
        {
            GetResult<IList<LookupVM>> lookupVMs = LookupsHelper.GetAdminLookupItems(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName);
            List<TransactionCategoryVM> transactionCategoryVMs = new List<TransactionCategoryVM>();

            if (lookupVMs.Result != null)
            {
                foreach (LookupVM lookupVM in lookupVMs.Result)
                {
                    if (lookupVM.Id == BarcodeDesignType.Attachment.LookupIdentity(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName))
                    {
                        continue;
                    }

                    transactionCategoryVMs.Add(new TransactionCategoryVM()
                    {
                        Id = lookupVM.Id,
                        Text = lookupVM.Text,
                    });
                }
            }

            return transactionCategoryVMs;
        }

        private string GetViewHtml(BarcodeDesignerVM barcodeDesignerVM)
        {
            string barcode2D = string.Format("<img style='width: {0}px;  height: {1}px;' class='imag2D' src='{2}/Content/Admin/Lib/images/morasalat/code_2d_v.png' />", barcodeDesignerVM.Width * 0.6, barcodeDesignerVM.Height * 0.2, UrlHelper.GetBaseUri());
            string barcode3D = string.Format("<img style='width: {1}px;  height: {1}px;' class='imag3D' src='{2}/Content/Admin/Lib/images/morasalat/code_3d_v.png' />", barcodeDesignerVM.Width * 0.3, barcodeDesignerVM.Height * 0.3, UrlHelper.GetBaseUri());

            /*
                         * the place holders for the Html string:
                         * 0: Barcode Designer Class (rtl,ltr)
                         * 1,2: 2D barcode label and value
                         * 3,4: 3D barcode label and value
                         * 5,6: Department label and value
                         * 7,8: Transaction Date label and value
                         * 9,10: Transaction Number label and value
                         * 11,12: Attachment label and value
                         * 13,14: Department Directed To label and value
                         * 
                         */
            switch ((BarcodeDesignType)barcodeDesignerVM.TypeId.LookupInternalID(LookupCategory.BarcodeDesignType, SessionInfo.CultureShortName))
            {
                case BarcodeDesignType.Inbound:
                    if (barcodeDesignerVM.Html == "")
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DepartmentPreparedInbound"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           "الاختصار", "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName"), "");
                    }
                    return UIHelper.RenderRazorViewToHtml(ControllerContext, "_InboundBarcodeDesignerPartial", barcodeDesignerVM);

                case BarcodeDesignType.Outbound:
                    if (barcodeDesignerVM.Html == "")
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.DirectedDepartment"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.OutboundNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName"), "");
                    }
                    return UIHelper.RenderRazorViewToHtml(ControllerContext, "_OutboundBarcodeDesignerPartial", barcodeDesignerVM);

                case BarcodeDesignType.OutboundInternal:
                    if (barcodeDesignerVM.Html == "")
                    {
                        barcodeDesignerVM.Html = "<div id=\"barCode\" style=\"position:relative;\" class=\"droppable ui-widget-header col-md-6 encryption_code size_c\"> </div>";
                    }
                    else
                    {
                        barcodeDesignerVM.Html = string.Format(barcodeDesignerVM.Html, "",
                           barcode2D, "",
                           barcode3D, "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.PreparedDepartment"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionDate"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TransactionNumber"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Attachments"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.Abbreviation"), "",
                           ResourceHelper.GetResourceValue(ResourceSet.Message, "Admin.BarcodeDesigner.TanentName"), "");
                    }
                    return UIHelper.RenderRazorViewToHtml(ControllerContext, "_InternalOutboundBarcodeDesignerPartial", barcodeDesignerVM);
            }
            return "";
        }
    }
}
