using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using AutoUpdaterDotNET;

namespace AutoNai3Tools
{
    internal static class Program
    {
        private const string VersionXmlUrl =
            "https://github.com/CyanAutumn/NovalAi3AutoMatic/releases/latest/download/version.xml";

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var mainForm = new Form1();
            if (!Debugger.IsAttached)
                mainForm.Shown += (_, __) => TryStartAutoUpdate();

            Application.Run(mainForm);
        }

        private static void TryStartAutoUpdate()
        {
            try
            {
                ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

                AutoUpdater.ReportErrors = false;
                AutoUpdater.ApplicationExitEvent += AutoUpdater_ApplicationExitEvent;
                AutoUpdater.Start(VersionXmlUrl);
            }
            catch
            {
            }
        }

        private static void AutoUpdater_ApplicationExitEvent()
        {
            Application.Exit();
        }
    }
}
