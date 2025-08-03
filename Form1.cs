using AutoNai3Tools.utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using AutoNai3Tools.body;
using static System.Net.WebRequestMethods;
using System.Net.Http;
using File = System.IO.File;

namespace AutoNai3Tools {
    public partial class Form1 : Form {
        public int runNum;
        public PicProperty picProps = new PicProperty();

        public Form1() {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;
            RefreshConfig();
            InitGrpEventArgs();
            cmbColorizeDerfy.SelectedIndex = 0;
            cmbEmotionEmotion.SelectedIndex = 0;
            cmbEmotionDefry.SelectedIndex = 0;
            Logger.Initialize(this);
            tabControl2.TabPages.Remove(tabPage15);
            tabControl2.TabPages.Remove(tabPage18);
            propertyGrid1.SelectedObject = picProps;
        }

        #region 固定画师，随机画师，随机提示词快速插入

        private void InitGrpEventArgs() {
            grpArtistFixed.MouseHover += EventGRBMouseHover;
            grpArtistFixed.MouseLeave += EventGRBMouseLeave;
            grpArtistFixed.MouseClick += EventGRBMouseClick;
            grpArtistRandom.MouseHover += EventGRBMouseHover;
            grpArtistRandom.MouseLeave += EventGRBMouseLeave;
            grpArtistRandom.MouseClick += EventGRBMouseClick;
            lblRandomPromp.MouseHover += EventGRBMouseHover;
            lblRandomPromp.MouseLeave += EventGRBMouseLeave;
            lblRandomPromp.MouseClick += EventGRBMouseClick;
        }

        private void EventGRBMouseHover(object sender, EventArgs e) {
            GroupBox groupBox = sender as GroupBox;
            if (groupBox == grpArtistFixed) {
                groupBox.Text = "光标处插入/删除<固定画师>";
                return;
            }
            else if (groupBox == grpArtistRandom) {
                groupBox.Text = "光标处插入/删除<随机画师>";
                return;
            }

            Label label = sender as Label;
            if (label == lblRandomPromp) {
                label.Text = "插入/删除<随机提示词>";
                return;
            }
        }

        private void EventGRBMouseLeave(object sender, EventArgs e) {
            GroupBox groupBox = sender as GroupBox;
            if (groupBox == grpArtistFixed) {
                groupBox.Text = "固定画师";
                return;
            }
            else if (groupBox == grpArtistRandom) {
                groupBox.Text = "随机画师";
                return;
            }

            Label label = sender as Label;
            if (label == lblRandomPromp) {
                label.Text = "随机提示词路径：";
                return;
            }
        }

        private void EventGRBMouseClick(object sender, EventArgs e) {
            string insertPrompt = null;
            GroupBox groupBox = sender as GroupBox;
            if (groupBox == grpArtistFixed) {
                insertPrompt = "<固定画师>";
            }
            else if (groupBox == grpArtistRandom) {
                insertPrompt = "<随机画师>";
            }

            Label label = sender as Label;
            if (label == lblRandomPromp) {
                insertPrompt = "<随机提示词>";
            }

            if (insertPrompt != null)
                Tools.InsertTextToTextBox(txtPrompt, insertPrompt);
        }

        #endregion

        private string GetOutputPath() {
            string OutPutPath = txtOutputPath.Text;
            if (!Directory.Exists(OutPutPath)) {
                Logger.Warn("未找到输出路径" + OutPutPath + "，进行创建");
                try {
                    Directory.CreateDirectory(OutPutPath);
                }
                catch (Exception e) {
                    Logger.Error(e.ToString());
                }
            }

            return txtOutputPath.Text;
        }

        int resolutionSelectIndex = 0;

        private int[] GetResolution(int runNum) {
            if (runNum == 0 || (runNum % picProps.RunKeepParams == 0 && chkKeepResolution.Checked == true) ||
                chkKeepResolution.Checked == false) {
                var resolutionList = picProps.ResolutionList.Split(new string[] { "\r\n" }, StringSplitOptions.None);
                ;
                if (picProps.ResolutionMode != ResolutionMode.固定) {
                    switch (picProps.ResolutionMode) {
                        case ResolutionMode.随机:
                            Random random = new Random();
                            resolutionSelectIndex = random.Next(0, resolutionList.Length);
                            break;
                        case ResolutionMode.顺序:
                            resolutionSelectIndex = (resolutionSelectIndex + 1) % resolutionList.Length;
                            break;
                    }

                    string[] _Resolution = resolutionList[resolutionSelectIndex].Split('x');
                    picProps.Width = int.Parse(_Resolution[0]);
                    picProps.Height = int.Parse(_Resolution[1]);
                }
            }

            return new int[] { picProps.Width, picProps.Height };
        }

        string prevNoArtistPrompt = "";

        private BodyBase GetNai3Body(int runNum) {
            int[] resolution = GetResolution(runNum);
            Dictionary<string, object> kwargs = picProps.GetProperty();
            kwargs.Add("negative_prompt", txtNegativePrompt.Text);

            // img2img
            if (img2ImgCurrentPath != null) {
                kwargs.Add("image", Tools.ConvertImageToBase64(img2ImgCurrentPath));
                kwargs.Add("strength", (float)nudImg2ImgStrength.Value);
                kwargs.Add("noise", (float)nudImg2ImgNoise.Value);
            }

            // vibe
            List<string> referenceImages = new List<string>();
            List<float> referenceInfoExtracted = new List<float>();
            List<float> referenceStrength = new List<float>();

            foreach (DataGridViewRow row in dgvVibe.Rows) {
                var picPath = row.Cells["Column1"].Value;
                if (picPath == null) continue;

                string base64img = Tools.ConvertImageToBase64(picPath.ToString());
                if (string.IsNullOrEmpty(base64img)) {
                    Logger.Error("图片转换失败，路径为" + picPath);
                    continue;
                }

                referenceImages.Add(base64img);

                var ie = row.Cells["Column2"].Value;
                referenceInfoExtracted.Add(ie != null ? float.Parse(ie.ToString()) : 0);

                var rs = row.Cells["Column3"].Value;
                referenceStrength.Add(rs != null ? float.Parse(rs.ToString()) : 0);
            }

            //vibe
            if (referenceImages.Count > 0) {
                kwargs.Add("reference_image_multiple", referenceImages);
                kwargs.Add("reference_information_extracted_multiple", referenceInfoExtracted);
                kwargs.Add("reference_strength_multiple", referenceStrength);
            }

            var prompt = Prompt.GetPrompt(txtPrompt.Text, this);
            prevNoArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
            string tPrompt = Prompt.GetDataPrompt(prompt);
            kwargs.Add("prompt", tPrompt);

            //nai4
            kwargs.Add("v4_negative_prompt",
                new V4Prompt(new Caption(txtNegativePrompt.Text, new List<CharCaption>()), null, null, false));
            kwargs.Add("v4_prompt", new V4Prompt(new Caption(tPrompt, new List<CharCaption>()), true, true, null));
            BodyBase body = BodyTools.GetBody(picProps.Model, kwargs);
            propertyGrid1.Refresh();
            return body;
        }

        BodyBase tempNai3Body = null;

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            int max_num = picProps.RunNum;
            for (int i = 0; i < picProps.RunNum; i++) {
                try {
                    runNum = i;
                    string output_path = txtOutputPath.Text;
                    NovalAi novalAi = new NovalAi();
                    try {
                        tempNai3Body = GetNai3Body(i);
                    }
                    catch (Exception ex) {
                        Logger.Error("参数错误：" + ex.ToString());
                        Logger.Info(
                            "-----------------------------------------------------------------------------------------------------------------------------------------");
                        continue;
                    }

                    Logger.Info("开始发送生图请求");
                    Bitmap img = novalAi.SendGenerateRequests(txtToken.Text, tempNai3Body, prevNoArtistPrompt, this);
                    if (!chkClosePicPreview.Checked) {
                        picView.Image = img;
                    }

                    Random random = new Random();
                    if (timer_status == false) {
                        timer.Dispose();
                        Logger.Info(
                            "-----------------------------------------------------------------------------------------------------------------------------------------");
                        break;
                    }

                    if (i == max_num - 1) {
                        Logger.Info("运行完毕，共运行" + (i + 1).ToString() + "次");
                        timer_status = false;
                        timer.Dispose();
                    }
                    else if (i % 10 == 0 && i != 0) {
                        if (nudSleepTimeLongHigh.Value < nudSleepTimeLongLow.Value) {
                            Logger.Info("设置页面中的休息时间左侧不得大于右侧，已自动更改完毕");
                            nudSleepTimeLongHigh.Value = nudSleepTimeLongLow.Value;
                        }

                        int delay = random.Next(((int)nudSleepTimeLongLow.Value) * 1000,
                            ((int)nudSleepTimeLongHigh.Value) * 1000);
                        Logger.Info("图片信息：" + tempNai3Body.prompt + "\r\n已运行" + (i + 1).ToString() + "次，开始长休" + delay +
                                    "毫秒");
                        Thread.Sleep(delay);
                    }
                    else {
                        if (nudSleepTimeShortHigh.Value < nudSleepTimeShortLow.Value) {
                            Logger.Info("设置页面中的休息时间左侧不得大于右侧，已自动更改完毕");
                            nudSleepTimeShortHigh.Value = nudSleepTimeShortLow.Value;
                        }

                        int delay = random.Next(((int)nudSleepTimeShortLow.Value) * 1000,
                            ((int)nudSleepTimeShortHigh.Value) * 1000);
                        Logger.Info("图片信息：" + tempNai3Body.prompt + "\r\n已运行" + (i + 1).ToString() + "次，开始短休" + delay +
                                    "毫秒");
                        Thread.Sleep(delay);
                    }

                    Logger.Info(
                        "-----------------------------------------------------------------------------------------------------------------------------------------");
                }
                catch { }
            }

            Action<string> actionDelegate = (x) => { this.btnGenerate.Text = x; };
            Action<string> actionDelegate2 = (x) => { this.btnGenerate.Enabled = true; };
            this.btnGenerate.Invoke(actionDelegate, "生成");
            this.btnGenerate.Invoke(actionDelegate2, "");
        }

        System.Timers.Timer timer = null;
        bool timer_status = false;

        private void btnGenerate_Click(object sender, EventArgs e) {
            if (timer_status) {
                timer.Dispose();
                timer_status = false;
                btnGenerate.Text = "等待当前生成结束";
                btnGenerate.Enabled = false;
            }
            else {
                timer = new System.Timers.Timer();
                timer.Elapsed += TimerElapsed;
                timer.AutoReset = false;
                timer.Start();
                timer_status = true;
                btnGenerate.Text = "停止";
            }
        }

        private void picView_Click(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(txtOutputPath.Text);
            }
            catch {
                Logger.Warn("无法打开输出文件夹");
            }
        }

        private void btnAddOrEditConfig_Click(object sender, EventArgs e) {
            Config.SaveToml(this, cmbConfigName.Text);
            RefreshConfig();
        }

        private void btnOpenConfigFolder_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("C:\\Users\\Public\\Documents\\auto_nai3_2\\");
        }

        private void btnDeleteConfig_Click(object sender, EventArgs e) {
            File.Delete("C:\\Users\\Public\\Documents\\auto_nai3_2\\" + cmbConfigName.Text + ".toml");
            RefreshConfig();
        }

        private void cmbConfigName_SelectedIndexChanged(object sender, EventArgs e) {
            Config.ReadToml(this, cmbConfigName.Text);
            propertyGrid1.Refresh();
        }

        private void RefreshConfig() {
            string folderPath = "C:\\Users\\Public\\Documents\\auto_nai3_2\\";
            if (!Directory.Exists(folderPath)) {
                try {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e) { }
            }

            string[] txtFiles = Directory.GetFiles(folderPath, "*.toml");
            cmbConfigName.Items.Clear();
            for (int idx = 0; idx < txtFiles.Length; idx++) {
                txtFiles[idx] = txtFiles[idx].Replace(folderPath, "");
                txtFiles[idx] = txtFiles[idx].Replace(".toml", "");
                cmbConfigName.Items.Add(txtFiles[idx]);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e) {
            Config.SaveToml(this, "上一次关闭时的自动保存");
            SystemConfig.SaveToml(this);
        }

        private void Form1_Load(object sender, EventArgs e) {
            try {
                Config.ReadToml(this, "上一次关闭时的自动保存");
                cmbConfigName.Text = "上一次关闭时的自动保存";
            }
            catch {
                Logger.Warn("未找到上一次关闭时的保存记录，以初始状态开始");
            }

            try {
                SystemConfig.ReadToml(this);
            }
            catch {
                Logger.Warn("未找到全局配置，以初始状态开始");
            }

            InitTagSnippetDGV();
        }

        private void cmbConfigName_MouseClick(object sender, MouseEventArgs e) {
            RefreshConfig();
        }

        private void btnClearAllLog_Click(object sender, EventArgs e) {
            txtLog.Text = "";
        }

        private void btnRandomPromptFolderPath_Click(object sender, EventArgs e) {
            string folderPath = Tools.SelectFolder();
            if (folderPath != null) {
                txtRandomPromptFolderPath.Text = folderPath;
            }
        }

        private void btnWildcardFolderPath_Click(object sender, EventArgs e) {
            string folderPath = Tools.SelectFolder();
            if (folderPath != null) {
                txtWildcardFolderPath.Text = folderPath;
                InitTagSnippetDGV();
            }
        }

        private void btnSetOutputFolder_Click(object sender, EventArgs e) {
            string folderPath = Tools.SelectFolder();
            if (folderPath != null) {
                txtOutputPath.Text = folderPath;
            }
        }

        #region vibe

        string vibeCurrentPicPath = null;

        private void picVibeView_Click(object sender, EventArgs e) {
            var t_path = Tools.SelectAndMappingPicToPictureBox(picVibeView);
            if (t_path != null)
                vibeCurrentPicPath = t_path;
        }

        private void btnVibeAdd_Click(object sender, EventArgs e) {
            if (vibeCurrentPicPath != null) {
                dgvVibe.Rows.Add(vibeCurrentPicPath, numVibeIE.Value, numVibeRS.Value);
                if (picVibeView.Image != null) {
                    picVibeView.Image.Dispose();
                    picVibeView.Image = null; // 确保引用被清空
                }

                vibeCurrentPicPath = null;
            }
            else {
                Logger.Warn("请点击左侧空白处选择一张图片后添加");
            }
        }

        private void btnVibeDelete_Click(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                int rowIndex = dgvVibe.CurrentRow.Index;
                dgvVibe.Rows.RemoveAt(rowIndex);
            }
            else {
                Logger.Warn("请先选择要删除的行");
            }
        }

        #endregion

        private void dgvSnippet_SelectionChanged(object sender, EventArgs e) {
            // 检查是否有当前选中行
            if (dgvVibe.CurrentRow != null) {
                DataGridViewRow selectedRow = dgvVibe.CurrentRow;
                vibeCurrentPicPath = selectedRow.Cells["Column1"].Value.ToString();
                var imgPath = selectedRow.Cells["Column1"].Value;
                var ie = selectedRow.Cells["Column2"].Value;
                numVibeIE.Value = (decimal)ie;
                var rs = selectedRow.Cells["Column3"].Value;
                numVibeRS.Value = (decimal)rs;

                // 如果 form.picView.Image 已经存在，先释放它
                if (picVibeView.Image != null) {
                    picVibeView.Image.Dispose();
                    picVibeView.Image = null; // 确保引用被清空
                }

                // 使用 MemoryStream 加载图片
                using (FileStream fs = new FileStream(imgPath.ToString(), FileMode.Open, FileAccess.Read)) {
                    using (MemoryStream ms = new MemoryStream()) {
                        fs.CopyTo(ms);
                        ms.Position = 0; // 重置流位置
                        picVibeView.Image = System.Drawing.Image.FromStream(ms);
                    }
                }
            }
        }

        private void btnVibeEdit_Click(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                DataGridViewRow selectedRow = dgvVibe.CurrentRow;
                selectedRow.Cells["Column1"].Value = vibeCurrentPicPath;
                selectedRow.Cells["Column2"].Value = numVibeIE.Value;
                selectedRow.Cells["Column3"].Value = numVibeRS.Value;
            }
            else {
                Logger.Warn("请先选择要修改的行");
            }
        }

        #region 详情页

        private void btnGetMorePrompt_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://pan.baidu.com/s/1CTFTVIo7vKzDRy62LNxMMw?pwd=ktur");
        }

        private void btnGetRollDoc_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://docs.qq.com/sheet/DRFdBdGxZaXdkc3pP?tab=7mb6q1");
        }

        private void btnParsePrompt_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://spell.novelai.dev/");
        }

        private void btnPushBackPic_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://huggingface.co/spaces/SmilingWolf/wd-tagger");
        }

        private void btnTutorial_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://cyanautumn.github.io/NovalAi3AutoMaticDoc/");
        }

        private void btnDocToolsBook_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://docs.qq.com/doc/p/230e7ada2a60d8e347d639edd5521f5e62332fe9");
        }

        private void btnDocGithub_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://docs.qq.com/doc/p/230e7ada2a60d8e347d639edd5521f5e62332fe9");
        }

        #endregion

        #region wildcard

        private void InitTagSnippetDGV() {
            try {
                string folderPath = txtWildcardFolderPath.Text;
                string[] txtFiles = Directory.GetFiles(folderPath, "*.txt");

                dgvTagSnippet.Rows.Clear();
                foreach (string file in txtFiles) {
                    string fileName = Path.GetFileName(file);
                    string fileContent = File.ReadAllText(file);

                    // 将文件名和内容添加到DataGridView中的新行
                    dgvTagSnippet.Rows.Add(fileName, fileContent);
                }
            }
            catch {
                Logger.Warn("wildcard文件夹下未找到任何相关文件");
            }
        }

        private void btnTagSnippetAdd_Click(object sender, EventArgs e) {
            if (txtTagSnippetName.Text != "") {
                foreach (DataGridViewRow row in dgvTagSnippet.Rows) {
                    if (row.Cells[0].Value != null) {
                        if (row.Cells[0].Value.ToString() == (txtTagSnippetName.Text +=
                                (txtTagSnippetName.Text.EndsWith(".txt") ? "" : ".txt"))) {
                            Logger.Warn("片段名已存在，无法添加");
                            return;
                        }
                    }
                }

                if (txtTagSnippetName.Text == "") {
                    Logger.Warn("片段名不能为空");
                    return;
                }

                string fileName = txtTagSnippetName.Text;
                if (!fileName.EndsWith(".txt"))
                    fileName = fileName + ".txt";
                string fileContent = txtTagSnippetValue.Text;
                string folderPath = txtWildcardFolderPath.Text;
                string filePath = Path.Combine(folderPath, fileName);
                Tools.IsExist(folderPath, true);
                File.WriteAllText(filePath, fileContent);
                dgvTagSnippet.Rows.Add(txtTagSnippetName.Text, txtTagSnippetValue.Text);

                Logger.Info("增加成功！");
            }
            else {
                Logger.Warn("请输入一个片段名");
            }
        }

        private void btnTagSnippetEdit_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow.Index == 0) {
                Logger.Warn("请先选中要编辑的行");
                return;
            }

            foreach (DataGridViewRow row in dgvTagSnippet.Rows) {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == txtTagSnippetName.Text) {
                    string fileContent = txtTagSnippetValue.Text;
                    string fileName = dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[0].Value.ToString();
                    string folderPath = txtWildcardFolderPath.Text;
                    string filePath = Path.Combine(folderPath, fileName);
                    File.WriteAllText(filePath, fileContent);
                    dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[1].Value = fileContent;
                    Logger.Info("修改成功");
                    return;
                }
            }

            Logger.Warn("片段名不存在");
        }

        private void btnTagSnippetDelete_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow != null) {
                int rowIndex = dgvTagSnippet.CurrentRow.Index;
                string fileName = dgvTagSnippet.Rows[dgvTagSnippet.CurrentRow.Index].Cells[0].Value.ToString();
                string folderPath = txtWildcardFolderPath.Text;
                string filePath = Path.Combine(folderPath, fileName);
                File.Delete(filePath);
                dgvTagSnippet.Rows.RemoveAt(rowIndex);
            }
            else {
                Logger.Warn("请先选择要删除的行");
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

        #region

        string directorToolsRemoveBGInputPath = null;

        private string GetBodyType(int input) {
            switch (input) {
                case 0:
                    return "bg-removal";
                case 1:
                    return "lineart";
                case 2:
                    return "sketch";
                case 3:
                    return "colorize";
                case 4:
                    return "emotion";
                case 5:
                    return "declutter";
            }

            return null;
        }

        private void picDirectorToolsRemoveBGInput_Click(object sender, EventArgs e) {
            var path = Tools.SelectAndMappingPicToPictureBox(picDirectorToolsInput);
            if (path != null)
                directorToolsRemoveBGInputPath = path;
        }

        public void ParseLineArtSign(int type) {
            if (directorToolsRemoveBGInputPath != null) {
                for (int i = 0; i < nudLineArtParseNum.Value; i++) {
                    string base64img = Tools.ConvertImageToBase64(directorToolsRemoveBGInputPath);
                    int width, height;
                    using (Image image = Image.FromFile(directorToolsRemoveBGInputPath)) {
                        width = image.Width;
                        height = image.Height;
                    }

                    Nai3DirectorToolsBody body = null;
                    if (type == 0 || type == 1 || type == 2 || type == 5)
                        body = new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type));
                    else if (type == 3)
                        body = new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type),
                            txtColorizePrompt.Text, cmbColorizeDerfy.SelectedIndex);
                    else if (type == 4)
                        body = new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type),
                            $"{cmbEmotionEmotion.Text};;{txtEmotionPrompt.Text}", cmbEmotionDefry.SelectedIndex);

                    if (body == null)
                        return;
                    NovalAi novalAi = new NovalAi();
                    Bitmap img = novalAi.SendDirectorToolsRequests(txtToken.Text, body, this);
                    picDirectorToolsOutput.Image = img;
                }
            }
            else {
                MessageBox.Show("请先选择图片");
            }
        }

        public void TaskLineArtFolder(object source, System.Timers.ElapsedEventArgs e, int count) {
            string[] validExtensions = { ".xbm", "tif", "ico", ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var files = Directory.GetFiles(txtLineArtInputFolder.Text, "*.*", SearchOption.AllDirectories)
                .Where(file => validExtensions.Contains(Path.GetExtension(file).ToLower()));

            foreach (string filePath in files) {
                directorToolsRemoveBGInputPath = filePath;
                if (picDirectorToolsInput.Image != null) {
                    picDirectorToolsInput.Image.Dispose();
                    picDirectorToolsInput.Image = null;
                }

                using (FileStream fs = new FileStream(directorToolsRemoveBGInputPath, FileMode.Open, FileAccess.Read)) {
                    using (MemoryStream ms = new MemoryStream()) {
                        fs.CopyTo(ms);
                        ms.Position = 0;
                        picDirectorToolsInput.Image = System.Drawing.Image.FromStream(ms);
                    }
                }

                ParseLineArtSign(count);
            }

            btnDirectorToolsRemoveBGRun.Text = "运行";
            btnDirectorToolsRemoveBGRun.Enabled = true;
        }

        public void ParseLineArtFolder(int type) {
            System.Timers.Timer timerLineArtFolder = new System.Timers.Timer(1);
            timerLineArtFolder.Elapsed += (sender, e) => TaskLineArtFolder(sender, e, type);
            timerLineArtFolder.AutoReset = false;
            timerLineArtFolder.Enabled = true;
        }

        private void btnDirectorToolsRemoveBGRun_Click(object sender, EventArgs e) {
            btnDirectorToolsRemoveBGRun.Text = "运行中";
            btnDirectorToolsRemoveBGRun.Enabled = false;
            switch (tabDirectorTools.SelectedIndex) {
                case 0:
                    ParseLineArtSign(0);
                    btnDirectorToolsRemoveBGRun.Text = "运行";
                    btnDirectorToolsRemoveBGRun.Enabled = true;
                    break;
                case 1:
                    if (rdoLineArtParseSignPic.Checked) {
                        ParseLineArtSign(1);
                        btnDirectorToolsRemoveBGRun.Text = "运行";
                        btnDirectorToolsRemoveBGRun.Enabled = true;
                    }
                    else if (rdoLineArtParseFolderPic.Checked)
                        ParseLineArtFolder(1);

                    break;
                case 2:
                    if (rdoLineArtParseSignPic.Checked) {
                        ParseLineArtSign(2);
                        btnDirectorToolsRemoveBGRun.Text = "运行";
                        btnDirectorToolsRemoveBGRun.Enabled = true;
                    }
                    else if (rdoLineArtParseFolderPic.Checked)
                        ParseLineArtFolder(2);

                    break;
                case 3:
                    if (rdoLineArtParseSignPic.Checked) {
                        ParseLineArtSign(3);
                        btnDirectorToolsRemoveBGRun.Text = "运行";
                        btnDirectorToolsRemoveBGRun.Enabled = true;
                    }
                    else if (rdoLineArtParseFolderPic.Checked)
                        ParseLineArtFolder(3);

                    break;
                case 4:
                    if (rdoLineArtParseSignPic.Checked) {
                        ParseLineArtSign(4);
                        btnDirectorToolsRemoveBGRun.Text = "运行";
                        btnDirectorToolsRemoveBGRun.Enabled = true;
                    }
                    else if (rdoLineArtParseFolderPic.Checked) {
                        ParseLineArtFolder(4);
                    }

                    break;
            }
        }

        private void picDirectorToolsRemoveBGOutput_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(txtOutputPath.Text);
        }

        private void btnSelectLineArtInputFolderPath_Click(object sender, EventArgs e) {
            string folderPath = Tools.SelectFolder();
            if (folderPath != null) {
                txtLineArtInputFolder.Text = folderPath;
            }
        }

        #endregion

        #region img2img

        string img2ImgCurrentPath;

        private void picImg2ImgView_Click(object sender, EventArgs e) {
            var t_path = Tools.SelectAndMappingPicToPictureBox(picImg2ImgView);
            if (t_path != null)
                img2ImgCurrentPath = t_path;
        }

        private void btnImg2ImgDel_Click(object sender, EventArgs e) {
            img2ImgCurrentPath = null;
            picImg2ImgView.Image.Dispose();
            picImg2ImgView.Image = null;
        }

        #endregion

        private int resizeAreaSize = 10;
        private const int WM_NCHITTEST = 0x84;
        private const int HTCLIENT = 1;
        private const int HTLEFT = 10;
        private const int HTRIGHT = 11;
        private const int HTTOP = 12;
        private const int HTTOPLEFT = 13;
        private const int HTTOPRIGHT = 14;
        private const int HTBOTTOM = 15;
        private const int HTBOTTOMLEFT = 16;
        private const int HTBOTTOMRIGHT = 17;

        protected override void WndProc(ref Message m) {
            base.WndProc(ref m);

            if (m.Msg == WM_NCHITTEST) {
                // 获取鼠标相对于窗体的位置
                int x = (m.LParam.ToInt32() & 0xFFFF);
                int y = (m.LParam.ToInt32() >> 16) & 0xFFFF;
                var clientPos = this.PointToClient(new System.Drawing.Point(x, y));

                // 判断在哪个边缘
                if (clientPos.X <= resizeAreaSize && clientPos.Y <= resizeAreaSize)
                    m.Result = (IntPtr)HTTOPLEFT;
                else if (clientPos.X >= this.ClientSize.Width - resizeAreaSize && clientPos.Y <= resizeAreaSize)
                    m.Result = (IntPtr)HTTOPRIGHT;
                else if (clientPos.X <= resizeAreaSize && clientPos.Y >= this.ClientSize.Height - resizeAreaSize)
                    m.Result = (IntPtr)HTBOTTOMLEFT;
                else if (clientPos.X >= this.ClientSize.Width - resizeAreaSize &&
                         clientPos.Y >= this.ClientSize.Height - resizeAreaSize)
                    m.Result = (IntPtr)HTBOTTOMRIGHT;
                else if (clientPos.Y <= resizeAreaSize)
                    m.Result = (IntPtr)HTTOP;
                else if (clientPos.Y >= this.ClientSize.Height - resizeAreaSize)
                    m.Result = (IntPtr)HTBOTTOM;
                else if (clientPos.X <= resizeAreaSize)
                    m.Result = (IntPtr)HTLEFT;
                else if (clientPos.X >= this.ClientSize.Width - resizeAreaSize)
                    m.Result = (IntPtr)HTRIGHT;
                else
                    m.Result = (IntPtr)HTCLIENT; // 其他区域
            }
        }
    }
}