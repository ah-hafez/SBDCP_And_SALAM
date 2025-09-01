using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MCS.UI.Areas.Admin.Models
{
    public class OrgHierarchyTreeViewModel
    {
        public List<OrgHierarchyTreeNodeViewModel> Nodes { get; set; } = new List<OrgHierarchyTreeNodeViewModel>();
        public string TreeId { get; set; }
        public string GetChildrenActionURL { get; set; }
        public int? SelectedNode { get; set; }
        public string CallBackFunction { get; set; }
        public int? UserId { get; set; }
        public string GetChildrenActionParameters { get; set; }
        public int? OrgUnitTreeMode { get; set; }
    }

    public class OrgHierarchyTreeNodeViewModel
    {
        public int Id { get; set; }

        public int? ParentId { get; set; }

        public string Name { get; set; }

        public string DepartmentNumber { get; set; }

        public bool IsSelected { get; set; }

        public bool IsSelectable { get; set; }
        public bool IsUserDefined { get; set; }

        public bool HasChilds { get; set; }

        public IList<OrgHierarchyTreeNodeViewModel> Childs = new List<OrgHierarchyTreeNodeViewModel>();
        public bool IsYesserRegistered { get; set; }
    }
}