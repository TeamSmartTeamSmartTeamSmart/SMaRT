// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ProjectInstaller.cs" company="Novomatic Gaming Industries GmbH">
//   Copyright 2017 Novomatic Gaming Industries GmbH.
// </copyright>
// <summary>
//   Defines the ProjectInstaller type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace SMaRT.AgentService
{
    using System.ComponentModel;
    using System.Configuration.Install;

    /// <summary>
    /// The project installer.
    /// </summary>
    [RunInstaller(true)]
    public partial class ProjectInstaller : Installer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProjectInstaller"/> class.
        /// </summary>
        public ProjectInstaller()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// The service installer after install.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ServiceInstallerAfterInstall(object sender, InstallEventArgs e)
        {
        }

        /// <summary>
        /// The process installer after install.
        /// </summary>
        /// <param name="sender">
        /// The sender.
        /// </param>
        /// <param name="e">
        /// The e.
        /// </param>
        private void ProcessInstallerAfterInstall(object sender, InstallEventArgs e)
        {
        }
    }
}
