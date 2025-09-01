using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MCS.Common;

namespace MorasalatOutlookAddIn.Forms
{
    public partial class OutboundInternalCreate : FormsBase, Interfaces.Forms.ITreeForm
    {
        #region Constructor
        public OutboundInternalCreate() : base()
        {
            InitializeComponent();
            this.FormSettings();
            base.FillComboBox(comboBoxSourceTypes, "LocalName", "Id", Business.UserProfileBus.GetSourceTypes(TransactionCategory.InternalOutbound));
            base.FillComboBox(comboBoxPriorities, "LocalName", "Id", Business.UserProfileBus.GetPriorities());
            base.FillComboBox(comboBoxUserUnits, "Name", "Id", Helper.UserOrgUnits);
            base.FillComboBox(comboBoxConfidentialityLevel, "Text", "Id", Business.UserProfileBus.GetConfidentialityLevel());
            this.FillEmailInformation();
            base.CloseProgress();
        }


        #endregion

        #region Properties

        int SelectedTreeId { get; set; }
        string SelectedTreeValue { get; set; }
        #endregion

        #region Methods
        private void FormSettings()
        {
            SelectedTreeId = -1;
            pictureBoxDeleteAtt.SizeMode = pictureBoxAddAtt.SizeMode = pictureBoxViewAtt.SizeMode = PictureBoxSizeMode.StretchImage;
            new ToolTip().SetToolTip(pictureBoxAddAtt, "إضافة مرفق");
            new ToolTip().SetToolTip(pictureBoxDeleteAtt, "حذف مرفق");
            new ToolTip().SetToolTip(pictureBoxViewAtt, "عرض مرفق");
            base.FormsBaseSettings(panelMain);
        }

        private void FillSoruceTypes()
        {
            comboBoxSourceTypes.DisplayMember = "LocalName";
            comboBoxSourceTypes.ValueMember = "Id";
            comboBoxSourceTypes.DataSource = Business.UserProfileBus.GetSourceTypes(TransactionCategory.InternalOutbound);
        }
        public void FillTreeSelectedValue(int selectedId, string selectedText)
        {
            SelectedTreeId = selectedId;
            SelectedTreeValue = selectedText;
            textBoxOrgName.Text = selectedText;
        }

        private void FillEmailInformation()
        {
            try
            {

                var mailItem = base.GetSelectedEmail();

                textBoxNotes.Text = mailItem.Body.Trim();
                textBoxSubject.Text = mailItem.Subject;
                string extenstions = ConfigurationManager.AppSettings[Helper.ConfirgurationKeys.ValidUploadExtenstions.ToString()];
                extenstions = extenstions.Split('|')[1];
                for (int i = 1; i < mailItem.Attachments.Count; i++)
                {
                    string fileExt = mailItem.Attachments[i].DisplayName.Split('.')[1];
                    fileExt = string.Format("*.{0}", fileExt);
                    if (extenstions.Contains(fileExt))
                    {
                        listBoxAttachments.Items.Add(mailItem.Attachments[i].DisplayName);
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        private void GetTransactionDocument(MCS.DTO.AddOutboundInternalDTO transObj)
        {
            string newFileName = Guid.NewGuid().ToString();

            if (listBoxAttachments.Items.Count > 0)
            {
                listBoxAttachments.SelectedIndex = 0;
                string fileName = listBoxAttachments.SelectedItem.ToString();
                var fileExtension = new System.IO.FileInfo(fileName).Extension.ToUpper();
                
                string doucumentFullPath = string.Empty;
                if (fileName.Contains("\\")) //its not email attachment.
                {
                    doucumentFullPath = fileName;
                }
                else
                {

                    var mailItem = base.GetSelectedEmail();
                    doucumentFullPath = @downloadPath + "\\" + newFileName + fileExtension;
                    mailItem.Attachments[fileName].SaveAsFile(doucumentFullPath);
                }

                using (MemoryStream memoryStream = new MemoryStream())
                {
                    FileStream fs = new FileStream(doucumentFullPath, FileMode.Open, FileAccess.Read);
                    fs.CopyTo(memoryStream);
                    transObj.DocumentDTO = new MCS.DTO.DocumentDTO
                    {
                        Content = memoryStream.ToArray(),
                        Size = new System.IO.FileInfo(doucumentFullPath).Length,
                        MimeType = fileExtension,
                        Name = newFileName
                    };
                }
            }
            else
            {
                transObj.DocumentDTO = new MCS.DTO.DocumentDTO
                {
                    Content = new byte[1],
                    Size = 1,
                    MimeType = ".TXT",
                    Name = newFileName
                };
            }
        }


        #endregion

        #region Events
        private void ButtonSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBoxOrgName.Text.Trim().Length == 0 || textBoxSubject.Text.Trim().Length == 0 ||
                    textBoxNotes.Text.Trim().Length == 0)
                {
                    MessageBox.Show("الرجاء إدخال الحقول التالية، صادر الى ، الملاحظات، الموضوع",
                        " مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (listBoxAttachments.Items.Count > 1)
                {
                    MessageBox.Show("الرجاء إدخال مرفق واحد فقط",
                       " مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                DateTime emailDate = DateTime.Now;
                var mailItem = base.GetSelectedEmail();
                emailDate = mailItem.CreationTime;

                MCS.DTO.AddOutboundInternalDTO transObj = new MCS.DTO.AddOutboundInternalDTO();
                this.GetTransactionDocument(transObj);
                transObj.HijriRecordDate = DateTimeUtility.ConvertToUmAlQuraCalendar(emailDate);
                transObj.OrgUnitId = Helper.UserOrgUnitId.Value;
                transObj.OutboundInternalBasicInfoAdd = new MCS.DTO.AddOutboundInternalBasicInfoDTO
                {
                    ConfidentialityLevelId = Convert.ToInt32(comboBoxConfidentialityLevel.SelectedValue),
                    PriorityLevelId = Convert.ToInt32(comboBoxPriorities.SelectedValue),
                    Subject = textBoxSubject.Text.Trim(),
                    Remarks = textBoxNotes.Text.Trim(),
                    TransactionTypeId = Convert.ToInt32(comboBoxSourceTypes.SelectedValue), // نوع الخطاب,
                    DeliveryMethodId = Convert.ToInt32(ConfigurationManager.AppSettings[Helper.ConfirgurationKeys.EmailDeliveryMethodId.ToString()]), //طريقة الاستلام
                    LetterTypeId = (int)HubConstants.LetterTypeId,
                    DirectedToId = Helper.UserId,
                    DirectedToOrgUnitId = Helper.UserOrgUnitId.Value,
                };

                string transNo = "0";
                string transId = "0";
                string message = Business.TransactionBus.CreateOutboundInternal(transObj, ref transNo, ref transId);
                if (message.Length > 0)
                {
                    MessageBox.Show(message, "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Helper.AddFieldToOutlook(Helper.OutlookFields.moraslatTransId, transId, mailItem);
                Helper.AddFieldToOutlook(Helper.OutlookFields.moraslatTransNo, transNo, mailItem);
                MessageBox.Show("تم حفظ المعاملة بنجاح", "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ButtonCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ButtonAddEntity_Click(object sender, EventArgs e)
        {
            Forms.TreeOrgs treeForm = new TreeOrgs(SelectedTreeId, this, TransactionCategorieColor.InternalOutbound, "صادر الى");
            treeForm.ShowDialog();
        }

        private void PictureBoxDeleteAtt_Click(object sender, EventArgs e)
        {
            if (listBoxAttachments.SelectedItem == null)
                return;

            int index = listBoxAttachments.SelectedIndex;

            if (listBoxAttachments.Items.Count > 0)
                listBoxAttachments.Items.RemoveAt(index);

            index = index == 0 ? 0 : index - 1;
            if (listBoxAttachments.Items.Count > 0)
            {
                listBoxAttachments.SelectedIndex = index;
            }
        }

        private void ListBoxAttachments_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            PictureBoxViewAtt_Click(pictureBoxViewAtt, e);
        }

        private void PictureBoxAddAtt_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = ConfigurationManager.AppSettings[Helper.ConfirgurationKeys.ValidUploadExtenstions.ToString()];
            openFileDialog.CheckFileExists = true;
            openFileDialog.AddExtension = true;
            openFileDialog.Multiselect = true;

            if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    if (listBoxAttachments.Items.Contains(fileName))
                    {
                        MessageBox.Show("اسم المرفق مضاف سابقا، لا يمكن تكرار اسماء المرفقات", "مراسلات", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    listBoxAttachments.Items.Add(fileName);
                }
            }


        }

        private void PictureBoxViewAtt_Click(object sender, EventArgs e)
        {
            try
            {
                if (listBoxAttachments.Items.Count == 0)
                    return;
                string attachmentName = listBoxAttachments.SelectedItem.ToString();

                if (attachmentName.Contains("\\")) //its not email attachment.
                {

                    base.ShowAttachment(attachmentName);
                    return;
                }

                var mailItem = base.GetSelectedEmail();
                mailItem.Attachments[attachmentName].SaveAsFile(@downloadPath + "\\" + attachmentName);
                base.ShowAttachment(@downloadPath + "\\" + attachmentName);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

        private void loadingPage_Load(object sender, EventArgs e)
        {

        }

        private void OutboundInternalCreate_Load(object sender, EventArgs e)
        {

        }
    }
}
