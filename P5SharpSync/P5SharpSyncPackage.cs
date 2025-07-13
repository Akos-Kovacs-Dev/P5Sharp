using Microsoft.VisualStudio.Shell;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using Task = System.Threading.Tasks.Task;

 
 
namespace P5SharpSync
{

    [PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
    [Guid(P5SharpSyncPackage.PackageGuidString)]
    [ProvideMenuResource("Menus.ctmenu", 1)]
    public sealed class P5SharpSyncPackage : AsyncPackage
    {
      
        /// <summary>
        /// P5SharpSyncPackage GUID string.
        /// </summary>
        public const string PackageGuidString = "eb20a7f5-6986-4def-b891-73a18751b276";

        #region Package Members

        protected override async Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
        {
            await this.JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

            await Command.InitializeAsync(this);
        }

        #endregion




    }


}
