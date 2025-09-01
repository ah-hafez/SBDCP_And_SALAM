using System.Collections.Generic;
using System.Linq;
using MCS.Framework.Controls.Mvc; 

namespace MCS.UI.Areas.Admin.Models.Tray
{
    public class TraysViewModel
    {
        public EditTrayVM EditTray { get; set; }
        public AjaxGrid<TrayVM> Trays { get; set; }
        public List<TrayVM> AllTrays { get; set; }

        public TraysViewModel()
        {
            EditTray = new EditTrayVM();
            Trays = (AjaxGrid<TrayVM>)new AjaxGridFactory().CreateAjaxGrid<TrayVM>(new List<TrayVM>().AsQueryable(), 1, false);
        }
    }
}