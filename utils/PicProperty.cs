using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoNai3Tools.utils
{
    class PicProperty
    {
        [Category("图片参数")]
        [Description("用户名")]
        public string  { get; set; }

        [Category("图片参数")]
        [Description("用户名")]
        public string Username { get; set; }

        [Category("基本参数")]
        [Description("是否启用功能")]
        public bool EnableFeature { get; set; }

        [Category("高级参数")]
        [Description("选项列表")]
        public MyOptions Option { get; set; }

        [Category("高级参数")]
        [Description("颜色选择")]
        public Color ThemeColor { get; set; }

        [Category("高级参数")]
        [Description("文件路径")]
        [Editor(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string FilePath { get; set; }
    }

    public enum MyOptions {
        选项A,
        选项B,
        选项C
    }
}
