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
    public partial class Assignment : FormsBase, Interfaces.Forms.ITreeForm
    {
        public Assignment():base()
        {
            InitializeComponent();
            this.SelectedTreeId = -1;
            base.FormsBaseSettings(panelMain);
            base.CloseProgress();
        }

        #region Properties
        int SelectedTreeId { get; set; }
        string SelectedTreeText { get; set; }
        #endregion

        public void FillTreeSelectedValue(int selectedId, string selectedText)
        {
            SelectedTreeId =  selectedId;
            SelectedTreeText = textBoxOrgName.Text = selectedText;

            comboBoxEmployees.DataSource = null;
            comboBoxEmployees.Items.Clear();
            base.FillComboBox(comboBoxEmployees, "LocalName", "Id", Business.UserProfileBus.GetUsersByOrgUnitId(SelectedTreeId));
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonSave_Click(object sender, EventArgs e)
        {
            if(comboBoxEmployees.SelectedValue == null || textBoxOrgName.Text.Trim().Length ==0)
            {
                MessageBox.Show("الرجاء إدخال الإداره أو الموظف المختص",
                       "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("هل أنت متأكد من عمليةالإحالة", "مراسلات", MessageBoxButtons.YesNo,MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            TransactionAssignmentDTO dtoObj = new TransactionAssignmentDTO();
            dtoObj.FromOrgUnitId = Helper.UserOrgUnitId.Value;
            dtoObj.FromUserId = Helper.UserId.Value;
            dtoObj.ActionId = 1; //not used
            dtoObj.DeliveryMethodId = (int)DeliveryMethodType.Electronic;
            dtoObj.ToOrgUnitId = this.SelectedTreeId;
            dtoObj.TrayId = (int)TrayType.OrgUnit;

            if (comboBoxEmployees.SelectedValue.ToString() != "-1")
            {
                dtoObj.TrayId = (int)TrayType.MyTransactions;
                dtoObj.ToUserId = Convert.ToInt32(comboBoxEmployees.SelectedValue);
            }

            dtoObj.IsAssigned = true;
            
            string transationId = Helper.GetOutlookFieldValue(base.GetSelectedEmail(), Helper.OutlookFields.moraslatTransId);
            if(transationId.Length ==0)
            {
                MessageBox.Show("لم يتم حفظ tranaction Id في النظام", "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string message = Business.TransactionBus.AssignmentCreate(dtoObj,transationId);
            if(message.Length ==0)
            {
                MessageBox.Show("تم إحالة المعاملة بنجاح", "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Helper.AddFieldToOutlook(Helper.OutlookFields.moraslatAssignment, "1", base.GetSelectedEmail());
                this.Close();
            }
            else
            {
                MessageBox.Show(message, "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void ButtonAddEntity_Click(object sender, EventArgs e)
        {
            Forms.TreeOrgs treeForm = new TreeOrgs(SelectedTreeId, this, TransactionCategorieColor.InternalOutbound,"محاله الى");
            treeForm.ShowDialog();
        }

        private void ComboBoxEmployees_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxEmployeeId.Text = string.Empty;
            if (comboBoxEmployees.SelectedValue.ToString() != "-1")
               textBoxEmployeeId.Text = comboBoxEmployees.SelectedValue.ToString();
        }
    }
}
