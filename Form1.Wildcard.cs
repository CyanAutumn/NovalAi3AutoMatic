using System;
using System.IO;
using System.Windows.Forms;
using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 {
        #region Wildcard

        private void InitTagSnippetDGV() {
            try {
                string folderPath = picProps.WildcardFolderPath;
                if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException();
                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

                dgvTagSnippet.Rows.Clear();
                foreach (string file in txtFiles) {
                    string fileName = Path.GetFileName(file);
                    string fileContent = File.ReadAllText(file);
                    dgvTagSnippet.Rows.Add(fileName, fileContent);
                }
            }
            catch (Exception ex) {
                Logger.Warn("未能加载 wildcard 片段文件",
                    context: Logger.Context(("folder", picProps.WildcardFolderPath), ("reason", ex.Message)));
            }
        }

        private void btnTagSnippetAdd_Click(object sender, EventArgs e) {
            if (txtTagSnippetName.Text != "") {
                foreach (DataGridViewRow row in dgvTagSnippet.Rows) {
                    if (row.Cells[0].Value != null) {
                        if (row.Cells[0].Value.ToString() == (txtTagSnippetName.Text +=
                                (txtTagSnippetName.Text.EndsWith(".txt") ? "" : ".txt"))) {
                            Logger.Warn("片段名已存在，无法添加",
                                context: Logger.Context(("snippet", txtTagSnippetName.Text)));
                            return;
                        }
                    }
                }

                if (txtTagSnippetName.Text == "") {
                    Logger.Warn("片段名不能为空",
                        context: Logger.Context(("action", "SnippetAdd")));
                    return;
                }

                string fileName = txtTagSnippetName.Text;
                if (!fileName.EndsWith(".txt"))
                    fileName = fileName + ".txt";
                string fileContent = txtTagSnippetValue.Text;
                string folderPath = picProps.WildcardFolderPath;
                string filePath = Path.Combine(folderPath, fileName);
                Tools.IsExist(folderPath, true);
                File.WriteAllText(filePath, fileContent);
                dgvTagSnippet.Rows.Add(txtTagSnippetName.Text, txtTagSnippetValue.Text);

                Logger.Info("片段已新增",
                    context: Logger.Context(("snippet", fileName), ("folder", folderPath)));
            }
            else {
                Logger.Warn("片段名不能为空",
                    context: Logger.Context(("action", "SnippetAdd")));
            }
        }

        private void btnTagSnippetEdit_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow.Index == 0) {
                Logger.Warn("未选择要编辑的片段",
                    context: Logger.Context(("action", "SnippetEdit")));
                return;
            }

            foreach (DataGridViewRow row in dgvTagSnippet.Rows) {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == txtTagSnippetName.Text) {
                    string fileContent = txtTagSnippetValue.Text;
                    string fileName = dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[0].Value.ToString();
                    string folderPath = picProps.WildcardFolderPath;
                    string filePath = Path.Combine(folderPath, fileName);
                    File.WriteAllText(filePath, fileContent);
                    dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[1].Value = fileContent;
                    Logger.Info("片段已更新",
                        context: Logger.Context(("snippet", fileName)));
                    return;
                }
            }

            Logger.Warn("片段名不存在",
                context: Logger.Context(("snippet", txtTagSnippetName.Text)));
        }

        private void btnTagSnippetDelete_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow != null) {
                int rowIndex = dgvTagSnippet.CurrentRow.Index;
                string fileName = dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[0].Value.ToString();
                string folderPath = picProps.WildcardFolderPath;
                string filePath = Path.Combine(folderPath, fileName);
                File.Delete(filePath);
                dgvTagSnippet.Rows.RemoveAt(rowIndex);
            }
            else {
                Logger.Warn("未选择要删除的片段",
                    context: Logger.Context(("action", "SnippetDelete")));
            }
        }

        private void dgvTagSnippet_CellClick(object sender, DataGridViewCellEventArgs e) {
            if (e.RowIndex >= 0) {
                DataGridViewRow selectedRow = dgvTagSnippet.Rows[e.RowIndex];
                txtTagSnippetName.Text = selectedRow.Cells[0].Value.ToString();
                txtTagSnippetValue.Text = selectedRow.Cells[1].Value.ToString();
                string cellPrompt = "<" + selectedRow.Cells[0].Value.ToString().Replace(".txt", "") + ">";
                Tools.InsertTextToTextBox(txtPrompt, cellPrompt);
            }
        }

        #endregion
    }
}
