using System;
using System.Diagnostics;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using AutoUpdaterDotNET;
using AutoNai3Tools.utils;

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
            ApplySavedCulture();
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

        private static void ApplySavedCulture()
        {
            try
            {
                var repository = new SystemConfigRepository();
                var config = repository.Load();
                var cultureName = config?.UiLanguage;
                if (string.IsNullOrWhiteSpace(cultureName))
                    return;

                var culture = new CultureInfo(cultureName);
                CultureInfo.DefaultThreadCurrentCulture = culture;
                CultureInfo.DefaultThreadCurrentUICulture = culture;
                Thread.CurrentThread.CurrentCulture = culture;
                Thread.CurrentThread.CurrentUICulture = culture;
            }
            catch
            {
            }
        }
    }
}
