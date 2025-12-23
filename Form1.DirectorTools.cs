using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 {
        #region Director Tools

        private string directorToolsRemoveBGInputPath;

        private DirectorToolExecutionOptions CaptureDirectorToolOptions() {
            return new DirectorToolExecutionOptions {
                Iterations = (int)nudLineArtParseNum.Value,
                ColorizePrompt = txtColorizePrompt.Text,
                ColorizeDefry = cmbColorizeDerfy.SelectedIndex,
                Emotion = cmbEmotionEmotion.Text,
                EmotionPrompt = txtEmotionPrompt.Text,
                EmotionDefry = cmbEmotionDefry.SelectedIndex,
                Token = settingProps.Token,
                Proxy = settingProps.Proxy
            };
        }

        private void picDirectorToolsRemoveBGInput_Click(object sender, EventArgs e) {
            var path = Vibe.SelectAndMappingPicToPictureBox(this);
            if (path != null)
                directorToolsRemoveBGInputPath = path;
        }

        private async Task ParseLineArtSignAsync(int type) {
            if (directorToolsRemoveBGInputPath == null) {
                MessageBox.Show("请先选择图片");
                return;
            }

            var options = CaptureDirectorToolOptions();
            await directorToolController.RunSingleAsync(directorToolsRemoveBGInputPath, type, options);
        }

        private async Task ParseLineArtFolderAsync(int type) {
            string folderPath = txtLineArtInputFolder.Text;
            if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath)) {
                MessageBox.Show("请选择有效的输入文件夹");
                return;
            }

            string[] validExtensions = { ".xbm", "tif", "ico", ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var files = Directory.GetFiles(folderPath, "*.*", SearchOption.AllDirectories)
                .Where(file => validExtensions.Contains(Path.GetExtension(file).ToLower())).ToList();

            if (files.Count == 0) {
                MessageBox.Show("所选文件夹中未找到可用图片");
                return;
            }

            var options = CaptureDirectorToolOptions();
            await directorToolController.RunBatchAsync(files, type, options);
        }

        private void ReplacePictureBoxImage(PictureBox pictureBox, Image newImage) {
            if (pictureBox.Image != null) {
                pictureBox.Image.Dispose();
                pictureBox.Image = null;
            }

            if (newImage != null)
                pictureBox.Image = newImage;
        }

        private async void btnDirectorToolsRemoveBGRun_Click(object sender, EventArgs e) {
            try {
                switch (tabDirectorTools.SelectedIndex) {
                    case 0:
                        await ParseLineArtSignAsync(0);
                        break;
                    case 1:
                        if (rdoLineArtParseSignPic.Checked)
                            await ParseLineArtSignAsync(1);
                        else if (rdoLineArtParseFolderPic.Checked)
                            await ParseLineArtFolderAsync(1);

                        break;
                    case 2:
                        if (rdoLineArtParseSignPic.Checked)
                            await ParseLineArtSignAsync(2);
                        else if (rdoLineArtParseFolderPic.Checked)
                            await ParseLineArtFolderAsync(2);

                        break;
                    case 3:
                        if (rdoLineArtParseSignPic.Checked)
                            await ParseLineArtSignAsync(3);
                        else if (rdoLineArtParseFolderPic.Checked)
                            await ParseLineArtFolderAsync(3);

                        break;
                    case 4:
                        if (rdoLineArtParseSignPic.Checked)
                            await ParseLineArtSignAsync(4);
                        else if (rdoLineArtParseFolderPic.Checked)
                            await ParseLineArtFolderAsync(4);

                        break;
                    case 5:
                        if (rdoLineArtParseSignPic.Checked)
                            await ParseLineArtSignAsync(5);
                        else if (rdoLineArtParseFolderPic.Checked)
                            await ParseLineArtFolderAsync(5);

                        break;
                }
            }
            catch (Exception ex) {
                Logger.Error("导演工具运行失败", exception: ex,
                    context: Logger.Context(("tabIndex", tabDirectorTools.SelectedIndex)));
                MessageBox.Show("导演工具运行失败，详情请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picDirectorToolsRemoveBGOutput_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(picProps.OutputPath);
        }

        private void btnSelectLineArtInputFolderPath_Click(object sender, EventArgs e) {
            string folderPath = Tools.SelectFolder(txtLineArtInputFolder.Text);
            if (folderPath != null) {
                txtLineArtInputFolder.Text = folderPath;
            }
        }

        #endregion
    }
}
