using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Office.Interop.Outlook;
using Microsoft.Office.Tools.Ribbon;

namespace MorasalatOutlookAddIn
{
    public partial class RibbonMain
    {
        private Explorer _activeExplorer;

        private void RibbonMain_Load(object sender, RibbonUIEventArgs e)
        {
            this.tab1.Label = Helper.GetCultureName == "ar" ? "مراسلات" : "Moraslat";
            var oApp = new Microsoft.Office.Interop.Outlook.Application();
            _activeExplorer = oApp.Explorers[1];
            _activeExplorer.SelectionChange += _activeExplorer_SelectionChange;
            _activeExplorer_SelectionChange();

        }

        private void _activeExplorer_SelectionChange()
        {
            var oApp = new Microsoft.Office.Interop.Outlook.Application();
            if (oApp.ActiveExplorer().Selection.Count == 0)
            {
                return;
            }

            Object selObject = oApp.ActiveExplorer().Selection[1];
            var mailItem = (selObject as Microsoft.Office.Interop.Outlook.MailItem);
            string tranactionNo = Helper.GetOutlookFieldValue(mailItem, Helper.OutlookFields.moraslatTransNo);
            string assignment = Helper.GetOutlookFieldValue(mailItem, Helper.OutlookFields.moraslatAssignment);
            btnAssignment.Enabled = false;
            if (tranactionNo.Length > 0)
            {
                labelTranactionNo.Label = Helper.GetCultureName == "ar" ? string.Format("رقم المعاملة : {0}", tranactionNo) : string.Format("Transaction No. : {0}", tranactionNo);

                if (assignment.Length == 0)
                {
                    btnAssignment.Enabled = true;
                }

            }

            labelTranactionNo.Visible = tranactionNo.Length > 0 ? true : false;
            btnAddInbound.Enabled = btnAddOutbound.Enabled = tranactionNo.Length > 0 ? false : true;

        }
        private void BtnAddOutbound_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (!this.CheckEmailIsValid())
                    return;

                Forms.OutboundInternalCreate obj = new Forms.OutboundInternalCreate();
                obj.ShowDialog();
            }
            catch (System.Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void BtnAddInbound_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (!this.CheckEmailIsValid())
                    return;

                Forms.InboundCreate obj = new Forms.InboundCreate();
                obj.ShowDialog();
            }
            catch (System.Exception ex)
            {
                ExceptionHandler(ex);
            }

        }

        private bool CheckEmailIsValid()
        {

            Business.UserProfileBus.Login();
            if (string.IsNullOrEmpty(Helper.AccessToken))
            {
                string message = string.Format("البريد الالكتروني غير معرف لنظام مراسلات، {0}.", Helper.GetEmailAddress);
                MessageBox.Show(message, "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                //btnAssignment.Enabled = btnAddInbound.Enabled = btnAddOutbound.Enabled = false;

                return false;
            }

            return true;
        }

        private void BtnAssignment_Click(object sender, RibbonControlEventArgs e)
        {
            try
            {
                if (!this.CheckEmailIsValid())
                    return;

                Forms.Assignment obj = new Forms.Assignment();
                obj.ShowDialog();
            }
            catch (System.Exception ex)
            {
                ExceptionHandler(ex);
            }
        }

        private void ExceptionHandler(System.Exception ex)
        {
            for (int i = 0; i < System.Windows.Forms.Application.OpenForms.Count; i++)
            {
                Form form = System.Windows.Forms.Application.OpenForms[i];

                if (form is Forms.Loading)
                    form.Close();
            }

            MessageBox.Show("حدث خطأ !");
        }
    }
}
