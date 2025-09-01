namespace MorasalatOutlookAddIn.Forms
{
    partial class Assignment
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonAddEntity = new System.Windows.Forms.Button();
            this.textBoxOrgName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelMain = new System.Windows.Forms.Panel();
            this.textBoxEmployeeId = new System.Windows.Forms.TextBox();
            this.buttonSave = new System.Windows.Forms.Button();
            this.buttonCancel = new System.Windows.Forms.Button();
            this.comboBoxEmployees = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonAddEntity
            // 
            this.buttonAddEntity.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonAddEntity.Location = new System.Drawing.Point(21, 65);
            this.buttonAddEntity.Name = "buttonAddEntity";
            this.buttonAddEntity.Size = new System.Drawing.Size(53, 23);
            this.buttonAddEntity.TabIndex = 4;
            this.buttonAddEntity.Text = "إختر";
            this.buttonAddEntity.UseVisualStyleBackColor = true;
            this.buttonAddEntity.Click += new System.EventHandler(this.ButtonAddEntity_Click);
            // 
            // textBoxOrgName
            // 
            this.textBoxOrgName.Enabled = false;
            this.textBoxOrgName.Location = new System.Drawing.Point(80, 65);
            this.textBoxOrgName.Name = "textBoxOrgName";
            this.textBoxOrgName.Size = new System.Drawing.Size(383, 22);
            this.textBoxOrgName.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(413, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(50, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "محاله إلى";
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.textBoxEmployeeId);
            this.panelMain.Controls.Add(this.buttonSave);
            this.panelMain.Controls.Add(this.buttonCancel);
            this.panelMain.Controls.Add(this.comboBoxEmployees);
            this.panelMain.Controls.Add(this.label2);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.textBoxOrgName);
            this.panelMain.Controls.Add(this.buttonAddEntity);
            this.panelMain.Location = new System.Drawing.Point(8, 12);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(480, 411);
            this.panelMain.TabIndex = 6;
            // 
            // textBoxEmployeeId
            // 
            this.textBoxEmployeeId.Enabled = false;
            this.textBoxEmployeeId.Location = new System.Drawing.Point(407, 139);
            this.textBoxEmployeeId.Name = "textBoxEmployeeId";
            this.textBoxEmployeeId.Size = new System.Drawing.Size(56, 22);
            this.textBoxEmployeeId.TabIndex = 18;
            // 
            // buttonSave
            // 
            this.buttonSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSave.Location = new System.Drawing.Point(388, 204);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(75, 34);
            this.buttonSave.TabIndex = 16;
            this.buttonSave.Text = "حفظ";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.ButtonSave_Click);
            // 
            // buttonCancel
            // 
            this.buttonCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCancel.Location = new System.Drawing.Point(307, 204);
            this.buttonCancel.Name = "buttonCancel";
            this.buttonCancel.Size = new System.Drawing.Size(75, 34);
            this.buttonCancel.TabIndex = 15;
            this.buttonCancel.Text = "إالغاء";
            this.buttonCancel.UseVisualStyleBackColor = true;
            this.buttonCancel.Click += new System.EventHandler(this.ButtonCancel_Click);
            // 
            // comboBoxEmployees
            // 
            this.comboBoxEmployees.FormattingEnabled = true;
            this.comboBoxEmployees.Location = new System.Drawing.Point(21, 137);
            this.comboBoxEmployees.Name = "comboBoxEmployees";
            this.comboBoxEmployees.Size = new System.Drawing.Size(380, 24);
            this.comboBoxEmployees.TabIndex = 7;
            this.comboBoxEmployees.SelectedIndexChanged += new System.EventHandler(this.ComboBoxEmployees_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(378, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 17);
            this.label2.TabIndex = 6;
            this.label2.Text = "الموظف المختص";
            // 
            // Assignment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(500, 450);
            this.Controls.Add(this.panelMain);
            this.Name = "Assignment";
            this.Text = "إحالة المعاملة";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonAddEntity;
        private System.Windows.Forms.TextBox textBoxOrgName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxEmployees;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxEmployeeId;
    }
}