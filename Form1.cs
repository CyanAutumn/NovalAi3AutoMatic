using AutoNai3Tools.Services;
using AutoNai3Tools.utils;
using System;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using AutoNai3Tools.Controllers;

namespace AutoNai3Tools {
    public partial class Form1 : Form {
        public int runNum;
        public PicProperty picProps = new PicProperty();
        public SettingProperty settingProps = new SettingProperty();
        private readonly GenerationController generationController;
        private readonly IGenerationDataProvider generationDataProvider;
        private readonly DirectorToolController directorToolController;
        private readonly IConfigService configService;
        private readonly IWildcardService wildcardService;

        public Form1() {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer,
                true);
            UpdateStyles();
            EnableDoubleBuffer(this);
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

            configService = new ConfigService();
            wildcardService = new WildcardService();

            generationDataProvider = new GenerationUiDataProvider(
                picProps,
                settingProps,
                this,
                () => txtPrompt.Text,
                () => txtNegativePrompt.Text,
                dgvVibe,
                CaptureImg2ImgOptions);
            generationController = new GenerationController(generationDataProvider);
            AttachGenerationControllerEvents();

            var directorProcessor = new DirectorToolProcessor();
            directorToolController = new DirectorToolController(directorProcessor, picProps);
            AttachDirectorToolEvents();
        }

        private void EnableDoubleBuffer(Control control) {
            if (control == null)
                return;

            var doubleBufferProperty = typeof(Control).GetProperty("DoubleBuffered",
                BindingFlags.Instance | BindingFlags.NonPublic);
            doubleBufferProperty?.SetValue(control, true, null);

            foreach (Control child in control.Controls) {
                EnableDoubleBuffer(child);
            }
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

        private void AttachGenerationControllerEvents() {
            generationController.IterationStarted += HandleGenerationIterationStarted;
            generationController.ImageReady += HandleGenerationImageReady;
            generationController.DelayPlanned += HandleGenerationDelayPlanned;
            generationController.Completed += HandleGenerationCompleted;
            generationController.Cancelled += HandleGenerationCancelled;
            generationController.Failed += HandleGenerationFailed;
            generationController.Started += HandleGenerationStarted;
            generationController.Stopped += HandleGenerationStopped;
        }

        private void AttachDirectorToolEvents() {
            directorToolController.BusyStateChanged += HandleDirectorToolBusyStateChanged;
            directorToolController.PreviewUpdated += HandleDirectorToolPreviewUpdated;
            directorToolController.OutputUpdated += HandleDirectorToolOutputUpdated;
            directorToolController.Completed += HandleDirectorToolCompleted;
            directorToolController.Failed += HandleDirectorToolFailed;
        }

        private void HandleGenerationStarted() {
            if (InvokeRequired) {
                BeginInvoke(new Action(HandleGenerationStarted));
                return;
            }

            btnGenerate.Text = "停止";
            btnGenerate.Enabled = true;
        }

        private void HandleGenerationStopped() {
            if (InvokeRequired) {
                BeginInvoke(new Action(HandleGenerationStopped));
                return;
            }

            ResetGenerationState();
        }

        private void HandleDirectorToolBusyStateChanged(bool isBusy) {
            if (InvokeRequired) {
                BeginInvoke(new Action<bool>(HandleDirectorToolBusyStateChanged), isBusy);
                return;
            }

            btnDirectorToolsRemoveBGRun.Text = isBusy ? "运行中" : "运行";
            btnDirectorToolsRemoveBGRun.Enabled = !isBusy;
        }

        private void HandleDirectorToolPreviewUpdated(Image preview) {
            if (InvokeRequired) {
                BeginInvoke(new Action<Image>(HandleDirectorToolPreviewUpdated), preview);
                return;
            }

            ReplacePictureBoxImage(picDirectorToolsInput, preview);
        }

        private void HandleDirectorToolOutputUpdated(Image image) {
            if (InvokeRequired) {
                BeginInvoke(new Action<Image>(HandleDirectorToolOutputUpdated), image);
                return;
            }

            ReplacePictureBoxImage(picDirectorToolsOutput, image);
        }

        private void HandleDirectorToolCompleted() {
            if (InvokeRequired) {
                BeginInvoke(new Action(HandleDirectorToolCompleted));
                return;
            }

            Logger.Info("导演工具任务完成");
        }

        private void HandleDirectorToolFailed(Exception exception) {
            if (InvokeRequired) {
                BeginInvoke(new Action<Exception>(HandleDirectorToolFailed), exception);
                return;
            }

            if (exception == null)
                return;

            if (exception is OperationCanceledException)
                return;

            MessageBox.Show("导演工具运行失败，详情请查看日志。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ResetGenerationState() {
            btnGenerate.Text = "生成";
            btnGenerate.Enabled = true;
        }

        private void btnGenerate_Click(object sender, EventArgs e) {
            if (generationController.IsGenerating) {
                RequestStopGeneration();
                return;
            }

            try {
                generationController.StartGeneration();
            }
            catch (Exception ex) {
                Logger.Error("构建生成参数失败", exception: ex,
                    context: Logger.Context(("action", "StartGeneration")));
                MessageBox.Show("生成参数无效，请检查设置", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void picView_Click(object sender, EventArgs e) {
            try {
                System.Diagnostics.Process.Start(picProps.OutputPath);
            }
            catch (Exception ex) {
                Logger.Warn("无法打开输出目录",
                    context: Logger.Context(("path", picProps.OutputPath), ("reason", ex.Message)));
            }
        }

        private void RequestStopGeneration() {
            btnGenerate.Text = "停止";
            btnGenerate.Enabled = false;
            generationController.RequestStopGeneration();
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

            Logger.Error("生成任务发生未处理异常", exception: ex,
                context: Logger.Context(("stage", "pipeline")));
            ResetGenerationState();
        }

        #endregion

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
