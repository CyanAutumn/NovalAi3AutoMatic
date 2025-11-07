using System;
using System.Windows.Forms;
using AutoNai3Tools.utils;

namespace AutoNai3Tools {
    public partial class Form1 {
        #region Img2Img

        private string img2ImgCurrentPath;

        private Img2ImgOptions CaptureImg2ImgOptions() {
            if (string.IsNullOrEmpty(img2ImgCurrentPath))
                return null;

            return new Img2ImgOptions(img2ImgCurrentPath, (float)nudImg2ImgStrength.Value,
                (float)nudImg2ImgNoise.Value);
        }

        private void picImg2ImgView_Click(object sender, EventArgs e) {
            var path = Vibe.SelectAndMappingPicToPictureBox(this);
            if (path != null)
                img2ImgCurrentPath = path;
        }

        private void btnImg2ImgDel_Click(object sender, EventArgs e) {
            img2ImgCurrentPath = null;
            picImg2ImgView.Image.Dispose();
            picImg2ImgView.Image = null;
        }

        #endregion
    }
}
