using AutoNai3Tools.body;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;

namespace AutoNai3Tools.utils {
    internal class Tools {
        public static string SelectFile(string filter, bool is_file) {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = Properties.Resources.Dialog_SelectTitle;
            openFileDialog.CheckFileExists = false;
            openFileDialog.FileName = Properties.Resources.Dialog_SelectDefaultFileName; // 设置一个默认的文件名
            openFileDialog.Filter = filter;
            if (openFileDialog.ShowDialog() == DialogResult.OK) {
                string path = null;
                if (is_file) {
                    path = openFileDialog.FileName;
                }
                else {
                    path = System.IO.Path.GetDirectoryName(openFileDialog.FileName);
                }
                return path;
            }
            return null;
        }

        public static string SelectFolder(string initialPath = null) {
            return FolderPicker.PickFolder(initialPath);
        }
        public static string SelectIMGFile() {
            return SelectFile("Image Files|*.jpg;*.jpeg;*.png;*.bmp", true);
        }
        public static string SelectVibeFile() {
            return SelectFile("Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.naiv4vibe", true);
        }

        public static bool IsExist(string path, bool isCreateFolder) {
            if (!Directory.Exists(path)) {
                if (isCreateFolder) {
                    Directory.CreateDirectory(path);
                    return true;
                }
                return false;
            }
            return true;
        }

        public static bool InsertCommasAroundCursor(TextBox textBox) {
            int cursorPosition = textBox.SelectionStart;
            string text = textBox.Text;

            bool needLeftComma = cursorPosition > 0 && text[cursorPosition - 1] != ',';
            bool needRightComma = cursorPosition < text.Length && text[cursorPosition] != ',';
            string insertText = (needLeftComma ? "," : "") + (needRightComma ? "," : "");

            if (!string.IsNullOrEmpty(insertText)) {
                textBox.Text = text.Insert(cursorPosition, insertText);
                textBox.SelectionStart = cursorPosition + (needLeftComma ? 1 : 0);
            }
            return needRightComma;
        }

        public static void InsertTextToTextBox(TextBox textBox, string insertPrompt) {
            if (textBox.Text.Contains(insertPrompt)) {
                //string pattern = ",\\s*"+insertPrompt+"\\s*,";
                //txtPrompt.Text = Regex.Replace(txtPrompt.Text,pattern, "");
                //string pattern = ",\\s*"+insertPrompt;
                //txtPrompt.Text = Regex.Replace(txtPrompt.Text,pattern, "");
                string pattern = insertPrompt + "\\s*,";
                int cursorPosition = textBox.SelectionStart;

                // 找到要删除的匹配项
                Match match = Regex.Match(textBox.Text, pattern);

                if (match.Success) {
                    int deletionStartIndex = match.Index;
                    int deletionLength = match.Length;

                    // 判断删除的文字在光标前面还是后面
                    if (deletionStartIndex < cursorPosition) {
                        // 如果删除的文字在光标前面，光标位置向前移动对应的格子
                        textBox.Text = Regex.Replace(textBox.Text, pattern, "");
                        textBox.Text = textBox.Text.Replace(insertPrompt, "");

                        // 调整光标位置
                        int newCursorPosition = cursorPosition - deletionLength;
                        if (newCursorPosition < 0)
                            newCursorPosition = 0;  // 防止光标位置变为负数

                        textBox.SelectionStart = newCursorPosition;
                    }
                    else {
                        // 如果删除的文字在光标后面，光标位置不变
                        textBox.Text = Regex.Replace(textBox.Text, pattern, "");
                        textBox.Text = textBox.Text.Replace(insertPrompt, "");

                        // 光标位置保持不变
                        textBox.SelectionStart = cursorPosition;
                    }
                }
                else {
                    // 如果没有找到匹配项，则直接删除插入提示文字，光标位置不变
                    textBox.Text = textBox.Text.Replace(insertPrompt, "");
                    textBox.SelectionStart = cursorPosition;
                }
            }
            else {
                int cursorPosition = textBox.SelectionStart;

                bool needRightComma = Tools.InsertCommasAroundCursor(textBox);
                if (cursorPosition == 0)
                    textBox.Text = insertPrompt + textBox.Text;
                else {
                    cursorPosition = textBox.SelectionStart;
                    textBox.Text = textBox.Text.Insert(cursorPosition, insertPrompt);
                }

                textBox.SelectionStart = cursorPosition + insertPrompt.Length + (needRightComma ? 1 : 0);
            }
        }

        public static string ConvertImageToBase64(string imagePath) {
            try {
                using (System.Drawing.Image image = System.Drawing.Image.FromFile(imagePath)) {
                    using (MemoryStream memoryStream = new MemoryStream()) {
                        image.Save(memoryStream, image.RawFormat);
                        byte[] imageBytes = memoryStream.ToArray();
                        string base64String = Convert.ToBase64String(imageBytes);
                        return base64String;
                    }
                }
            }
            catch (Exception ex) {
                Console.WriteLine("发生错误: " + ex.Message);
                return null;
            }
        }

        public static void ShowImage(string path, PictureBox pictureBox) {
            if (pictureBox.Image != null) {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

            if (path.EndsWith(".naiv4vibe")) return;
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                using (MemoryStream ms = new MemoryStream()) {
                    fs.CopyTo(ms);
                    ms.Position = 0;
                    pictureBox.Image = System.Drawing.Image.FromStream(ms);
                }
            }
        }

        public static string[] GetFileLine(string folderPath, string fileName) {
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
            string filePath = folderPath + "\\" + fileName + ".txt";
            string[] results = File.ReadAllLines(filePath);
            return results;
        }

        public static int GetFileSize(string folderPath) {
            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
            return txtFiles.Length;
        }

        private const int PromptReadMaxAttempts = 3;
        private const int PromptReadBaseDelayMs = 200;
        private const int CloudTimeoutHResult = unchecked((int)0x80070185);

        private static string TryReadPromptFile(string path, out Exception error) {
            error = null;
            for (int attempt = 1; attempt <= PromptReadMaxAttempts; attempt++) {
                try {
                    return File.ReadAllText(path).Replace(Environment.NewLine, " ");
                }
                catch (FileNotFoundException ex) {
                    error = ex;
                    return null;
                }
                catch (DirectoryNotFoundException ex) {
                    error = ex;
                    return null;
                }
                catch (UnauthorizedAccessException ex) {
                    error = ex;
                    return null;
                }
                catch (IOException ex) {
                    error = ex;
                    if (ex.HResult == CloudTimeoutHResult) {
                        return null;
                    }

                    if (attempt < PromptReadMaxAttempts) {
                        System.Threading.Thread.Sleep(PromptReadBaseDelayMs * attempt);
                    }
                }
            }

            return null;
        }

        public static string GetPromptFromFolderTxt(string folderPath, int index) {
            if (string.IsNullOrWhiteSpace(folderPath)) {
                Logger.Warn("随机提示词文件夹路径为空");
                return string.Empty;
            }

            string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");
            if (txtFiles.Length == 0) {
                Logger.Warn("随机提示词文件夹内未找到txt文件",
                    context: Logger.Context(("folderPath", folderPath)));
                return string.Empty;
            }

            int normalizedIndex = ((index % txtFiles.Length) + txtFiles.Length) % txtFiles.Length;
            Exception readError;
            string content = TryReadPromptFile(txtFiles[normalizedIndex], out readError);
            if (content != null) {
                return content;
            }

            for (int offset = 1; offset < txtFiles.Length; offset++) {
                int candidateIndex = (normalizedIndex + offset) % txtFiles.Length;
                content = TryReadPromptFile(txtFiles[candidateIndex], out _);
                if (content != null) {
                    Logger.Warn("随机提示词读取失败，已切换到其他文件",
                        context: Logger.Context(("path", txtFiles[normalizedIndex]), ("fallback", txtFiles[candidateIndex])));
                    return content;
                }
            }

            Logger.Error("随机提示词读取失败，所有文件不可用", exception: readError,
                context: Logger.Context(("folderPath", folderPath)));
            return string.Empty;
        }
    }
}
