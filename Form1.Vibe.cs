using System;
using System.Windows.Forms;
using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 {
        #region Vibe

        private string vibeCurrentPicPath;

        private void picVibeView_Click(object sender, EventArgs e) {
            var path = Vibe.SelectAndMappingPicToPictureBox(this);
            if (path != null)
                vibeCurrentPicPath = path;
        }

        private void btnVibeAdd_Click(object sender, EventArgs e) {
            if (vibeCurrentPicPath != null) {
                dgvVibe.Rows.Add(vibeCurrentPicPath, nudVibeIE.Value, numVibeRS.Value);
                if (picVibeView.Image != null) {
                    picVibeView.Image.Dispose();
                    picVibeView.Image = null;
                }

                vibeCurrentPicPath = null;
            }
            else {
                Logger.Warn("未选择可添加的参考图",
                    context: Logger.Context(("action", "VibeAdd")));
            }
        }

        private void btnVibeDelete_Click(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                int rowIndex = dgvVibe.CurrentRow.Index;
                dgvVibe.Rows.RemoveAt(rowIndex);
            }
            else {
                Logger.Warn("未选择要删除的参考图",
                    context: Logger.Context(("action", "VibeDelete")));
            }
        }

        private void dgvSnippet_SelectionChanged(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                DataGridViewRow selectedRow = dgvVibe.CurrentRow;
                vibeCurrentPicPath = selectedRow.Cells["Column1"].Value.ToString();
                var imgPath = selectedRow.Cells["Column1"].Value;
                var ie = selectedRow.Cells["Column2"].Value;
                nudVibeIE.Value = (decimal)ie;
                var rs = selectedRow.Cells["Column3"].Value;
                numVibeRS.Value = (decimal)rs;

                Vibe.SetVibeInterfaceStatus(vibeCurrentPicPath, this);
                Tools.ShowImage(imgPath.ToString(), picVibeView);
            }
        }

        private void btnVibeEdit_Click(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                DataGridViewRow selectedRow = dgvVibe.CurrentRow;
                selectedRow.Cells["Column1"].Value = vibeCurrentPicPath;
                selectedRow.Cells["Column2"].Value = nudVibeIE.Value;
                selectedRow.Cells["Column3"].Value = numVibeRS.Value;
            }
            else {
                Logger.Warn("未选择要修改的参考图",
                    context: Logger.Context(("action", "VibeEdit")));
            }
        }

        #endregion
    }
}
