using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCS.Common;
using TreeNode = System.Windows.Forms.TreeNode;

namespace MorasalatOutlookAddIn.Forms
{
    public class FormsBase : Form
    {
        protected Forms.Loading loadingPage;
        protected FormsBase()
        {
            this.ShowProgress();

        }
        protected void CloseProgress()
        {

            loadingPage.Close();


        }
        private void ShowProgress()
        {

            // this.Invoke((MethodInvoker)delegate () {
            loadingPage = new Loading();
            loadingPage.Show();
            //Application.Run();
            //   });

        }
        public void FormsBaseSettings(System.Windows.Forms.Panel panelMain)
        {
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            if (Helper.GetCultureName == "ar")
            {
                this.RightToLeft = panelMain.RightToLeft = RightToLeft.Yes;
            }
        }

        public string downloadPath = Path.GetTempPath(); //Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads"

        protected void FillTreeWithUnits(int? paernId, TreeView treeViewOrgs, TransactionCategorieColor transType, string searchQuery)
        {
            List<MCS.DTO.ExternalPartyDTO> units = Business.TransactionBus.GetUnits(transType, paernId, searchQuery);

            List<TreeNode> nodes = new List<TreeNode>();

            if (units == null || units.Count() == 0)
            {
                return;
            }


            treeViewOrgs.BeginUpdate();
            treeViewOrgs.Nodes.Clear();
            TreeNode rootNode;
            if (string.IsNullOrEmpty(searchQuery))
            {
                units.Where(o => o.ParentId == null || o.ParentId < 1).ToList().ForEach(orgParent =>
                {
                    rootNode = new TreeNode();
                    rootNode.Name = orgParent.Id.ToString();
                    rootNode.Text = string.Format("{0} - {1}", orgParent.Id, orgParent.LocalName);
                    units.Where(oo => oo.ParentId != null && oo.ParentId > 0 && oo.ParentId == orgParent.Id).ToList().ForEach(unitItem =>
                    {
                        rootNode.Nodes.Add(AddTreeChilds(units, unitItem, paernId));
                    });
                    treeViewOrgs.Nodes.Add(rootNode);
                });
            }
            else
            {
                units.ToList().ForEach(orgParent =>
                {
                    rootNode = new TreeNode();
                    rootNode.Name = orgParent.Id.ToString();
                    rootNode.Text = string.Format("{0} - {1}", orgParent.Id, orgParent.LocalName);
                    treeViewOrgs.Nodes.Add(rootNode);
                });
            }

            Cursor.Current = Cursors.Default;
            treeViewOrgs.EndUpdate();


        }
        private TreeNode AddTreeChilds(List<MCS.DTO.ExternalPartyDTO> units, MCS.DTO.ExternalPartyDTO unit, int? parentId)
        {
            TreeNode treeNode = new TreeNode()
            {
                //Checked = unit.IsSelected,
                Name = unit.Id.ToString(),
                Text = string.Format("{0} - {1}", unit.Id, unit.Name)

            };

            //MessageBox.Show(selectedOrgUnitId.ToString());
            //if (selectedOrgUnitId != -1)
            //{
            //    var parentNode = treeViewOrgs.Nodes.OfType<TreeNode>()
            //                .FirstOrDefault(node => node.Name.Equals(selectedOrgUnitId));
            //    MessageBox.Show(Parent.Text);
            //    while (parentNode != null)
            //    {
            //        MessageBox.Show(Parent.Text);
            //        parentNode.Expand();
            //        parentNode = parentNode.Parent;
            //    }

            //}
            units.Where(o => o.ParentId == unit.Id).ToList().ForEach(orgItem =>
            {
                treeNode.Nodes.Add(AddTreeChilds(units, orgItem, parentId));
            });

            return treeNode;
        }

        protected void FillSubNodes(int parentId, TreeView treeViewOrgs, TreeNode selectedNode, TransactionCategorieColor transType)
        {
            List<MCS.DTO.ExternalPartyDTO> units = Business.TransactionBus.GetUnits(transType, parentId, null);


            if (units == null || units.Count() == 0)
            {
                return;
            }

            TreeNode node;
            units.Where(u => u.ParentId == Convert.ToInt32(selectedNode.Name)).ToList().ForEach(orgchild =>
            {
                node = new TreeNode();
                node.Name = orgchild.Id.ToString();
                node.Text = string.Format("{0} - {1}", orgchild.Id, orgchild.LocalName);
                selectedNode.Nodes.Add(node);
            });
            selectedNode.Expand();

        }

        protected Microsoft.Office.Interop.Outlook.MailItem GetSelectedEmail()
        {
            var oApp = new Microsoft.Office.Interop.Outlook.Application();
            if (oApp.ActiveExplorer().Selection.Count == 0)
            {
                return null;
            }

            Object selObject = oApp.ActiveExplorer().Selection[1];

            if (selObject is Microsoft.Office.Interop.Outlook.MailItem)
            {
                var mailItem = (selObject as Microsoft.Office.Interop.Outlook.MailItem);
                return mailItem;
            }

            return null;
        }
        protected void ShowAttachment(string fileName)
        {
            var process = new System.Diagnostics.Process();
            process.StartInfo = new System.Diagnostics.ProcessStartInfo() { UseShellExecute = true, FileName = fileName };
            process.Start();
        }

        protected void FillComboBox(ComboBox comboBox, string displayMember, string valueMember, object dataSourceObj)
        {
            comboBox.DisplayMember = displayMember;
            comboBox.ValueMember = valueMember;
            comboBox.DataSource = dataSourceObj;
        }


    }
}
