namespace SMaRT.AgentService
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
            this.ProcessInstaller = new System.ServiceProcess.ServiceProcessInstaller();
            this.SMaRTServiceInstaller = new System.ServiceProcess.ServiceInstaller();
            // 
            // ProcessInstaller
            // 
            this.ProcessInstaller.Account = System.ServiceProcess.ServiceAccount.LocalSystem;
            this.ProcessInstaller.Password = null;
            this.ProcessInstaller.Username = null;
            this.ProcessInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ProcessInstallerAfterInstall);
            // 
            // SMaRTServiceInstaller
            // 
            this.SMaRTServiceInstaller.Description = "Installer for the Monitoring Service of the Installer.";
            this.SMaRTServiceInstaller.DisplayName = "SMaRT Monitoring Service";
            this.SMaRTServiceInstaller.ServiceName = "SMaRT Monitor";
            this.SMaRTServiceInstaller.StartType = System.ServiceProcess.ServiceStartMode.Automatic;
            this.SMaRTServiceInstaller.AfterInstall += new System.Configuration.Install.InstallEventHandler(this.ServiceInstallerAfterInstall);
            // 
            // ProjectInstaller
            // 
            this.Installers.AddRange(new System.Configuration.Install.Installer[] {
            this.ProcessInstaller,
            this.SMaRTServiceInstaller});

        }

        #endregion

        private System.ServiceProcess.ServiceProcessInstaller ProcessInstaller;
        private System.ServiceProcess.ServiceInstaller SMaRTServiceInstaller;
    }
}