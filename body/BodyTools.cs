using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoNai3Tools.body {
    public class BodyTools {
        public enum Model {
            [Description("nai-diffusion-2")] Nai2,
            [Description("nai-diffusion-3")] Nai3,
            [Description("nai-diffusion-furry-3")] Nai3_Furry,

            [Description("nai-diffusion-4-curated-preview")]
            Nai4_Preview,
            [Description("nai-diffusion-4-full")] Nai4_Full,
            [Description("nai-diffusion-4-5-curated")] Nai4_5_Curated,
        }

        public static BodyBase GetBody(Model modelName, Dictionary<string, object> kwargs) {
            switch (modelName) {
                case Model.Nai2:
                    return new Nai2(kwargs);
                case Model.Nai3:
                    return new Nai3(kwargs);
                case Model.Nai3_Furry:
                    return new Nai3Furry(kwargs);
                case Model.Nai4_Preview:
                    return new Nai4Preview(kwargs);
                case Model.Nai4_Full:
                    return new Nai4Full(kwargs);
                case Model.Nai4_5_Curated:
                    return new Nai4_5Curated(kwargs);
            }

            throw new Exception("选择的模型无效");
        }

        public static string GetEnumDescription(Enum value) {
            var field = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute));
            return attribute?.Description ?? value.ToString();
        }
    }
}