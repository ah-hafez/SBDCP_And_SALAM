using System.Collections.Generic;
using MCS.GridMvc.Ajax.GridExtensions;

namespace MCS.UI.Areas.Admin.Models.Lookups
{
    public class FormViewModel
    {
        public FormAddVM AddForm { get; set; }
        public FormEditVM EditForm { get; set; }
        public AjaxGrid<FormVM> FormVMs { get; set; } = (AjaxGrid<FormVM>)new AjaxGridFactory().CreateAjaxGrid(new List<FormVM>(), 1, 0, false);
        public FormViewModel()
        {
            AddForm = new FormAddVM();
            EditForm = new FormEditVM();
        }
    }
}