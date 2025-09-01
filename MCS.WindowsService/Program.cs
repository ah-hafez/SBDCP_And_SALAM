using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MCS.WindowsService
{
    //Install a service  : Uninstall a service : "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe"  [Service.exe Path]
    //UnInstall a Service : Install a service  : "C:\Windows\Microsoft.NET\Framework\v4.0.30319\InstallUtil.exe"  [Service.exe Path] -u
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun = new ServiceBase[] { new EmailSender() };
            ServiceBase.Run(ServicesToRun);
            //Application.Run(new TestOperation());//uncomment If you need to test service
        }
    }
}
