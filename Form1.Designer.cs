namespace AutoNai3Tools
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel5 = new System.Windows.Forms.Panel();
            this.picView = new System.Windows.Forms.PictureBox();
            this.txtPicInfo = new System.Windows.Forms.TextBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.txtPromptBlackList = new System.Windows.Forms.TextBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.rdoResolutionFixed = new System.Windows.Forms.RadioButton();
            this.label5 = new System.Windows.Forms.Label();
            this.lstResolutionList = new System.Windows.Forms.ListBox();
            this.numResolutionWidth = new System.Windows.Forms.NumericUpDown();
            this.numResolutionHeight = new System.Windows.Forms.NumericUpDown();
            this.btnAddResolution = new System.Windows.Forms.Button();
            this.btnDeleteResolution = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.rdoResolutionOrder = new System.Windows.Forms.RadioButton();
            this.rdoResolutionRandom = new System.Windows.Forms.RadioButton();
            this.panel6 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.numSteps = new System.Windows.Forms.NumericUpDown();
            this.chkDyn = new System.Windows.Forms.CheckBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chkSmea = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbSampler = new System.Windows.Forms.ComboBox();
            this.numScale = new System.Windows.Forms.NumericUpDown();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.grpArtistRandom = new System.Windows.Forms.GroupBox();
            this.txtArtistRandom = new System.Windows.Forms.TextBox();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.numArtistMin = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.numArtistMax = new System.Windows.Forms.NumericUpDown();
            this.chkArtistModify = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numDefaultArtistWeightIncreaseMax = new System.Windows.Forms.NumericUpDown();
            this.numDefaultArtistWeightReduceMax = new System.Windows.Forms.NumericUpDown();
            this.grpArtistFixed = new System.Windows.Forms.GroupBox();
            this.txtArtistFixed = new System.Windows.Forms.TextBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.dgvTagSnippet = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel10 = new System.Windows.Forms.Panel();
            this.txtTagSnippetValue = new System.Windows.Forms.TextBox();
            this.panel11 = new System.Windows.Forms.Panel();
            this.btnTagSnippetEdit = new System.Windows.Forms.Button();
            this.btnTagSnippetDelete = new System.Windows.Forms.Button();
            this.btnTagSnippetAdd = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTagSnippetName = new System.Windows.Forms.TextBox();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.panel9 = new System.Windows.Forms.Panel();
            this.btnVibeAdd = new System.Windows.Forms.Button();
            this.btnVibeDelete = new System.Windows.Forms.Button();
            this.btnVibeEdit = new System.Windows.Forms.Button();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.numVibeRS = new System.Windows.Forms.NumericUpDown();
            this.numVibeIE = new System.Windows.Forms.NumericUpDown();
            this.picVibeView = new System.Windows.Forms.PictureBox();
            this.dgvVibe = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.chkKeepResolution = new System.Windows.Forms.CheckBox();
            this.chkKeepRandomPrompt = new System.Windows.Forms.CheckBox();
            this.chkKeepWildcard = new System.Windows.Forms.CheckBox();
            this.chkKeepRandomArtist = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtProxy = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtToken = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtNegativePrompt = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPrompt = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnRandomFilePath = new System.Windows.Forms.Button();
            this.txtRandomPromptFolderPath = new System.Windows.Forms.TextBox();
            this.lblRandomPromp = new System.Windows.Forms.Label();
            this.btnWildcardFolderPath = new System.Windows.Forms.Button();
            this.txtWildcardFolderPath = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.btnSetOutputFolder = new System.Windows.Forms.Button();
            this.chkSavePromptToTxtNoArtist = new System.Windows.Forms.CheckBox();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkSavePromptToTxt = new System.Windows.Forms.CheckBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.cmbConfigName = new System.Windows.Forms.ComboBox();
            this.btnAddOrEditConfig = new System.Windows.Forms.Button();
            this.btnOpenConfigFolder = new System.Windows.Forms.Button();
            this.btnDeleteConfig = new System.Windows.Forms.Button();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.numGenerateMaxNum = new System.Windows.Forms.NumericUpDown();
            this.numKeepParams = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.btnClearLog = new System.Windows.Forms.Button();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.btnPushBackPic = new System.Windows.Forms.Button();
            this.btnParsePrompt = new System.Windows.Forms.Button();
            this.btnGetRollDoc = new System.Windows.Forms.Button();
            this.btnGetMorePrompt = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label16 = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage10 = new System.Windows.Forms.TabPage();
            this.tabPage11 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picView)).BeginInit();
            this.tabControl2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResolutionWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numResolutionHeight)).BeginInit();
            this.panel6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).BeginInit();
            this.tabPage3.SuspendLayout();
            this.grpArtistRandom.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArtistMin)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArtistMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultArtistWeightIncreaseMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultArtistWeightReduceMax)).BeginInit();
            this.grpArtistFixed.SuspendLayout();
            this.tabPage8.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagSnippet)).BeginInit();
            this.panel10.SuspendLayout();
            this.panel11.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.groupBox9.SuspendLayout();
            this.panel9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVibeRS)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVibeIE)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVibeView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVibe)).BeginInit();
            this.tabPage5.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGenerateMaxNum)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepParams)).BeginInit();
            this.tabPage4.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabPage9.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1301, 836);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel5);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 26);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(1293, 806);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "生图";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.picView);
            this.panel5.Controls.Add(this.txtPicInfo);
            this.panel5.Controls.Add(this.tabControl2);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 305);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1287, 498);
            this.panel5.TabIndex = 1;
            // 
            // picView
            // 
            this.picView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.picView.Location = new System.Drawing.Point(335, 0);
            this.picView.Name = "picView";
            this.picView.Size = new System.Drawing.Size(952, 443);
            this.picView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picView.TabIndex = 1;
            this.picView.TabStop = false;
            this.picView.Click += new System.EventHandler(this.picView_Click);
            // 
            // txtPicInfo
            // 
            this.txtPicInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.txtPicInfo.Location = new System.Drawing.Point(335, 443);
            this.txtPicInfo.Multiline = true;
            this.txtPicInfo.Name = "txtPicInfo";
            this.txtPicInfo.Size = new System.Drawing.Size(952, 55);
            this.txtPicInfo.TabIndex = 2;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage2);
            this.tabControl2.Controls.Add(this.tabPage3);
            this.tabControl2.Controls.Add(this.tabPage8);
            this.tabControl2.Controls.Add(this.tabPage7);
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Left;
            this.tabControl2.Location = new System.Drawing.Point(0, 0);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(335, 498);
            this.tabControl2.TabIndex = 4;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox8);
            this.tabPage2.Controls.Add(this.groupBox5);
            this.tabPage2.Controls.Add(this.panel6);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(327, 468);
            this.tabPage2.TabIndex = 0;
            this.tabPage2.Text = "生成参数";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.txtPromptBlackList);
            this.groupBox8.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox8.Location = new System.Drawing.Point(3, 283);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(321, 182);
            this.groupBox8.TabIndex = 1;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "提示词黑名单";
            // 
            // txtPromptBlackList
            // 
            this.txtPromptBlackList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPromptBlackList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPromptBlackList.Location = new System.Drawing.Point(3, 19);
            this.txtPromptBlackList.Multiline = true;
            this.txtPromptBlackList.Name = "txtPromptBlackList";
            this.txtPromptBlackList.Size = new System.Drawing.Size(315, 160);
            this.txtPromptBlackList.TabIndex = 0;
            this.txtPromptBlackList.Text = resources.GetString("txtPromptBlackList.Text");
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.rdoResolutionFixed);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.lstResolutionList);
            this.groupBox5.Controls.Add(this.numResolutionWidth);
            this.groupBox5.Controls.Add(this.numResolutionHeight);
            this.groupBox5.Controls.Add(this.btnAddResolution);
            this.groupBox5.Controls.Add(this.btnDeleteResolution);
            this.groupBox5.Controls.Add(this.label1);
            this.groupBox5.Controls.Add(this.rdoResolutionOrder);
            this.groupBox5.Controls.Add(this.rdoResolutionRandom);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox5.Location = new System.Drawing.Point(3, 74);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(321, 209);
            this.groupBox5.TabIndex = 61;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "生图尺寸";
            // 
            // rdoResolutionFixed
            // 
            this.rdoResolutionFixed.AutoSize = true;
            this.rdoResolutionFixed.Checked = true;
            this.rdoResolutionFixed.Location = new System.Drawing.Point(118, 19);
            this.rdoResolutionFixed.Name = "rdoResolutionFixed";
            this.rdoResolutionFixed.Size = new System.Drawing.Size(50, 21);
            this.rdoResolutionFixed.TabIndex = 56;
            this.rdoResolutionFixed.TabStop = true;
            this.rdoResolutionFixed.Text = "固定";
            this.rdoResolutionFixed.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 165);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(284, 34);
            this.label5.TabIndex = 55;
            this.label5.Text = "请在添加尺寸前确保所添加的尺寸在网页端生图成功\r\n大尺寸将会消耗点数";
            // 
            // lstResolutionList
            // 
            this.lstResolutionList.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lstResolutionList.FormattingEnabled = true;
            this.lstResolutionList.ItemHeight = 21;
            this.lstResolutionList.Items.AddRange(new object[] {
            "832x1216",
            "1216x832",
            "1024x1024"});
            this.lstResolutionList.Location = new System.Drawing.Point(10, 46);
            this.lstResolutionList.Name = "lstResolutionList";
            this.lstResolutionList.Size = new System.Drawing.Size(295, 67);
            this.lstResolutionList.TabIndex = 47;
            // 
            // numResolutionWidth
            // 
            this.numResolutionWidth.Location = new System.Drawing.Point(36, 127);
            this.numResolutionWidth.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numResolutionWidth.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numResolutionWidth.Name = "numResolutionWidth";
            this.numResolutionWidth.Size = new System.Drawing.Size(55, 23);
            this.numResolutionWidth.TabIndex = 48;
            this.numResolutionWidth.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // numResolutionHeight
            // 
            this.numResolutionHeight.Location = new System.Drawing.Point(106, 127);
            this.numResolutionHeight.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numResolutionHeight.Minimum = new decimal(new int[] {
            32,
            0,
            0,
            0});
            this.numResolutionHeight.Name = "numResolutionHeight";
            this.numResolutionHeight.Size = new System.Drawing.Size(55, 23);
            this.numResolutionHeight.TabIndex = 49;
            this.numResolutionHeight.Value = new decimal(new int[] {
            32,
            0,
            0,
            0});
            // 
            // btnAddResolution
            // 
            this.btnAddResolution.Location = new System.Drawing.Point(167, 126);
            this.btnAddResolution.Name = "btnAddResolution";
            this.btnAddResolution.Size = new System.Drawing.Size(48, 23);
            this.btnAddResolution.TabIndex = 50;
            this.btnAddResolution.Text = "添加";
            this.btnAddResolution.UseVisualStyleBackColor = true;
            this.btnAddResolution.Click += new System.EventHandler(this.btnAddResolution_Click);
            // 
            // btnDeleteResolution
            // 
            this.btnDeleteResolution.Location = new System.Drawing.Point(221, 126);
            this.btnDeleteResolution.Name = "btnDeleteResolution";
            this.btnDeleteResolution.Size = new System.Drawing.Size(48, 23);
            this.btnDeleteResolution.TabIndex = 51;
            this.btnDeleteResolution.Text = "删除";
            this.btnDeleteResolution.UseVisualStyleBackColor = true;
            this.btnDeleteResolution.Click += new System.EventHandler(this.btnDeleteResolution_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(94, 129);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(14, 17);
            this.label1.TabIndex = 52;
            this.label1.Text = "x";
            // 
            // rdoResolutionOrder
            // 
            this.rdoResolutionOrder.AutoSize = true;
            this.rdoResolutionOrder.Location = new System.Drawing.Point(9, 19);
            this.rdoResolutionOrder.Name = "rdoResolutionOrder";
            this.rdoResolutionOrder.Size = new System.Drawing.Size(50, 21);
            this.rdoResolutionOrder.TabIndex = 53;
            this.rdoResolutionOrder.Text = "顺序";
            this.rdoResolutionOrder.UseVisualStyleBackColor = true;
            // 
            // rdoResolutionRandom
            // 
            this.rdoResolutionRandom.AutoSize = true;
            this.rdoResolutionRandom.Location = new System.Drawing.Point(62, 19);
            this.rdoResolutionRandom.Name = "rdoResolutionRandom";
            this.rdoResolutionRandom.Size = new System.Drawing.Size(50, 21);
            this.rdoResolutionRandom.TabIndex = 54;
            this.rdoResolutionRandom.Text = "随机";
            this.rdoResolutionRandom.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.label11);
            this.panel6.Controls.Add(this.numSteps);
            this.panel6.Controls.Add(this.chkDyn);
            this.panel6.Controls.Add(this.label9);
            this.panel6.Controls.Add(this.chkSmea);
            this.panel6.Controls.Add(this.label8);
            this.panel6.Controls.Add(this.cmbSampler);
            this.panel6.Controls.Add(this.numScale);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(321, 71);
            this.panel6.TabIndex = 62;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(7, 11);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(68, 17);
            this.label11.TabIndex = 44;
            this.label11.Text = "采样方式：";
            // 
            // numSteps
            // 
            this.numSteps.Location = new System.Drawing.Point(233, 8);
            this.numSteps.Maximum = new decimal(new int[] {
            28,
            0,
            0,
            0});
            this.numSteps.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numSteps.Name = "numSteps";
            this.numSteps.Size = new System.Drawing.Size(72, 23);
            this.numSteps.TabIndex = 40;
            this.numSteps.Value = new decimal(new int[] {
            28,
            0,
            0,
            0});
            // 
            // chkDyn
            // 
            this.chkDyn.AutoSize = true;
            this.chkDyn.Enabled = false;
            this.chkDyn.Location = new System.Drawing.Point(82, 38);
            this.chkDyn.Name = "chkDyn";
            this.chkDyn.Size = new System.Drawing.Size(53, 21);
            this.chkDyn.TabIndex = 46;
            this.chkDyn.Text = "DYN";
            this.chkDyn.UseVisualStyleBackColor = true;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(162, 10);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 17);
            this.label9.TabIndex = 39;
            this.label9.Text = "steps：";
            // 
            // chkSmea
            // 
            this.chkSmea.AutoSize = true;
            this.chkSmea.Location = new System.Drawing.Point(10, 37);
            this.chkSmea.Name = "chkSmea";
            this.chkSmea.Size = new System.Drawing.Size(61, 21);
            this.chkSmea.TabIndex = 45;
            this.chkSmea.Text = "SMEA";
            this.chkSmea.UseVisualStyleBackColor = true;
            this.chkSmea.CheckedChanged += new System.EventHandler(this.chkSmea_CheckedChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(163, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(43, 17);
            this.label8.TabIndex = 38;
            this.label8.Text = "CFG：";
            // 
            // cmbSampler
            // 
            this.cmbSampler.FormattingEnabled = true;
            this.cmbSampler.Items.AddRange(new object[] {
            "Euler",
            "Euler Ancestral",
            "DPM++ 2S Ancesstral",
            "DPM++ 2M",
            "DPM++ SDE",
            "DDIM"});
            this.cmbSampler.Location = new System.Drawing.Point(78, 8);
            this.cmbSampler.Name = "cmbSampler";
            this.cmbSampler.Size = new System.Drawing.Size(72, 25);
            this.cmbSampler.TabIndex = 43;
            // 
            // numScale
            // 
            this.numScale.DecimalPlaces = 1;
            this.numScale.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numScale.Location = new System.Drawing.Point(233, 35);
            this.numScale.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numScale.Name = "numScale";
            this.numScale.Size = new System.Drawing.Size(72, 23);
            this.numScale.TabIndex = 37;
            this.numScale.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.grpArtistRandom);
            this.tabPage3.Controls.Add(this.grpArtistFixed);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(327, 468);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "画师";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // grpArtistRandom
            // 
            this.grpArtistRandom.Controls.Add(this.txtArtistRandom);
            this.grpArtistRandom.Controls.Add(this.panel8);
            this.grpArtistRandom.Controls.Add(this.panel7);
            this.grpArtistRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grpArtistRandom.Location = new System.Drawing.Point(3, 149);
            this.grpArtistRandom.Name = "grpArtistRandom";
            this.grpArtistRandom.Size = new System.Drawing.Size(321, 316);
            this.grpArtistRandom.TabIndex = 3;
            this.grpArtistRandom.TabStop = false;
            this.grpArtistRandom.Text = "随机画师";
            // 
            // txtArtistRandom
            // 
            this.txtArtistRandom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArtistRandom.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtArtistRandom.Location = new System.Drawing.Point(3, 99);
            this.txtArtistRandom.Multiline = true;
            this.txtArtistRandom.Name = "txtArtistRandom";
            this.txtArtistRandom.Size = new System.Drawing.Size(315, 135);
            this.txtArtistRandom.TabIndex = 31;
            this.txtArtistRandom.Text = "画师A\r\n画师B,1,2,1,2\r\n画师C,0,0,1,2\r\n画师D,0,0,1,2|画师E,1,2,0,0";
            // 
            // panel8
            // 
            this.panel8.Controls.Add(this.label10);
            this.panel8.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel8.Location = new System.Drawing.Point(3, 234);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(315, 79);
            this.panel8.TabIndex = 33;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 3);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(296, 68);
            this.label10.TabIndex = 0;
            this.label10.Text = "格式：\r\n画师名,减权最小值,减权最大值,加权最小值,加权最大值\r\n如果不写，则默认为全局减权和加权\r\n可以抽画师组，画师之间以|隔开";
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label13);
            this.panel7.Controls.Add(this.label12);
            this.panel7.Controls.Add(this.numArtistMin);
            this.panel7.Controls.Add(this.label7);
            this.panel7.Controls.Add(this.numArtistMax);
            this.panel7.Controls.Add(this.chkArtistModify);
            this.panel7.Controls.Add(this.label6);
            this.panel7.Controls.Add(this.numDefaultArtistWeightIncreaseMax);
            this.panel7.Controls.Add(this.numDefaultArtistWeightReduceMax);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel7.Location = new System.Drawing.Point(3, 19);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(315, 80);
            this.panel7.TabIndex = 32;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(159, 6);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(101, 17);
            this.label13.TabIndex = 32;
            this.label13.Text = "全局加权{}Max：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(3, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 17);
            this.label12.TabIndex = 31;
            this.label12.Text = "全局减权[]Max：";
            // 
            // numArtistMin
            // 
            this.numArtistMin.Location = new System.Drawing.Point(127, 29);
            this.numArtistMin.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numArtistMin.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArtistMin.Name = "numArtistMin";
            this.numArtistMin.Size = new System.Drawing.Size(73, 23);
            this.numArtistMin.TabIndex = 19;
            this.numArtistMin.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(211, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(17, 17);
            this.label7.TabIndex = 30;
            this.label7.Text = "~";
            // 
            // numArtistMax
            // 
            this.numArtistMax.Location = new System.Drawing.Point(239, 29);
            this.numArtistMax.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numArtistMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numArtistMax.Name = "numArtistMax";
            this.numArtistMax.Size = new System.Drawing.Size(70, 23);
            this.numArtistMax.TabIndex = 20;
            this.numArtistMax.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // chkArtistModify
            // 
            this.chkArtistModify.AutoSize = true;
            this.chkArtistModify.Location = new System.Drawing.Point(2, 58);
            this.chkArtistModify.Name = "chkArtistModify";
            this.chkArtistModify.Size = new System.Drawing.Size(143, 21);
            this.chkArtistModify.TabIndex = 21;
            this.chkArtistModify.Text = "画师前添加artist:前缀";
            this.chkArtistModify.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 32);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(128, 17);
            this.label6.TabIndex = 18;
            this.label6.Text = "随机抽取画师数范围：";
            // 
            // numDefaultArtistWeightIncreaseMax
            // 
            this.numDefaultArtistWeightIncreaseMax.Location = new System.Drawing.Point(260, 2);
            this.numDefaultArtistWeightIncreaseMax.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numDefaultArtistWeightIncreaseMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDefaultArtistWeightIncreaseMax.Name = "numDefaultArtistWeightIncreaseMax";
            this.numDefaultArtistWeightIncreaseMax.Size = new System.Drawing.Size(49, 23);
            this.numDefaultArtistWeightIncreaseMax.TabIndex = 29;
            this.numDefaultArtistWeightIncreaseMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numDefaultArtistWeightReduceMax
            // 
            this.numDefaultArtistWeightReduceMax.Location = new System.Drawing.Point(104, 2);
            this.numDefaultArtistWeightReduceMax.Maximum = new decimal(new int[] {
            6,
            0,
            0,
            0});
            this.numDefaultArtistWeightReduceMax.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numDefaultArtistWeightReduceMax.Name = "numDefaultArtistWeightReduceMax";
            this.numDefaultArtistWeightReduceMax.Size = new System.Drawing.Size(49, 23);
            this.numDefaultArtistWeightReduceMax.TabIndex = 28;
            this.numDefaultArtistWeightReduceMax.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // grpArtistFixed
            // 
            this.grpArtistFixed.Controls.Add(this.txtArtistFixed);
            this.grpArtistFixed.Dock = System.Windows.Forms.DockStyle.Top;
            this.grpArtistFixed.Location = new System.Drawing.Point(3, 3);
            this.grpArtistFixed.Name = "grpArtistFixed";
            this.grpArtistFixed.Size = new System.Drawing.Size(321, 146);
            this.grpArtistFixed.TabIndex = 4;
            this.grpArtistFixed.TabStop = false;
            this.grpArtistFixed.Text = "固定画师";
            // 
            // txtArtistFixed
            // 
            this.txtArtistFixed.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtArtistFixed.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtArtistFixed.Location = new System.Drawing.Point(3, 19);
            this.txtArtistFixed.Multiline = true;
            this.txtArtistFixed.Name = "txtArtistFixed";
            this.txtArtistFixed.Size = new System.Drawing.Size(315, 124);
            this.txtArtistFixed.TabIndex = 0;
            this.txtArtistFixed.Text = "artist:画师1,artist:画师2";
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.dgvTagSnippet);
            this.tabPage8.Controls.Add(this.panel10);
            this.tabPage8.Location = new System.Drawing.Point(4, 26);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage8.Size = new System.Drawing.Size(327, 468);
            this.tabPage8.TabIndex = 4;
            this.tabPage8.Text = "wildcard";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // dgvTagSnippet
            // 
            this.dgvTagSnippet.AllowUserToAddRows = false;
            this.dgvTagSnippet.AllowUserToDeleteRows = false;
            this.dgvTagSnippet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTagSnippet.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2});
            this.dgvTagSnippet.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTagSnippet.Location = new System.Drawing.Point(3, 3);
            this.dgvTagSnippet.Name = "dgvTagSnippet";
            this.dgvTagSnippet.ReadOnly = true;
            this.dgvTagSnippet.RowHeadersVisible = false;
            this.dgvTagSnippet.RowTemplate.Height = 23;
            this.dgvTagSnippet.Size = new System.Drawing.Size(321, 314);
            this.dgvTagSnippet.TabIndex = 1;
            this.dgvTagSnippet.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvTagSnippet_CellClick);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.Frozen = true;
            this.dataGridViewTextBoxColumn1.HeaderText = "片段名";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "内容";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 200;
            // 
            // panel10
            // 
            this.panel10.Controls.Add(this.txtTagSnippetValue);
            this.panel10.Controls.Add(this.panel11);
            this.panel10.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel10.Location = new System.Drawing.Point(3, 317);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(321, 148);
            this.panel10.TabIndex = 0;
            // 
            // txtTagSnippetValue
            // 
            this.txtTagSnippetValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtTagSnippetValue.Location = new System.Drawing.Point(0, 0);
            this.txtTagSnippetValue.Multiline = true;
            this.txtTagSnippetValue.Name = "txtTagSnippetValue";
            this.txtTagSnippetValue.Size = new System.Drawing.Size(211, 148);
            this.txtTagSnippetValue.TabIndex = 0;
            // 
            // panel11
            // 
            this.panel11.Controls.Add(this.btnTagSnippetEdit);
            this.panel11.Controls.Add(this.btnTagSnippetDelete);
            this.panel11.Controls.Add(this.btnTagSnippetAdd);
            this.panel11.Controls.Add(this.label15);
            this.panel11.Controls.Add(this.txtTagSnippetName);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel11.Location = new System.Drawing.Point(211, 0);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(110, 148);
            this.panel11.TabIndex = 1;
            // 
            // btnTagSnippetEdit
            // 
            this.btnTagSnippetEdit.Location = new System.Drawing.Point(6, 93);
            this.btnTagSnippetEdit.Name = "btnTagSnippetEdit";
            this.btnTagSnippetEdit.Size = new System.Drawing.Size(101, 23);
            this.btnTagSnippetEdit.TabIndex = 5;
            this.btnTagSnippetEdit.Text = "修改";
            this.btnTagSnippetEdit.UseVisualStyleBackColor = true;
            this.btnTagSnippetEdit.Click += new System.EventHandler(this.btnTagSnippetEdit_Click);
            // 
            // btnTagSnippetDelete
            // 
            this.btnTagSnippetDelete.Location = new System.Drawing.Point(6, 122);
            this.btnTagSnippetDelete.Name = "btnTagSnippetDelete";
            this.btnTagSnippetDelete.Size = new System.Drawing.Size(101, 23);
            this.btnTagSnippetDelete.TabIndex = 4;
            this.btnTagSnippetDelete.Text = "删除";
            this.btnTagSnippetDelete.UseVisualStyleBackColor = true;
            this.btnTagSnippetDelete.Click += new System.EventHandler(this.btnTagSnippetDelete_Click);
            // 
            // btnTagSnippetAdd
            // 
            this.btnTagSnippetAdd.Location = new System.Drawing.Point(6, 64);
            this.btnTagSnippetAdd.Name = "btnTagSnippetAdd";
            this.btnTagSnippetAdd.Size = new System.Drawing.Size(101, 23);
            this.btnTagSnippetAdd.TabIndex = 2;
            this.btnTagSnippetAdd.Text = "添加";
            this.btnTagSnippetAdd.UseVisualStyleBackColor = true;
            this.btnTagSnippetAdd.Click += new System.EventHandler(this.btnTagSnippetAdd_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(6, 3);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(56, 17);
            this.label15.TabIndex = 1;
            this.label15.Text = "片段名：";
            // 
            // txtTagSnippetName
            // 
            this.txtTagSnippetName.Location = new System.Drawing.Point(6, 23);
            this.txtTagSnippetName.Name = "txtTagSnippetName";
            this.txtTagSnippetName.Size = new System.Drawing.Size(101, 23);
            this.txtTagSnippetName.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.groupBox9);
            this.tabPage7.Location = new System.Drawing.Point(4, 26);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(327, 468);
            this.tabPage7.TabIndex = 3;
            this.tabPage7.Text = "Vibe";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // groupBox9
            // 
            this.groupBox9.Controls.Add(this.panel9);
            this.groupBox9.Controls.Add(this.dgvVibe);
            this.groupBox9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox9.Location = new System.Drawing.Point(3, 3);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Size = new System.Drawing.Size(321, 462);
            this.groupBox9.TabIndex = 0;
            this.groupBox9.TabStop = false;
            this.groupBox9.Text = "Vibe";
            // 
            // panel9
            // 
            this.panel9.Controls.Add(this.btnVibeAdd);
            this.panel9.Controls.Add(this.btnVibeDelete);
            this.panel9.Controls.Add(this.btnVibeEdit);
            this.panel9.Controls.Add(this.label21);
            this.panel9.Controls.Add(this.label19);
            this.panel9.Controls.Add(this.numVibeRS);
            this.panel9.Controls.Add(this.numVibeIE);
            this.panel9.Controls.Add(this.picVibeView);
            this.panel9.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel9.Location = new System.Drawing.Point(3, 267);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(315, 192);
            this.panel9.TabIndex = 1;
            // 
            // btnVibeAdd
            // 
            this.btnVibeAdd.Location = new System.Drawing.Point(174, 108);
            this.btnVibeAdd.Name = "btnVibeAdd";
            this.btnVibeAdd.Size = new System.Drawing.Size(131, 23);
            this.btnVibeAdd.TabIndex = 7;
            this.btnVibeAdd.Text = "添加";
            this.btnVibeAdd.UseVisualStyleBackColor = true;
            this.btnVibeAdd.Click += new System.EventHandler(this.btnVibeAdd_Click);
            // 
            // btnVibeDelete
            // 
            this.btnVibeDelete.Location = new System.Drawing.Point(174, 166);
            this.btnVibeDelete.Name = "btnVibeDelete";
            this.btnVibeDelete.Size = new System.Drawing.Size(131, 23);
            this.btnVibeDelete.TabIndex = 6;
            this.btnVibeDelete.Text = "删除";
            this.btnVibeDelete.UseVisualStyleBackColor = true;
            this.btnVibeDelete.Click += new System.EventHandler(this.btnVibeDelete_Click);
            // 
            // btnVibeEdit
            // 
            this.btnVibeEdit.Location = new System.Drawing.Point(174, 137);
            this.btnVibeEdit.Name = "btnVibeEdit";
            this.btnVibeEdit.Size = new System.Drawing.Size(131, 23);
            this.btnVibeEdit.TabIndex = 5;
            this.btnVibeEdit.Text = "修改";
            this.btnVibeEdit.UseVisualStyleBackColor = true;
            this.btnVibeEdit.Click += new System.EventHandler(this.btnVibeEdit_Click);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(171, 54);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(119, 17);
            this.label21.TabIndex = 4;
            this.label21.Text = "Reference Strength";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(171, 8);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(134, 17);
            this.label19.TabIndex = 3;
            this.label19.Text = "Information Extracted";
            // 
            // numVibeRS
            // 
            this.numVibeRS.DecimalPlaces = 2;
            this.numVibeRS.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numVibeRS.Location = new System.Drawing.Point(174, 74);
            this.numVibeRS.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVibeRS.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numVibeRS.Name = "numVibeRS";
            this.numVibeRS.Size = new System.Drawing.Size(131, 23);
            this.numVibeRS.TabIndex = 2;
            this.numVibeRS.Value = new decimal(new int[] {
            6,
            0,
            0,
            65536});
            // 
            // numVibeIE
            // 
            this.numVibeIE.DecimalPlaces = 2;
            this.numVibeIE.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numVibeIE.Location = new System.Drawing.Point(174, 28);
            this.numVibeIE.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numVibeIE.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numVibeIE.Name = "numVibeIE";
            this.numVibeIE.Size = new System.Drawing.Size(131, 23);
            this.numVibeIE.TabIndex = 1;
            this.numVibeIE.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // picVibeView
            // 
            this.picVibeView.Dock = System.Windows.Forms.DockStyle.Left;
            this.picVibeView.Location = new System.Drawing.Point(0, 0);
            this.picVibeView.Name = "picVibeView";
            this.picVibeView.Size = new System.Drawing.Size(165, 192);
            this.picVibeView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picVibeView.TabIndex = 0;
            this.picVibeView.TabStop = false;
            this.picVibeView.Click += new System.EventHandler(this.picVibeView_Click);
            // 
            // dgvVibe
            // 
            this.dgvVibe.AllowUserToAddRows = false;
            this.dgvVibe.AllowUserToDeleteRows = false;
            this.dgvVibe.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVibe.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Column2,
            this.Column3});
            this.dgvVibe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvVibe.Location = new System.Drawing.Point(3, 19);
            this.dgvVibe.Name = "dgvVibe";
            this.dgvVibe.ReadOnly = true;
            this.dgvVibe.RowHeadersVisible = false;
            this.dgvVibe.RowTemplate.Height = 23;
            this.dgvVibe.Size = new System.Drawing.Size(315, 440);
            this.dgvVibe.TabIndex = 0;
            this.dgvVibe.SelectionChanged += new System.EventHandler(this.dgvSnippet_SelectionChanged);
            // 
            // Column1
            // 
            this.Column1.Frozen = true;
            this.Column1.HeaderText = "路径";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "提取信息 IE";
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "参考强度 RS";
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox7);
            this.tabPage5.Controls.Add(this.label18);
            this.tabPage5.Controls.Add(this.txtProxy);
            this.tabPage5.Controls.Add(this.label17);
            this.tabPage5.Controls.Add(this.txtToken);
            this.tabPage5.Controls.Add(this.label4);
            this.tabPage5.Location = new System.Drawing.Point(4, 26);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(327, 468);
            this.tabPage5.TabIndex = 2;
            this.tabPage5.Text = "设置";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.chkKeepResolution);
            this.groupBox7.Controls.Add(this.chkKeepRandomPrompt);
            this.groupBox7.Controls.Add(this.chkKeepWildcard);
            this.groupBox7.Controls.Add(this.chkKeepRandomArtist);
            this.groupBox7.Location = new System.Drawing.Point(6, 93);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(313, 83);
            this.groupBox7.TabIndex = 59;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "参数不变";
            // 
            // chkKeepResolution
            // 
            this.chkKeepResolution.AutoSize = true;
            this.chkKeepResolution.Checked = true;
            this.chkKeepResolution.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepResolution.Location = new System.Drawing.Point(144, 49);
            this.chkKeepResolution.Name = "chkKeepResolution";
            this.chkKeepResolution.Size = new System.Drawing.Size(99, 21);
            this.chkKeepResolution.TabIndex = 4;
            this.chkKeepResolution.Text = "生图尺寸不变";
            this.chkKeepResolution.UseVisualStyleBackColor = true;
            // 
            // chkKeepRandomPrompt
            // 
            this.chkKeepRandomPrompt.AutoSize = true;
            this.chkKeepRandomPrompt.Checked = true;
            this.chkKeepRandomPrompt.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepRandomPrompt.Location = new System.Drawing.Point(11, 49);
            this.chkKeepRandomPrompt.Name = "chkKeepRandomPrompt";
            this.chkKeepRandomPrompt.Size = new System.Drawing.Size(111, 21);
            this.chkKeepRandomPrompt.TabIndex = 3;
            this.chkKeepRandomPrompt.Text = "随机提示词不变";
            this.chkKeepRandomPrompt.UseVisualStyleBackColor = true;
            // 
            // chkKeepWildcard
            // 
            this.chkKeepWildcard.AutoSize = true;
            this.chkKeepWildcard.Checked = true;
            this.chkKeepWildcard.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepWildcard.Location = new System.Drawing.Point(144, 22);
            this.chkKeepWildcard.Name = "chkKeepWildcard";
            this.chkKeepWildcard.Size = new System.Drawing.Size(100, 21);
            this.chkKeepWildcard.TabIndex = 2;
            this.chkKeepWildcard.Text = "wildcard不变";
            this.chkKeepWildcard.UseVisualStyleBackColor = true;
            // 
            // chkKeepRandomArtist
            // 
            this.chkKeepRandomArtist.AutoSize = true;
            this.chkKeepRandomArtist.Checked = true;
            this.chkKeepRandomArtist.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkKeepRandomArtist.Location = new System.Drawing.Point(11, 22);
            this.chkKeepRandomArtist.Name = "chkKeepRandomArtist";
            this.chkKeepRandomArtist.Size = new System.Drawing.Size(99, 21);
            this.chkKeepRandomArtist.TabIndex = 1;
            this.chkKeepRandomArtist.Text = "随机画师不变";
            this.chkKeepRandomArtist.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(93, 73);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(157, 17);
            this.label18.TabIndex = 2;
            this.label18.Text = "例如http://127.0.0.1:10809";
            // 
            // txtProxy
            // 
            this.txtProxy.Location = new System.Drawing.Point(66, 47);
            this.txtProxy.Name = "txtProxy";
            this.txtProxy.Size = new System.Drawing.Size(246, 23);
            this.txtProxy.TabIndex = 1;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(14, 50);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(44, 17);
            this.label17.TabIndex = 0;
            this.label17.Text = "代理：";
            // 
            // txtToken
            // 
            this.txtToken.Location = new System.Drawing.Point(66, 11);
            this.txtToken.Name = "txtToken";
            this.txtToken.Size = new System.Drawing.Size(246, 23);
            this.txtToken.TabIndex = 57;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 58;
            this.label4.Text = "Token：";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1287, 302);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(959, 302);
            this.panel2.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.groupBox2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 149);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(959, 153);
            this.panel4.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtNegativePrompt);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(959, 153);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "负面提示词";
            // 
            // txtNegativePrompt
            // 
            this.txtNegativePrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNegativePrompt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNegativePrompt.Location = new System.Drawing.Point(3, 19);
            this.txtNegativePrompt.Multiline = true;
            this.txtNegativePrompt.Name = "txtNegativePrompt";
            this.txtNegativePrompt.Size = new System.Drawing.Size(953, 131);
            this.txtNegativePrompt.TabIndex = 0;
            this.txtNegativePrompt.Text = resources.GetString("txtNegativePrompt.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPrompt);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(959, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "正面提示词";
            // 
            // txtPrompt
            // 
            this.txtPrompt.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrompt.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtPrompt.Location = new System.Drawing.Point(3, 19);
            this.txtPrompt.Multiline = true;
            this.txtPrompt.Name = "txtPrompt";
            this.txtPrompt.Size = new System.Drawing.Size(953, 127);
            this.txtPrompt.TabIndex = 0;
            this.txtPrompt.Text = " <原神>,<固定画师>,<随机画师>,1girl,loli,solo,catgirl,white hair,blue eyes,<衣服>,<随机提示词>";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnRandomFilePath);
            this.panel3.Controls.Add(this.txtRandomPromptFolderPath);
            this.panel3.Controls.Add(this.lblRandomPromp);
            this.panel3.Controls.Add(this.btnWildcardFolderPath);
            this.panel3.Controls.Add(this.txtWildcardFolderPath);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.btnSetOutputFolder);
            this.panel3.Controls.Add(this.chkSavePromptToTxtNoArtist);
            this.panel3.Controls.Add(this.txtOutputPath);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.chkSavePromptToTxt);
            this.panel3.Controls.Add(this.groupBox4);
            this.panel3.Controls.Add(this.btnGenerate);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.numGenerateMaxNum);
            this.panel3.Controls.Add(this.numKeepParams);
            this.panel3.Controls.Add(this.label20);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(959, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(328, 302);
            this.panel3.TabIndex = 1;
            // 
            // btnRandomFilePath
            // 
            this.btnRandomFilePath.Location = new System.Drawing.Point(274, 162);
            this.btnRandomFilePath.Name = "btnRandomFilePath";
            this.btnRandomFilePath.Size = new System.Drawing.Size(40, 23);
            this.btnRandomFilePath.TabIndex = 66;
            this.btnRandomFilePath.Text = "...";
            this.btnRandomFilePath.UseVisualStyleBackColor = true;
            this.btnRandomFilePath.Click += new System.EventHandler(this.btnRandomPromptFolderPath_Click);
            // 
            // txtRandomPromptFolderPath
            // 
            this.txtRandomPromptFolderPath.Location = new System.Drawing.Point(125, 164);
            this.txtRandomPromptFolderPath.Name = "txtRandomPromptFolderPath";
            this.txtRandomPromptFolderPath.Size = new System.Drawing.Size(142, 23);
            this.txtRandomPromptFolderPath.TabIndex = 64;
            this.txtRandomPromptFolderPath.Text = ".\\prompt\\prompt_by_风吟";
            // 
            // lblRandomPromp
            // 
            this.lblRandomPromp.AutoSize = true;
            this.lblRandomPromp.Location = new System.Drawing.Point(8, 167);
            this.lblRandomPromp.Name = "lblRandomPromp";
            this.lblRandomPromp.Size = new System.Drawing.Size(104, 17);
            this.lblRandomPromp.TabIndex = 65;
            this.lblRandomPromp.Text = "随机提示词路径：";
            // 
            // btnWildcardFolderPath
            // 
            this.btnWildcardFolderPath.Location = new System.Drawing.Point(274, 191);
            this.btnWildcardFolderPath.Name = "btnWildcardFolderPath";
            this.btnWildcardFolderPath.Size = new System.Drawing.Size(40, 23);
            this.btnWildcardFolderPath.TabIndex = 63;
            this.btnWildcardFolderPath.Text = "...";
            this.btnWildcardFolderPath.UseVisualStyleBackColor = true;
            this.btnWildcardFolderPath.Click += new System.EventHandler(this.btnWildcardFolderPath_Click);
            // 
            // txtWildcardFolderPath
            // 
            this.txtWildcardFolderPath.Location = new System.Drawing.Point(99, 192);
            this.txtWildcardFolderPath.Name = "txtWildcardFolderPath";
            this.txtWildcardFolderPath.Size = new System.Drawing.Size(169, 23);
            this.txtWildcardFolderPath.TabIndex = 5;
            this.txtWildcardFolderPath.Text = ".\\wildcard";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(9, 195);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(86, 17);
            this.label14.TabIndex = 62;
            this.label14.Text = "<>文件路径：";
            // 
            // btnSetOutputFolder
            // 
            this.btnSetOutputFolder.Location = new System.Drawing.Point(274, 219);
            this.btnSetOutputFolder.Name = "btnSetOutputFolder";
            this.btnSetOutputFolder.Size = new System.Drawing.Size(40, 23);
            this.btnSetOutputFolder.TabIndex = 61;
            this.btnSetOutputFolder.Text = "...";
            this.btnSetOutputFolder.UseVisualStyleBackColor = true;
            this.btnSetOutputFolder.Click += new System.EventHandler(this.btnSetOutputFolder_Click);
            // 
            // chkSavePromptToTxtNoArtist
            // 
            this.chkSavePromptToTxtNoArtist.AutoSize = true;
            this.chkSavePromptToTxtNoArtist.Location = new System.Drawing.Point(177, 123);
            this.chkSavePromptToTxtNoArtist.Name = "chkSavePromptToTxtNoArtist";
            this.chkSavePromptToTxtNoArtist.Size = new System.Drawing.Size(137, 21);
            this.chkSavePromptToTxtNoArtist.TabIndex = 50;
            this.chkSavePromptToTxtNoArtist.Text = "同名的txt不保存画师";
            this.chkSavePromptToTxtNoArtist.UseVisualStyleBackColor = true;
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(99, 219);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.Size = new System.Drawing.Size(169, 23);
            this.txtOutputPath.TabIndex = 60;
            this.txtOutputPath.Text = ".\\output";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 222);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(68, 17);
            this.label3.TabIndex = 59;
            this.label3.Text = "输出路径：";
            // 
            // chkSavePromptToTxt
            // 
            this.chkSavePromptToTxt.AutoSize = true;
            this.chkSavePromptToTxt.Location = new System.Drawing.Point(18, 123);
            this.chkSavePromptToTxt.Name = "chkSavePromptToTxt";
            this.chkSavePromptToTxt.Size = new System.Drawing.Size(137, 21);
            this.chkSavePromptToTxt.TabIndex = 49;
            this.chkSavePromptToTxt.Text = "保存提示词到同名txt";
            this.chkSavePromptToTxt.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.cmbConfigName);
            this.groupBox4.Controls.Add(this.btnAddOrEditConfig);
            this.groupBox4.Controls.Add(this.btnOpenConfigFolder);
            this.groupBox4.Controls.Add(this.btnDeleteConfig);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox4.Location = new System.Drawing.Point(0, 250);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(328, 52);
            this.groupBox4.TabIndex = 6;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "配置文件";
            // 
            // cmbConfigName
            // 
            this.cmbConfigName.FormattingEnabled = true;
            this.cmbConfigName.Location = new System.Drawing.Point(6, 20);
            this.cmbConfigName.Name = "cmbConfigName";
            this.cmbConfigName.Size = new System.Drawing.Size(154, 25);
            this.cmbConfigName.TabIndex = 1;
            this.cmbConfigName.SelectedIndexChanged += new System.EventHandler(this.cmbConfigName_SelectedIndexChanged);
            this.cmbConfigName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cmbConfigName_MouseClick);
            // 
            // btnAddOrEditConfig
            // 
            this.btnAddOrEditConfig.Location = new System.Drawing.Point(166, 20);
            this.btnAddOrEditConfig.Name = "btnAddOrEditConfig";
            this.btnAddOrEditConfig.Size = new System.Drawing.Size(48, 23);
            this.btnAddOrEditConfig.TabIndex = 2;
            this.btnAddOrEditConfig.Text = "增/存";
            this.btnAddOrEditConfig.UseVisualStyleBackColor = true;
            this.btnAddOrEditConfig.Click += new System.EventHandler(this.btnAddOrEditConfig_Click);
            // 
            // btnOpenConfigFolder
            // 
            this.btnOpenConfigFolder.Location = new System.Drawing.Point(274, 20);
            this.btnOpenConfigFolder.Name = "btnOpenConfigFolder";
            this.btnOpenConfigFolder.Size = new System.Drawing.Size(48, 23);
            this.btnOpenConfigFolder.TabIndex = 4;
            this.btnOpenConfigFolder.Text = "目录";
            this.btnOpenConfigFolder.UseVisualStyleBackColor = true;
            this.btnOpenConfigFolder.Click += new System.EventHandler(this.btnOpenConfigFolder_Click);
            // 
            // btnDeleteConfig
            // 
            this.btnDeleteConfig.Location = new System.Drawing.Point(220, 20);
            this.btnDeleteConfig.Name = "btnDeleteConfig";
            this.btnDeleteConfig.Size = new System.Drawing.Size(48, 23);
            this.btnDeleteConfig.TabIndex = 3;
            this.btnDeleteConfig.Text = "删除";
            this.btnDeleteConfig.UseVisualStyleBackColor = true;
            this.btnDeleteConfig.Click += new System.EventHandler(this.btnDeleteConfig_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGenerate.Location = new System.Drawing.Point(18, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(296, 73);
            this.btnGenerate.TabIndex = 0;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 31;
            this.label2.Text = "生成数量：";
            // 
            // numGenerateMaxNum
            // 
            this.numGenerateMaxNum.Location = new System.Drawing.Point(87, 95);
            this.numGenerateMaxNum.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numGenerateMaxNum.Name = "numGenerateMaxNum";
            this.numGenerateMaxNum.Size = new System.Drawing.Size(71, 23);
            this.numGenerateMaxNum.TabIndex = 30;
            this.numGenerateMaxNum.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // numKeepParams
            // 
            this.numKeepParams.Location = new System.Drawing.Point(241, 95);
            this.numKeepParams.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numKeepParams.Name = "numKeepParams";
            this.numKeepParams.Size = new System.Drawing.Size(71, 23);
            this.numKeepParams.TabIndex = 47;
            this.numKeepParams.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(172, 99);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(68, 17);
            this.label20.TabIndex = 48;
            this.label20.Text = "参数不变：";
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.btnClearLog);
            this.tabPage4.Controls.Add(this.txtLog);
            this.tabPage4.Location = new System.Drawing.Point(4, 26);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(1293, 806);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "日志";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // btnClearLog
            // 
            this.btnClearLog.Location = new System.Drawing.Point(1172, 0);
            this.btnClearLog.Name = "btnClearLog";
            this.btnClearLog.Size = new System.Drawing.Size(104, 23);
            this.btnClearLog.TabIndex = 1;
            this.btnClearLog.Text = "清空日志";
            this.btnClearLog.UseVisualStyleBackColor = true;
            this.btnClearLog.Click += new System.EventHandler(this.btnClearAllLog_Click);
            // 
            // txtLog
            // 
            this.txtLog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtLog.HideSelection = false;
            this.txtLog.Location = new System.Drawing.Point(0, 0);
            this.txtLog.Multiline = true;
            this.txtLog.Name = "txtLog";
            this.txtLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtLog.Size = new System.Drawing.Size(1293, 806);
            this.txtLog.TabIndex = 0;
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.btnPushBackPic);
            this.tabPage6.Controls.Add(this.btnParsePrompt);
            this.tabPage6.Controls.Add(this.btnGetRollDoc);
            this.tabPage6.Controls.Add(this.btnGetMorePrompt);
            this.tabPage6.Controls.Add(this.pictureBox1);
            this.tabPage6.Controls.Add(this.label16);
            this.tabPage6.Location = new System.Drawing.Point(4, 26);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(1293, 806);
            this.tabPage6.TabIndex = 4;
            this.tabPage6.Text = "关于";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // btnPushBackPic
            // 
            this.btnPushBackPic.Location = new System.Drawing.Point(523, 122);
            this.btnPushBackPic.Name = "btnPushBackPic";
            this.btnPushBackPic.Size = new System.Drawing.Size(315, 32);
            this.btnPushBackPic.TabIndex = 5;
            this.btnPushBackPic.Text = "在线图片反推";
            this.btnPushBackPic.UseVisualStyleBackColor = true;
            this.btnPushBackPic.Click += new System.EventHandler(this.btnPushBackPic_Click);
            // 
            // btnParsePrompt
            // 
            this.btnParsePrompt.Location = new System.Drawing.Point(523, 84);
            this.btnParsePrompt.Name = "btnParsePrompt";
            this.btnParsePrompt.Size = new System.Drawing.Size(315, 32);
            this.btnParsePrompt.TabIndex = 4;
            this.btnParsePrompt.Text = "在线解析图片prompt";
            this.btnParsePrompt.UseVisualStyleBackColor = true;
            this.btnParsePrompt.Click += new System.EventHandler(this.btnParsePrompt_Click);
            // 
            // btnGetRollDoc
            // 
            this.btnGetRollDoc.Location = new System.Drawing.Point(523, 46);
            this.btnGetRollDoc.Name = "btnGetRollDoc";
            this.btnGetRollDoc.Size = new System.Drawing.Size(315, 32);
            this.btnGetRollDoc.TabIndex = 3;
            this.btnGetRollDoc.Text = "Nai3-Roll画风专用表（卡拉卡拉）需要登录QQ";
            this.btnGetRollDoc.UseVisualStyleBackColor = true;
            this.btnGetRollDoc.Click += new System.EventHandler(this.btnGetRollDoc_Click);
            // 
            // btnGetMorePrompt
            // 
            this.btnGetMorePrompt.Location = new System.Drawing.Point(523, 8);
            this.btnGetMorePrompt.Name = "btnGetMorePrompt";
            this.btnGetMorePrompt.Size = new System.Drawing.Size(315, 32);
            this.btnGetMorePrompt.TabIndex = 2;
            this.btnGetMorePrompt.Text = "点我获取更多随机prompt";
            this.btnGetMorePrompt.UseVisualStyleBackColor = true;
            this.btnGetMorePrompt.Click += new System.EventHandler(this.btnGetMorePrompt_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(416, 559);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(491, 239);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("喵字动漫体v1.002", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(300, 281);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(663, 275);
            this.label16.TabIndex = 0;
            this.label16.Text = "本软件仅用于学习跟交流使用\r\n\r\n请确保文件来源可信，避免拿到被有心人二次修改过的软件\r\n\r\n运行前请确认魔法工具已开启且能正常访问nai3页面\r\n\r\n软件设置每" +
    "次运行中间有延迟是为了防止账号被恶意使用而导致的封号\r\n\r\n请不要连续执行过多次数\r\n\r\n意见与bug反馈群：119125703";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.tabControl3);
            this.tabPage9.Location = new System.Drawing.Point(4, 26);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(1293, 806);
            this.tabPage9.TabIndex = 5;
            this.tabPage9.Text = "导演工具";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage10);
            this.tabControl3.Controls.Add(this.tabPage11);
            this.tabControl3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(1287, 800);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage10
            // 
            this.tabPage10.Location = new System.Drawing.Point(4, 26);
            this.tabPage10.Name = "tabPage10";
            this.tabPage10.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage10.Size = new System.Drawing.Size(1279, 770);
            this.tabPage10.TabIndex = 0;
            this.tabPage10.Text = "tabPage10";
            this.tabPage10.UseVisualStyleBackColor = true;
            // 
            // tabPage11
            // 
            this.tabPage11.Location = new System.Drawing.Point(4, 26);
            this.tabPage11.Name = "tabPage11";
            this.tabPage11.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage11.Size = new System.Drawing.Size(348, 135);
            this.tabPage11.TabIndex = 1;
            this.tabPage11.Text = "tabPage11";
            this.tabPage11.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1301, 836);
            this.Controls.Add(this.tabControl1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "Nai3自动生图脚本v2.1.0";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picView)).EndInit();
            this.tabControl2.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numResolutionWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numResolutionHeight)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSteps)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numScale)).EndInit();
            this.tabPage3.ResumeLayout(false);
            this.grpArtistRandom.ResumeLayout(false);
            this.grpArtistRandom.PerformLayout();
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numArtistMin)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numArtistMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultArtistWeightIncreaseMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultArtistWeightReduceMax)).EndInit();
            this.grpArtistFixed.ResumeLayout(false);
            this.grpArtistFixed.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTagSnippet)).EndInit();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.panel11.PerformLayout();
            this.tabPage7.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numVibeRS)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numVibeIE)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picVibeView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVibe)).EndInit();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numGenerateMaxNum)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numKeepParams)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabPage9.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        public System.Windows.Forms.TabPage tabPage1;
        public System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.Panel panel2;
        public System.Windows.Forms.Panel panel4;
        public System.Windows.Forms.GroupBox groupBox2;
        public System.Windows.Forms.GroupBox groupBox1;
        public System.Windows.Forms.Panel panel3;
        public System.Windows.Forms.Button btnGenerate;
        public System.Windows.Forms.Panel panel5;
        public System.Windows.Forms.PictureBox picView;
        public System.Windows.Forms.Button btnAddOrEditConfig;
        public System.Windows.Forms.ComboBox cmbConfigName;
        public System.Windows.Forms.Button btnOpenConfigFolder;
        public System.Windows.Forms.Button btnDeleteConfig;
        public System.Windows.Forms.TextBox txtNegativePrompt;
        public System.Windows.Forms.TextBox txtPrompt;
        public System.Windows.Forms.TextBox txtPicInfo;
        public System.Windows.Forms.GroupBox groupBox4;
        public System.Windows.Forms.TabPage tabPage4;
        public System.Windows.Forms.CheckBox chkDyn;
        public System.Windows.Forms.CheckBox chkSmea;
        public System.Windows.Forms.Label label11;
        public System.Windows.Forms.NumericUpDown numScale;
        public System.Windows.Forms.ComboBox cmbSampler;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.Label label9;
        public System.Windows.Forms.NumericUpDown numSteps;
        public System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown numGenerateMaxNum;
        public System.Windows.Forms.NumericUpDown numKeepParams;
        public System.Windows.Forms.Label label20;
        public System.Windows.Forms.ListBox lstResolutionList;
        public System.Windows.Forms.TextBox txtOutputPath;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox txtToken;
        public System.Windows.Forms.RadioButton rdoResolutionRandom;
        public System.Windows.Forms.RadioButton rdoResolutionOrder;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.Button btnDeleteResolution;
        public System.Windows.Forms.Button btnAddResolution;
        public System.Windows.Forms.NumericUpDown numResolutionHeight;
        public System.Windows.Forms.NumericUpDown numResolutionWidth;
        public System.Windows.Forms.CheckBox chkSavePromptToTxtNoArtist;
        public System.Windows.Forms.CheckBox chkSavePromptToTxt;
        public System.Windows.Forms.GroupBox groupBox5;
        public System.Windows.Forms.Button btnSetOutputFolder;
        public System.Windows.Forms.Label label5;
        public System.Windows.Forms.Panel panel6;
        public System.Windows.Forms.NumericUpDown numDefaultArtistWeightIncreaseMax;
        public System.Windows.Forms.NumericUpDown numDefaultArtistWeightReduceMax;
        public System.Windows.Forms.Label label6;
        public System.Windows.Forms.NumericUpDown numArtistMin;
        public System.Windows.Forms.NumericUpDown numArtistMax;
        public System.Windows.Forms.CheckBox chkArtistModify;
        public System.Windows.Forms.GroupBox grpArtistRandom;
        public System.Windows.Forms.Panel panel7;
        public System.Windows.Forms.Label label7;
        public System.Windows.Forms.Panel panel8;
        public System.Windows.Forms.Label label10;
        public System.Windows.Forms.TextBox txtLog;
        public System.Windows.Forms.TextBox txtArtistRandom;
        public System.Windows.Forms.Label label13;
        public System.Windows.Forms.Label label12;
        public System.Windows.Forms.GroupBox groupBox8;
        public System.Windows.Forms.TextBox txtPromptBlackList;
        public System.Windows.Forms.TabControl tabControl2;
        public System.Windows.Forms.TabPage tabPage2;
        public System.Windows.Forms.TabPage tabPage3;
        public System.Windows.Forms.GroupBox grpArtistFixed;
        public System.Windows.Forms.TextBox txtArtistFixed;
        public System.Windows.Forms.Button btnWildcardFolderPath;
        public System.Windows.Forms.TextBox txtWildcardFolderPath;
        public System.Windows.Forms.Label label14;
        public System.Windows.Forms.TabPage tabPage6;
        public System.Windows.Forms.Button btnRandomFilePath;
        public System.Windows.Forms.TextBox txtRandomPromptFolderPath;
        public System.Windows.Forms.Label lblRandomPromp;
        public System.Windows.Forms.PictureBox pictureBox1;
        public System.Windows.Forms.Label label16;
        public System.Windows.Forms.Button btnGetMorePrompt;
        public System.Windows.Forms.RadioButton rdoResolutionFixed;
        public System.Windows.Forms.TabPage tabPage5;
        public System.Windows.Forms.TextBox txtProxy;
        public System.Windows.Forms.Label label17;
        public System.Windows.Forms.Label label18;
        public System.Windows.Forms.Button btnClearLog;
        public System.Windows.Forms.GroupBox groupBox7;
        public System.Windows.Forms.CheckBox chkKeepRandomPrompt;
        public System.Windows.Forms.CheckBox chkKeepWildcard;
        public System.Windows.Forms.CheckBox chkKeepRandomArtist;
        public System.Windows.Forms.CheckBox chkKeepResolution;
        public System.Windows.Forms.Button btnGetRollDoc;
        public System.Windows.Forms.TabPage tabPage7;
        public System.Windows.Forms.GroupBox groupBox9;
        public System.Windows.Forms.Panel panel9;
        public System.Windows.Forms.DataGridView dgvVibe;
        public System.Windows.Forms.Label label21;
        public System.Windows.Forms.Label label19;
        public System.Windows.Forms.NumericUpDown numVibeRS;
        public System.Windows.Forms.NumericUpDown numVibeIE;
        public System.Windows.Forms.PictureBox picVibeView;
        public System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        public System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        public System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        public System.Windows.Forms.Button btnVibeAdd;
        public System.Windows.Forms.Button btnVibeDelete;
        public System.Windows.Forms.Button btnVibeEdit;
        public System.Windows.Forms.Button btnParsePrompt;
        public System.Windows.Forms.Button btnPushBackPic;
        public System.Windows.Forms.TabPage tabPage8;
        public System.Windows.Forms.DataGridView dgvTagSnippet;
        public System.Windows.Forms.Panel panel10;
        public System.Windows.Forms.TextBox txtTagSnippetValue;
        public System.Windows.Forms.Panel panel11;
        public System.Windows.Forms.Button btnTagSnippetAdd;
        public System.Windows.Forms.Label label15;
        public System.Windows.Forms.TextBox txtTagSnippetName;
        public System.Windows.Forms.Button btnTagSnippetDelete;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        public System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        public System.Windows.Forms.Button btnTagSnippetEdit;
        private System.Windows.Forms.TabPage tabPage9;
        private System.Windows.Forms.TabControl tabControl3;
        private System.Windows.Forms.TabPage tabPage10;
        private System.Windows.Forms.TabPage tabPage11;
    }
}

