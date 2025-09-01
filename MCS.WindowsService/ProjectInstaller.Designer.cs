namespace MCS.WindowsService
{
    partial class ProjectInstaller
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

        #region Component Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.serviceProcessInstaller1 = new System.ServiceProcess.ServiceProcessInstaller();
            this.ServiceTaskCheck = new System.ServiceProcess.ServiceInstaller();
            this.ServiceEmailSender = new System.ServiceProcess.ServiceInstaller();
            this.ServiceDocumentMigration = new System.ServiceProcess.ServiceInstaller();
            // 
            // serviceProcessInstaller1
            // 
            this.serviceProcessInstaller1.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.serviceProcessInstaller1.Password = null;
            this.serviceProcessInstaller1.Username = null;
            // 
            // ServiceTaskCheck
            // 
            this.ServiceTaskCheck.DisplayName = "MCS.TaskExpirationCheck";
            this.ServiceTaskCheck.ServiceName = "TaskExpirationCheck";
            // 
            // ServiceEmailSender
            // 
            this.ServiceEmailSender.DisplayName = "MCS.EmailSender";
            this.ServiceEmailSender.ServiceName = "EmailSender";
            // 
            // ServiceDocumentMigration
            // 
            this.ServiceDocumentMigration.DisplayName = "MCS.DocumentMigration";
            this.ServiceDocumentMigration.ServiceName = "DocumentMigration";
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.serviceProcessInstaller1,
            this.ServiceTaskCheck,
            this.ServiceEmailSender,
            this.ServiceDocumentMigration});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller serviceProcessInstaller1;
        private System.ServiceProcess.ServiceInstaller ServiceEmailSender;
        private System.ServiceProcess.ServiceInstaller ServiceTaskCheck;
        private System.ServiceProcess.ServiceInstaller ServiceDocumentMigration;
    }
}