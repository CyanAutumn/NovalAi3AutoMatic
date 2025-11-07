using System.ComponentModel;

namespace AutoNai3Tools.utils {
    public class SettingProperty {
        [Category("认证")]
        [DisplayName("Token")]
        public string Token { get; set; }

        [Category("认证")]
        [DisplayName("代理地址")]
        public string Proxy { get; set; }

        [Category("显示")]
        [DisplayName("关闭图片预览")]
        public bool ClosePicPreview { get; set; }

        [Category("参数保持")]
        [DisplayName("随机画师不变")]
        public bool KeepRandomArtist { get; set; } = true;

        [Category("参数保持")]
        [DisplayName("Wildcard 不变")]
        public bool KeepWildcard { get; set; } = true;

        [Category("参数保持")]
        [DisplayName("随机提示词不变")]
        public bool KeepRandomPrompt { get; set; } = true;

        [Category("参数保持")]
        [DisplayName("生图尺寸不变")]
        public bool KeepResolution { get; set; } = true;

        [Category("休眠")]
        [DisplayName("短休最小秒")]
        public int SleepTimeShortLow { get; set; } = 5;

        [Category("休眠")]
        [DisplayName("短休最大秒")]
        public int SleepTimeShortHigh { get; set; } = 8;

        [Category("休眠")]
        [DisplayName("长休最小秒")]
        public int SleepTimeLongLow { get; set; } = 20;

        [Category("休眠")]
        [DisplayName("长休最大秒")]
        public int SleepTimeLongHigh { get; set; } = 25;
    }
}
