using MCS.Common.CustomAttributes;
using System.Collections.Generic;

namespace MCS.Common
{
    public enum TreeMode
    {
        Multiple,
        Single,
        SingleNotMandatory,
        MultiCheckbox
    }

    public class TreeViewModel
    {
        #region Properties

        public TreeMode Mode { get; set; }
        [CustomDisplayName("User.Admin.Orgunit")]
        public TreeNode RootNode { get; set; }
        public Dictionary<int, TreeNode> Nodes { get; set; }

        #endregion Properties

        #region Methods

        public void BuildTree()
        {
            TreeNode parent;

            foreach (var node in Nodes.Values)
            {
                if (Nodes.TryGetValue(node.ParentId, out parent) && node.Id != node.ParentId)
                {
                    node.Parent = parent;
                    parent.Childs.Add(node);
                }
            }
        }

        #endregion Methods
    }

    public class TreeNode
    {
        #region Properties

        public int Id { get; set; }

        public int ParentId;

        public TreeNode Parent;

        public TreeMode Mode { get; set; }

        public string Name { get; set; }

        public string DepartmentNumber { get; set; }

        public bool IsSelected { get; set; }

        public bool Selectable { get; set; }
        public bool IsUserDefined { get; set; }

        public bool HasChilds { get; set; }

        public IList<TreeNode> Childs = new List<TreeNode>();
        public bool IsYesserRegistered { get; set; }
        public int? ExternalId { get; set; }
        public string CheckboxFunction { get; set; }

        #endregion Properties
    }
}
