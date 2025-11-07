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

        public static void Info(string message, bool showToTextBox = true, bool saveToLocal = true) => LogMessage(_logger.Info, "[Info]", message, showToTextBox, saveToLocal);
        public static void Error(string message, bool showToTextBox = true, bool saveToLocal = true) => LogMessage(_logger.Error, "[Error]", message, showToTextBox, saveToLocal);
        public static void Debug(string message, bool showToTextBox = true, bool saveToLocal = true) => LogMessage(_logger.Debug, "[Debug]", message, showToTextBox, saveToLocal);
        public static void Warn(string message, bool showToTextBox = true, bool saveToLocal = true) => LogMessage(_logger.Warn, "[Warn]", message, showToTextBox, saveToLocal);
        public static void Fatal(string message, bool showToTextBox = true, bool saveToLocal = true) => LogMessage(_logger.Fatal, "[Fatal]", message, showToTextBox, saveToLocal);

        private static void LogMessage(Action<string> logAction, string prefix, string message, bool showToTextBox, bool saveToLocal) {
            string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;
            
            // 根据saveToLocal参数决定是否保存到本地文件
            if (saveToLocal) {
                logAction(message);
            }
            
            // 根据showToTextBox参数决定是否显示到界面
            if (showToTextBox) {
                FormLog($"{prefix} [{className}]: {message}");
            }
        }

        public static void PicInfo(string message) {
            _form.txtPicInfo.Text = message;
        }
    }
}