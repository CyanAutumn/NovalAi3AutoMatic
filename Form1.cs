using AutoNai3Tools.utils;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Timers;
using System.Windows.Forms;

namespace AutoNai3Tools {
    public partial class Form1 : Form {
        public Logger log;
        public int runNum;
        public Form1() {
            InitializeComponent();
            lstResolutionList.SelectedIndex = 0;
            cmbSampler.SelectedIndex = 0;
            Control.CheckForIllegalCrossThreadCalls = false;
            RefreshConfig();
            InitGrpEventArgs();
            cmbColorizeDerfy.SelectedIndex = 0;
            cmbEmotionEmotion.SelectedIndex = 0;
            cmbEmotionDefry.SelectedIndex = 0;
            cmbNoiseSchedule.SelectedIndex = 0;
            log = new Logger(this);
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
                log.Warn("未找到输出路径" + OutPutPath + "，进行创建");
                try {
                    Directory.CreateDirectory(OutPutPath);
                }
                catch (Exception e) {
                    log.Error(e.ToString());
                }
            }
            return txtOutputPath.Text;
        }

        private int[] GetResolution(int runNum) {
            if (chkKeepResolution.Checked) {
                if (runNum == 0 || runNum % numKeepParams.Value == 0) {
                    if (rdoResolutionOrder.Checked) {
                        int selectIndex = lstResolutionList.SelectedIndex + 1;
                        lstResolutionList.SelectedIndex = selectIndex >= lstResolutionList.Items.Count ? 0 : selectIndex;
                    }
                    else if (rdoResolutionRandom.Checked) {
                        Random random = new Random();
                        int selectIndex = random.Next(0, lstResolutionList.Items.Count);
                        lstResolutionList.SelectedIndex = selectIndex;
                    }
                }
            }
            else {
                if (rdoResolutionOrder.Checked) {
                    int selectIndex = lstResolutionList.SelectedIndex + 1;
                    lstResolutionList.SelectedIndex = selectIndex >= lstResolutionList.Items.Count ? 0 : selectIndex;
                }
                else if (rdoResolutionRandom.Checked) {
                    Random random = new Random();
                    int selectIndex = random.Next(0, lstResolutionList.Items.Count);
                    lstResolutionList.SelectedIndex = selectIndex;
                }
            }
            log.Info("分辨率：" + lstResolutionList.SelectedItem.ToString());
            string[] strResolution = lstResolutionList.SelectedItem.ToString().Split('x');
            int[] resultResolution = new int[2] { int.Parse(strResolution[0]), int.Parse(strResolution[1]) };
            return resultResolution;
        }

        private string GetSampler() {
            int ret_idx = cmbSampler.SelectedIndex;
            string[] sampler = new string[] { "k_euler", "k_euler_ancestral", "k_dpmpp_2s_ancestral", "k_dpmpp_2m_sde", "k_dpmpp_2m", "k_dpmpp_sde", "ddim_v3" };
            log.Info("采样：" + sampler[ret_idx]);
            return sampler[ret_idx];
        }

        private int GetSeed() {
            int result = 0;
            if (!cbkSeedFixed.Checked) {
                Random random = new Random();
                nudSeed.Value = random.Next(0, 1000000000);
            }
            result = ((int)nudSeed.Value);
            log.Info($"种子：{result}");
            return result;
        }

        string prevNoArtistPrompt = "";
        private Nai3GenerateImageBody GetNai3Body(int runNum) {
            Nai3Parmeters parmeters = new Nai3Parmeters();
            int[] resolution = GetResolution(runNum);
            parmeters.width = resolution[0];
            parmeters.height = resolution[1];
            parmeters.sampler = GetSampler();
            parmeters.steps = ((int)numSteps.Value);
            parmeters.scale = ((float)numScale.Value);
            parmeters.cfg_rescale = ((float)nudCFG.Value);
            parmeters.noise_schedule = cmbNoiseSchedule.Text;
            parmeters.sm = chkSmea.Checked;
            parmeters.sm_dyn = chkDyn.Checked;
            parmeters.negative_prompt = txtNegativePrompt.Text;
            parmeters.seed = GetSeed();
            if (chkVariety.Checked)
                parmeters.skip_cfg_above_sigma = 19;
            parmeters.dynamic_thresholding = chkDecrisp.Checked;
            //img2img
            if (img2ImgCurrentPath != null) {
                string base64img = Tools.ConvertImageToBase64(img2ImgCurrentPath);
                parmeters.image = base64img;
                parmeters.strength = ((float)nudImg2ImgStrength.Value);
                parmeters.noise = ((float)nudImg2ImgNoise.Value);
            }

            //vibe
            foreach (DataGridViewRow row in dgvVibe.Rows) {
                var picPath = row.Cells["Column1"].Value;
                string base64img = Tools.ConvertImageToBase64(picPath.ToString());
                if (base64img == null) {
                    log.Error("图片转换失败，路径为" + picPath);
                    continue;
                }
                parmeters.reference_image_multiple.Add(base64img);
                var ie = row.Cells["Column2"].Value;
                parmeters.reference_information_extracted_multiple.Add(float.Parse(ie.ToString()));
                var rs = row.Cells["Column3"].Value;
                parmeters.reference_strength_multiple.Add(float.Parse(rs.ToString()));
            }
            var prompt = Prompt.GetPrompt(txtPrompt.Text, this);
            prevNoArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
            Nai3GenerateImageBody nai3Body = new Nai3GenerateImageBody(input: Prompt.GetDataPrompt(prompt), parameters: parmeters);
            if (img2ImgCurrentPath != null)
                nai3Body.action = "img2img";
            return nai3Body;
        }

        Nai3GenerateImageBody tempNai3Body = null;

        private void TimerElapsed(object sender, ElapsedEventArgs e) {
            int max_num = int.Parse(numGenerateMaxNum.Value.ToString());
            for (int i = 0; i < max_num; i++) {
                try {
                    runNum = i;
                    string output_path = txtOutputPath.Text;
                    NovalAi novalAi = new NovalAi();
                    try {
                        tempNai3Body = GetNai3Body(i);
                    }
                    catch (Exception ex) {
                        log.Error("参数错误：" + ex.ToString());
                        log.Info("-----------------------------------------------------------------------------------------------------------------------------------------");
                        continue;
                    }
                    log.Info("开始发送生图请求");
                    Bitmap img = novalAi.SendGenerateRequests(txtToken.Text, tempNai3Body, prevNoArtistPrompt, this);
                    if (!chkClosePicPreview.Checked) {
                        picView.Image = img;
                    }
                    Random random = new Random();
                    if (timer_status == false) {
                        timer.Dispose();
                        log.Info("-----------------------------------------------------------------------------------------------------------------------------------------");
                        break;
                    }
                    if (i == max_num - 1) {
                        log.Info("运行完毕，共运行" + (i + 1).ToString() + "次");
                        timer_status = false;
                        timer.Dispose();
                    }
                    else if (i % 10 == 0 && i != 0) {
                        if (nudSleepTimeLongHigh.Value < nudSleepTimeLongLow.Value) {
                            log.Info("设置页面中的休息时间左侧不得大于右侧，已自动更改完毕");
                            nudSleepTimeLongHigh.Value = nudSleepTimeLongLow.Value;
                        }
                        int delay = random.Next(((int)nudSleepTimeLongLow.Value) * 1000, ((int)nudSleepTimeLongHigh.Value) * 1000);
                        log.Info("图片信息：" + tempNai3Body.input + "\r\n已运行" + (i + 1).ToString() + "次，开始长休" + delay + "毫秒");
                        Thread.Sleep(delay);
                    }
                    else {
                        if (nudSleepTimeShortHigh.Value < nudSleepTimeShortLow.Value) {
                            log.Info("设置页面中的休息时间左侧不得大于右侧，已自动更改完毕");
                            nudSleepTimeShortHigh.Value = nudSleepTimeShortLow.Value;
                        }
                        int delay = random.Next(((int)nudSleepTimeShortLow.Value) * 1000, ((int)nudSleepTimeShortHigh.Value) * 1000);
                        log.Info("图片信息：" + tempNai3Body.input + "\r\n已运行" + (i + 1).ToString() + "次，开始短休" + delay + "毫秒");
                        Thread.Sleep(delay);
                    }
                    log.Info("-----------------------------------------------------------------------------------------------------------------------------------------");
                }
                catch {
                }
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

        private void btnAddResolution_Click(object sender, EventArgs e) {
            lstResolutionList.Items.Add(numResolutionWidth.Value.ToString() + "x" + numResolutionHeight.Value.ToString());
        }

        private void btnDeleteResolution_Click(object sender, EventArgs e) {
            if (lstResolutionList.Items.Count == 1) {
                log.Error("至少需要保留一个分辨率");
                return;
            }
            if (lstResolutionList.SelectedItem != null)
                lstResolutionList.Items.Remove(lstResolutionList.SelectedItem);

            lstResolutionList.SelectedIndex = 0;
        }

        private void picView_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start(txtOutputPath.Text);
        }

        private void chkSmea_CheckedChanged(object sender, EventArgs e) {
            if (chkSmea.Checked) {
                chkDyn.Enabled = true;
            }
            else {
                chkDyn.Checked = false;
                chkDyn.Enabled = false;
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
        }

        private void RefreshConfig() {
            string folderPath = "C:\\Users\\Public\\Documents\\auto_nai3_2\\";
            //判断文件夹是否存在
            if (!Directory.Exists(folderPath)) {
                //创建文件夹
                try {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e) {
                }
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
                log.Warn("未找到上一次关闭时的保存记录，以初始状态开始");
            }
            try {
                SystemConfig.ReadToml(this);
            }
            catch {
                log.Warn("未找到全局配置，以初始状态开始");
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
                log.Warn("请点击左侧空白处选择一张图片后添加");
            }
        }

        private void btnVibeDelete_Click(object sender, EventArgs e) {
            if (dgvVibe.CurrentRow != null) {
                int rowIndex = dgvVibe.CurrentRow.Index;
                dgvVibe.Rows.RemoveAt(rowIndex);
            }
            else {
                log.Warn("请先选择要删除的行");
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
                log.Warn("请先选择要修改的行");
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
        #endregion

        #region wildcard
        private void InitTagSnippetDGV() {
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

        private void btnTagSnippetAdd_Click(object sender, EventArgs e) {
            if (txtTagSnippetName.Text != "") {
                foreach (DataGridViewRow row in dgvTagSnippet.Rows) {
                    if (row.Cells[0].Value != null) {
                        if (row.Cells[0].Value.ToString() == (txtTagSnippetName.Text += (txtTagSnippetName.Text.EndsWith(".txt") ? "" : ".txt"))) {
                            log.Warn("片段名已存在，无法添加");
                            return;
                        }
                    }
                }
                if (txtTagSnippetName.Text == "") {
                    log.Warn("片段名不能为空");
                    return;
                }

                string fileName = txtTagSnippetName.Text;
                if (!fileName.EndsWith(".txt"))
                    fileName = fileName + ".txt";
                string fileContent = txtTagSnippetValue.Text;
                string folderPath = txtWildcardFolderPath.Text;
                string filePath = Path.Combine(folderPath, fileName);
                File.WriteAllText(filePath, fileContent);
                dgvTagSnippet.Rows.Add(txtTagSnippetName.Text, txtTagSnippetValue.Text);

                log.Info("增加成功！");
            }
            else {
                log.Warn("请输入一个片段名");
            }
        }

        private void btnTagSnippetEdit_Click(object sender, EventArgs e) {
            if (dgvTagSnippet.CurrentRow.Index == 0) {
                log.Warn("请先选中要编辑的行");
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
                    log.Info("修改成功");
                    return;
                }
            }
            log.Warn("片段名不存在");
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
                log.Warn("请先选择要删除的行");
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
                        body = new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type), txtColorizePrompt.Text, cmbColorizeDerfy.SelectedIndex);
                    else if (type == 4)
                        body = new Nai3DirectorToolsBody(height, width, base64img, GetBodyType(type), $"{cmbEmotionEmotion.Text};;{txtEmotionPrompt.Text}", cmbEmotionDefry.SelectedIndex);

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
            var files = Directory.GetFiles(txtLineArtInputFolder.Text, "*.*", SearchOption.AllDirectories).Where(file => validExtensions.Contains(Path.GetExtension(file).ToLower()));

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
    }
}