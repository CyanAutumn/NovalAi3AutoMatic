using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;
using Microsoft.VisualBasic.Logging;

namespace AutoNai3Tools.utils {
    public class Logger {
        private readonly ILog _logger;
        private Form1 form;

        public Logger(Form1 form) {
            _logger = LogManager.GetLogger(typeof(Logger));
            this.form = form;
        }

        public void FormLog(string msg) {
            string logMessage = $"[{DateTime.Now}] {msg}\r\n";
            UpdateTextBox(this.form.txtLog, logMessage, append: true);
            UpdateTextBox(this.form.txtPicInfo, logMessage, append: false);
        }

        private void UpdateTextBox(TextBox textBox, string message, bool append) {
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

        public void Info(string message) => LogMessage(_logger.Info, "[Info]", message);
        public void Error(string message) => LogMessage(_logger.Error, "[Error]", message);
        public void Debug(string message) => LogMessage(_logger.Debug, "[Debug]", message);
        public void Warn(string message) => LogMessage(_logger.Warn, "[Warn]", message);
        public void Fatal(string message) => LogMessage(_logger.Fatal, "[Fatal]", message);

        private void LogMessage(Action<string> logAction, string prefix, string message) {
            logAction(message);
            FormLog($"{prefix}: {message}");
        }

        public void PicInfo(string message) {
            this.form.txtPicInfo.Text = message;
        }
    }
}
