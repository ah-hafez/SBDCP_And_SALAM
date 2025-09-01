using System.Collections.Generic;
using MCS.Common;
using MCS.GridMvc.Ajax.GridExtensions;
using MCS.UI.Areas.User.Models.OrgUnit;
using MCS.UI.Areas.User.Models.Shared;
using MCS.UI.Areas.User.Models.Transaction;

namespace MCS.UI.Areas.User.Models
{
    public class AutoCompleteViewModel
    {
        public string autoCompleteControlid { get; set; }
        public string hdnIdToSaveValue { get; set; }
        public string items { get; set; }
        public string content { get; set; }
        public bool matchAnywhere { get; set; }
        public string hdnExtraParametersId { get; set; }
        public bool selectFirstIndex { get; set; }
        public string validationClass { get; set; }
        public string onChangeCallback { get; set; }
        public string selectedId { get; set; }
        public string inputClassName { get; set; }
        public string ulClassName { get; set; }
        public string buttonId { get; set; }
        public string validationGroup { get; set; }        
    }
}
