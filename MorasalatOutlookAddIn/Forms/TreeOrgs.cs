using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCS.Common;
using MCS.DTO;

namespace MorasalatOutlookAddIn.Forms
{
    public partial class TreeOrgs : FormsBase
    {

        private Interfaces.Forms.ITreeForm _openerForm;
        private TransactionCategorieColor _transType;
        private int? _selectedOrgId;
        public TreeOrgs(int? selectedOrgId,Interfaces.Forms.ITreeForm openerForm, TransactionCategorieColor transType,string pageName)
        {
            InitializeComponent();
            base.CloseProgress();
            base.FormsBaseSettings(panelMain);
            _openerForm = openerForm;
            _transType = transType;
            _selectedOrgId = selectedOrgId;
            this.Text = pageName;
            base.FillTreeWithUnits(null, treeViewOrgs, _transType,null);
            this.SetDefualtSelection();
            

        }

        private void SetDefualtSelection()
        {
            if (treeViewOrgs.Nodes.Count == 0)
                return;

            treeViewOrgs.Nodes[0].Checked = true;
            treeViewOrgs.SelectedNode = treeViewOrgs.Nodes[0];
            TreeViewOrgs_NodeMouseDoubleClick(treeViewOrgs, null);
        }

        private void ButtonSelect_Click(object sender, EventArgs e)
        {
            try
            {
                _openerForm.FillTreeSelectedValue(Convert.ToInt32(treeViewOrgs.SelectedNode.Name), treeViewOrgs.SelectedNode.Text);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        
    }

        private void ButtonClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void TreeViewOrgs_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (treeViewOrgs.SelectedNode.Nodes.Count > 0)
                return;

            base.FillSubNodes(Convert.ToInt32(treeViewOrgs.SelectedNode.Name), treeViewOrgs, treeViewOrgs.SelectedNode, _transType);

        }

        private void TextBoxSearch_TextChanged(object sender, EventArgs e)
        {
            TextBox txt = sender as TextBox;
            if (txt == textBoxUnitId)
                textBoxUnitName.Text = string.Empty;
            else
                textBoxUnitId.Text = string.Empty;

        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            string searchText = textBoxUnitId.Text.Trim().Length > 0 ? textBoxUnitId.Text.Trim() : textBoxUnitName.Text.Trim();

            if (searchText.Length > 0)
                base.FillTreeWithUnits(null, treeViewOrgs, _transType, searchText);
            else
                base.FillTreeWithUnits(null, treeViewOrgs, _transType, null);

            this.SetDefualtSelection();
        }

       

        private void loadingPage_Load(object sender, EventArgs e)
        {

        }
    }
}
