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
using AutoNai3Tools.novalai;
using System.Collections;

namespace AutoNai3Tools {
    public partial class Form1 : Form {
        public int runNum;
        public PicProperty picProps = new PicProperty();
        public SettingProperty settingProps = new SettingProperty();
        private CancellationTokenSource generationCancellationSource;
        private GenerationPipeline currentGenerationPipeline;
        private bool isGenerating;

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
            propertyGridSettings.SelectedObject = settingProps;
        }

        #region 固定画师，随机画师，随机提示词快速插入

        private void InitGrpEventArgs() {
            grpArtistFixed.MouseHover += EventGRBMouseHover;
            grpArtistFixed.MouseLeave += EventGRBMouseLeave;
            grpArtistFixed.MouseClick += EventGRBMouseClick;
            grpArtistRandom.MouseHover += EventGRBMouseHover;
            grpArtistRandom.MouseLeave += EventGRBMouseLeave;
            grpArtistRandom.MouseClick += EventGRBMouseClick;
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

            if (insertPrompt != null)
                Tools.InsertTextToTextBox(txtPrompt, insertPrompt);
        }

        #endregion

        int resolutionSelectIndex = 0;

        private int[] GetResolution(int runNum) {
            if (runNum == 0 || (runNum % picProps.RunKeepParams == 0 && settingProps.KeepResolution) ||
                !settingProps.KeepResolution) {
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

        

        private GenerationRequest BuildGenerationRequest(GenerationContext context, int runIndex) {
            runNum = runIndex;
            _ = GetResolution(runIndex);
            Dictionary<string, object> kwargs = context.PicProps.GetProperty();
            kwargs["negative_prompt"] = context.NegativePrompt;

            if (context.HasImg2Img) {
                kwargs["image"] = Tools.ConvertImageToBase64(context.Img2Img.ImagePath);
                kwargs["strength"] = context.Img2Img.Strength;
                kwargs["noise"] = context.Img2Img.Noise;
            }

            List<VibeData> vibes = context.Vibes.Select(v => new VibeData {
                imagePath = v.ImagePath,
                informationExtracted = v.InformationExtracted,
                referenceStrength = v.ReferenceStrength
            }).ToList();

            if (vibes.Count > 0)
                vibes = Vibe.GetVibe(context.PicProps.Model, vibes, context.SettingProps.Token);

            List<string> t_rim = new List<string>();
            List<float> t_riem = new List<float>();
            List<float> t_rsm = new List<float>();
            foreach (var vibe in vibes) {
                t_rim.Add(vibe.base64Image);
                t_riem.Add(vibe.informationExtracted);
                t_rsm.Add(vibe.referenceStrength);
            }
            if (t_rim.Count > 0) {
                kwargs["reference_image_multiple"] = t_rim;
                kwargs["reference_information_extracted_multiple"] = t_riem;
                kwargs["reference_strength_multiple"] = t_rsm;
            }

            var prompt = Prompt.GetPrompt(context.PromptText, this);
            string noArtistPrompt = Prompt.GetNoArtistPrompt(prompt);
            string tPrompt = Prompt.GetDataPrompt(prompt);
            kwargs["prompt"] = tPrompt;

            kwargs["v4_negative_prompt"] =
                new V4Prompt(new Caption(context.NegativePrompt, new List<CharCaption>()), null, null, false);
            kwargs["v4_prompt"] = new V4Prompt(new Caption(tPrompt, new List<CharCaption>()), true, true, null);
            BodyBase body = BodyTools.GetBody(context.PicProps.Model, kwargs);
            return new GenerationRequest(body, tPrompt, noArtistPrompt);
        }

        private GenerationContext BuildGenerationContext() {
            List<VibeSelection> selections = new List<VibeSelection>();
            foreach (DataGridViewRow row in dgvVibe.Rows) {
                var picPath = row.Cells["Column1"].Value;
                if (picPath == null)
                    continue;

                float informationExtracted = ParseFloat(row.Cells["Column2"].Value);
                float referenceStrength = ParseFloat(row.Cells["Column3"].Value);
                selections.Add(new VibeSelection(picPath.ToString(), informationExtracted, referenceStrength));
            }

            Img2ImgOptions img2Img = null;
            if (!string.IsNullOrEmpty(img2ImgCurrentPath)) {
                img2Img = new Img2ImgOptions(img2ImgCurrentPath, (float)nudImg2ImgStrength.Value,
                    (float)nudImg2ImgNoise.Value);
            }

            return new GenerationContext(picProps, settingProps, txtPrompt.Text, txtNegativePrompt.Text, selections,
                img2Img, picProps.RunNum);
        }

        private static float ParseFloat(object value) {
            if (value == null)
                return 0f;

            if (value is float f)
                return f;
            if (value is double d)
                return (float)d;
            if (value is decimal dec)
                return (float)dec;

            float result;
            return float.TryParse(value.ToString(), out result) ? result : 0f;
        }

        private void StartGeneration(GenerationContext context) {
            generationCancellationSource = new CancellationTokenSource();
            currentGenerationPipeline = new GenerationPipeline(context, index => BuildGenerationRequest(context, index));
            currentGenerationPipeline.IterationStarted += HandleGenerationIterationStarted;
            currentGenerationPipeline.ImageReady += HandleGenerationImageReady;
            currentGenerationPipeline.DelayPlanned += HandleGenerationDelayPlanned;
            currentGenerationPipeline.Completed += HandleGenerationCompleted;
            currentGenerationPipeline.Cancelled += HandleGenerationCancelled;
            currentGenerationPipeline.Failed += HandleGenerationFailed;

            isGenerating = true;
            btnGenerate.Text = "停止";
            btnGenerate.Enabled = true;

            _ = currentGenerationPipeline.RunAsync(generationCancellationSource.Token);
        }

        private void RequestStopGeneration() {
            if (!isGenerating || generationCancellationSource == null)
                return;

            btnGenerate.Text = "停止中...";
            btnGenerate.Enabled = false;
            generationCancellationSource.Cancel();
        }

        private void HandleGenerationIterationStarted(int iteration) {
            if (InvokeRequired) {
                BeginInvoke(new Action<int>(HandleGenerationIterationStarted), iteration);
                return;
            }

            propertyGrid1.Refresh();
        }

        private void HandleGenerationImageReady(int iteration, Bitmap bitmap) {
            if (bitmap == null)
                return;

            if (InvokeRequired) {
                BeginInvoke(new Action<int, Bitmap>(HandleGenerationImageReady), iteration, bitmap);
                return;
            }

            if (settingProps.ClosePicPreview) {
                bitmap.Dispose();
                return;
            }

            if (picView.Image != null) {
                picView.Image.Dispose();
                picView.Image = null;
            }

            picView.Image = bitmap;
        }

        private void HandleGenerationDelayPlanned(int iteration, DelayInfo delayInfo, string prompt) {
            // 保留扩展接口，当前无需额外 UI 行为
        }

        private void HandleGenerationCompleted() {
            if (InvokeRequired) {
                BeginInvoke(new Action(HandleGenerationCompleted));
                return;
            }

            ResetGenerationState();
        }

        private void HandleGenerationCancelled() {
            if (InvokeRequired) {
                BeginInvoke(new Action(HandleGenerationCancelled));
                return;
            }

            ResetGenerationState();
        }

        private void HandleGenerationFailed(Exception ex) {
            if (InvokeRequired) {
                BeginInvoke(new Action<Exception>(HandleGenerationFailed), ex);
                return;
            }

            Logger.Error($"生成任务失败：{ex.Message}");
            ResetGenerationState();
        }

        private void ResetGenerationState() {
            isGenerating = false;
            btnGenerate.Text = "生成";
            btnGenerate.Enabled = true;

            generationCancellationSource?.Dispose();
            generationCancellationSource = null;

            if (currentGenerationPipeline != null) {
                currentGenerationPipeline.IterationStarted -= HandleGenerationIterationStarted;
                currentGenerationPipeline.ImageReady -= HandleGenerationImageReady;
                currentGenerationPipeline.DelayPlanned -= HandleGenerationDelayPlanned;
                currentGenerationPipeline.Completed -= HandleGenerationCompleted;
                currentGenerationPipeline.Cancelled -= HandleGenerationCancelled;
                currentGenerationPipeline.Failed -= HandleGenerationFailed;
            }

            currentGenerationPipeline = null;
        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            if (isGenerating) {
                RequestStopGeneration();
                return;
            }

            GenerationContext context;
            try {
                context = BuildGenerationContext();
            }
            catch (Exception ex) {
                Logger.Error("构建生成参数失败：" + ex);
                MessageBox.Show("生成参数无效，请检查设置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            StartGeneration(context);
        }

        private void picView_Click(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(picProps.OutputPath);
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
            InitTagSnippetDGV();
            propertyGrid1.Refresh();
            propertyGridSettings.Refresh();
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
            propertyGrid1.Refresh();
            propertyGridSettings.Refresh();
        }

        private void propertyGrid1_PropertyValueChanged(object s, PropertyValueChangedEventArgs e) {
            if (e.ChangedItem?.PropertyDescriptor?.Name == nameof(picProps.WildcardFolderPath)) {
                InitTagSnippetDGV();
            }
        }

        private void cmbConfigName_MouseClick(object sender, MouseEventArgs e) {
            RefreshConfig();
        }

        private void btnClearAllLog_Click(object sender, EventArgs e) {
            txtLog.Text = "";
        }

        #region vibe

        string vibeCurrentPicPath = null;

        private void picVibeView_Click(object sender, EventArgs e) {
            var t_path = Vibe.SelectAndMappingPicToPictureBox(this);
            if (t_path != null)
                vibeCurrentPicPath = t_path;
        }

        private void btnVibeAdd_Click(object sender, EventArgs e) {
            if (vibeCurrentPicPath != null) {
                dgvVibe.Rows.Add(vibeCurrentPicPath, nudVibeIE.Value, numVibeRS.Value);
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
                string folderPath = picProps.WildcardFolderPath;
                if (string.IsNullOrWhiteSpace(folderPath) || !Directory.Exists(folderPath))
                    throw new DirectoryNotFoundException();
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
                string folderPath = picProps.WildcardFolderPath;
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
                    string folderPath = picProps.WildcardFolderPath;
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
                string folderPath = picProps.WildcardFolderPath;
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
            var path = Vibe.SelectAndMappingPicToPictureBox(this);
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
                    Bitmap img = novalAi.SendDirectorToolsRequests(settingProps.Token, body, picProps, settingProps.Proxy);
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
            System.Diagnostics.Process.Start(picProps.OutputPath);
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
            var t_path = Vibe.SelectAndMappingPicToPictureBox(this);
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
