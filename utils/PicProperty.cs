using AutoNai3Tools.body;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace AutoNai3Tools.utils {
    public class PicProperty  {
        [Category("生成")] [DisplayName("模型")] public BodyTools.Model Model { get; set; }
        [Category("生成")] [DisplayName("Noise Schedule(噪声)")] public NoiseOptions Noise { get; set; }

        private int _steps=28;
        [Category("生成")] [DisplayName("Steps(步数)")] public int Steps {
            get { return _steps; }
            set {
                if (value > 28) {
                    _steps = 28;
                } else if (value < 1) {
                    _steps = 1;
                }
                else {
                    _steps = value;
                }
            }
        }
        [Category("生成")] [DisplayName("Sampler(采样)")] public SamplerOptions Sampler { get; set; }

        private Switch _smea;  
        [Category("优化")] [DisplayName("SMEA")] public Switch Smea {
            get { return _smea; }  
            set {
                _smea = value; 
                if (value == Switch.关) {
                    Dyn = Switch.关; 
                }
            }
        }
        [Category("优化")] [DisplayName("DYN")] public Switch Dyn { get; set; }

        private float _scale;
        [Category("生成")] [DisplayName("Prompt Guidance(Scale)")] public float Scale {
            get { return _scale; }
            set {
                if (value > 10) {
                    _scale = 10;
                }
                else if (value < 0) {
                    _scale = 0;
                }
                else {
                    _scale = (float)Math.Round(value, 1);
                }
            }
        }
        [Category("生成")] [DisplayName("Prompt Guidance Rescale(CFG)")] public float CFG { get; set; }
        [Category("生成")] [DisplayName("Decrisp")] public Switch Decrisp { get; set; }
        [Category("生成")] [DisplayName("分辨率切换模式")] public ResolutionMode ResolutionMode { get; set; }
        [Category("生成")] [DisplayName("分辨率列表")][Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))] public string ResolutionList { get; set; } = "832x1216\r\n1216x832\r\n1024x1024";
        [Category("生成")][DisplayName("width")] public int Width { get; set; } = 832;
        [Category("生成")][DisplayName("height")] public int Height { get; set; } = 1216;
        [Category("生成")][DisplayName("固定种子")] public Switch FixedSeeds { get; set; } = Switch.关;

        [Category("生成")] [DisplayName("Seed(种子)")] public long Seeds { get; set; }
        private const string DefaultPromptBlackList =
            "cyan hair,grey hair,blonde hair,yellow hair,pink hair,orange hair,black hair,red hair,green hair,blue hair,purple hair,white hair,silver hair,brown hair,gold hair,light green hair,light grey hair,light gold hair,light silver hair,light brown hair,light red hair,light yellow hair,light black hair,light orange hair,light purple hair,light blue hair,light blonde hair,light white hair,light cyan hair,light pink hair,long hair,short hair,very long hair,double bun,twintails,curly hair,straight hair,ponytail,single side bun,side ponytail,bangs,ahoge,hair between eyes,blunt bangs,yellow eyes,purple eyes,blonde eyes,black eyes,grey eyes,white eyes,green eyes,blue eyes,gold eyes,brown eyes,red eyes,orange eyes,cyan eyes,silver eyes,pink eyes,masterpiece,best quality,breasts,small breasts,medium breasts,large breasts,huge breasts";

        [Category("提示词")]
        [DisplayName("提示词黑名单")]
        [Editor(typeof(MultiLineTextEditor), typeof(UITypeEditor))]
        public string PromptBlackList { get; set; } = DefaultPromptBlackList;
        [Category("优化")] [DisplayName("Variety")] public VarietyOptions Variety { get; set; }
        [Category("优化")] [DisplayName("Variety自定义值")] public double VarietyNum { get; set; }
        [Category("运行")][DisplayName("跑图数量")] public int RunNum { get; set; } = 1;
        [Category("运行")][DisplayName("参数固定数量")] public int RunKeepParams { get; set; } = 1;

        public Dictionary<string, object> GetProperty() {
            if (FixedSeeds == Switch.关) {
                Random random = new Random();
                Seeds = (long)(random.NextDouble() * 10000000000);
            }
            var kwargs = new Dictionary<string, object> {
                ["noise_schedule"] = GetEnumDescription(Noise),
                ["steps"] = Steps,
                ["sampler"] = GetEnumDescription(Sampler),
                ["sm"] = Smea == Switch.开,
                ["sm_dyn"] = Dyn == Switch.开,
                ["scale"] = Scale,
                ["dynamic_thresholding"] = Decrisp == Switch.开,
                ["cfg_rescale"] = CFG,
                ["seed"] = Seeds,
                ["width"] = Width,
                ["height"] = Height,
            };
                //OnPropertyChanged("Seeds");
                //OnPropertyChanged("Width");
                //OnPropertyChanged("Height");

            if (Variety != VarietyOptions.关) {
                kwargs["autoSmea"] = true;
                kwargs["skip_cfg_above_sigma"] = Variety == VarietyOptions.开 ? 19 : VarietyNum;

                if (Variety == VarietyOptions.自定义_风险参数) {
                    kwargs["deliberate_euler_ancestral_bug"] = false;
                    kwargs["prefer_brownian"] = true;
                }
            }

            return kwargs;
        }

        private static string GetEnumDescription<T>(T value) where T : Enum {
            FieldInfo fi = value.GetType().GetField(value.ToString());
            return fi?.GetCustomAttribute<DescriptionAttribute>()?.Description ?? value.ToString();
        }

        //public event PropertyChangedEventHandler PropertyChanged;
        //protected void OnPropertyChanged(string name) {
        //    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        //}
    }

    public enum Switch {
         开,
         关
    }

    public enum ResolutionMode {
        固定,
        顺序,
        随机
    }

    public enum NoiseOptions {
        [Description("native")] native,
        [Description("karras")] karras,
        [Description("exponential")] exponential,
        [Description("polyexponential")] polyexponential
    }

    public enum SamplerOptions {
        [Description("k_euler")] Euler,
        [Description("k_euler_ancestral")] Euler_Ancestral,
        [Description("k_dpmpp_2s_ancestral")] DPMpp_2S_Ancestral,
        [Description("k_dpmpp_2m_sde")] DPMpp_2M_SDE,
        [Description("k_dpmpp_2m")] DPMpp_2M,
        [Description("k_dpmpp_sde")] DPMpp_SDE,
        [Description("ddim_v3")] DDIM
    }

    public enum VarietyOptions {
        关,
        开,
        自定义_风险参数
    }

    public class MultiLineTextForm : Form {
        private TextBox textBox;
        private Button btnOK;

        public string ResultText => textBox.Text;

        public MultiLineTextForm(string initialText) {
            this.Text = "编辑文本";
            this.Width = 500;
            this.Height = 400;

            textBox = new TextBox() {
                Multiline = true,
                Dock = DockStyle.Fill,
                ScrollBars = ScrollBars.Vertical,
                Text = initialText,
                AcceptsReturn = true,
                AcceptsTab = true,
            };

            btnOK = new Button() {
                Text = "确定",
                Dock = DockStyle.Bottom,
                DialogResult = DialogResult.OK
            };

            this.Controls.Add(textBox);
            this.Controls.Add(btnOK);
        }
    }

    public class MultiLineTextEditor : UITypeEditor {
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.Modal; // 使用模态弹窗
        }

        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            string currentValue = value as string ?? "";

            using (var form = new MultiLineTextForm(currentValue)) {
                if (form.ShowDialog() == DialogResult.OK) {
                    return form.ResultText;
                }
            }

            return value; 
        }
    }
}
