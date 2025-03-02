using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Microsoft.VisualBasic.Logging;

namespace AutoNai3Tools.utils {
    public class Logger {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(Logger));
        private static Form1 _form;

        public static void Initialize(Form1 form) {
            _form = form;
        }

        public static void FormLog(string msg) {
            string logMessage = $"[{DateTime.Now}] {msg}\r\n";
            UpdateTextBox(_form.txtLog, logMessage, append: true);
            UpdateTextBox(_form.txtPicInfo, logMessage, append: false);
        }

        private static void UpdateTextBox(TextBox textBox, string message, bool append) {
            if (textBox.InvokeRequired) {
                textBox.Invoke(new Action(() => UpdateTextBox(textBox, message, append)));
            }
            else {
                if (append) {
                    textBox.AppendText(message);
                }
                else {
                    textBox.Text = message;
                }

                textBox.SelectionStart = textBox.Text.Length;
                textBox.ScrollToCaret();
            }
        }

        public static void Info(string message) => LogMessage(_logger.Info, "[Info]", message);
        public static void Error(string message) => LogMessage(_logger.Error, "[Error]", message);
        public static void Debug(string message) => LogMessage(_logger.Debug, "[Debug]", message);
        public static void Warn(string message) => LogMessage(_logger.Warn, "[Warn]", message);
        public static void Fatal(string message) => LogMessage(_logger.Fatal, "[Fatal]", message);

        private static void LogMessage(Action<string> logAction, string prefix, string message) {
            string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
            logAction(message);
            FormLog($"{prefix} [{className}]: {message}");
        }

        public static void PicInfo(string message) {
            _form.txtPicInfo.Text = message;
        }
    }
}