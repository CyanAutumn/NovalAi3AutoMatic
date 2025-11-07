using System;
using System.Windows.Forms;
using AutoNai3Tools.Services;
using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 {
        #region Wildcard

        private void InitTagSnippetDGV() {
            try {
                string folderPath = picProps.WildcardFolderPath;
                dgvTagSnippet.Rows.Clear();
                var snippets = wildcardService.LoadSnippets(folderPath);
                foreach (var snippet in snippets)
                    dgvTagSnippet.Rows.Add(snippet.Name, snippet.Content);
            }
            catch (Exception ex) {
                Logger.Warn("未能加载 wildcard 片段文件",
                    context: Logger.Context(("folder", picProps.WildcardFolderPath), ("reason", ex.Message)));
            }
        }

        private void btnTagSnippetAdd_Click(object sender, EventArgs e) {
            string folderPath = picProps.WildcardFolderPath;
            string name = txtTagSnippetName.Text;
            if (string.IsNullOrWhiteSpace(folderPath)) {
                MessageBox.Show("请先在设置中配置 Wildcard 目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (string.IsNullOrWhiteSpace(name)) {
                Logger.Warn("片段名不能为空",
                    context: Logger.Context(("action", "SnippetAdd")));
                return;
            }

            try {
                var snippet = wildcardService.AddSnippet(folderPath, name, txtTagSnippetValue.Text);
                dgvTagSnippet.Rows.Add(snippet.Name, snippet.Content);
                txtTagSnippetName.Text = snippet.Name;
                txtTagSnippetValue.Text = snippet.Content;
                Logger.Info("片段已新增",
                    context: Logger.Context(("snippet", snippet.Name), ("folder", folderPath)));
            }
            catch (Exception ex) {
                Logger.Warn("添加片段失败",
                    context: Logger.Context(("snippet", name), ("reason", ex.Message)));
                MessageBox.Show($"添加片段失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTagSnippetEdit_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow == null) {
                Logger.Warn("未选择要编辑的片段",
                    context: Logger.Context(("action", "SnippetEdit")));
                return;
            }

            string folderPath = picProps.WildcardFolderPath;
            if (string.IsNullOrWhiteSpace(folderPath)) {
                MessageBox.Show("请先在设置中配置 Wildcard 目录", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string fileName = dgvTagSnippet.CurrentRow.Cells[0].Value?.ToString();
            if (string.IsNullOrWhiteSpace(fileName)) {
                Logger.Warn("未找到要编辑的片段",
                    context: Logger.Context(("action", "SnippetEdit")));
                return;
            }

            try {
                var snippet = wildcardService.UpdateSnippet(folderPath, fileName, txtTagSnippetValue.Text);
                dgvTagSnippet.CurrentRow.Cells[1].Value = snippet.Content;
                txtTagSnippetValue.Text = snippet.Content;
                Logger.Info("片段已更新",
                    context: Logger.Context(("snippet", snippet.Name)));
            }
            catch (Exception ex) {
                Logger.Warn("更新片段失败",
                    context: Logger.Context(("snippet", fileName), ("reason", ex.Message)));
                MessageBox.Show($"更新片段失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnTagSnippetDelete_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow != null) {
                int rowIndex = dgvTagSnippet.CurrentRow.Index;
                string fileName = dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[0].Value.ToString();
                string folderPath = picProps.WildcardFolderPath;
                try {
                    wildcardService.DeleteSnippet(folderPath, fileName);
                    dgvTagSnippet.Rows.RemoveAt(rowIndex);
                }
                catch (Exception ex) {
                    Logger.Warn("删除片段失败",
                        context: Logger.Context(("snippet", fileName), ("reason", ex.Message)));
                    MessageBox.Show($"删除片段失败：{ex.Message}", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
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
