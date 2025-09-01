using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.User.Models.Lookups
{
    public class TemplateViewModel
    {
        public TemplateAddVM AddTemplate { get; set; }
        public TemplateEditVM EditTemplate { get; set; }
        public AjaxGrid<TemplateVM> TemplateVMs { get; set; } = (AjaxGrid<TemplateVM>)new AjaxGridFactory().CreateAjaxGrid(new List<TemplateVM>(), 1, 0, false);
        public TemplateViewModel()
        {
            AddTemplate = new TemplateAddVM();
            EditTemplate = new TemplateEditVM();
        }
    }
}