namespace MorasalatOutlookAddIn
{
    partial class RibbonMain : Microsoft.Office.Tools.Ribbon.RibbonBase
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public RibbonMain()
            : base(Globals.Factory.GetRibbonFactory())
        {
            InitializeComponent();
        }

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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RibbonMain));
            this.tab1 = this.Factory.CreateRibbonTab();
            this.groupAddTrans = this.Factory.CreateRibbonGroup();
            this.btnAddOutbound = this.Factory.CreateRibbonButton();
            this.btnAddInbound = this.Factory.CreateRibbonButton();
            this.groupTransfer = this.Factory.CreateRibbonGroup();
            this.btnAssignment = this.Factory.CreateRibbonButton();
            this.groupName = this.Factory.CreateRibbonGroup();
            this.labelTranactionNo = this.Factory.CreateRibbonLabel();
            this.tab1.SuspendLayout();
            this.groupAddTrans.SuspendLayout();
            this.groupTransfer.SuspendLayout();
            this.groupName.SuspendLayout();
            this.SuspendLayout();
            // 
            // tab1
            // 
            this.tab1.ControlId.ControlIdType = Microsoft.Office.Tools.Ribbon.RibbonControlIdType.Office;
            this.tab1.Groups.Add(this.groupAddTrans);
            this.tab1.Groups.Add(this.groupTransfer);
            this.tab1.Groups.Add(this.groupName);
            this.tab1.Label = "مراسلات";
            this.tab1.Name = "tab1";
            // 
            // groupAddTrans
            // 
            this.groupAddTrans.Items.Add(this.btnAddOutbound);
            this.groupAddTrans.Items.Add(this.btnAddInbound);
            this.groupAddTrans.Name = "groupAddTrans";
            // 
            // btnAddOutbound
            // 
            this.btnAddOutbound.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAddOutbound.Image = global::MorasalatOutlookAddIn.Properties.Resources.create_new_icon_29;
            this.btnAddOutbound.Label = "إنشاء معامله داخلية";
            this.btnAddOutbound.Name = "btnAddOutbound";
            this.btnAddOutbound.ScreenTip = "إنشاء معامله داخلية";
            this.btnAddOutbound.ShowImage = true;
            this.btnAddOutbound.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnAddOutbound_Click);
            // 
            // btnAddInbound
            // 
            this.btnAddInbound.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAddInbound.Image = ((System.Drawing.Image)(resources.GetObject("btnAddInbound.Image")));
            this.btnAddInbound.Label = "إنشاء وارد";
            this.btnAddInbound.Name = "btnAddInbound";
            this.btnAddInbound.ScreenTip = "إنشاء وارد";
            this.btnAddInbound.ShowImage = true;
            this.btnAddInbound.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnAddInbound_Click);
            // 
            // groupTransfer
            // 
            this.groupTransfer.Items.Add(this.btnAssignment);
            this.groupTransfer.Name = "groupTransfer";
            // 
            // btnAssignment
            // 
            this.btnAssignment.ControlSize = Microsoft.Office.Core.RibbonControlSize.RibbonControlSizeLarge;
            this.btnAssignment.Image = global::MorasalatOutlookAddIn.Properties.Resources.employee_396_733919;
            this.btnAssignment.Label = "إحالة";
            this.btnAssignment.Name = "btnAssignment";
            this.btnAssignment.ShowImage = true;
            this.btnAssignment.Click += new Microsoft.Office.Tools.Ribbon.RibbonControlEventHandler(this.BtnAssignment_Click);
            // 
            // groupName
            // 
            this.groupName.Items.Add(this.labelTranactionNo);
            this.groupName.Name = "groupName";
            // 
            // labelTranactionNo
            // 
            this.labelTranactionNo.Label = "Tranaction No";
            this.labelTranactionNo.Name = "labelTranactionNo";
            // 
            // RibbonMain
            // 
            this.Name = "RibbonMain";
            this.RibbonType = "Microsoft.Outlook.Explorer, Microsoft.Outlook.Mail.Read";
            this.Tabs.Add(this.tab1);
            this.Load += new Microsoft.Office.Tools.Ribbon.RibbonUIEventHandler(this.RibbonMain_Load);
            this.tab1.ResumeLayout(false);
            this.tab1.PerformLayout();
            this.groupAddTrans.ResumeLayout(false);
            this.groupAddTrans.PerformLayout();
            this.groupTransfer.ResumeLayout(false);
            this.groupTransfer.PerformLayout();
            this.groupName.ResumeLayout(false);
            this.groupName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal Microsoft.Office.Tools.Ribbon.RibbonTab tab1;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupAddTrans;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAddOutbound;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAddInbound;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupTransfer;
        internal Microsoft.Office.Tools.Ribbon.RibbonButton btnAssignment;
        internal Microsoft.Office.Tools.Ribbon.RibbonGroup groupName;
        internal Microsoft.Office.Tools.Ribbon.RibbonLabel labelTranactionNo;
    }

    partial class ThisRibbonCollection
    {
        internal RibbonMain RibbonMain
        {
            get { return this.GetRibbon<RibbonMain>(); }
        }
    }
}
