using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AutoNai3Tools.body;
using Nett;

namespace AutoNai3Tools.utils {
    internal class SystemConfig {
        public string Token { get; set; }
        public List<SnippetItem> SnippetItems { get; set; } // 添加用于保存dgvSnippet行数据的属性
        public string PromptBlackList { get; set; }
        public int? SleepTimeShortLow { get; set; }
        public int? SleepTimeShortHigh { get; set; }
        public int? SleepTimeLongLow { get; set; }
        public int? SleepTimeLongHigh { get; set; }

        public static void SaveToml(Form1 form) {
            string folderPath = "C:\\Users\\Public\\Documents\\auto_nai3_system\\";
            if (!Directory.Exists(folderPath)) {
                try {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e) { }
            }

            SystemConfig obj = new SystemConfig();
            obj.Token = form.txtToken.Text;
            obj.PromptBlackList = form.txtPromptBlackList.Text;
            obj.SleepTimeShortLow = ((int)form.nudSleepTimeShortLow.Value);
            obj.SleepTimeShortHigh = ((int)form.nudSleepTimeShortHigh.Value);
            obj.SleepTimeLongLow = ((int)form.nudSleepTimeLongLow.Value);
            obj.SleepTimeLongHigh = ((int)form.nudSleepTimeLongHigh.Value);
            Toml.WriteFile(obj, Path.Combine(folderPath, "config.toml"));
        }

        public static void ReadToml(Form1 form) {
            string configFilePath = "C:\\Users\\Public\\Documents\\auto_nai3_system\\config.toml";

            if (File.Exists(configFilePath)) {
                SystemConfig obj = Toml.ReadFile<SystemConfig>(configFilePath);
                form.txtToken.Text = obj.Token;
                form.txtPromptBlackList.Text = obj.PromptBlackList;
                form.nudSleepTimeShortLow.Value = obj.SleepTimeShortLow == null ? 5 : ((int)obj.SleepTimeShortLow);
                form.nudSleepTimeShortHigh.Value = obj.SleepTimeShortHigh == null ? 8 : ((int)obj.SleepTimeShortHigh);
                form.nudSleepTimeLongLow.Value = obj.SleepTimeLongLow == null ? 20 : ((int)obj.SleepTimeLongLow.Value);
                form.nudSleepTimeLongHigh.Value =
                    obj.SleepTimeLongHigh == null ? 25 : ((int)obj.SleepTimeLongHigh.Value);
            }
        }
    }


    internal class Config {
        // prompt
        public string Prompt { get; set; }

        public string NegativePrompt { get; set; }

        //public string PromptBlackList { get; set; }
        public int GenerateMaxNum { get; set; }
        public int KeepParams { get; set; }
        public bool SavePromptToTxt { get; set; }
        public bool SavePromptToTxtNoArtist { get; set; }
        public ResolutionMode ResolutionMode { get; set; }
        public string RandomPromptFolderPath { get; set; }
        public string WildcardFolderPath { get; set; }
        public string OutputPath { get; set; }
        public string Token { get; set; }
        public int SamplerIndex { get; set; }
        public int Steps { get; set; }
        public float Scale { get; set; }
        public float CFG { get; set; }
        public int Noise { get; set; }
        public bool Smea { get; set; }
        public bool Dyn { get; set; }
        public string[] ResolutionList { get; set; }

        //artist
        public string ArtistFixed { get; set; }
        public string ArtistRandom { get; set; }
        public int DefaultArtistWeightReduceMax { get; set; }
        public int DefaultArtistWeightIncreaseMax { get; set; }
        public int ArtistMin { get; set; }
        public int ArtistMax { get; set; }
        public bool ArtistModify { get; set; }
        public string Proxy { get; set; }
        public bool KeepRandomArtist { get; set; }
        public bool KeepWildcard { get; set; }
        public bool KeepRandomPrompt { get; set; }
        public bool KeepResolution { get; set; }
        public bool Decrisp { get; set; }
        public long Seeds { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool Variety { get; set; }
        public bool VarietyDefault { get; set; }
        public double VarietyNum { get; set; }
        public Switch FixedSeeds { get; set; }
        public BodyTools.Model ModelSelect { get; set; }

        public static void SaveToml(Form1 form, string fileName) {
            string folderPath = "C:\\Users\\Public\\Documents\\auto_nai3_2\\";
            //判断文件夹是否存在
            if (!Directory.Exists(folderPath)) {
                //创建文件夹
                try {
                    Directory.CreateDirectory(folderPath);
                }
                catch (Exception e) { }
            }

            Config obj = new Config();
            obj.Prompt = form.txtPrompt.Text;
            obj.NegativePrompt = form.txtNegativePrompt.Text;
            //obj.PromptBlackList = form.txtPromptBlackList.Text;
            obj.GenerateMaxNum = ((int)form.numGenerateMaxNum.Value);
            obj.KeepParams = ((int)form.numKeepParams.Value);
            obj.SavePromptToTxt = form.chkSavePromptToTxt.Checked;
            obj.SavePromptToTxtNoArtist = form.chkSavePromptToTxtNoArtist.Checked;
            obj.ResolutionMode = form.picProps.ResolutionMode;
            obj.RandomPromptFolderPath = form.txtRandomPromptFolderPath.Text;
            obj.WildcardFolderPath = form.txtWildcardFolderPath.Text;
            obj.OutputPath = form.txtOutputPath.Text;
            //obj.Token = form.txtToken.Text;
            obj.SamplerIndex = (int)form.picProps.Sampler;
            obj.Steps = form.picProps.Steps;
            obj.Scale = form.picProps.Scale;
            obj.CFG = form.picProps.CFG;
            obj.Noise = (int)form.picProps.Noise;
            obj.Smea = form.picProps.Smea==Switch.开;
            obj.Dyn = form.picProps.Dyn==Switch.开;
            List<string> resolutionList = new List<string>();
            var _resolutionList = form.picProps.ResolutionList.Split(new string[] { "\r\n" }, StringSplitOptions.None);
            for (int i = 0; i < _resolutionList.Length; i++) {
                resolutionList.Add(_resolutionList[i]);
            }
            obj.ResolutionList = resolutionList.ToArray();
            obj.ArtistFixed = form.txtArtistFixed.Text;
            obj.ArtistRandom = form.txtArtistRandom.Text;
            obj.DefaultArtistWeightReduceMax = ((int)form.numDefaultArtistWeightReduceMax.Value);
            obj.DefaultArtistWeightIncreaseMax = ((int)form.numDefaultArtistWeightIncreaseMax.Value);
            obj.ArtistMin = ((int)form.numArtistMin.Value);
            obj.ArtistMax = ((int)form.numArtistMax.Value);
            obj.Proxy = form.txtProxy.Text;
            obj.KeepRandomArtist = form.chkKeepRandomArtist.Checked;
            obj.KeepWildcard = form.chkKeepWildcard.Checked;
            obj.KeepRandomPrompt = form.chkKeepRandomPrompt.Checked;
            obj.KeepResolution = form.chkKeepResolution.Checked;
            obj.Decrisp = form.picProps.Decrisp == Switch.开;
            obj.FixedSeeds = form.picProps.FixedSeeds ;
            obj.Seeds = form.picProps.Seeds;
            obj.Width = form.picProps.Width;
            obj.Height = form.picProps.Height;
            obj.Variety = form.picProps.Variety != VarietyOptions.关;
            obj.VarietyDefault = form.picProps.Variety == VarietyOptions.自定义_风险参数;
            obj.VarietyNum = form.picProps.VarietyNum;
            obj.ModelSelect = form.picProps.Model;
            Toml.WriteFile(obj, folderPath + fileName + ".toml");
        }

        public static void ReadToml(Form1 form, string fileName) {
            Config obj = Toml.ReadFile<Config>("C:\\Users\\Public\\Documents\\auto_nai3_2\\" + fileName + ".toml");
            form.txtPrompt.Text = obj.Prompt;
            form.txtNegativePrompt.Text = obj.NegativePrompt;
            //form.txtPromptBlackList.Text = obj.PromptBlackList;
            form.numGenerateMaxNum.Value = obj.GenerateMaxNum;
            form.numKeepParams.Value = obj.KeepParams;
            form.chkSavePromptToTxt.Checked = obj.SavePromptToTxt;
            form.chkSavePromptToTxtNoArtist.Checked = obj.SavePromptToTxtNoArtist;
            form.picProps.ResolutionMode= obj.ResolutionMode;
            form.txtRandomPromptFolderPath.Text = obj.RandomPromptFolderPath;
            form.txtWildcardFolderPath.Text = obj.WildcardFolderPath;
            form.txtOutputPath.Text = obj.OutputPath;
            //form.txtToken.Text = obj.Token;
            form.picProps.Sampler = (SamplerOptions)obj.SamplerIndex;
            form.picProps.Steps = obj.Steps;
            form.picProps.Scale = obj.Scale;
            form.picProps.CFG = obj.CFG;
            form.picProps.Noise = (NoiseOptions)obj.Noise;
            form.picProps.Smea = obj.Smea?Switch.开:Switch.关;
            form.picProps.Dyn = obj.Dyn ? Switch.开 : Switch.关;
            if (obj.ResolutionList == null) {
                obj.ResolutionList = new string[] { "832x1216", "1216x832", "1024x1024" };
            }
            form.picProps.ResolutionList = string.Join("\r\n", obj.ResolutionList);

            form.txtArtistFixed.Text = obj.ArtistFixed;
            form.txtArtistRandom.Text = obj.ArtistRandom;
            form.numDefaultArtistWeightReduceMax.Value = obj.DefaultArtistWeightReduceMax;
            form.numDefaultArtistWeightIncreaseMax.Value = obj.DefaultArtistWeightIncreaseMax;
            form.numArtistMin.Value = obj.ArtistMin;
            form.numArtistMax.Value = obj.ArtistMax;
            form.txtProxy.Text = obj.Proxy;
            form.chkKeepRandomArtist.Checked = obj.KeepRandomArtist;
            form.chkKeepWildcard.Checked = obj.KeepWildcard;
            form.chkKeepRandomPrompt.Checked = obj.KeepRandomPrompt;
            form.chkKeepResolution.Checked = obj.KeepResolution;
            form.picProps.Decrisp = obj.Decrisp ? Switch.开 : Switch.关; ;
            form.picProps.FixedSeeds = obj.FixedSeeds ;
            form.picProps.Seeds = obj.Seeds;
            form.picProps.Width = obj.Width;
            form.picProps.Height = obj.Height;
            form.picProps.Variety = obj.Variety ? obj.VarietyDefault ? VarietyOptions.自定义_风险参数: VarietyOptions.开 : VarietyOptions.关; ;
            form.picProps.VarietyNum = obj.VarietyNum;
            form.picProps.Model= obj.ModelSelect;
        }
    }

    class SnippetItem {
        public string Name { get; set; }
        public string Value { get; set; }
    }
}