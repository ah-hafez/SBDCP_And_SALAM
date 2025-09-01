namespace MorasalatOutlookAddIn.Forms
{
    partial class TreeOrgs
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
            this.treeViewOrgs = new System.Windows.Forms.TreeView();
            this.buttonSelect = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.panelMain = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.labelName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSearch = new System.Windows.Forms.Button();
            this.textBoxUnitName = new System.Windows.Forms.TextBox();
            this.textBoxUnitId = new System.Windows.Forms.TextBox();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // treeViewOrgs
            // 
            this.treeViewOrgs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treeViewOrgs.Location = new System.Drawing.Point(12, 101);
            this.treeViewOrgs.Name = "treeViewOrgs";
            this.treeViewOrgs.Size = new System.Drawing.Size(405, 454);
            this.treeViewOrgs.TabIndex = 0;
            this.treeViewOrgs.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.TreeViewOrgs_NodeMouseDoubleClick);
            // 
            // buttonSelect
            // 
            this.buttonSelect.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonSelect.Location = new System.Drawing.Point(343, 564);
            this.buttonSelect.Name = "buttonSelect";
            this.buttonSelect.Size = new System.Drawing.Size(75, 34);
            this.buttonSelect.TabIndex = 16;
            this.buttonSelect.Text = "إختيار";
            this.buttonSelect.UseVisualStyleBackColor = true;
            this.buttonSelect.Click += new System.EventHandler(this.ButtonSelect_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonClose.Location = new System.Drawing.Point(247, 564);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(75, 34);
            this.buttonClose.TabIndex = 15;
            this.buttonClose.Text = "إغلاق";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.ButtonClose_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.label3);
            this.panelMain.Controls.Add(this.labelName);
            this.panelMain.Controls.Add(this.label1);
            this.panelMain.Controls.Add(this.btnSearch);
            this.panelMain.Controls.Add(this.textBoxUnitName);
            this.panelMain.Controls.Add(this.textBoxUnitId);
            this.panelMain.Controls.Add(this.treeViewOrgs);
            this.panelMain.Controls.Add(this.buttonSelect);
            this.panelMain.Controls.Add(this.buttonClose);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(441, 605);
            this.panelMain.TabIndex = 17;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(388, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(30, 17);
            this.label3.TabIndex = 22;
            this.label3.Text = "الرقم";
            // 
            // labelName
            // 
            this.labelName.AutoSize = true;
            this.labelName.Location = new System.Drawing.Point(302, 24);
            this.labelName.Name = "labelName";
            this.labelName.Size = new System.Drawing.Size(32, 17);
            this.labelName.TabIndex = 21;
            this.labelName.Text = "الإسم";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 17);
            this.label1.TabIndex = 20;
            // 
            // btnSearch
            // 
            this.btnSearch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSearch.Location = new System.Drawing.Point(12, 48);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(75, 34);
            this.btnSearch.TabIndex = 19;
            this.btnSearch.Text = "بحث";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.BtnSearch_Click);
            // 
            // textBoxUnitName
            // 
            this.textBoxUnitName.Location = new System.Drawing.Point(96, 54);
            this.textBoxUnitName.Name = "textBoxUnitName";
            this.textBoxUnitName.Size = new System.Drawing.Size(238, 22);
            this.textBoxUnitName.TabIndex = 18;
            this.textBoxUnitName.TextChanged += new System.EventHandler(this.TextBoxSearch_TextChanged);
            // 
            // textBoxUnitId
            // 
            this.textBoxUnitId.Location = new System.Drawing.Point(352, 54);
            this.textBoxUnitId.Name = "textBoxUnitId";
            this.textBoxUnitId.Size = new System.Drawing.Size(66, 22);
            this.textBoxUnitId.TabIndex = 17;
            this.textBoxUnitId.TextChanged += new System.EventHandler(this.TextBoxSearch_TextChanged);
            // 
            // TreeOrgs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(441, 605);
            this.Controls.Add(this.panelMain);
            this.Name = "TreeOrgs";
            this.Text = "الجهات";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView treeViewOrgs;
        private System.Windows.Forms.Button buttonSelect;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.TextBox textBoxUnitName;
        private System.Windows.Forms.TextBox textBoxUnitId;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label labelName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
    }
}